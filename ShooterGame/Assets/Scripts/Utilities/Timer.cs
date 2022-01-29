using System;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    ~Timer()
    {
        m_timedActions.Clear();
    }

    private List<TimedAction> m_timedActions = new List<TimedAction>();

    public void Update()
    {
        for (int i = 0; i < m_timedActions.Count; i++)
        {
            if(m_timedActions[i].StepTimer())//If timer went off and action executed
            {
                m_timedActions.RemoveAt(i);
            }
        }
    }

    public void TimedExecution(Action aMethod,float aTimer)
    {
        m_timedActions.Add(new TimedAction(aMethod,aTimer));
    }
}

public class TimedAction
{
    float m_timer;
    Action m_action;
    public TimedAction(Action aAction, float aTimer)
    {
        m_action = aAction;
        m_timer = aTimer;
    }
    public bool StepTimer()    //Returns true if timer's off and action has been invoked
    {
        if (m_timer > 0)
        {
            m_timer -= Time.deltaTime;
            return false;
        }
        else
        {
            if (m_action != null)
            {
                m_action.Invoke();
            }
            return true;
        }
    }
}