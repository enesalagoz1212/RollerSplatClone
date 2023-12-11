using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RollerSplatClone.Controllers;
using RollerSplatClone.ScritableObjects;
using DG.Tweening;
using RollerSplatClone.Pooling;

namespace RollerSplatClone.Managers
{
	public class LevelManager : MonoBehaviour
	{
		public static LevelManager Instance { get; private set; }
		private BallController _ballController;
		private PoolController _poolController;

		private Level _currentLevelData;
		public Level[] levels;
		public GameObject levelContainer;

		public Vector3 particlePositionOne;
		public Vector3 particlePositionTwo;
		public Vector3 particlePositionThree;

		private Color _colorWall = Color.white;
		private Color _colorGround = Color.black;
		private Color _levelColor;

		private float _unitPerPixel;
		private int _isPaintGroundController = 0;

		private int _spawnedGroundCount;

		private List<Transform> spawnedGroundList = new List<Transform>();

		private GroundController[,] groundControllers;
		public void Initialize(BallController ballController,PoolController poolController)
		{
			_ballController = ballController;
			_poolController = poolController;
		
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
				var newParticle1 = _poolController.GetParticle(particlePositionOne);
				var newParticle2 = _poolController.GetParticle(particlePositionTwo);
				var newParticle3 = _poolController.GetParticle(particlePositionThree);

				DOVirtual.DelayedCall(2f, () =>
				{
					ReturnObjectsToPool();
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
			int levelIndex = BallPrefsManager.CurrentLevel;
			int mood = levelIndex % levels.Length;
			if (mood == 0)
			{
				mood = levels.Length;
			}
			_currentLevelData = levels[mood - 1];
			_levelColor = GetRandomColor();
			_ballController.ColorTheBall(_levelColor);
			LevelGenerate();
		}

		
		private void LevelGenerate()
		{
			_unitPerPixel = 1f;
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
						GameObject wallObj = _poolController.GetWall(spawnPos);
					}
					else if (pixelColor == _colorGround)
					{
						GameObject groundObj = _poolController.GetGround(spawnPos);
						var groundController = groundObj.GetComponent<GroundController>();
						spawnedGroundList.Add(groundObj.transform);
						_spawnedGroundCount++;

						if (bonusLevel)
						{
							var newBonus = _poolController.GetGold(spawnPos);

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

		private void ReturnObjectsToPool()
		{
			_poolController.ReturnAllObjectsToThePool();

			spawnedGroundList.Clear();
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

