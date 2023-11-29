using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RollerSplatClone.Controllers
{
    public class GroundController : MonoBehaviour
    {
        public MeshRenderer meshRenderer;
        public Vector3 position;
        private bool _isPainted;
		public Color paintedColor;

		public int xIndex;
		public int yIndex;

		private void Awake()
		{
			position = transform.position;
			_isPainted = false;
		}

		public void Initialize(int x, int y)
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
				paintedColor = color;
			}
		}


	}
}
