using System.Collections;
using UnityEngine;

namespace Behaviourtree
{
public class BehaviourTree : MonoBehaviour
{
    protected Node m_root;
    protected Hashtable m_blackboard;

    NodeState Update()
    {
        return m_root.Execute();
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
