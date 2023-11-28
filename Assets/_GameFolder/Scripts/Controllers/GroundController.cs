using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RollerSplatClone.Controllers
{
    public class GroundController : MonoBehaviour
    {
        public MeshRenderer meshRenderer;
        public Vector3 position;
        public bool isPainted;

		public int xIndex;
		public int yIndex;

		private void Awake()
		{
			position = transform.position;
			isPainted = false;
		}

		public void Initialize(int x, int y)
		{
			xIndex = x;
			yIndex = y;
		}
	}

}
