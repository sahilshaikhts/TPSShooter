using Behaviourtree;
using System;
using UnityEngine;

/// <summary>
///m_duration class can be used as a condition before executing other nodes.
///When executed for the first time, the countdown begins and when checked next time, it will return Failed if timer isn't 0 otherwise it will reset and return success.
/// </summary>
public class Countdown : Node
{
    private float m_startTime=-1;
     
    private float m_duration;
    private string key_duration;

    public Countdown(BehaviourTree aTree,float aDuration): base(aTree)
    {
        m_duration = aDuration;
    }
    public Countdown(BehaviourTree aTree, string aKeyDuration) : base(aTree)
    {
        key_duration = aKeyDuration;
    }
    public override NodeState Execute()
    {
        //if countdown is in progress.
        if(m_startTime!=-1)
        {
            float timeElapsed=Time.time-m_startTime;
            if(timeElapsed>m_duration)
            {
                m_state = NodeState.Sucessful;
                m_startTime = -1;
            }else
            {
                m_state = NodeState.Failed;
            }
        }else
        {
            m_startTime = Time.time;
            m_state = NodeState.Failed;
        }

        return m_state;
    }
}
