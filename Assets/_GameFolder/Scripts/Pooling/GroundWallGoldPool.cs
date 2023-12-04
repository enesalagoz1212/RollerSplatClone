using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RollerSplatClone.Managers;
using DG.Tweening;

namespace RollerSplatClone.Pooling
{
	public class GroundWallGoldPool : MonoBehaviour
	{
		public GameObject wallPrefab;
		public GameObject groundPrefab;
		public GameObject goldPrefab;

		public GameObject pools;

		private int activeWallCount;
		private int activeGroundCount;
		private int activeGoldCount;

		public int initializeWallPoolSize;
		public int initializeGroundPoolSize;
		public int initializeGoldPoolSize;

		private Stack<GameObject> pooledWalls = new Stack<GameObject>();
		private Stack<GameObject> pooledGrounds = new Stack<GameObject>();
		private Stack<GameObject> pooledGolds = new Stack<GameObject>();

		private float _unitPerPixel;

		public float UnitPerPixel
		{
			get { return _unitPerPixel; }
		}

		private void OnEnable()
		{
			GameManager.OnGameEnd += OnGameEnd;
		}

		private void OnDisable()
		{

			GameManager.OnGameEnd -= OnGameEnd;
		}
		public void Initialize()
		{
			InitializePool();
		}
		private void InitializePool()
		{
			for (int i = 0; i < initializeWallPoolSize; i++)
			{
				GameObject wallPooled = Instantiate(wallPrefab, pools.transform);
				wallPooled.SetActive(false);
				pooledWalls.Push(wallPooled);
			}
			for (int j = 0; j < initializeGroundPoolSize; j++)
			{
				GameObject groundPooled = Instantiate(groundPrefab, pools.transform);
				groundPooled.SetActive(false);
				pooledGrounds.Push(groundPooled);
			}
			for (int k = 0; k < initializeGoldPoolSize; k++)
			{
				GameObject goldPooled = Instantiate(goldPrefab, pools.transform);
				goldPooled.SetActive(false);
				pooledGolds.Push(goldPooled);
			}
		}


		public GameObject GetWall(Vector3 wallPosition)
		{
			if (pooledWalls.Count > 0)
			{
				GameObject wall = pooledWalls.Pop();
				wall.transform.position = wallPosition;
				wall.SetActive(true);
				activeWallCount++;
				return wall;
			}
			else
			{
				GameObject newWall = Instantiate(wallPrefab, wallPosition, Quaternion.identity);
				return newWall;
			}
		}

		public GameObject GetGround(Vector3 groundPosition)
		{
			if (pooledGrounds.Count > 0)
			{
				GameObject ground = pooledGrounds.Pop();
				ground.transform.position = groundPosition;
				ground.SetActive(true);
				activeGroundCount++;
				return ground;
			}
			else
			{
				GameObject newGround = Instantiate(groundPrefab, groundPosition, Quaternion.identity);
				return newGround;
			}
		}
		
		public GameObject GetGold(Vector3 goldPosition)
		{
			if (pooledGrounds.Count > 0)
			{
				GameObject ground = pooledGrounds.Pop();
				ground.transform.position = goldPosition;
				ground.SetActive(true);
				activeGoldCount++;
				return ground;
			}
			else
			{
				GameObject newGold	= Instantiate(groundPrefab, goldPosition, Quaternion.identity);
				return newGold;
			}
		}

		public void ReturnWall(GameObject wall)
		{
			wall.SetActive(false);
			pooledWalls.Push(wall);
			activeWallCount--;
		}

		public void ReturnGround(GameObject ground)
		{
			ground.SetActive(false);
			pooledWalls.Push(ground);
			activeGroundCount--;
		}

		public void ReturnGold(GameObject gold)
		{
			gold.SetActive(false);
			pooledWalls.Push(gold);
			activeGoldCount--;
		}


		private void OnGameEnd(bool isSuccessful)
		{
			if (isSuccessful)
			{
				DOVirtual.DelayedCall(1f, () =>
				{
					Debug.Log("1");
					ReturnAllObjectsToPool();
				});
			}
		}

		private void ReturnAllObjectsToPool()
		{
			while (activeWallCount > 0)
			{
				ReturnWall(pooledWalls.Pop());
			}

			while (activeGroundCount > 0)
			{
				ReturnGround(pooledGrounds.Pop());
			}

			while (activeGoldCount > 0)
			{
				ReturnGold(pooledGolds.Pop());
			}
		}
	}

}
