using System;

using UnityEngine;

using JlMetroidvaniaProject.Utility;
using JlMetroidvaniaProject.Datas;

namespace JlMetroidvaniaProject
{
    public sealed class GameManager : JlBehaviour
    {
        public static GameManager s_gameManager
        {
            get
            {
                if(s_m_gameManager == null)
                    throw new NullReferenceException("Game Manager is NULL.");

                return s_m_gameManager;
            }
        }

        public static InputSystem s_inputSystem
        {
            get
            {
                if(s_m_inputSystem == null)
                    throw new NullReferenceException("Input System is NULL.");

                return s_m_inputSystem;
            }
        }

        private static InputState s_m_inputState;
        private static InputSystem s_m_inputSystem;
        private static GameManager s_m_gameManager;

        protected override void Awake()
        {
            base.Awake();

            s_m_inputState = new InputState();
            s_m_inputSystem = new InputSystem(s_m_inputState);

            if(s_m_gameManager == null)
                s_m_gameManager = this;
            else
                DestroyImmediate(this.gameObject);
        }

        protected override void Update()
        {
            if(s_m_inputSystem != null)
                s_m_inputState.Update();
        }
    }
}