using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace JlMetroidvaniaProject.Dummy
{
    public class DummyPlayerController_1 : MonoBehaviour
    {
        public float c_MAX_FALLING_SPEED;
        public float speed;

        private float m_lookingDirection; // -1.0f, 0.0f, 1.0f
        private bool m_isMoving;
        private float m_movingDirection;
        private bool m_bOnGround;
        private bool m_isFalling;
        private bool m_isFlying;
        private bool m_isSit;
        private bool m_isHeadUp;
        private bool m_bOnWallSide;
        private float m_wallSideDirection; // -1.0f, 0.0f, 1.0f

        private Rigidbody2D m_rigidbody;

        private Vector2 m_tempVector2;
        private Vector3 m_tempVector3;

        private DummyInputSystem m_isystem;

        void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();

            m_tempVector2 = Vector2.zero;
            m_tempVector3 = Vector3.zero;
        }

        void Start()
        {
            
        }

        void FixedUpdate()
        {
            m_PhysicsLogic();
        }

        void Update()
        {
            m_SetAdditionalState(); // I*
            m_Input(); // I
        }

        private void m_SetAdditionalState()
        {
            m_lookingDirection = transform.rotation.y == 0.0f ? 1.0f : -1.0f;
        }

        private void m_Input()
        {
            if(DummyProgramManager.CanGetInputSystem(out m_isystem))
            {
                m_isMoving = m_isystem.isPressingHorizontalNegative ^ m_isystem.isPressingHorizontalPositive;
                m_movingDirection = m_isystem.horizontalAxis;
            }
        }

        private void m_PhysicsLogic()
        {
            if(m_isMoving)
            {
                m_Move2D(m_rigidbody, m_movingDirection, speed, m_lookingDirection);
            }
            else
            {
                DummyPhysicsUtility2D.s_SetVelocity2D(m_rigidbody, 0, DummyAxisType.X);
            }

            DummyPhysicsUtility2D.s_SetVelocityLimit2D(m_rigidbody, c_MAX_FALLING_SPEED, DummyAxisType.Y, DummyClampMode.Min);
        }

        private bool m_Move2D(Rigidbody2D rigidbody, float direction, float absoluteMoveSpeed, float lookingDirection)
        {
            if(direction != 0 && absoluteMoveSpeed > 1e-6)
            {
                if(direction * lookingDirection < 0)
                {
                    m_Flip();
                }

                DummyPhysicsUtility2D.s_SetVelocity2D(rigidbody, speed * direction, DummyAxisType.X);

                return true;
            }

            return false;
        }

        private void m_Flip()
        {
            m_tempVector3.x = transform.rotation.x;
            m_tempVector3.y = transform.rotation.y == 0.0f ? 180.0f : 0.0f;
            m_tempVector3.z = 0.0f;

            transform.eulerAngles = m_tempVector3;
        }

        // NOTE: 미구현
        private bool m_Jump2D(Rigidbody2D rigidbody, float absoluteJumpSpeed, int jumpedCount, int maxJumpedCount, int jumpingSteps, int maxJumpingSteps)
        {
            if(jumpingSteps < maxJumpingSteps)
            {
                DummyPhysicsUtility2D.s_SetVelocity2D(rigidbody, absoluteJumpSpeed, DummyAxisType.Y);
                return true;
            }

            return false;
        }
    }
}