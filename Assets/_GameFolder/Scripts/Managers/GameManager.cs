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
		public static Action<int> OnGoldScored;

		[SerializeField] private LevelManager levelManager;
		[SerializeField] private BallMovement ballMovement;
		[SerializeField] private UiManager uiManager;
		[SerializeField] private InputManager inputManager;
		[SerializeField] private PaintController paintController;
		[SerializeField] private CameraController cameraController;

		public int goldScore;
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
			cameraController.Initialize(levelManager);

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
						BallPrefsManager.CurrentLevel++;

						DOVirtual.DelayedCall(2f, () =>
						{
							ResetGame();
							ChangeState(GameState.Menu);
						});				
					}
					break;
				default:
					break;
			}
		}

		public void IncreaseGoldScore(int score)
		{
			BallPrefsManager.GoldScore += score;
			OnGoldScored?.Invoke(BallPrefsManager.GoldScore);
		}
	}
}

