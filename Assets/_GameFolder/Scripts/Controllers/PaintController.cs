using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RollerSplatClone.Managers;

namespace RollerSplatClone.Controllers
{
	public class PaintController : MonoBehaviour
	{
		//private BallController _ballController;

		//private Color[] randomColors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta ,Color.cyan,Color.grey};
		//public Renderer _ballRenderer;

		//public void Initialize(BallController ballController)
		//{
		//	_ballController = ballController;
		//	Color ballColor = GetRandomColor();
		//	ColorTheBall(ballColor);
		//	UiManager.Instance.EndCanvas.UpdateEndImageColor(ballColor);
		//}

		//private void OnEnable()
		//{
		//	GameManager.OnMenuOpen += OnGameMenu;
		//	GameManager.OnGameReset += OnGameReset;
		//}

		//private void OnDisable()
		//{
		//	GameManager.OnMenuOpen -= OnGameMenu;
		//	GameManager.OnGameReset -= OnGameReset;
		//}

		//private void OnGameMenu()
		//{
		
		//}

		//private void OnGameReset()
		//{
		//	Color ballColor = GetRandomColor();
		//	ColorTheBall(ballColor);
		//}

		//public Color GetRandomColor()
		//{
		//	int randomIndex = Random.Range(0, randomColors.Length);
		//	return randomColors[randomIndex];
		//}

		//public void ColorTheBall(Color color)
		//{
		//	_ballRenderer.material.color = color;
		//}

		//public Color GetBallColor()
		//{
		//	return _ballRenderer.material.color;
		//}
	}
}
