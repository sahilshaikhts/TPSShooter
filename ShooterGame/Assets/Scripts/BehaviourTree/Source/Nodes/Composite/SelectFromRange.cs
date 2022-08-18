using UnityEngine;

namespace Behaviourtree
{
    /// <summary>
    /// SelectFromRange class restricts which nodes are allowed to be executed.
    /// For 'n' child node, if the value is between the corresponding range in the range array then it will be executed.
    /// Return value similar to Selector.
    /// </summary>
    public class SelectFromRange : Node
    {
        Vector2[] m_range;
        string key_value;

        /// <summary>
        /// For an 'n' child node to be executed,the value of given variable must be within the range provided through the constructor corresponding to the nth index of the array. 
        /// </summary>
        /// <param name="aTree"></param>
        /// <param name="range">Pass in range of value for each relative 'n' child node. Vector2(MIN,MAX)</param>
        public SelectFromRange(BehaviourTree aTree, Vector2[] aRange, string aKeyValueToCheck) : base(aTree)
        {
            key_value = aKeyValueToCheck;
            m_range = aRange;
        }

        public override NodeState Execute()
        {
            float valueToCheck=(float)GetData(key_value);
            for (int i = 0; i < m_childNodes.Count; i++)
            {
                if (valueToCheck >= m_range[i].x && valueToCheck < m_range[i].y)
                {
                    switch (m_childNodes[i].Execute())
                    {
                        case NodeState.Sucessful:
                            return NodeState.Sucessful;
                        case NodeState.Failed:
                            break;
                        case NodeState.Running:
                            return NodeState.Running;
                    }
                }
            }
            return NodeState.Failed;
        }

    }
}