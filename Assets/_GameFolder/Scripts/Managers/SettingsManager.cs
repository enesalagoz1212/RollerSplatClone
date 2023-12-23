using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RollerSplatClone.Managers;

namespace RollerSplatClone.Managers
{
	[System.Serializable]
	public class GameSettings
	{
		public bool IsSoundOn;
		public bool IsVibrationOn;
		public int CurrentLevel;
	}
	public class SettingsManager : MonoBehaviour
	{
		public static SettingsManager Instance;
		private const string SettingsKey = "GameSettings";

		private GameSettings currentSettings;

		public void Initialize()
		{
			LoadSettings();
		}

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				Destroy(gameObject);
			}
			
		}

		private void OnEnable()
		{
			GameManager.OnGameEnd += OnGameEnd;
		}

		private void OnDisable()
		{
			GameManager.OnGameEnd -= OnGameEnd;
			
		}

		private void OnGameEnd(bool isSuccessful)
		{
			if (isSuccessful)
			{
				currentSettings.CurrentLevel++;
			}
		}

		private void OnApplicationQuit()
		{
			SaveSettings();
		}

		public void SaveSettings()
		{
			if (currentSettings != null)
			{
				var json = JsonUtility.ToJson(currentSettings);
				PlayerPrefs.SetString(SettingsKey, json);
			}
		}

		private void LoadSettings()
		{
			if (PlayerPrefs.HasKey(SettingsKey))
			{
				string json = PlayerPrefs.GetString(SettingsKey);
				currentSettings = JsonUtility.FromJson<GameSettings>(json);
			}
			else
			{
				currentSettings = new GameSettings
				{
					IsSoundOn = true,
					IsVibrationOn = true,
					CurrentLevel = 1
				};
			}
		}


		public int GetCurrentLevel()
		{
			if (currentSettings != null)
			{
				return currentSettings.CurrentLevel;
			}
			return 1;
		}
	}
}

