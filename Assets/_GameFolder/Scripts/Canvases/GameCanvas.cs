using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RollerSplatClone.Managers;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace RollerSplatClone.Canvases
{
    public class GameCanvas : MonoBehaviour
    {
        public GameObject gamePanel;
		public GameObject goldPanel;
        public TextMeshProUGUI gameLevelText;
		public TextMeshProUGUI gameGoldScoreText;

		public RectTransform panelLevelTextRectTransform;
		public RectTransform panelGoldTextRectTransform;
		public void Initialize()
		{
			
		}

		private void OnEnable()
		{
			GameManager.OnMenuOpen += OnGameMenu;
			GameManager.OnGameEnd += OnGameEnd;
			GameManager.OnGoldScored += OnGoldScored;
		}

		private void OnDisable()
		{
			GameManager.OnMenuOpen -= OnGameMenu;
			GameManager.OnGameEnd -= OnGameEnd;
			GameManager.OnGoldScored -= OnGoldScored;
		}

		private void OnGameMenu()
		{
			MovePanelkLevelTextDown();
			UpdateGameLevelText();
			OnGoldScored(BallPrefsManager.GoldScore);
			MovePanelGoldTextLeft();
		}

		private void OnGameEnd(bool isSuccessful)
		{
			if (isSuccessful)
			{
				MovePanelLevelTextUp();
				MovePanelGoldTextRight();
			}
		}

		private void OnGoldScored(int score)
		{
			gameGoldScoreText.text = $"Gold: {BallPrefsManager.GoldScore.ToString()}";
		}

		private void UpdateGameLevelText()
		{
			var gameLevel = BallPrefsManager.CurrentLevel;
			gameLevelText.text = "LEVEL " + gameLevel.ToString();
		}

		private void MovePanelLevelTextUp()
		{
			if (panelLevelTextRectTransform == null)
			{
				Debug.LogError("Panel null");
				return;
			}

			Vector2 targetPosition = panelLevelTextRectTransform.anchoredPosition + new Vector2(0f, 355f);
			panelLevelTextRectTransform.DOAnchorPos(targetPosition, 1f).SetEase(Ease.OutQuad); ; 
		}

		private void MovePanelkLevelTextDown()
		{
			if (panelLevelTextRectTransform == null)
			{
				Debug.LogError("Panel null");
				return;
			}
			Vector2 targetPosition = panelLevelTextRectTransform.anchoredPosition + new Vector2(0f, -355f);
			panelLevelTextRectTransform.DOAnchorPos(targetPosition, 1f).SetEase(Ease.OutBounce);
		
		}

		private void MovePanelGoldTextRight()
		{
			if (panelGoldTextRectTransform==null)
			{
				Debug.Log("Panel gold null");
				return;
			}
			Vector3 targetPosition = panelGoldTextRectTransform.anchoredPosition + new Vector2(400f, 0f);
			panelGoldTextRectTransform.DOAnchorPos(targetPosition, 1f).SetEase(Ease.OutQuad);

		}

		private void MovePanelGoldTextLeft()
		{
			if (panelGoldTextRectTransform == null)
			{
				Debug.Log("Panel gold null");
				return;
			}
			Vector3 targetPosition = panelGoldTextRectTransform.anchoredPosition + new Vector2(-400f, 0f);
			panelGoldTextRectTransform.DOAnchorPos(targetPosition, 1f).SetEase(Ease.OutBounce);
		}
	}
}

