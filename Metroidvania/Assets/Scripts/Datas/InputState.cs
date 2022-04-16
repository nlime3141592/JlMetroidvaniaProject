using System;

using UnityEngine;

namespace JlMetroidvaniaProject.Datas
{
    public sealed class InputState : StateBase
    {
        public event Action evhdlr_checkEvent;

        private readonly int m_inputTypeCount;
        private readonly int m_pressTypeCount;

        private readonly KeyCode[] m_keyCodeMappingTable; // InputType -> UnityEngine.KeyCode function.
        private readonly bool[] m_keyCodeStateTable; // record function call result of Input.GetKeyXXX(Keycode) => bool function.

        // Function Requires
        // 1. InputType -> UnityEngine.KeyCode mapping table
        // 2. parse Pressing state, but parameter is InputType.
        //    so, requires InputType -> UnityEngine.KeyCode and calls Input.GetKeyXXX(KeyCode) => bool.
        // 3. index: (InputType) * (PressTypeCount) + (PressType)

        public bool this[InputType inputType, PressType pressType]
        {
            get
            {
                if(m_keyCodeMappingTable == null)
                    throw new NullReferenceException("Key table doesn't exist.");

                if(inputType == InputType.None) return false;

                return m_keyCodeStateTable[m_GetStateIndex(inputType, pressType)];
            }
        }

        public InputState()
        {
            m_inputTypeCount = Enum.GetValues(typeof(InputType)).Length;
            m_pressTypeCount = Enum.GetValues(typeof(PressType)).Length;

            m_keyCodeMappingTable = new KeyCode[m_inputTypeCount];
            m_keyCodeStateTable = new bool[m_inputTypeCount * m_pressTypeCount];

            int i;

            for(i = 0; i < m_inputTypeCount; i++)
            {
                m_keyCodeMappingTable[i] = m_InputTypeToKeyCode((InputType)i);
            }
        }

        private static KeyCode m_InputTypeToKeyCode(InputType inputType)
        {
            int inputTypeNum = (int)inputType;

            if(inputTypeNum == (int)InputType.None)
            {
                return KeyCode.None;
            }
            else if(inputTypeNum >= (int)InputType.A && inputTypeNum <= (int)InputType.Z)
            {
                int difference = inputTypeNum - (int)InputType.A;
                return (KeyCode)((int)KeyCode.A + difference);
            }
            else if(inputTypeNum >= (int)InputType.Key0 && inputTypeNum <= (int)InputType.Key9)
            {
                int difference = inputTypeNum - (int)InputType.Key0;
                return (KeyCode)((int)KeyCode.Alpha0 + difference);
            }
            else if(inputTypeNum >= (int)InputType.Num0 && inputTypeNum <= (int)InputType.Num9)
            {
                int difference = inputTypeNum - (int)InputType.Num0;
                return (KeyCode)((int)KeyCode.Keypad0 + difference);
            }
            else if(inputTypeNum >= (int)InputType.ArrowU && inputTypeNum <= (int)InputType.ArrowL)
            {
                int difference = inputTypeNum - (int)InputType.ArrowU;
                return (KeyCode)((int)KeyCode.UpArrow + difference);
            }
            else
            {
                switch(inputType)
                {
                    case InputType.ShiftL: return KeyCode.LeftShift;
                    case InputType.ShiftR: return KeyCode.RightShift;
                    case InputType.CtrlL: return KeyCode.LeftControl;
                    case InputType.CtrlR: return KeyCode.RightControl;
                    case InputType.Space: return KeyCode.Space;
                    case InputType.Escape: return KeyCode.Escape;
                    case InputType.Enter: return KeyCode.Return;
                    case InputType.Tab: return KeyCode.Tab;
                    default: throw new ArgumentException("Invalid InputType Value.");
                }
            }
        }

        private int m_GetStateIndex(InputType inputType, PressType pressType)
        {
            return (int)inputType * m_pressTypeCount + (int)pressType;
        }

        public override void Update()
        {
            if(evhdlr_checkEvent != null)
                evhdlr_checkEvent();
                
            m_UpdateKeys();
        }

        private void m_UpdateKeys()
        {
            int i;
            
            for(i = 0; i < m_inputTypeCount; i++)
            {
                m_keyCodeStateTable[m_GetStateIndex((InputType)i, PressType.Down)] = Input.GetKeyDown(m_keyCodeMappingTable[i]);
                m_keyCodeStateTable[m_GetStateIndex((InputType)i, PressType.Pressing)] = Input.GetKey(m_keyCodeMappingTable[i]);
                m_keyCodeStateTable[m_GetStateIndex((InputType)i, PressType.Up)] = Input.GetKeyUp(m_keyCodeMappingTable[i]);
            }
        }
    }
}