using UnityEngine;

using JlMetroidvaniaProject.Utility;

namespace JlMetroidvaniaProject
{
    public class DummyPlayerController_2 : JlBehaviour
    {
        private Rigidbody2D m_rigidbody;

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            RaycastHit2D hit;
            bool CanRaycast = PhysicsUtility2D.RaycastTerrain2D(transform.position, Vector2.down, 0.51f, out hit);

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
            if(GameManager.s_inputSystem.isPressDownJump)
            {
                PhysicsUtility2D.DoUniformMotion2D(m_rigidbody, 5.5f, AxisType.Y);
            }
        }

        private void m_Move()
        {
            bool left = Input.GetKey(KeyCode.LeftArrow);
            bool right = Input.GetKey(KeyCode.RightArrow);

            if(left ^ right)
            {
                float direction = left ? -1.0f : 1.0f;

                PhysicsUtility2D.DoUniformMotion2D(m_rigidbody, 2.5f * direction, AxisType.X);
            }
            else
            {
                PhysicsUtility2D.DoUniformMotion2D(m_rigidbody, 0.0f, AxisType.X);
            }
        }
    }
}