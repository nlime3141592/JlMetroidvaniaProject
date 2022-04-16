using System;

using UnityEngine;

using JlMetroidvaniaProject.Utility;
using JlMetroidvaniaProject.Physics;

namespace JlMetroidvaniaProject.Datas
{
    /// <summary>
    /// 지형과의 충돌 상태를 기록하는 클래스입니다.
    /// </summary>
    public sealed class TerrainCollisionState : StateBase
    {
        /// <summary>
        /// 왼쪽 벽에 닿았는지 여부를 확인합니다.
        /// </summary>
        /// <returns>왼쪽 벽에 닿았다면 true</returns>
        public bool isLeftWallSticking => m_isLeft;

        /// <summary>
        /// 오른쪽 벽에 닿았는지 여부를 확인합니다.
        /// </summary>
        /// <returns>오른쪽 벽에 닿았다면 true</returns>
        public bool isRightWallSticking => m_isRight;

        /// <summary>
        /// 바닥에 닿았는지 여부를 확인합니다.
        /// </summary>
        /// <returns>바닥에 닿았다면 true</returns>
        public bool isOnGround => m_isBottom;

        private bool m_isLeft;
        private bool m_isRight;
        private bool m_isBottom;

        private TerrainCollisionStateInitializer m_initializer;
        private RaycastHit2D m_hitLeft;
        private RaycastHit2D m_hitRight;
        private RaycastHit2D m_hitBottom;

        public TerrainCollisionState(TerrainCollisionStateInitializer initializer)
        {
            m_initializer = initializer;
        }

        public override void Update()
        {
            m_isLeft = MetroidPhysics.Raycast2D((Vector2)m_initializer.leftOrigin.position, Vector2.left, m_initializer.leftRayDistance, Constant.c_TERRAIN_LAYER_NAME, out m_hitLeft);
            m_isRight = MetroidPhysics.Raycast2D((Vector2)m_initializer.rightOrigin.position, Vector2.right, m_initializer.rightRayDistance, Constant.c_TERRAIN_LAYER_NAME, out m_hitRight);
            m_isBottom = MetroidPhysics.Raycast2D((Vector2)m_initializer.bottomOrigin.position, Vector2.down, m_initializer.bottomRayDistance, Constant.c_TERRAIN_LAYER_NAME, out m_hitBottom);
        }
    }
}