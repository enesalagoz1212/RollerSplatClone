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


		[Header("Tiles Prefabs")]
		[SerializeField] private GameObject prefabWall;
		[SerializeField] private GameObject prefabGround;
		[SerializeField] private GameObject prefabGold;

		private Color colorWall = Color.white;
		private Color colorGround = Color.black;
		private Color levelColor;

		private float unitPerPixel;
		private int _isPaintGroundController = 0;

		private int _spawnedGroundCount;
		private int _currentLevelIndex;

		private List<Transform> spawnedGroundList = new List<Transform>();
		private List<Transform> spawnedWallList = new List<Transform>();
		private List<GameObject> goldSpawnlist = new List<GameObject>();

		private Transform bottomLeftWall;
		private Transform bottomRightWall;

		private GroundController[,] groundControllers;

		public GameObject gold;
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
			levelColor = GetRandomColor();
			_ballController.ColorTheBall(levelColor);
			LevelGenerate();
		}

		private void LevelGenerate()
		{

			unitPerPixel = prefabWall.transform.lossyScale.x;
			float halfUnitPerPixel = unitPerPixel;

			float width = levels[_currentLevelIndex - 1].levelTexture.width;
			float height = levels[_currentLevelIndex - 1].levelTexture.height;


			_spawnedGroundCount = 0;
			spawnedGroundList.Clear();

			Vector3 offset = (new Vector3(width / 2, 0f, height / 2) * unitPerPixel) - new Vector3(halfUnitPerPixel, 0f, halfUnitPerPixel);

			bool bonusLevel = levels[_currentLevelIndex - 1].isBonusLevel;

			groundControllers = new GroundController[(int)width, (int)height];

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					//Get pixel color :
					Color pixelColor = levels[_currentLevelIndex - 1].levelTexture.GetPixel(x, y);

					Vector3 spawnPos = ((new Vector3(x, 0f, y) * unitPerPixel) - offset);

					if (pixelColor == colorWall)
					{
						GameObject wallObj = Spawn(prefabWall, spawnPos);
						spawnedWallList.Add(wallObj.transform);
					}
					else if (pixelColor == colorGround)
					{
						GameObject groundObj = Spawn(prefabGround, spawnPos);
						var groundController = groundObj.GetComponent<GroundController>();
						spawnedGroundList.Add(groundObj.transform);
						_spawnedGroundCount++;

						if (bonusLevel)
						{
							GameObject goldObj = Spawn(prefabGold, spawnPos);
							//goldSpawnlist.Add(goldObj);
						}

						groundControllers[x, y] = groundController;
						groundController.Initialize(x, y);
					}
				}

			}

			var spawnGroundController = ReturnSpawnGroundController();
			_ballController.AssignSpawnPosition(spawnGroundController);
			PaintTargetGround(spawnGroundController);
		}

		private GameObject Spawn(GameObject prefab, Vector3 position)
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

			}

			return targetGroundController;
		}

		public void PaintTargetGround(GroundController groundController)
		{
			if (groundController.IsPainted)
			{
				return;
			}
			groundController.PaintGround(levelColor);
			_isPaintGroundController++;
			CheckLevelEnd();
			if (levels[_currentLevelIndex - 1].isBonusLevel)
			{
				DestroyGoldOnPaintedGround(groundController);
			}
		}

		private void DestroyGoldOnPaintedGround(GroundController groundController)
		{
			if (groundController.IsPainted)
			{
				GameManager.Instance.IncreaseGoldScore(1);
				//Destroy(gold);
			}
		}

		private void CheckLevelEnd()
		{
			if (_isPaintGroundController >= _spawnedGroundCount)
			{
				Debug.Log("Sonraki level");
				GameManager.Instance.GameEnd(true);
			}
		}

		private Color GetRandomColor()
		{
			Color[] randomColors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta, Color.cyan, Color.grey };
			int randomIndex = Random.Range(0, randomColors.Length);
			return randomColors[randomIndex];
		}

		public Vector3 GetBottomLeftWallPosition()
		{
			return bottomLeftWall.position;
		}

		public Vector3 GetBottomRightWallPosition()
		{
			return bottomRightWall.position;
		}

		public int GetSpawnedGroundCount()
		{
			return _spawnedGroundCount;
		}

		private void ClearLevel()
		{
			foreach (Transform child in levelContainer.transform)
			{
				Destroy(child.gameObject);
			}
			spawnedGroundList.Clear();
			spawnedWallList.Clear();
		}

		public Level GetLevelData()
		{
			return _currentLevelData;
		}
	}
}

