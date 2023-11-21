using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RollerSplatClone.ScritableObjects
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "Custom/Level")]
    public class Level : ScriptableObject
    {
        public Texture2D levelTexture;

    }
}

