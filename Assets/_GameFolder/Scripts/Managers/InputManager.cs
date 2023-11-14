using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RollerSplatClone.Controllers;

namespace RollerSplatClone.Managers
{
	public class InputManager : MonoBehaviour
	{
		public static InputManager Instance { get; private set; }
		private BallMovement _ballMovement;

		public bool isInputEnabled { get; private set; } = true;

		public float horizontalSpeed;

		private bool _isFirstDraging = true;
		private bool _isDragging;
		private Vector3 _firstTouchPosition;
		private Vector3 _lastTouchPosition;

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

		public void Initialize(BallMovement ballMovement)
		{
			_ballMovement = ballMovement;
		}

		public void OnScreenTouch(PointerEventData eventData)
		{
			if (!isInputEnabled)
			{
				return;
			}
			_isDragging = true;

			_firstTouchPosition = Input.mousePosition;
			_ballMovement.ChangeState(PlayerState.None);
			_isFirstDraging = true;
		}

		public void OnScreenDrag(PointerEventData eventData)
		{
			if (!isInputEnabled || !_isDragging)
			{
				return;
			}

			if (GameManager.Instance.GameState != GameState.Playing)
			{
				return;
			}

			_lastTouchPosition= Input.mousePosition;
			

			float touchDifferenceX = GetTouchDifferenceX();
			float touchDifferenceY = GetTouchDifferenceY();
			if (_isFirstDraging)
			{
				if (Mathf.Abs(touchDifferenceX) > Mathf.Abs(touchDifferenceY))  // x > y 
				{
					if (touchDifferenceX > 0)
					{
						_ballMovement.ChangeState(PlayerState.Right);
					}
					else
					{
						_ballMovement.ChangeState(PlayerState.Left);
					}
				}
				else    // y > x
				{
					if (touchDifferenceY > 0)
					{
						_ballMovement.ChangeState(PlayerState.Forward);
					}
					else
					{
						_ballMovement.ChangeState(PlayerState.Back);
					}
				}

				_isFirstDraging = false;
			}			
		}

		public void OnScreenUp(PointerEventData eventData)
		{
			_isDragging = false;
			Debug.Log(_lastTouchPosition - _firstTouchPosition);
		}

		public float GetTouchDifferenceX()
		{
			return _lastTouchPosition.x - _firstTouchPosition.x;
		}

		public float GetTouchDifferenceY()
		{
			return _lastTouchPosition.y - _firstTouchPosition.y;
		}
	}

}
