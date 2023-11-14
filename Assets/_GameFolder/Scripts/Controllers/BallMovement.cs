using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using RollerSplatClone.Managers;

namespace RollerSplatClone.Controllers
{
	public enum PlayerState
	{
		None = 0,
		Left = 1,
		Right = 2,
		Forward = 3,
		Back = 4,
	}

	public class BallMovement : MonoBehaviour
	{
		public PlayerState PlayerState { get; set; }

		public LayerMask wallsLayer;

		private bool _canMove;

		public float moveDuration;

		public Ease move;
		public void Initialize()
		{

		}


		private void OnEnable()
		{
			GameManager.OnGameStarted += OnGameStart;
		}

		private void OnDisable()
		{
			GameManager.OnGameStarted -= OnGameStart;
			
		}

		private void OnGameStart()
		{
			_canMove = true;
		}

		void Update()
		{
			
		}

		public void ChangeState(PlayerState playerState)
		{
			PlayerState = playerState;
			Debug.Log($"Player State: {playerState}");

			switch (PlayerState)
			{
				case PlayerState.None:
					_canMove = true;
					break;
				case PlayerState.Left:
					if (GameManager.Instance.GameState == GameState.Playing && _canMove == true)
					{
						MovementBall(Vector3.left);
					}
					break;
				case PlayerState.Right:
					if (GameManager.Instance.GameState == GameState.Playing && _canMove == true)
					{
						MovementBall(Vector3.right);
					}

					break;
				case PlayerState.Forward:

					if (GameManager.Instance.GameState == GameState.Playing && _canMove == true)
					{
						MovementBall(Vector3.forward);
					}
					break;
				case PlayerState.Back:
					if (GameManager.Instance.GameState == GameState.Playing && _canMove == true)
					{
						MovementBall(Vector3.back);
					}
					break;
				default:
					break;
			}

		}

		private void MovementBall(Vector3 direction)
		{
			_canMove = false;
			Ray ray = new Ray(transform.position, direction);
			RaycastHit hit;

			Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, wallsLayer.value))
			{
				transform.DOMove(hit.point, moveDuration).SetEase(move);
			}

		}
	}
}

