using System;
using System.Collections.Generic;

class StateMachine
{
    public int state
    {
        get
        {
            return m_state;
        }

        set
        {
            ChangeState(value);
        }
    }

    private int m_state;
    private Dictionary<int, Action> m_inputs;
    private Dictionary<int, Action> m_logics;
    private Dictionary<int, Action> m_enters;
    private Dictionary<int, Action> m_ends;

    public StateMachine(int initialState)
    {
        m_state = initialState;

        m_inputs = new Dictionary<int, Action>();
        m_logics = new Dictionary<int, Action>();
        m_enters = new Dictionary<int, Action>();
        m_ends = new Dictionary<int, Action>();
    }

    public void SetCallbacks(int state, Action func_input, Action func_logic, Action func_enter, Action func_end)
    {
        m_inputs.Add(state, func_input);
        m_logics.Add(state, func_logic);
        m_enters.Add(state, func_enter);
        m_ends.Add(state, func_end);
    }

    public void ChangeState(int nextState)
    {
        if(nextState == m_state)
            return;

        // TODO: 이 지점에서 정의되지 않은 상태를 입력받을 때 예외처리는 어떻게 할 것인가?

        if(m_ends[m_state] != null)
            m_ends[m_state]();

        m_state = nextState;

        if(m_enters[m_state] != null)
            m_enters[m_state]();
    }

    public void RestartState()
    {
        if(m_ends[m_state] != null)
            m_ends[m_state]();

        if(m_enters[m_state] != null)
            m_enters[m_state]();
    }

    public void UpdateInput()
    {
        if(m_inputs[m_state] != null)
            m_inputs[m_state]();
    }

    public void UpdateLogic()
    {
        if(m_logics[m_state] != null)
            m_logics[m_state]();
    }

    private void m_EnterState()
    {
        if(m_enters[m_state] != null)
            m_enters[m_state]();
    }

    private void m_EndState()
    {
        if(m_ends[m_state] != null)
            m_ends[m_state]();
    }
}