using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RollerSplatClone.Controllers;

namespace RollerSplatClone.Managers
{
	public enum GameState
	{
		Menu = 0,
		Start = 1,
		Playing = 2,
		Reset = 3,
		End = 4,
	}

	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance { get; private set; }
		public GameState GameState { get; set; }

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

		public static Action OnMenuOpen;
		public static Action OnGameStarted;
		public static Action OnGamePlaying;
		public static Action<bool> OnGameEnd;
		public static Action OnGameReset;
		public static Action OnGameLevel;
		public static Action<int> OnDiamondScored;

		[SerializeField] private LevelManager levelManager;
		[SerializeField] private BallMovement ballMovement;
		[SerializeField] private UiManager uiManager;
		[SerializeField] private InputManager inputManager;
		private void Start()
		{
			GameInitialize();
		}

		private void GameInitialize()
		{
			uiManager.Initialize(this, ballMovement, inputManager);
			inputManager.Initialize(ballMovement);
			ballMovement.Initialize();
			levelManager.Initialize(ballMovement);

			ChangeState(GameState.Menu);
		}

		public void OnGameStart()
		{
			ChangeState(GameState.Start);
		}

		public void ChangeState(GameState gameState)
		{
			GameState = gameState;
			Debug.Log($"Game State: {gameState}");

			switch (GameState)
			{
				case GameState.Menu:
					OnMenuOpen?.Invoke();
					break;
				case GameState.Start:
					OnGameStarted?.Invoke();
					ChangeState(GameState.Playing);
					break;
				case GameState.Playing:
					break;
				case GameState.Reset:
					break;
				case GameState.End:
					break;
				default:
					break;
			}
		}
	}
}

