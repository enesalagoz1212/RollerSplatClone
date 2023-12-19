using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RollerSplatClone.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "Custom/Level")]
    public class Level : ScriptableObject
    {
        public Texture2D levelTexture;

        public bool isBonusLevel;

    }
}

