using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using RollerSplatClone.Managers;
using Vector3 = UnityEngine.Vector3;

namespace RollerSplatClone.Controllers
{
    public class CameraController : MonoBehaviour
    {
	    public Vector3 defaultCameraPos;
	    public float yGap;
	    public float zGap;
	    
        private LevelManager _levelManager;

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
                
			}
		}
        
		public void SetCameraSettings()
		{
			float width = _levelManager.ReturnLevelWidth();
            Vector3 cameraPosition = CalculateCameraPosition(width);
            transform.position = cameraPosition;
        }

        private Vector3 CalculateCameraPosition(float width)
        {
	        Vector3 cameraPosition = defaultCameraPos + Vector3.up * (width * yGap) + Vector3.back * (width * zGap);

            return cameraPosition;
        }
    }

}
