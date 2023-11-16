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
			Color ballColor = GetRandomColor();
			ColorTheBall(ballColor);
		}


		private Color GetRandomColor()
		{
			int randomIndex = Random.Range(0, randomColors.Length);
			return randomColors[randomIndex];
		}

		private void ColorTheBall(Color color)
		{
			_ballRenderer.material.color = color;
		}

	

	}

}
