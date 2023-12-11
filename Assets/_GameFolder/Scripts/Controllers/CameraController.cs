using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RollerSplatClone.Managers;

namespace RollerSplatClone.Controllers
{
    public class CameraController : MonoBehaviour
    {
        private LevelManager _levelManager;

        private int _levelIndex;
        public void Initialize(LevelManager levelManager)
		{
            _levelManager = levelManager;
        }

		private void OnEnable()
		{
            GameManager.OnMenuOpen += OnGameMenu;
            GameManager.OnGameEnd += OnGameEnd;
		}

		private void OnDisable()
		{
            GameManager.OnMenuOpen -= OnGameMenu;
            GameManager.OnGameEnd -= OnGameEnd;
			
		}
		private void Awake()
		{
          
        
		}

        private void OnGameMenu()
		{
            SetCameraSettings();
		}

        private void OnGameEnd(bool isSuccessful)
		{
			if (isSuccessful)
			{
                _levelIndex++;
			}
			
		}
		public void SetCameraSettings()
        {
            float width = _levelManager.levels[BallPrefsManager.CurrentLevel-1].levelTexture.width;
            Vector3 cameraPosition = CalculateCameraPosition(width);
            transform.position = cameraPosition;
        }

        private Vector3 CalculateCameraPosition(float width)
        {
            Vector3 cameraPosition = Vector3.zero;

            if (width == 7)
            {
                cameraPosition = new Vector3(0.5f, 13.16f, -5.2f);
            }
            else if (width == 9)
            {
                cameraPosition = new Vector3(0.5f, 16.6f, -7.2f);
            }
            else if (width == 10)
            {
                cameraPosition = new Vector3(0.5f, 18.5f, -8.27f);
            }
            else if (width == 11)
            {
                cameraPosition = new Vector3(0.5f, 20.1f, -9.21f);
            }

            return cameraPosition;
        }
    }

}
