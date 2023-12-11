using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RollerSplatClone.Managers;
using DG.Tweening;
using UnityEngine.UIElements;
using RollerSplatClone.Controllers;

namespace RollerSplatClone.Pooling
{
	public class PoolController : MonoBehaviour
	{
		private GroundController _groundController;

		public GameObject wallPrefab;
		public GameObject groundPrefab;
		public GameObject goldPrefab;
		public GameObject particlePrefab;

		public GameObject pools;

		public int initializeWallPoolSize;
		public int initializeGroundPoolSize;
		public int initializeGoldPoolSize;
		public int initializeParticlePoolSize;

		private List<GameObject> pooledWalls = new List<GameObject>();
		private List<GameObject> pooledGrounds = new List<GameObject>();
		private List<GameObject> pooledGolds = new List<GameObject>();
		private List<GameObject> pooledParticle = new List<GameObject>();

		private void OnEnable()
		{
			GameManager.OnGameEnd += OnGameEnd;
		}

		private void OnDisable()
		{
			GameManager.OnGameEnd -= OnGameEnd;
		}

		public void Initialize(GroundController groundController)
		{
			_groundController = groundController;
			InitializePool();
		}

		private void InitializePool()
		{
			CreatePooledObjects(wallPrefab, initializeWallPoolSize, pooledWalls);
			CreatePooledObjects(groundPrefab, initializeGroundPoolSize, pooledGrounds);
			CreatePooledObjects(goldPrefab, initializeGoldPoolSize, pooledGolds);
			CreatePooledObjects(particlePrefab, initializeParticlePoolSize, pooledParticle);
		}

		private void CreatePooledObjects(GameObject prefab, int poolSize, List<GameObject> poolList)
		{
			for (int i = 0; i < poolSize; i++)
			{
				GameObject pooledObject = Instantiate(prefab, pools.transform);
				poolList.Add(pooledObject);
				pooledObject.SetActive(false);
			}
		}

		private GameObject GetPooledObject(List<GameObject> poolList, GameObject poolPrefab, Vector3 position)
		{
			for (int i = 0; i < poolList.Count; i++)
			{
				var pooledObject = poolList[i];
				if (!pooledObject.activeSelf)
				{
					pooledObject.transform.position = position;
					pooledObject.SetActive(true);
					return pooledObject;
				}
			}

			GameObject newObject = Instantiate(poolPrefab, position, Quaternion.identity);
			poolList.Add(newObject);
			newObject.transform.position = position;
			return newObject;
		}

		public void ReturnPooledObject(GameObject obj)
		{
			obj.SetActive(false);
		}

		public GameObject GetWall(Vector3 wallPosition)
		{
			return GetPooledObject(pooledWalls, wallPrefab, wallPosition);
		}

		public GameObject GetGround(Vector3 groundPosition)
		{
			return GetPooledObject(pooledGrounds, groundPrefab, groundPosition);
		}

		public GameObject GetGold(Vector3 goldPosition)
		{
			return GetPooledObject(pooledGolds, goldPrefab, goldPosition);
		}

		public GameObject GetParticle(Vector3 particlePosition)
		{
			return GetPooledObject(pooledParticle, particlePrefab, particlePosition);
		}

		private void OnGameEnd(bool isSuccessful)
		{
			if (isSuccessful)
			{
				DOVirtual.DelayedCall(1f, () =>
				{

				});
			}
		}

		public void ReturnAllObjectsToThePool()
		{
			for (int i = 0; i < pooledWalls.Count; i++)
			{
				pooledWalls[i].SetActive(false);
			}
			for (int i = 0; i < pooledGrounds.Count; i++)
			{
				pooledGrounds[i].SetActive(false);
			}
			for (int i = 0; i < pooledParticle.Count; i++)
			{
				pooledParticle[i].SetActive(false);
			}

		}

		public void ReturnObjectsThePool()
		{
			for (int i = 0; i < pooledGolds.Count; i++)
			{
				pooledGolds[i].SetActive(false);
			}
		}

	}

}
