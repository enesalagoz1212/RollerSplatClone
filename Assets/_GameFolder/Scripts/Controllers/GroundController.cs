using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RollerSplatClone.Managers;

namespace RollerSplatClone.Controllers
{
	public class GroundController : MonoBehaviour
	{
		public MeshRenderer meshRenderer;
		public GameObject goldPrefab;
		private GameObject _createdGold;

		public bool _isPainted;
		public int xIndex;
		public int yIndex;

		private Color defaultColor = Color.black;
		private void Awake()
		{
			_isPainted = false;
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
				DestroyGold();
			}

		}

		private void DestroyGold()
		{
			if(_createdGold != null)
			{
				Destroy(_createdGold);
			}
		}

		public void SpawnGold()
		{
			_createdGold = Instantiate(goldPrefab, transform.position, Quaternion.identity);
			_createdGold.transform.parent = transform;
		}
	}
}
