using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RollerSplatClone.Managers
{
	public enum GameState
	{
		Menu=0,
		Start=1,
		Playing=2,
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
		public static Action OnGameStart;
		public static Action OnGamePlaying;
		public static Action<bool> OnGameEnd;
		public static Action OnGameReset;
		public static Action OnGameLevel;
		public static Action<int> OnDiamondScored;


		private void Start()
		{
			GameInitialize();
		}

		private void GameInitialize()
		{
			ChangeState(GameState.Menu);
		}

		public void ChangeState(GameState gameState)
		{
			GameState = gameState;
			Debug.Log($"Game State: {gameState}");

			switch (GameState)
			{
				case GameState.Menu:
					break;
				case GameState.Start:
					break;
				case GameState.Playing:
					break;
				default:
					break;
			}
		}
	}
}

