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
		public BallMovement _ballMovement;
		private PaintController _paintController;
		private Level _currentLevelData;
		public Level[] levels;
		public GameObject levelContainer;


		[Header("Tiles Prefabs")]
		[SerializeField] private GameObject prefabWall;
		[SerializeField] private GameObject prefabGround;
		[SerializeField] private GameObject prefabGold;

		private Color colorWall = Color.white;
		private Color colorGround = Color.black;

		private float unitPerPixel;
		private int _isPaintGroundController = 0;

		private int _spawnedGroundCount;
		private int _currentLevelIndex;

		private List<GroundController> _currentDirectionGroundControllers = new List<GroundController>();

		private List<Transform> spawnedGroundList = new List<Transform>();
		private List<Transform> spawnedWallList = new List<Transform>();

		private Transform bottomLeftWall;
		private Transform bottomRightWall;

		private GroundController[,] groundControllers;

		public void Initialize(BallMovement ballMovement, PaintController paintController)
		{
			_ballMovement = ballMovement;
			_paintController = paintController;
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
			_currentLevelIndex = BallPrefsManager.CurrentLevel;
			Generate();
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
					Generate();

				});

			}
		}

		private void OnGameReset()
		{
			_isPaintGroundController = 0;
		}

		private void OnGameMenu()
		{
			var startGroundController = ReturnSpawnGroundController();
			_currentDirectionGroundControllers.Add(startGroundController);

			PaintCurrentDirectionGrounds(_paintController.GetBallColor());
		}

		private void Generate()
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

						//if (bonusLevel)
						//{
						//	GameObject goldObj = Spawn(prefabGold, spawnPos);
						//}

						groundControllers[x, y] = groundController;
						groundController.Initialize(x, y);
					}
				}

			}

			_ballMovement.AssignSpawnPosition(ReturnSpawnGroundController());
		
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
			_currentDirectionGroundControllers.Add(currentGroundController);
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
						_currentDirectionGroundControllers.Add(targetGroundController);
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
						_currentDirectionGroundControllers.Add(targetGroundController);
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
						_currentDirectionGroundControllers.Add(targetGroundController);
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
						_currentDirectionGroundControllers.Add(targetGroundController);
					}
					break;
			}
			DOVirtual.DelayedCall(0.2f, () =>
			{
				PaintCurrentDirectionGrounds(_paintController.GetBallColor());
			});

			return targetGroundController;
		}

		public void PaintCurrentDirectionGrounds(Color color)
		{

			foreach (var groundController in _currentDirectionGroundControllers)
			{
				if (!groundController.IsPainted)
				{
					Debug.Log("12");
					groundController.PaintGround(color);
					_isPaintGroundController++;

					if (_isPaintGroundController==TotalGroundControllers())
					{
						Debug.Log("Sonraki level");
						GameManager.Instance.GameEnd(true);
					}
				}
			}

			
		}

		private int TotalGroundControllers()
		{
			int totalGroundControllers = 0;

			for (int x = 0; x < groundControllers.GetLength(0); x++)
			{
				for (int y = 0; y < groundControllers.GetLength(1); y++)
				{
					if (groundControllers[x, y] != null)
					{
						totalGroundControllers++;
					}
				}
			}

			return totalGroundControllers;
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
		}

		public Level GetLevelData()
		{
			return _currentLevelData;
		}
	}
}

