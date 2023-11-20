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
		[SerializeField] private Texture2D _levelTexture;

		[Header("Tiles Prefabs")]
		[SerializeField] private GameObject prefabWall;
		[SerializeField] private GameObject prefabGround;

		private Color colorWall = Color.white;
		private Color colorGround = Color.black;

		private float unitPerPixel;
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

			Generate();
		}

		private void OnEnable()
		{
			GameManager.OnMenuOpen += OnGameMenu;
		}

		private void OnDisable()
		{
			GameManager.OnMenuOpen -= OnGameMenu;			
		}

		
		private void Generate()
		{
			unitPerPixel = prefabWall.transform.lossyScale.x;
			float halfUnitPerPixel = unitPerPixel;

			float width = _levelTexture.width;
			float height = _levelTexture.height;

			Vector3 offset = (new Vector3(width/2 , 0f, height/2 ) * unitPerPixel)
							 - new Vector3(halfUnitPerPixel, 0f, halfUnitPerPixel);

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					//Get pixel color :
					Color pixelColor = _levelTexture.GetPixel(x, y);

					Vector3 spawnPos = ((new Vector3(x, 0f, y) * unitPerPixel) - offset);

					if (pixelColor == colorWall)
					{
						Spawn(prefabWall, spawnPos);
					}
					else if (pixelColor == colorGround)
					{
						Spawn(prefabGround, spawnPos);
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

