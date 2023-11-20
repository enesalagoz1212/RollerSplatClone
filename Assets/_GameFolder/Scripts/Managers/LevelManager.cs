using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RollerSplatClone.Controllers;
using RollerSplatClone.ScritableObjects;

namespace RollerSplatClone.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }
		private BallMovement _ballMovement;
		//public LevelScriptableObject[] levels;
		//private LevelScriptableObject _currentLevelData;

		public GameObject levelContainer;
		//private GameObject _currentLevel;

		[Header("Level texture")]
		[SerializeField] private Texture2D[] _levelTextures;

		[Header("Tiles Prefabs")]
		[SerializeField] private GameObject prefabWall;
		[SerializeField] private GameObject prefabGround;

		private Color colorWall = Color.white;
		private Color colorGround = Color.black;

		private float unitPerPixel;

		private int spawnedGroundCount;
		private int _currentLevelIndex;
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
			_currentLevelIndex = 0;
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

			float width = _levelTextures[_currentLevelIndex].width;
			float height = _levelTextures[_currentLevelIndex].height;


			spawnedGroundCount = 0;

			Vector3 offset = (new Vector3(width/2 , 0f, height/2 ) * unitPerPixel)
							 - new Vector3(halfUnitPerPixel, 0f, halfUnitPerPixel);

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					//Get pixel color :
					Color pixelColor = _levelTextures[_currentLevelIndex].GetPixel(x, y);

					Vector3 spawnPos = ((new Vector3(x, 0f, y) * unitPerPixel) - offset);

					if (pixelColor == colorWall)
					{
						Spawn(prefabWall, spawnPos);
					}
					else if (pixelColor == colorGround)
					{
						Spawn(prefabGround, spawnPos);
						spawnedGroundCount++;
					}
				}
			}
		}

		private void Spawn(GameObject prefab, Vector3 position)
		{
			//fix Y position:
			position.y = prefab.transform.position.y;

			GameObject obj = Instantiate(prefab, position, Quaternion.identity);

			obj.transform.parent = levelContainer.transform;

		}

		public int GetSpawnedGroundCount()
		{
			return spawnedGroundCount;
		}

		private void OnGameEnd(bool isSuccessful)
		{
			if (isSuccessful)
			{
				ClearLevel();
				_currentLevelIndex++;
				Generate();
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
		//	int currentLevelData = BallPrefsManager.CurrentLevel;
		//	_currentLevelData = levels[currentLevelData - 1];

		//	CreateNextLevel();
		//	levelContainer.gameObject.SetActive(true);		
		}

		//public LevelScriptableObject GetLevelData()
		//{
		//	return _currentLevelData;
		//}

		//private void CreateNextLevel()
		//{
		//	if (_currentLevel!=null)
		//	{
		//		Destroy(_currentLevel);
		//	}

		//	int levelIndex = BallPrefsManager.CurrentLevel;

		//	_currentLevelData = levels[levelIndex - 1];
		//	GameObject nextLevelPrefab = _currentLevelData.levelPrefab;
		//	_currentLevel = Instantiate(nextLevelPrefab, levelContainer.transform);
		//}

		
	}
}

