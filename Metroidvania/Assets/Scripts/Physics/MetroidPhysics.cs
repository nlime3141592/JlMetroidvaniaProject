using System;

using UnityEngine;

using JlMetroidvaniaProject.Utility;

namespace JlMetroidvaniaProject.Physics
{
    public static class MetroidPhysics
    {
        private static Vector2 s_m_tempVector2_01;
        private static Vector2 s_m_tempVector2_02;

        static MetroidPhysics()
        {
            s_m_tempVector2_01 = Vector2.zero;
            s_m_tempVector2_02 = Vector2.zero;
        }

        /// <summary>
        /// 2차원 등속도 운동
        /// </summary>
        /// <param name="rigidbody">2차원 물리 속성 GameObject</param>
        /// <param name="velocity">속력</param>
        /// <param name="axis">속력을 적용할 축</param>
        public static void DoUniformMotion2D(Rigidbody2D rigidbody, float velocity, AxisType axis)
        {
            // Vector2 s_m_tempVector2_01: 새롭게 적용할 속도

            switch(axis)
            {
                case AxisType.X:
                    s_m_tempVector2_01.x = velocity;
                    s_m_tempVector2_01.y = rigidbody.velocity.y;
                    rigidbody.velocity = s_m_tempVector2_01;
                    break;

                case AxisType.Y:
                    s_m_tempVector2_01.x = rigidbody.velocity.x;
                    s_m_tempVector2_01.y = velocity;
                    rigidbody.velocity = s_m_tempVector2_01;
                    break;
                
                default:
                    Debug.Assert(false, "axis allows only these two. (AxisType.X or AxisType.Y)");
                    break;
            }
        }

        /// <summary>
        /// 개별 축 속도 제한
        /// </summary>
        /// <param name="rigidbody">2차원 물리 속성 GameObject</param>
        /// <param name="limitVelocity">제한 속력</param>
        /// <param name="axis">속력 제한을 적용할 축</param>
        /// <param name="mode">속력 제한 모드(Min, Max)</param>
        public static void LimitVelocity2D(Rigidbody2D rigidbody, float limitVelocity, AxisType axis, ClampType mode)
        {
            // Vector2 s_m_tempVector2_01: 새롭게 적용할 속도

            s_m_tempVector2_01.x = rigidbody.velocity.x;
            s_m_tempVector2_01.y = rigidbody.velocity.y;

            if(axis == AxisType.X)
            {
                if(mode == ClampType.Min)
                {
                    s_m_tempVector2_01.x = MathUtility.Min(rigidbody.velocity.x, limitVelocity);
                }
                else if(mode == ClampType.Max)
                {
                    s_m_tempVector2_01.x = MathUtility.Max(rigidbody.velocity.x, limitVelocity);
                }
                else
                {
                    Debug.Assert(false, "mode allows only these two. (ClampType.Min or ClampType.Max)");
                }
            }
            else if(axis == AxisType.Y)
            {
                if(mode == ClampType.Min)
                {
                    s_m_tempVector2_01.y = MathUtility.Min(rigidbody.velocity.y, limitVelocity);
                }
                else if(mode == ClampType.Max)
                {
                    s_m_tempVector2_01.y = MathUtility.Max(rigidbody.velocity.y, limitVelocity);
                }
                else
                {
                    Debug.Assert(false, "mode allows only these two. (ClampType.Min or ClampType.Max)");
                }
            }
            else
            {
                Debug.Assert(false, "axis allows only these two. (AxisType.X or AxisType.Y)");
            }

            rigidbody.velocity = s_m_tempVector2_01;
        }

        /// <summary>
        /// 충돌 영역의 존재를 확인합니다.
        /// </summary>
        /// <param name="origin">중심 위치</param>
        /// <param name="direction">충돌 감지 방향</param>
        /// <param name="maxRaycastLength">최대 충돌 감지 길이</param>
        /// <param name="layerName">충돌 판정할 레이어 이름</param>
        /// <param name="hit">(읽기 전용) 충돌한 물체의 정보</param>
        /// <returns>
        /// 충돌 영역 감지 성공: true
        /// </returns>
        public static bool Raycast2D(Vector2 origin, Vector2 direction, float maxRaycastLength, string layerName, out RaycastHit2D hit)
        {
            if(maxRaycastLength < 0) Debug.Assert(false, "maxRaycastLength cannot be negative.");

            hit = Physics2D.Raycast(origin, direction, maxRaycastLength, 1 << LayerMask.NameToLayer(layerName));

            return hit.collider != null;
        }
    }
}