using System;

using UnityEngine;

namespace JlMetroidvaniaProject.Datas
{
    [Serializable]
    public class VelocityStateInitializer : IStateInitializer
    {
        public Rigidbody2D rigidbody;
    }
}