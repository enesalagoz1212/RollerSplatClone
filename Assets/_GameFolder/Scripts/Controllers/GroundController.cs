using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RollerSplatClone.Managers;
using RollerSplatClone.Controllers;
using RollerSplatClone.Pooling;

namespace RollerSplatClone.Controllers
{
	public class GroundController : MonoBehaviour
	{
		public MeshRenderer meshRenderer;

		public bool _isPainted;
		
		public int xIndex;
		public int yIndex;
		private GameObject _goldObject;
		
		private Color _defaultColor = Color.black;
		
		private void Awake()
		{
			_isPainted = false;
		}

		private void OnEnable()
		{
			
		}
		private void OnDisable()
		{
			meshRenderer.material.color = _defaultColor;
			_isPainted = false;
		}
		
		public void Init(int x, int y, GameObject goldObject)
		{
			xIndex = x;
			yIndex = y;
			_goldObject = goldObject;
		}

		public bool IsPainted
		{
			get { return _isPainted; }
		}

		public void PaintGround(Color color)
		{
			if (!_isPainted)
			{
				meshRenderer.material.color = color;
				_isPainted = true;

				if (_goldObject != null)
				{
					_goldObject.SetActive(false);
					GameManager.Instance.IncreaseGoldScore(1);
				}
			}
		}
	}
}
