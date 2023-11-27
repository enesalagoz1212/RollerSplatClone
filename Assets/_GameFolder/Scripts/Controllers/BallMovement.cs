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

	public enum Direction
	{
		North=0,
		South=1,
		East=2,
		West=3,
	}

	public class BallMovement : MonoBehaviour
	{
		public PlayerState PlayerState { get; set; }

		private PaintController _paintController;
		private LevelManager _levelManager;

		private bool _canMove;
		private GameObject _ballInstantiated;

		//public GameObject ballPrefab;
		//public Transform ballMovementTransform;
		public LayerMask wallsLayer;
		public float moveDuration;
		public Ease move;

		private List<GameObject> touchedGrounds = new List<GameObject>();
		public void Initialize(PaintController paintController, LevelManager levelManager)
		{
			_paintController = paintController;
			_levelManager = levelManager;
		}

		private void Awake()
		{
			
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
			ResetBallPosition();

			touchedGrounds.Clear();
		}

		private void ResetBallPosition()
		{
			Vector3 bottomLeftGroundPosition = _levelManager.GetBottomLeftGroundPosition();
			transform.position = new Vector3(bottomLeftGroundPosition.x, transform.position.y, bottomLeftGroundPosition.z);
		}


		public void ChangeState(PlayerState playerState)
		{
			PlayerState = playerState;
			//Debug.Log($"Player State: {playerState}");

			switch (PlayerState)
			{
				case PlayerState.None:
					_canMove = true;
					break;
				case PlayerState.Left:
					if (GameManager.Instance.GameState == GameState.Playing && _canMove == true)
					{
						MovementBall(Direction.West);
					}
					break;
				case PlayerState.Right:
					if (GameManager.Instance.GameState == GameState.Playing && _canMove == true)
					{
						MovementBall(Direction.East);
					}

					break;
				case PlayerState.Forward:

					if (GameManager.Instance.GameState == GameState.Playing && _canMove == true)
					{
						MovementBall(Direction.North);
					}
					break;
				case PlayerState.Back:
					if (GameManager.Instance.GameState == GameState.Playing && _canMove == true)
					{
						MovementBall(Direction.South);
					}
					break;
				default:
					break;
			}

		}

		private void MovementBall(Direction direction)
		{
			Vector3 targetPosition = transform.position;

			switch (direction)
			{
				case Direction.North:
					targetPosition.z += 1f;
					break;
				case Direction.South:
					targetPosition.z -= 1f;
					break;
				case Direction.East:
					targetPosition.x += 1f;
					break;
				case Direction.West:
					targetPosition.x -= 1f;
					break;
			}

			if (_levelManager.CanMoveInDirection(targetPosition, direction))
			{
				transform.DOMove(targetPosition, moveDuration).SetEase(move);
			}
			else
			{
				Debug.Log("Hareket etmek mümkün deðil");
			}
		}




		public List<GameObject> GetTouchedGrounds()
		{
			return touchedGrounds;
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

