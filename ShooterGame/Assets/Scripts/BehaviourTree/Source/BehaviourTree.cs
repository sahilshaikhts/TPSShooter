using System.Collections;
using UnityEngine;

namespace Behaviourtree
{
public class BehaviourTree : MonoBehaviour
{
    protected Node m_root;
    protected Hashtable m_blackboard;
    private void OnEnable()
    {
        m_blackboard=new Hashtable();
    }
    protected NodeState UpdateNodes()
    {
        return m_root.Execute();
    }


    public void AddKey(string key)
    {
        if (m_blackboard.ContainsKey(key) == false)
        {
            m_blackboard.Add(key, null);
        }
    }
    public void AddKey(string key,Object aValue)
    {
        if (m_blackboard.ContainsKey(key) == false)
        {
            m_blackboard.Add(key, aValue);
        }
    }
    public object GetData(string aKey)
    {
       if( m_blackboard.ContainsKey(aKey))
       {
            return m_blackboard[aKey];
       }
            return null;
    }

    public void SetData(string aKey,object aData)
    {
        m_blackboard[aKey] = aData;
    }
}
}
