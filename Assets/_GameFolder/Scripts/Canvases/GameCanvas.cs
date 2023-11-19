using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RollerSplatClone.Managers;
using UnityEngine.UI;
using TMPro;

namespace RollerSplatClone.Canvases
{
    public class GameCanvas : MonoBehaviour
    {

        public GameObject gamePanel;
        public TextMeshProUGUI gameLevelText;
		public void Initialize()
		{
			
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

		private void OnGameMenu()
		{
			gamePanel.gameObject.SetActive(true);
			UpdateGameLevelText();
		}

		private void OnGameEnd(bool isSuccessful)
		{
			if (isSuccessful)
			{
				gamePanel.gameObject.SetActive(false);
			}
		}
		private void UpdateGameLevelText()
		{
			var gameLevel = BallPrefsManager.CurrentLevel;
			gameLevelText.text = "LEVEL " + gameLevel.ToString();
		}
	}
}

