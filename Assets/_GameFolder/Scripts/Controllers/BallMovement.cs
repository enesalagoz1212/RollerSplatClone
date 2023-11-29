using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using RollerSplatClone.Managers;
using TMPro;

namespace RollerSplatClone.Controllers
{
	public enum PlayerState
	{
		Idle = 0,
		Moving = 1,
		End = 2,
	}

	public enum Direction
	{
		North = 0,
		South = 1,
		East = 2,
		West = 3,
	}

	public class BallMovement : MonoBehaviour
	{
		public PlayerState PlayerState { get; set; }

		private PaintController _paintController;
		private LevelManager _levelManager;
		private GroundController _groundController;

		private bool _canMove;

		//private GameObject _ballInstantiated;
		//public GameObject ballPrefab;
		//public Transform ballMovementTransform;
		public LayerMask wallsLayer;
		public float moveDuration;
		public Ease move;

		public void Initialize(PaintController paintController, LevelManager levelManager)
		{
			_paintController = paintController;
			_levelManager = levelManager;
		}

		private void OnEnable()
		{
			GameManager.OnMenuOpen += OnGameMenu;
			GameManager.OnGameStarted += OnGameStart;
			GameManager.OnGameReset += OnGameReset;
		}

		private void OnDisable()
		{
			GameManager.OnMenuOpen -= OnGameMenu;
			GameManager.OnGameStarted -= OnGameStart;
			GameManager.OnGameReset -= OnGameReset;
		}

		private void OnGameMenu()
		{
			//OnBallInstantiate();

		}


		private void OnGameStart()
		{
			_canMove = true;
		}

		private void OnGameReset()
		{

		}

		public void AssignSpawnPosition(GroundController groundController)
		{
			_groundController = groundController;
			transform.position = _groundController.position;
		}

		public void OnScreenDrag(Direction direction)
		{
			if (!_canMove)
			{
				return;
			}
			var targetGroundController = _levelManager.ReturnDirectionGroundController(direction, _groundController);

			if (targetGroundController != null)
			{
				_canMove = false;
				var targetPosition = targetGroundController.transform.position;
				DOVirtual.DelayedCall(0.2f, () =>
				{
					PaintGroundControllersInDirection();
				});
				transform.DOMove(targetPosition, moveDuration).SetEase(move).OnComplete(() =>
				{
					_groundController = targetGroundController;
					_canMove = true;

				});

			}
			else
			{
				Debug.Log($"Can not move!");
			}
		}

		public void PaintGroundControllersInDirection()
		{
			foreach (var groundController in _levelManager.GetCurrentDirectionGroundControllers())
			{
				groundController.PaintGround(_paintController.GetBallColor());
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

