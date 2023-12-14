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

	public class BallController : MonoBehaviour
	{
		public PlayerState PlayerState { get; set; }

		private LevelManager _levelManager;
		private GroundController _groundController;

		private bool _canMove;
		public Renderer ballRenderer;
		public LayerMask wallsLayer;
		public float moveDuration;
		public Ease move;

		private float jumpDelay = 10f;
		private float lastInputTime;
		private bool isJumping = false;
		private Tween jumpTween;
		public void Initialize(LevelManager levelManager)
		{
			_levelManager = levelManager;
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

		public void AssignSpawnPosition(GroundController groundController)
		{
			_groundController = groundController;
			transform.position = _groundController.transform.position;
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
				if (isJumping)
				{
					jumpTween.Kill();
					isJumping = false;
				}
				_canMove = false;
				var targetPosition = targetGroundController.transform.position;
				transform.DOMove(targetPosition, moveDuration).SetEase(move).OnComplete(() =>
				{
					_groundController = targetGroundController;
					_canMove = true;
					lastInputTime = Time.time;
				});
			}
			else
			{
				Debug.Log($"Can not move!");
			}
		}

		private void Update()
		{
			if (GameManager.Instance.GameState == GameState.Playing)
			{

				JumpAnimation();
				if (Input.GetMouseButtonDown(0) && isJumping)
				{
					KillJumpAnimation();
					ReturnToOriginalPosition();
					lastInputTime = Time.time;
				}
			}
		}

		private void ReturnToOriginalPosition()
		{
			transform.DOMove(_groundController.transform.position, moveDuration).SetEase(move);
		}

		private void JumpAnimation()
		{
			if (Time.time - lastInputTime > jumpDelay && !isJumping)
			{
				var jumpHeight = 1.5f;
				isJumping = true;

				jumpTween = transform.DOJump(transform.position, jumpHeight, 1, 0.7f).SetLoops(-1, LoopType.Restart);
			}
		}

		public void KillJumpAnimation()
		{
			if (jumpTween != null && jumpTween.IsActive())
			{
				isJumping = false;
				jumpTween.Kill();
			}
		}

		public void ColorTheBall(Color color)
		{
			ballRenderer.material.color = color;
		}
	}
}

