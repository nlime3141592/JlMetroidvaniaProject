using System.Diagnostics;

using UnityEngine;

namespace JlMetroidvaniaProject.Dummy
{
    public class DummyPhysicsUtility2D
    {
        private static float s_m_tempFloat1;
        private static Vector2 s_m_tempVector2;

        static DummyPhysicsUtility2D()
        {
            s_m_tempVector2 = Vector2.zero;
        }

        public static void s_SetVelocity2D(Rigidbody2D rigidbody, float speed, DummyAxisType axis)
        {
            switch(axis)
            {
                case DummyAxisType.X:
                    s_m_tempVector2.x = speed;
                    s_m_tempVector2.y = rigidbody.velocity.y;
                    rigidbody.velocity = s_m_tempVector2;
                    break;
                
                case DummyAxisType.Y:
                    s_m_tempVector2.x = rigidbody.velocity.x;
                    s_m_tempVector2.y = speed;
                    rigidbody.velocity = s_m_tempVector2;
                    break;

                default:
                    System.Diagnostics.Debug.Assert(false, "Rigidbody2D component can have only X and Y axis type.");
                    break;
            }
        }

        public static void s_SetVelocityLimit2D(Rigidbody2D rigidbody, float limitSpeed, DummyAxisType axis, DummyClampMode mode)
        {
            switch(axis)
            {
                case DummyAxisType.X:
                    s_SetVelocity2D(rigidbody, s_GetLimitSpeed(rigidbody.velocity.x, limitSpeed, mode), axis);
                    break;
                
                case DummyAxisType.Y:
                    s_SetVelocity2D(rigidbody, s_GetLimitSpeed(rigidbody.velocity.y, limitSpeed, mode), axis);
                    break;

                default:
                    System.Diagnostics.Debug.Assert(false, "Rigidbody2D component can have only X and Y axis type.");
                    break;
            }
        }

        public static float s_GetLimitSpeed(float speed, float limitSpeed, DummyClampMode mode)
        {
            switch(mode)
            {
                case DummyClampMode.Min:
                    return speed < limitSpeed ? limitSpeed : speed;
                
                case DummyClampMode.Max:
                    return speed > limitSpeed ? limitSpeed : speed;

                default:
                    System.Diagnostics.Debug.Assert(false, "ClampMode enumeration can have only Min or Max.");
                    return speed;
            }
        }

        // NOTE: 미구현
        public static bool s_DetectRaycast2D(Vector2 origin, Vector2 offset, Vector2 direction, float distance, int layerMask, out RaycastHit2D hitInstance)
        {
            RaycastHit2D hit;

            hit = Physics2D.Raycast(origin + offset, direction, distance, layerMask);

            hitInstance = hit;

            return hit.collider != null;
        }
    }

    public enum DummyAxisType
    {
        ALL = 0,
        A, B, C, D, E, F, G,
        H, I, J, K, L, M, N,
        O, P, Q, R, S, T, U,
        V, W, X, Y, Z
    }

    public enum DummyClampMode
    {
        Min = -1,
        Max = 1
    }
}