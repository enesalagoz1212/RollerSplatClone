using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RollerSplatClone.Controllers;
using DG.Tweening;

namespace RollerSplatClone.Managers
{
	public class InputManager : MonoBehaviour
	{
		public static InputManager Instance { get; private set; }
		private BallController _ballController;

		public bool isInputEnabled { get; private set; } = true;

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

		public void Initialize(BallController ballController)
		{
			_ballController = ballController;
		}

		public void OnScreenTouch(PointerEventData eventData)
		{
			if (!isInputEnabled)
			{
				return;
			}
			if (!_isDragging)
			{
				_isDragging = true;
				_firstTouchPosition = Input.mousePosition;
				_isFirstDraging = true;
			}
			_ballController.OnScreenTap();
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

            _lastTouchPosition = Input.mousePosition;

			float touchDifferenceX = GetTouchDifferenceX();
			float touchDifferenceY = GetTouchDifferenceY();
			if (_isFirstDraging)
			{
				if (Mathf.Abs(touchDifferenceX) > Mathf.Abs(touchDifferenceY))  // x > y 
				{
                    if (touchDifferenceX > 0)
					{
						_ballController.OnScreenDrag(Direction.East);
                    }
					else
					{
						_ballController.OnScreenDrag(Direction.West);
                    }
				}
				else    // y > x
				{
					if (touchDifferenceY > 0)
					{
						_ballController.OnScreenDrag(Direction.North);
                    }
					else
					{
						_ballController.OnScreenDrag(Direction.South);
                    }
				}

				_isFirstDraging = false;
			}
        }

        public void OnScreenUp(PointerEventData eventData)
		{
			DOVirtual.DelayedCall(0.3f, () =>
			{
				_isDragging = false;
			});
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
