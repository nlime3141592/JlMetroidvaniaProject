using UnityEngine;

namespace JlMetroidvaniaProject.Datas
{
    /// <summary>
    /// 속도 상태를 기록하는 클래스입니다.
    /// </summary>
    public sealed class VelocityState : StateBase
    {
        /// <summary>
        /// 오른쪽으로 움직이고 있는지 여부를 확인합니다.
        /// </summary>
        /// <returns>오른쪽으로 움직이고 있다면 true</returns>
        public bool isRight => m_axisX > Constant.c_INFINITE_MIN_POSITIVE;

        /// <summary>
        /// 왼쪽으로 움직이고 있는지 여부를 확인합니다.
        /// </summary>
        /// <returns>왼쪽으로 움직이고 있다면 true</returns>
        public bool isLeft => m_axisX < Constant.c_INFINITE_MIN_NEGATIVE;

        /// <summary>
        /// 위로 올라가고 있는지 여부를 확인합니다.
        /// </summary>
        /// <returns>위로 올라가고 있다면 true</returns>
        public bool isFlying => m_axisY > Constant.c_INFINITE_MIN_POSITIVE;

        /// <summary>
        /// 아래로 내려가고 있는지 여부를 확인합니다.
        /// </summary>
        /// <returns>아래로 내려가고 있다면 true</returns>
        public bool isFalling => m_axisY < Constant.c_INFINITE_MIN_NEGATIVE;

        /// <summary>
        /// 정규화된 속도 방향 벡터의 X축 성분을 반환합니다.
        /// </summary>
        /// <returns>the X of normaized velocity. (-1.0f, 0.0f, 1.0f)</returns>
        public float axisX => m_axisX;

        /// <summary>
        /// 정규화된 속도 방향 벡터의 Y축 성분을 반환합니다.
        /// </summary>
        /// <returns>the Y of normaized velocity. (-1.0f, 0.0f, 1.0f)</returns>
        public float axisY => m_axisY;

        private float m_axisX;
        private float m_axisY;

        private Rigidbody2D m_rigidbody;

        public VelocityState(Rigidbody2D rigidbody)
        {
            m_rigidbody = rigidbody;
        }

        public override void Update()
        {
            m_axisX = m_UpdateAxis(m_rigidbody.velocity.x);
            m_axisY = m_UpdateAxis(m_rigidbody.velocity.y);
        }

        private float m_UpdateAxis(float velocity)
        {
            if(velocity < Constant.c_INFINITE_MIN_NEGATIVE) return -1.0f;
            else if(velocity > Constant.c_INFINITE_MIN_POSITIVE) return 1.0f;
            return 0.0f;
        }
    }
}