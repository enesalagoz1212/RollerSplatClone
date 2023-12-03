using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RollerSplatClone.Controllers;
using RollerSplatClone.ScritableObjects;
using DG.Tweening;

namespace RollerSplatClone.Managers
{
	public class LevelManager : MonoBehaviour
	{
		public static LevelManager Instance { get; private set; }
		private BallController _ballController;

		private Level _currentLevelData;
		public Level[] levels;
		public GameObject levelContainer;

		[SerializeField] private GameObject prefabWall;
		[SerializeField] private GameObject prefabGround;

		private Color _colorWall = Color.white;
		private Color _colorGround = Color.black;
		private Color _levelColor;

		private float _unitPerPixel;
		private int _isPaintGroundController = 0;

		private int _spawnedGroundCount;
		private int _currentLevelIndex;

		private List<Transform> spawnedGroundList = new List<Transform>();

		private GroundController[,] groundControllers;
		public void Initialize(BallController ballController)
		{
			_ballController = ballController;
		}

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(this);
			}
			else
			{
				Instance = this;
			}
		}

		private void OnEnable()
		{
			GameManager.OnMenuOpen += OnGameMenu;
			GameManager.OnGameEnd += OnGameEnd;
			GameManager.OnGameReset += OnGameReset;
		}

		private void OnDisable()
		{
			GameManager.OnMenuOpen -= OnGameMenu;
			GameManager.OnGameEnd -= OnGameEnd;
			GameManager.OnGameReset -= OnGameReset;
		}

		private void OnGameEnd(bool isSuccessful)
		{
			if (isSuccessful)
			{
				BallPrefsManager.CurrentLevel = _currentLevelIndex;
				_currentLevelIndex++;
				if (_currentLevelIndex % levels.Length == 0)
				{
					_currentLevelIndex = levels.Length;
				}

				int moodCurrentLevel = _currentLevelIndex % levels.Length;
				_currentLevelData = levels[moodCurrentLevel];

				DOVirtual.DelayedCall(2f, () =>
				{
					ClearLevel();
					_isPaintGroundController = 0;
				});
			}
		}

		private void OnGameReset()
		{
			_isPaintGroundController = 0;

		}

		private void OnGameMenu()
		{
			_currentLevelIndex = BallPrefsManager.CurrentLevel;
			int moodCurrentLevel = _currentLevelIndex % levels.Length;
			if (moodCurrentLevel == 0)
			{
				moodCurrentLevel = levels.Length;
			}
			_currentLevelIndex = moodCurrentLevel ;
			_levelColor = GetRandomColor();
			_ballController.ColorTheBall(_levelColor);
			LevelGenerate();
		}

		private void LevelGenerate()
		{
			_currentLevelData = levels[_currentLevelIndex - 1];

			_unitPerPixel = prefabWall.transform.lossyScale.x;
			float halfUnitPerPixel = _unitPerPixel;

			float width = _currentLevelData.levelTexture.width;
			float height = _currentLevelData.levelTexture.height;


			_spawnedGroundCount = 0;
			spawnedGroundList.Clear();

			Vector3 offset = (new Vector3(width / 2, 0f, height / 2) * _unitPerPixel) - new Vector3(halfUnitPerPixel, 0f, halfUnitPerPixel);

			bool bonusLevel = _currentLevelData.isBonusLevel;

			groundControllers = new GroundController[(int)width, (int)height];

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					//Get pixel color :
					Color pixelColor = _currentLevelData.levelTexture.GetPixel(x, y);
					Vector3 spawnPos = ((new Vector3(x, 0f, y) * _unitPerPixel) - offset);

					if (pixelColor == _colorWall)
					{
						GameObject wallObj = Spawn(prefabWall, spawnPos);
					}
					else if (pixelColor == _colorGround)
					{
						GameObject groundObj = Spawn(prefabGround, spawnPos);
						var groundController = groundObj.GetComponent<GroundController>();
						spawnedGroundList.Add(groundObj.transform);
						_spawnedGroundCount++;

						if (bonusLevel)
						{
							groundController.SpawnGold();

						}

						groundControllers[x, y] = groundController;
						groundController.Init(x, y);
					}
				}

			}

			var spawnGroundController = ReturnSpawnGroundController();
			_ballController.AssignSpawnPosition(spawnGroundController);
			PaintTargetGround(spawnGroundController);
		}

		public GameObject Spawn(GameObject prefab, Vector3 position)
		{
			position.y = prefab.transform.position.y;
			GameObject obj = Instantiate(prefab, position, Quaternion.identity);
			obj.transform.parent = levelContainer.transform;

			return obj;
		}

		public GroundController ReturnSpawnGroundController()
		{
			for (int x = 0; x < groundControllers.GetLength(0); x++)
			{
				for (int y = 0; y < groundControllers.GetLength(1); y++)
				{
					if (groundControllers[x, y] != null)
					{
						return groundControllers[x, y];
					}
				}
			}
			return null;
		}

		public GroundController ReturnDirectionGroundController(Direction direction, GroundController currentGroundController)
		{
			var xIndex = currentGroundController.xIndex;
			var yIndex = currentGroundController.yIndex;
			GroundController targetGroundController = null;

			var targetGroundControllers = new List<GroundController>();
			switch (direction)
			{
				case Direction.North: // Y++
					for (int y = yIndex + 1; y < groundControllers.GetLength(1); y++)
					{
						if (groundControllers[xIndex, y] == null)
						{
							break;
						}
						targetGroundController = groundControllers[xIndex, y];
						targetGroundControllers.Add(targetGroundController);
					}
					break;

				case Direction.South: // Y--
					for (int y = yIndex - 1; y >= 0; y--)
					{
						if (groundControllers[xIndex, y] == null)
						{
							break;
						}
						targetGroundController = groundControllers[xIndex, y];
						targetGroundControllers.Add(targetGroundController);
					}
					break;

				case Direction.East: // X++
					for (int x = xIndex + 1; x < groundControllers.GetLength(0); x++)
					{
						if (groundControllers[x, yIndex] == null)
						{
							break;
						}
						targetGroundController = groundControllers[x, yIndex];
						targetGroundControllers.Add(targetGroundController);
					}
					break;

				case Direction.West: // X--
					for (int x = xIndex - 1; x >= 0; x--)
					{
						if (groundControllers[x, yIndex] == null)
						{
							break;
						}
						targetGroundController = groundControllers[x, yIndex];
						targetGroundControllers.Add(targetGroundController);
					}
					break;
			}
			for (int i = 0; i < targetGroundControllers.Count; i++)
			{
				PaintTargetGround(targetGroundControllers[i]);
				if (_currentLevelData.isBonusLevel)
				{
					GameManager.Instance.IncreaseGoldScore(1);
				}
			}
			return targetGroundController;
		}

		public void PaintTargetGround(GroundController groundController)
		{
			if (groundController.IsPainted)
			{
				return;
			}
			groundController.PaintGround(_levelColor);
			_isPaintGroundController++;
			CheckLevelEnd();
		}

		private void CheckLevelEnd()
		{
			if (_isPaintGroundController >= _spawnedGroundCount)
			{
				GameManager.Instance.GameEnd(true);
			}
		}

		private Color GetRandomColor()
		{
			Color[] randomColors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta, Color.cyan, Color.grey };
			int randomIndex = Random.Range(0, randomColors.Length);
			return randomColors[randomIndex];
		}

		private void ClearLevel()
		{
			foreach (Transform child in levelContainer.transform)
			{
				Destroy(child.gameObject);
			}
			spawnedGroundList.Clear();
		}
	}
}

