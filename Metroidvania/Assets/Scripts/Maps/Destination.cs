using UnityEngine;

namespace JlMetroidvaniaProject.Maps
{
    public class Destination : JlBehaviour
    {
        public Vector2 position;

        protected override void Awake()
        {
            position = Vector2.zero;
            position.x = transform.position.x;
            position.y = transform.position.y;
        }

        protected override void Update()
        {
            position.x = transform.position.x;
            position.y = transform.position.y;
        }
    }
}