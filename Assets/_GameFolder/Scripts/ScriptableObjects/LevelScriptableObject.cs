using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RollerSplatClone.ScritableObjects
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "Custom/Level")]
    public class LevelScriptableObject : ScriptableObject
    {
        [Header("Starting positions")]
        public Vector3 ballStartPosition;

        [Header("Level Prefab")]
        public GameObject levelPrefab;

        public int addToGroundList;
    }
}

