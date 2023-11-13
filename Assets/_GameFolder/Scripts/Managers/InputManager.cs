using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RollerSplatClone.Managers
{
	public class InputManager : MonoBehaviour
	{
		public static InputManager Instance { get; private set; }
		public bool isInputEnabled { get; private set; } = true;

		public float horizontalSpeed;

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

		public void Initialize()
		{

		}

		public void OnScreenTouch(PointerEventData eventData)
		{
			if (!isInputEnabled)
			{
				return;
			}

			_firstTouchPosition = Input.mousePosition;

			float firstTouchX = _firstTouchPosition.x;
			float firstTouchY = _firstTouchPosition.y;

			_isDragging = true;
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

			float lastTouchX = _lastTouchPosition.x;
			float lastTouchY = _lastTouchPosition.y;
			

	

		}

		public void OnScreenUp(PointerEventData eventData)
		{
			_isDragging = false;
			Debug.Log(_lastTouchPosition - _firstTouchPosition);
		}
	}

}
