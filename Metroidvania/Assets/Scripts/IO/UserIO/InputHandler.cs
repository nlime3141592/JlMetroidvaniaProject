using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class InputHandler
{
    #region Input States
    // Input states
    public static InputData data => s_m_data;
    private static InputData s_m_data;
    #endregion

    #region Key Code Index Definitions
    public const int idx_MoveLeft = 0;
    public const int idx_MoveRight = 1;
    public const int idx_Sit = 2;
    public const int idx_HeadUp = 3;
    public const int idx_Jump = 4;
    public const int idx_Dash = 5;

    #endregion

    #region Key Code Definitions
    // Key code definitions
    private const int c_keyCount = 20;
    private static KeyCode[] keys;

    #endregion

    #region Static Constructor
    static InputHandler()
    {
        int i;

        keys = new KeyCode[c_keyCount];

        for(i = 0; i < c_keyCount; i++)
            keys[i] = KeyCode.None;

        keys[0] = KeyCode.LeftArrow;
        keys[1] = KeyCode.RightArrow;
        keys[2] = KeyCode.DownArrow;
        keys[3] = KeyCode.UpArrow;
        keys[4] = KeyCode.Space;
        keys[5] = KeyCode.LeftShift;
    }

    #endregion

    #region Utilities
    // Utility functions
    // 조작키 변경을 위해 만들어둔 함수.
    public static bool ChangeKey(int idx_key, KeyCode key)
    {
        int i;

        for(i = 0; i < keys.Length; i++)
        {
            if(i == idx_key || keys[i] == KeyCode.None)
                continue;
            else if(key == keys[idx_key])
            {
                // TODO:
                // KeyCode.None을 할당하는 방식은 다른 조작키에 할당된 키보드 키를 제거하고 새로 설정하고자 하는 조작키에 키보드 키를 등록하는 방식이다.
                // false를 반환하는 방식은 다른 조작키에 할당된 키보드 키가 이미 있다면 함수 자체를 종료해버리는 방식이다. return값 false는 키 변경 실패, true는 키 변경 성공을 의미한다.
                // 선택적으로 주석 해제하여 적용하면 된다.
                // keys[i] = KeyCode.None;
                // return false;
            }
        }

        keys[idx_key] = key;
        return true;
    }

    #endregion

    #region Update Logics
    // Update logics
    // InputManager 클래스에서 수행함.
    public static void Update()
    {
        s_m_data.xNegDown = Input.GetKeyDown(keys[idx_MoveLeft]);
        s_m_data.xNegUp = Input.GetKeyUp(keys[idx_MoveLeft]);
        s_m_data.xPosDown = Input.GetKeyDown(keys[idx_MoveRight]);
        s_m_data.xPosUp = Input.GetKeyUp(keys[idx_MoveRight]);
        s_m_data.xNegative = Input.GetKey(keys[idx_MoveLeft]) ? -1 : 0;
        s_m_data.xPositive = Input.GetKey(keys[idx_MoveRight]) ? 1 : 0;
        s_m_data.xInput = s_m_data.xNegative + s_m_data.xPositive;

        s_m_data.yNegDown = Input.GetKeyDown(keys[idx_Sit]);
        s_m_data.yNegUp = Input.GetKeyUp(keys[idx_Sit]);
        s_m_data.yPosDown = Input.GetKeyDown(keys[idx_HeadUp]);
        s_m_data.yPosUp = Input.GetKeyUp(keys[idx_HeadUp]);
        s_m_data.yNegative = Input.GetKey(keys[idx_Sit]) ? -1 : 0;
        s_m_data.yPositive = Input.GetKey(keys[idx_HeadUp]) ? 1 : 0;
        s_m_data.yInput = s_m_data.yNegative + s_m_data.yPositive;

        s_m_data.jumpDown = Input.GetKeyDown(keys[idx_Jump]);
        s_m_data.jumpUp = Input.GetKeyUp(keys[idx_Jump]);
        s_m_data.jumpPressing = Input.GetKey(keys[idx_Jump]);

        s_m_data.dashDown = Input.GetKeyDown(keys[idx_Dash]);
        s_m_data.dashUp = Input.GetKeyUp(keys[idx_Dash]);
        s_m_data.dashPressing = Input.GetKey(keys[idx_Dash]);
    }

    #endregion

    #region Debugging
    private static StringBuilder msg;
    public static string GetDatas()
    {
        if(msg == null)
            msg = new StringBuilder();

        msg.Clear();

        msg.Append("// ");
        msg.Append(s_m_data.xNegDown ? "1 " : "0 ");
        msg.Append(s_m_data.xNegUp ? "1 " : "0 ");
        msg.Append(s_m_data.xPosDown ? "1 " : "0 ");
        msg.Append(s_m_data.xPosUp ? "1 " : "0 ");
        msg.Append(s_m_data.xNegative == 0 ? "0 " : "-1 ");
        msg.Append(s_m_data.xPositive == 0 ? "0 " : "1 ");
        msg.Append(s_m_data.xInput == 0 ? "0 " : s_m_data.xInput.ToString() + " ");
        msg.Append("// ");

        msg.Append(s_m_data.yNegDown ? "1 " : "0 ");
        msg.Append(s_m_data.yNegUp ? "1 " : "0 ");
        msg.Append(s_m_data.yPosDown ? "1 " : "0 ");
        msg.Append(s_m_data.yPosUp ? "1 " : "0 ");
        msg.Append(s_m_data.yNegative == 0 ? "0 " : "-1 ");
        msg.Append(s_m_data.yPositive == 0 ? "0 " : "1 ");
        msg.Append(s_m_data.yInput == 0 ? "0 " : s_m_data.yInput.ToString() + " ");
        msg.Append("// ");

        msg.Append(s_m_data.jumpDown ? "1 " : "0 ");
        msg.Append(s_m_data.jumpUp ? "1 " : "0 ");
        msg.Append(s_m_data.jumpPressing ? "1 " : "0 ");
        msg.Append("// ");

        msg.Append(s_m_data.dashDown ? "1 " : "0 ");
        msg.Append(s_m_data.dashUp ? "1 " : "0 ");
        msg.Append(s_m_data.dashPressing ? "1 " : "0 ");
        msg.Append("// ");

        return msg.ToString();
    }
    #endregion
}
