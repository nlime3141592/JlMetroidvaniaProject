using System;

using UnityEngine;

namespace JlMetroidvaniaProject.Dummy
{
    public class DummyInputSystem
    {
        public event EventHandler evhdlr_checkEvent;

        public bool isPressingHorizontal => m_isPressingHorizontal;
        public bool isPressingHorizontalNegative => m_isPressingHorizontalNegative;
        public bool isPressingHorizontalPositive => m_isPressingHorizontalPositive;
        public float horizontalAxis => m_horizontalAxis;

        private bool m_isPressingHorizontal;
        private bool m_isPressingHorizontalNegative;
        private bool m_isPressingHorizontalPositive;
        private float m_horizontalAxis;

        public DummyInputSystem()
        {
            evhdlr_checkEvent += (sender, args) => { UnityEngine.Debug.Log("On CheckInputs()"); };
        }

        public void CheckInputs()
        {
            m_isPressingHorizontal = Input.GetButton("Horizontal");
            m_horizontalAxis = Input.GetAxisRaw("Horizontal");
            m_isPressingHorizontalNegative = m_horizontalAxis < 0;
            m_isPressingHorizontalPositive = m_horizontalAxis > 0;

            evhdlr_checkEvent((object)this, null);
        }
    }
}