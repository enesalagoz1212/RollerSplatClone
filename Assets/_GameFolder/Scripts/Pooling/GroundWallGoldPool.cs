using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RollerSplatClone.Pooling
{
    public class GroundWallGoldPool : MonoBehaviour
    {
        public GameObject wallPrefab;
        public GameObject groundPrefab;
        public GameObject goldPrefab;

        public GameObject pools;

        public int initializeWallPoolSize;
        public int initializeGroundPoolSize;
        public int initializeGoldPoolSize;

        private Stack<GameObject> pooledWalls = new Stack<GameObject>();
        private Stack<GameObject> pooledGrounds = new Stack<GameObject>();
        private Stack<GameObject> pooledGolds = new Stack<GameObject>();
        
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
			for (int k = 0; k <initializeGoldPoolSize ; k++)
			{
                GameObject goldPooled = Instantiate(goldPrefab, pools.transform);
                goldPooled.SetActive(false);
                pooledGolds.Push(goldPooled);
			}
		}
		void Start()
        {
            InitializePool();
        }

        public GameObject GetWall(Vector3 wallPosition)
		{
			if (pooledWalls.Count>0)
			{
                GameObject wall = pooledWalls.Pop();
                wall.transform.position = wallPosition;
                wall.SetActive(true);
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
			if (pooledGrounds.Count>0)
			{
                GameObject ground = pooledGrounds.Pop();
                ground.transform.position = groundPosition;
                ground.SetActive(true);
                return ground;
			}
			else
			{
                GameObject newGround = Instantiate(groundPrefab, groundPosition, Quaternion.identity);
                return newGround;
			}
		}

        void Update()
        {

        }
    }

}
