using System;

using UnityEngine;

using JlMetroidvaniaProject.Utility;

namespace JlMetroidvaniaProject
{
    public sealed class GameManager : JlBehaviour
    {
        public static GameManager s_gameManager
        {
            get
            {
                if(s_m_gameManager == null)
                {
                    throw new NullReferenceException("Game Manager is NULL.");
                }

                return s_m_gameManager;
            }
        }

        public static InputSystem s_inputSystem
        {
            get
            {
                if(s_m_inputSystem == null)
                {
                    throw new NullReferenceException("Input System is NULL.");
                }

                return s_m_inputSystem;
            }
        }

        private static GameManager s_m_gameManager;
        private static InputSystem s_m_inputSystem;

        private void Awake()
        {
            m_InstantiateSingletonInstance();
            m_InstantiateSingletoneInputSystem();
        }

        private void m_InstantiateSingletonInstance()
        {
            if(s_m_gameManager == null)
            {
                s_m_gameManager = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                GameObject.DestroyImmediate(this.gameObject);
            }
        }

        private void m_InstantiateSingletoneInputSystem()
        {
            if(s_m_inputSystem == null)
            {
                s_m_inputSystem = new InputSystem();
            }
        }

        private void Update()
        {
            s_m_inputSystem.CheckInputs();
        }
    }
}