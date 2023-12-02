using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RollerSplatClone.Managers;

namespace RollerSplatClone.Controllers
{
	public class GroundController : MonoBehaviour
	{
		public MeshRenderer meshRenderer;
		public Vector3 position;
		public bool _isPainted;

		public int xIndex;
		public int yIndex;

		public GameObject goldPrefab;
		private void Awake()
		{
			position = transform.position;
			_isPainted = false;
		}

		private void Start()
		{
		
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

		public void DestoyGold()
		{
			Destroy(goldPrefab);
		}

		public void SpawnGold()
		{

			GameObject goldObj = Instantiate(goldPrefab, transform.position, Quaternion.identity);
			goldObj.transform.parent = transform;

		}
	}
}
