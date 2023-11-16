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
		public LevelScriptableObject[] levels;
		private LevelScriptableObject _currentLevelData;

		public GameObject levelContainer;
		private GameObject _currentLevel;
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
		}

		private void OnEnable()
		{
			GameManager.OnMenuOpen += OnGameMenu;
		}

		private void OnDisable()
		{
			GameManager.OnMenuOpen -= OnGameMenu;
			
		}

		private void OnGameMenu()
		{
			int currentLevelData = BallPrefsManager.CurrentLevel;
			int moodCurrentLevel = currentLevelData % levels.Length;
			if (moodCurrentLevel == 0)
			{
				moodCurrentLevel = levels.Length;
			}
			_currentLevelData = levels[moodCurrentLevel - 1];

			CreateNextLevel();
			levelContainer.gameObject.SetActive(true);

		
		}

	
		public LevelScriptableObject GetLevelData()
		{
			return _currentLevelData;
		}

		private void CreateNextLevel()
		{
			if (_currentLevel!=null)
			{
				Destroy(_currentLevel);
			}

			int levelIndex = BallPrefsManager.CurrentLevel;
			int mood = levelIndex % levels.Length;

			if (mood == 0)
			{
				mood = levels.Length;
			}

			LevelScriptableObject currentLevelData = levels[mood - 1];
			GameObject nextLevelPrefab = currentLevelData.levelPrefab;
			_currentLevel = Instantiate(nextLevelPrefab, levelContainer.transform);
		}

		
	}
}

