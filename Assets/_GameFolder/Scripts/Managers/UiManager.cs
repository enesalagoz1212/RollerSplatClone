using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RollerSplatClone.Canvases;
using RollerSplatClone.Controllers;

namespace RollerSplatClone.Managers
{
	public class UiManager : MonoBehaviour
	{
		public static UiManager Instance { get; private set; }

		[SerializeField] private MenuCanvas menuCanvas;
		[SerializeField] private GameCanvas gameCanvas;
		[SerializeField] private InputCanvas inputCanvas;

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

		public void Initialize(GameManager gameManager, BallMovement ballMovement, InputManager inputManager)
		{
			menuCanvas.Initialize(gameManager);
			gameCanvas.Initialize();
			inputCanvas.Initialize(inputManager);
		}

	}

}
