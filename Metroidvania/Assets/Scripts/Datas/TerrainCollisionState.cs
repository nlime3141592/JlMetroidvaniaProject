using System;

using UnityEngine;

using JlMetroidvaniaProject.Utility;

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

        private TerrainCollisionStateOption m_updateOption;
        private RaycastHit2D m_hitLeft;
        private RaycastHit2D m_hitRight;
        private RaycastHit2D m_hitBottom;

        public TerrainCollisionState(TerrainCollisionStateOption updateOption)
        {
            m_updateOption = updateOption;
        }

        public override void Update()
        {
            m_isLeft = PhysicsUtility2D.RaycastTerrain2D((Vector2)m_updateOption.leftOrigin.position, Vector2.left, m_updateOption.leftRayDistance, out m_hitLeft);
            m_isRight = PhysicsUtility2D.RaycastTerrain2D((Vector2)m_updateOption.rightOrigin.position, Vector2.right, m_updateOption.rightRayDistance, out m_hitRight);
            m_isBottom = PhysicsUtility2D.RaycastTerrain2D((Vector2)m_updateOption.bottomOrigin.position, Vector2.down, m_updateOption.bottomRayDistance, out m_hitBottom);
        }
    }

    [Serializable]
    public struct TerrainCollisionStateOption
    {
        public Transform leftOrigin;
        public Transform rightOrigin;
        public Transform bottomOrigin;
        public float leftRayDistance;
        public float rightRayDistance;
        public float bottomRayDistance;
    }
}