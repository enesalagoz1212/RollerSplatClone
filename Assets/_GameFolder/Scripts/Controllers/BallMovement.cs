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

		private bool _canMove;
		private GameObject _ballInstantiated;

		//public GameObject ballPrefab;
		//public Transform ballMovementTransform;
		public LayerMask wallsLayer;
		public float moveDuration;
		public Ease move;
		public void Initialize()
		{

		}


		private void OnEnable()
		{
			GameManager.OnMenuOpen += OnGameMenu;
			GameManager.OnGameStarted += OnGameStart;
		}

		private void OnDisable()
		{
			GameManager.OnMenuOpen -= OnGameMenu;
			GameManager.OnGameStarted -= OnGameStart;

		}

		private void OnGameMenu()
		{
			//OnBallInstantiate();
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
				float hitDistance = hit.distance;
				if (hitDistance>1f)
				{
					transform.DOMove(hit.point, moveDuration).SetEase(move);
				}
				else
				{
					_canMove = true;
				}
			}
		}

	/*	 private void OnBallInstantiate()
		{
			if (ballPrefab!=null)
			{
				if (_ballInstantiated!=null)
				{
					Destroy(_ballInstantiated);
				}

				_ballInstantiated = Instantiate(ballPrefab, new Vector3(-4f, 0.9f, -4f), Quaternion.identity);
			}
		}
	*/ // Instantiate ball 
	}
}

