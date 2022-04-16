using System;

using UnityEngine;

namespace JlMetroidvaniaProject.Datas
{
    [Serializable]
    public class TerrainCollisionStateInitializer : IStateInitializer
    {
        public Transform leftOrigin;
        public Transform rightOrigin;
        public Transform bottomOrigin;
        public float leftRayDistance;
        public float rightRayDistance;
        public float bottomRayDistance;
    }
}