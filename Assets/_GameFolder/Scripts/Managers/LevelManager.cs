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
			_currentLevelData = levels[currentLevelData - 1];

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

			_currentLevelData = levels[levelIndex - 1];
			GameObject nextLevelPrefab = _currentLevelData.levelPrefab;
			_currentLevel = Instantiate(nextLevelPrefab, levelContainer.transform);
		}

		
	}
}

