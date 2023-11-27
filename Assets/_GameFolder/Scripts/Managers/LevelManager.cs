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
		private BallMovement _ballMovement;
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
		private Vector3 bottomLeftGroundPosition;

		private List<Transform> spawnedWallList = new List<Transform>();
		private Transform bottomLeftWall;
		private Transform bottomRightWall;

		private GameObject[,] tilesArray;
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
			bottomLeftGroundPosition = Vector3.zero;

			Vector3 offset = (new Vector3(width / 2, 0f, height / 2) * unitPerPixel) - new Vector3(halfUnitPerPixel, 0f, halfUnitPerPixel);

			bool bonusLevel = levels[_currentLevelIndex - 1].isBonusLevel;

			tilesArray = new GameObject[(int)width, (int)height];

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

						tilesArray[x, y] = wallObj;
					}
					else if (pixelColor == colorGround)
					{
						GameObject groundObj = Spawn(prefabGround, spawnPos);
						spawnedGroundList.Add(groundObj.transform);
						spawnedGroundCount++;

						if (bottomLeftGroundPosition == Vector3.zero ||
							groundObj.transform.position.x < bottomLeftGroundPosition.x ||
							(groundObj.transform.position.x == bottomLeftGroundPosition.x &&
							 groundObj.transform.position.z < bottomLeftGroundPosition.z))
						{
							bottomLeftGroundPosition = groundObj.transform.position;

						}
						else
						{
							if (bonusLevel)
							{
								GameObject goldObj = Spawn(prefabGold, spawnPos);

							}
						}
						tilesArray[x, y] = groundObj;
					}
				}

			}
		}

		private GameObject Spawn(GameObject prefab, Vector3 position)
		{
			position.y = prefab.transform.position.y;
			GameObject obj = Instantiate(prefab, position, Quaternion.identity);
			obj.transform.parent = levelContainer.transform;


			return obj;
		}

		public bool CanMoveInDirection(Vector3 currentPosition, Direction direction)
		{
			int x = Mathf.RoundToInt(currentPosition.x);
			int z = Mathf.RoundToInt(currentPosition.z);

			switch (direction)
			{
				case Direction.North:
					z += 1;
					break;
				case Direction.South:
					z -= 1;
					break;
				case Direction.East:
					x += 1;
					break;
				case Direction.West:
					x -= 1;
					break;
			}

			if (x >= 0 && x < tilesArray.GetLength(0) && z >= 0 && z < tilesArray.GetLength(1))
			{
				Debug.Log($" x: {x}, y: {z}");
				GameObject tileObject = tilesArray[x, z];

				if (tileObject != null && tileObject.CompareTag("Ground"))
				{
					Debug.Log("Can move");
					return true;
				}
				else
				{
					Debug.Log("Hareket edemiyor -  Ground deðil");
				}
			}
			else
			{
				Debug.Log($" x: {x}, y: {z}");
				Debug.Log("hareket edemiyor- sýnýrlarýn dýsýnda!");
			}

			// Hareket edilemez
			return false;
		}

		public Vector3 GetBottomLeftGroundPosition()
		{
			return bottomLeftGroundPosition;
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

