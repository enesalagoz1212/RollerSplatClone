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
				if (hitDistance > 1f)
				{
					Vector3 targetPoint = hit.point - direction * 0.5f;
					transform.DOMove(targetPoint, moveDuration).SetEase(move);
				}
				else
				{
					_canMove = true;
				}
			}
		}


		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Ground"))
			{

				Renderer groundRenderer = other.GetComponent<Renderer>();

				if (groundRenderer != null)
				{
					Color groundColor = groundRenderer.material.color;
					Color ballColor = _paintController._ballRenderer.material.color;

					if (groundColor != ballColor)
					{
						if (!touchedGrounds.Contains(other.gameObject))
						{
							touchedGrounds.Add(other.gameObject);
							//Debug.Log($"Total Grounds: {touchedGrounds.Count}");

							DOVirtual.DelayedCall(0.1f, () =>
							{
								groundRenderer.material.color = ballColor;
							});

							int spawnedGroundCount = _levelManager.GetSpawnedGroundCount();

							if (touchedGrounds.Count >= spawnedGroundCount)
							{
								//Debug.Log("Level basarili");
								GameManager.Instance.GameEnd(true);
							}
						}

					}
				}
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

