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

		private void Awake()
		{
			position = transform.position;
			isPainted = false;
		}
	}

}
