using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RollerSplatClone.Managers;
using DG.Tweening;

namespace RollerSplatClone.Canvases
{
    public class EndCanvas : MonoBehaviour
    {
        public GameObject endPanel;
        public TextMeshProUGUI endLevelText;
		public Image endImage;

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
			endPanel.gameObject.SetActive(false);
		}

		private void OnGameEnd(bool isSuccessful)
		{
			if (isSuccessful)
			{
				DOVirtual.DelayedCall(0.5f, () =>
				{
					endPanel.gameObject.SetActive(true);
					UpdateEndLevelText();
				});
				
			}
		}


		private void UpdateEndLevelText()
		{
			var finishedLevel = BallPrefsManager.CurrentLevel;
			endLevelText.text = "LEVEL " + finishedLevel.ToString();
		}

		public void UpdateEndImageColor(Color color)
		{
			endImage.color = color;
		}
	}

}
