using System;

using UnityEngine;

using JlMetroidvaniaProject.Datas;
using JlMetroidvaniaProject.Utility;

namespace JlMetroidvaniaProject
{
    public sealed class InputSystem : StateBase
    {
        public event Action evhdlr_checkEvent;

        public bool isPressingHorizontal => m_isPressingHorizontal;
        public bool isPressingHorizontalNegative => m_isPressingHorizontalNegative;
        public bool isPressingHorizontalPositive => m_isPressingHorizontalPositive;
        public float horizontalAxis => m_horizontalAxis;
        public bool isPressDownJump => m_isPressDownJump;

        private bool m_isPressingHorizontal;
        private bool m_isPressingHorizontalNegative;
        private bool m_isPressingHorizontalPositive;
        private float m_horizontalAxis;
        private bool m_isPressDownJump;

        public InputSystem()
        {
            evhdlr_checkEvent += () => { DebugUtility.UnityLog("On CheckInputs()"); };
        }

        public override void Update()
        {
            m_isPressingHorizontal = Input.GetButton("Horizontal");
            m_horizontalAxis = Input.GetAxisRaw("Horizontal");
            m_isPressingHorizontalNegative = m_horizontalAxis < 0;
            m_isPressingHorizontalPositive = m_horizontalAxis > 0;
            m_isPressDownJump = Input.GetButtonDown("Jump");

            evhdlr_checkEvent();
        }
    }
}