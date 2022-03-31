using UnityEngine;

using JlMetroidvaniaProject.Datas;
using JlMetroidvaniaProject.Utility;

namespace JlMetroidvaniaProject.Controllers
{
    public class PlayerController : JlBehaviour
    {
        private Rigidbody2D m_rigidbody;
        private StatTable m_statTable;

        private InputSystem m_inputSystem;
        private VelocityState m_velocityState;
        private TerrainCollisionState m_collisionState;
        public TerrainCollisionStateOption terrainCollisionStateOption;

        private float m_inputAxis;

        protected override void Awake()
        {
            // component allocation
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_statTable = GetComponent<StatTable>();

            // internal class instantiation
            m_velocityState = new VelocityState(m_rigidbody);
            m_collisionState = new TerrainCollisionState(terrainCollisionStateOption);
        }

        protected override void Start()
        {
            m_inputSystem = GameManager.s_inputSystem;
        }

        protected override void FixedUpdate()
        {
            // 1차 상태와 2차 상태를 가지고 실제 기능을 수행
            m_DoAction(Time.fixedDeltaTime);
        }

        protected override void Update()
        {
            m_UpdateStates();
        }

        private void m_UpdateStates()
        {
            // 1차 상태 Update
            m_UpdateFirstStates();

            // 2차 상태 Update
            m_UpdateSecondaryStates();
        }

        private void m_UpdateFirstStates()
        {
            m_velocityState.Update();
            m_collisionState.Update();
        }

        private void m_UpdateSecondaryStates()
        {
            if(m_inputSystem.isPressingHorizontalNegative ^ m_inputSystem.isPressingHorizontalPositive)
                m_inputAxis = m_inputSystem.isPressingHorizontalNegative ? -1.0f : 1.0f;
            else
                m_inputAxis = 0.0f;
        }

        private void m_DoAction(float deltaTime)
        {
            // m_Move(m_statTable.moveSpeed, m_inputAxis, deltaTime);
        }

        // private void m_Move(float speed, float axis, float deltaTime);
    }
}