using System;

using UnityEngine;
using Cinemachine;

namespace JlMetroidvaniaProject.Utility
{
    public static class CinemachineUtility2D
    {
        /// <summary>
        /// 가상 카메라의 타겟팅을 해제합니다.
        /// </summary>
        /// <param name="virtualCamera">대상 가상 카메라</param>
        public static void UntargetTransform(CinemachineVirtualCamera virtualCamera)
        {
            if(virtualCamera == null)
            {
                DebugUtility.UnityAssert("Camera cannot be null. only availables Cinemachine.CinemachineVirtualCamera Type.");
            }
            else
            {
                virtualCamera.Follow = null;
            }
        }

        /// <summary>
        /// 가상 카메라가 타겟을 잡습니다.
        /// </summary>
        /// <param name="virtualCamera">주체 가상 카메라</param>
        /// <param name="targetTransform">타겟 대상 Transform Component</param>
        public static void TargetTransform(CinemachineVirtualCamera virtualCamera, Transform targetTransform)
        {
            if(virtualCamera == null)
            {
                DebugUtility.UnityAssert("Camera cannot be null. only availables Cinemachine.CinemachineVirtualCamera Type.");
            }
            else if(targetTransform == null)
            {
                DebugUtility.UnityAssert("target cannot be null.");
            }
            else
            {
                virtualCamera.Follow = targetTransform;
            }
        }

        /// <summary>
        /// 가상 카메라의 추적 지연 시간을 설정합니다.
        /// </summary>
        /// <param name="virtualCamera">적용 대상 카메라</param>
        /// <param name="tracingDelay">추적 지연 시간 (0.0f 이상 20.0f 이하의 실수)</param>
        /// <param name="axis">추적 지연 시간을 적용할 축</param>
        // AxisType.X, AxisType.Y 모두에 대해 tracingDelay를 0.0f로 설정하면 카메라가 타겟에 영구히 고정된 효과를 얻는다.
        public static void SetTraceDelay(CinemachineVirtualCamera virtualCamera, float tracingDelay, AxisType axis)
        {
            if(virtualCamera == null)
            {
                DebugUtility.UnityAssert("Camera cannot be null. only availables Cinemachine.CinemachineVirtualCamera Type.");
            }
            else
            {
                float minValue = 0.0f;
                float maxValue = 20.0f;
                CinemachineFramingTransposer transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

                // NOTE: 아래 코드와 정확히 같은 동작을 한다.
                // tracingDelay = tracingDelay < minValue ? minValue : tracingDelay;
                // tracingDelay = tracingDelay > maxValue > maxValue : tracingDelay;                
                tracingDelay = MathUtility.Min(maxValue, MathUtility.Max(minValue, tracingDelay));

                switch(axis)
                {
                    case AxisType.X:
                        transposer.m_XDamping = tracingDelay;
                        transposer.m_ZDamping = 0.0f;
                        break;

                    case AxisType.Y:
                        transposer.m_YDamping = tracingDelay;
                        transposer.m_ZDamping = 0.0f;
                        break;

                    default:
                        DebugUtility.UnityAssert("axis allows only these two. (AxisType.X or AxisType.Y)");
                        break;
                }
            }
        }

        /// <summary>
        /// 가상 카메라의 중심을 타겟으로부터 일정 거리만큼 이동시켜 타겟팅 위치를 조정합니다.
        /// </summary>
        /// <param name="virtualCamera">적용 대상 가상 카메라</param name>
        /// <param name="bias">조정 거리</param>
        /// <param name="axis">타겟팅 위치를 조정할 축</param>
        public static void SetBias(CinemachineVirtualCamera virtualCamera, float bias, AxisType axis)
        {
            if(virtualCamera == null)
            {
                DebugUtility.UnityAssert("Camera cannot be null. only availables Cinemachine.CinemachineVirtualCamera Type.");
            }
            else
            {
                CinemachineFramingTransposer transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

                switch(axis)
                {
                    case AxisType.X:
                        transposer.m_TrackedObjectOffset.x = bias;
                        transposer.m_TrackedObjectOffset.z = 0.0f;
                        break;

                    case AxisType.Y:
                        transposer.m_TrackedObjectOffset.y = bias;
                        transposer.m_TrackedObjectOffset.z = 0.0f;
                        break;

                    default:
                        DebugUtility.UnityAssert("axis allows only these two. (AxisType.X or AxisType.Y)");
                        break;
                }
            }
        }
    }
}