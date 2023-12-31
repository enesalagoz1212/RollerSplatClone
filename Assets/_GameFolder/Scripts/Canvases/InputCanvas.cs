using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RollerSplatClone.Managers;

namespace RollerSplatClone.Canvases
{
    public class InputCanvas : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
		private InputManager _inputManager;
		private bool _inputEnabled = true;

		public void Initialize(InputManager inputManager)
		{
			_inputManager = inputManager;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (_inputEnabled)
			{
                _inputManager.OnScreenTouch(eventData);
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
            if (_inputEnabled)
			{
                _inputManager.OnScreenDrag(eventData);
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
            _inputManager.OnScreenUp(eventData);
		}

		public void DisableInput()
		{
			_inputEnabled = false;
		}

		public void EnableInput()
		{
			_inputEnabled = true;
		}
	}

}
