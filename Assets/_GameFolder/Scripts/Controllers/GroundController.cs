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
		private PoolController _poolController;
		public MeshRenderer meshRenderer;

		public bool _isPainted;
		public int xIndex;
		public int yIndex;

		private Color defaultColor = Color.black;
		private void Awake()
		{
			_isPainted = false;
		}

		public void Initialize(PoolController poolController)
		{
			_poolController = poolController;
		}
		private void OnEnable()
		{
			
		}
		private void OnDisable()
		{

			meshRenderer.material.color = defaultColor;
			_isPainted = false;
		}

		
		public void Init(int x, int y)
		{
			xIndex = x;
			yIndex = y;
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
			}
		}
	}
}
