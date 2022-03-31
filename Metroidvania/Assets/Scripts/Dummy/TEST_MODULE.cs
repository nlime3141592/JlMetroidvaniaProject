using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Cinemachine;

using JlMetroidvaniaProject.Utility;

namespace JlMetroidvaniaProject.Dummy
{
    public class TEST_MODULE : JlBehaviour
    {
        public bool check1;
        public bool check2;
        public bool check3;
        public bool check4;

        public CinemachineVirtualCamera virtualCamera;
        public Transform playerTransform;
        public Vector2 traceDelay;
        public Vector2 targetOffset;

        protected override void Start()
        {
            
        }

        protected override void Update()
        {
            if(check1)
            {
                check1 = false;
                CinemachineUtility2D.UntargetTransform(virtualCamera);
            }
            if(check2)
            {
                check2 = false;
                CinemachineUtility2D.TargetTransform(virtualCamera, playerTransform);
            }
            if(check3)
            {
                check3 = false;
                CinemachineUtility2D.SetTraceDelay(virtualCamera, traceDelay.x, AxisType.X);
                CinemachineUtility2D.SetTraceDelay(virtualCamera, traceDelay.y, AxisType.Y);
            }
            if(check4)
            {
                check4 = false;
                CinemachineUtility2D.SetBias(virtualCamera, targetOffset.x, AxisType.X);
                CinemachineUtility2D.SetBias(virtualCamera, targetOffset.y, AxisType.Y);
            }
        }
    }
}