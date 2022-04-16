using UnityEngine;

using JlMetroidvaniaProject.Utility;
using JlMetroidvaniaProject.Physics;

namespace JlMetroidvaniaProject
{
    public class DummyPlayerController_2 : JlBehaviour
    {
        private Rigidbody2D m_rigidbody;

        protected override void Start()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
        }

        protected override void Update()
        {
            RaycastHit2D hit;
            bool CanRaycast = MetroidPhysics.Raycast2D(transform.position, Vector2.down, 0.51f, Constant.c_TERRAIN_LAYER_NAME, out hit);

            if(hit)
            {
                float distance = Vector2.Distance(transform.position, hit.point);

                // DebugUtility.UnityLog("height = {0}", distance);

                if(distance < 0.51f)
                {
                    m_Jump();
                }    
            }

            m_Move();
        }

        private void m_Jump()
        {
            // if(GameManager.s_inputSystem.isPressDownJump) MetroidPhysics.DoUniformMotion2D(m_rigidbody, 5.5f, AxisType.Y);
        }

        private void m_Move()
        {
            bool left = Input.GetKey(KeyCode.LeftArrow);
            bool right = Input.GetKey(KeyCode.RightArrow);

            if(left ^ right)
            {
                float direction = left ? -1.0f : 1.0f;

                MetroidPhysics.DoUniformMotion2D(m_rigidbody, 2.5f * direction, AxisType.X);
            }
            else
            {
                MetroidPhysics.DoUniformMotion2D(m_rigidbody, 0.0f, AxisType.X);
            }
        }
    }
}