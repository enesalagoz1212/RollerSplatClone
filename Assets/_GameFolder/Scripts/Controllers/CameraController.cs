using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RollerSplatClone.Managers;

namespace RollerSplatClone.Controllers
{
    public class CameraController : MonoBehaviour
    {
        private LevelManager _levelManager;

        public float cameraDistance = 10f;
        public void Initialize(LevelManager levelManager)
		{
            _levelManager = levelManager;
        }

		private void OnEnable()
		{
            GameManager.OnMenuOpen += OnGameMenu;
		}

		private void OnDisable()
		{
            GameManager.OnMenuOpen -= OnGameMenu;
			
		}
		private void Awake()
		{
          
        
		}

        private void OnGameMenu()
		{
            SetCameraSettings();
		}

		public void SetCameraSettings()
        {

        }
    }

}
