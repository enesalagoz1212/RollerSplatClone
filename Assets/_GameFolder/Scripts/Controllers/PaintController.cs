using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RollerSplatClone.Managers;

namespace RollerSplatClone.Controllers
{
	public class PaintController : MonoBehaviour
	{
		private BallMovement _ballMovement;

		private Color[] randomColors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta ,Color.cyan,Color.grey};
		public Renderer _ballRenderer;

		public void Initialize(BallMovement ballMovement)
		{
			_ballMovement = ballMovement;
			Color ballColor = GetRandomColor();
			ColorTheBall(ballColor);

			UiManager.Instance.EndCanvas.UpdateEndImageColor(ballColor);
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
			
		}

		public Color GetRandomColor()
		{
			int randomIndex = Random.Range(0, randomColors.Length);
			return randomColors[randomIndex];
		}

		public void ColorTheBall(Color color)
		{
			_ballRenderer.material.color = color;
		}

		public Color GetBallColor()
		{
			return _ballRenderer.material.color;
		}
	}
}
