using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RollerSplatClone.Controllers;
using DG.Tweening;

namespace RollerSplatClone.Managers
{
	public enum GameState
	{
		Menu = 0,
		Start = 1,
		Playing = 2,
		Reset = 3,
		End = 4,
		NextLevel=5,
	}

	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance { get; private set; }
		public GameState GameState { get; set; }

		private bool _isSuccessful;
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
		public static Action<bool> OnGameEnd;
		public static Action OnGameReset;
		public static Action OnGameLevel;
		public static Action<int> OnDiamondScored;

		[SerializeField] private LevelManager levelManager;
		[SerializeField] private BallMovement ballMovement;
		[SerializeField] private UiManager uiManager;
		[SerializeField] private InputManager inputManager;
		[SerializeField] private PaintController paintController;
		private void Start()
		{
			GameInitialize();
		}

		private void GameInitialize()
		{
			uiManager.Initialize(this, ballMovement, inputManager);
			inputManager.Initialize(ballMovement);
			ballMovement.Initialize(paintController,levelManager);
			levelManager.Initialize(ballMovement);
			paintController.Initialize(ballMovement);

			ChangeState(GameState.Menu);
		}

		public void OnGameStart()
		{
			ChangeState(GameState.Start);
		}

		public  void ResetGame()
		{
			ChangeState(GameState.Reset);
		}

		public void NextLevel()
		{
			ChangeState(GameState.NextLevel);
		}

		public void GameEnd(bool isSuccessful)
		{
			_isSuccessful = isSuccessful;
			ChangeState(GameState.End);
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
					OnGameReset?.Invoke();
					break;
				case GameState.End:
					if (_isSuccessful)
					{
						OnGameEnd?.Invoke(true);
						DOVirtual.DelayedCall(1.5f, () =>
						{
							ResetGame();
						});

						NextLevel();
					}
					break;
				case GameState.NextLevel:
					OnGameLevel?.Invoke();
					BallPrefsManager.CurrentLevel++;
					break;
				default:
					break;
			}
		}
	}
}

