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

		public EndCanvas EndCanvas => endCanvas;

		[SerializeField] private MenuCanvas menuCanvas;
		[SerializeField] private GameCanvas gameCanvas;
		[SerializeField] private InputCanvas inputCanvas;
		[SerializeField] private EndCanvas endCanvas;


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
			endCanvas.Initialize();
		}

	}

}
