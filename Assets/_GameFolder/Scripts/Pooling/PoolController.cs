using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RollerSplatClone.Managers;
using DG.Tweening;

namespace RollerSplatClone.Pooling
{
    public class PoolController : MonoBehaviour
    {
        public GameObject wallPrefab;
        public GameObject groundPrefab;
        public GameObject goldPrefab;

        public GameObject pools;

        public int initializeWallPoolSize;
        public int initializeGroundPoolSize;
        public int initializeGoldPoolSize;

        private List<GameObject> pooledWalls = new List<GameObject>();
        private List<GameObject> pooledGrounds = new List<GameObject>();
        private List<GameObject> pooledGolds = new List<GameObject>();

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
            CreatePooledObjects(wallPrefab, initializeWallPoolSize, pooledWalls);
            CreatePooledObjects(groundPrefab, initializeGroundPoolSize, pooledGrounds);
            CreatePooledObjects(goldPrefab, initializeGoldPoolSize, pooledGolds);
        }

        private void CreatePooledObjects(GameObject prefab, int poolSize, List<GameObject> poolList)
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject pooledObject = Instantiate(prefab, pools.transform);
                pooledObject.SetActive(false);
                poolList.Add(pooledObject);
            }
        }

        private GameObject GetPooledObject(List<GameObject> poolList, Vector3 position)
        {
            if (poolList.Count > 0)
            {
                GameObject pooledObject = poolList[poolList.Count - 1];
                poolList.RemoveAt(poolList.Count - 1);
                pooledObject.transform.position = position;
                pooledObject.SetActive(true);
                return pooledObject;
            }
            else
            {
                GameObject newObject = Instantiate(wallPrefab, position, Quaternion.identity);
                return newObject;
            }
        }

        private void ReturnPooledObject(List<GameObject> poolList, GameObject obj)
        {
            obj.SetActive(false);
            poolList.Add(obj);
        }

        public GameObject GetWall(Vector3 wallPosition)
        {
            return GetPooledObject(pooledWalls, wallPosition);
        }

        public GameObject GetGround(Vector3 groundPosition)
        {
            return GetPooledObject(pooledGrounds, groundPosition);
        }

        public GameObject GetGold(Vector3 goldPosition)
        {
            return GetPooledObject(pooledGolds, goldPosition);
        }

        public void ReturnWall(GameObject wall)
        {
            ReturnPooledObject(pooledWalls, wall);
        }

        public void ReturnGround(GameObject ground)
        {
            ReturnPooledObject(pooledGrounds, ground);
        }

        public void ReturnGold(GameObject gold)
        {
            ReturnPooledObject(pooledGolds, gold);
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
    }

}
