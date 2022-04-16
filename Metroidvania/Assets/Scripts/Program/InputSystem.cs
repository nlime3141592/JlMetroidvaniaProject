using System;
using System.Collections.Generic;

using JlMetroidvaniaProject.Datas;

namespace JlMetroidvaniaProject
{
    public class InputSystem
    {
        private InputState m_inputState;
        private List<InputType>[] m_keyMappingTable;

        public bool this[KeyType keyType, PressType pressType]
        {
            get
            {
                int keyTypeInt = (int)keyType;

                if(m_IsNullAssignedKey(keyType))
                    return false;

                return m_CanGetKey(keyType, pressType);
            }
        }

        public InputSystem(InputState inputState)
        {
            int keyCount;

            m_inputState = inputState;
            keyCount = Enum.GetValues(typeof(KeyType)).Length;
            m_keyMappingTable = new List<InputType>[keyCount];

            m_InitializeInstance();
        }

        public void Subscribe(KeyType keyType, InputType inputType)
        {
            int keyTypeInt = (int)keyType;

            if(m_keyMappingTable[keyTypeInt] == null)
                m_keyMappingTable[keyTypeInt] = new List<InputType>();

            if(!m_keyMappingTable[keyTypeInt].Contains(inputType))
                m_keyMappingTable[keyTypeInt].Add(inputType);
        }

        public void Unsubscribe(KeyType keyType, InputType inputType)
        {
            int keyTypeInt = (int)keyType;

            if(m_IsNullAssignedKey(keyType))
                return;

            if(m_keyMappingTable[keyTypeInt].Contains(inputType))
                m_keyMappingTable[keyTypeInt].Remove(inputType);
        }

        private bool m_IsNullAssignedKey(KeyType keyType)
        {
            int keyTypeInt = (int)keyType;

            return m_keyMappingTable[keyTypeInt] == null || m_keyMappingTable[keyTypeInt].Count == 0;
        }

        private bool m_CanGetKey(KeyType keyType, PressType pressType)
        {
            int keyTypeInt = (int)keyType;
            int i;

            for(i = 0; i < m_keyMappingTable[keyTypeInt].Count; i++)
                if(m_inputState[m_keyMappingTable[keyTypeInt][i], pressType])
                    return true;

            return false;
        }

        private void m_InitializeInstance()
        {
            this.Subscribe(KeyType.Jump, InputType.C);
            this.Subscribe(KeyType.Jump, InputType.Space);
            this.Subscribe(KeyType.MoveL, InputType.A);
            this.Subscribe(KeyType.MoveL, InputType.ArrowL);
            this.Subscribe(KeyType.MoveR, InputType.D);
            this.Subscribe(KeyType.MoveR, InputType.ArrowR);
            this.Subscribe(KeyType.Sit, InputType.S);
            this.Subscribe(KeyType.Sit, InputType.ArrowD);
            this.Subscribe(KeyType.HeadUp, InputType.W);
            this.Subscribe(KeyType.HeadUp, InputType.ArrowU);
            this.Subscribe(KeyType.Gliding, InputType.G);
            this.Subscribe(KeyType.Dash, InputType.ShiftL);
        }
    }
}