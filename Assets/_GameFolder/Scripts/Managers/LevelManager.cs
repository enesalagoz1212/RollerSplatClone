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

		private int spawnedGroundCount;
		private int _currentLevelIndex;

		private List<Transform> spawnedGroundList = new List<Transform>();

		private List<Transform> spawnedWallList = new List<Transform>();
		private Transform bottomLeftWall;
		private Transform bottomRightWall;

		private GroundController[,] groundControllers;

		public void Initialize(BallMovement ballMovement)
		{
			_ballMovement = ballMovement;
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
		}

		private void OnDisable()
		{
			GameManager.OnMenuOpen -= OnGameMenu;
			GameManager.OnGameEnd -= OnGameEnd;
		}


		private void Generate()
		{
			unitPerPixel = prefabWall.transform.lossyScale.x;
			float halfUnitPerPixel = unitPerPixel;

			float width = levels[_currentLevelIndex - 1].levelTexture.width;
			float height = levels[_currentLevelIndex - 1].levelTexture.height;


			spawnedGroundCount = 0;
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
						spawnedGroundCount++;

						//if (bonusLevel)
						//{
						//    GameObject goldObj = Spawn(prefabGold, spawnPos);
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
					}
					break;

				case Direction.South: // Y--
					for (int y = yIndex - 1; y < groundControllers.GetLength(1); y--)
					{
						if (groundControllers[xIndex, y] == null)
						{
							break;
						}
						targetGroundController = groundControllers[xIndex, y];
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
					}
					break;

				case Direction.West: // X--
					for (int x = xIndex - 1; x < groundControllers.GetLength(0); x--)
					{
						if (groundControllers[x, yIndex]==null)
						{
							break;
						}
						targetGroundController = groundControllers[x, yIndex];
					}
					break;
			}


			return targetGroundController;
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
			return spawnedGroundCount;
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

		private void ClearLevel()
		{
			foreach (Transform child in levelContainer.transform)
			{
				Destroy(child.gameObject);
			}
		}


		private void OnGameMenu()
		{

		}

		public Level GetLevelData()
		{
			return _currentLevelData;
		}


	}
}

