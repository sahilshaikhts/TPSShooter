using Behaviourtree;

public class SetValue<T> : Node
{
    private string key_variable;
    private string key_value;
    T m_value;
    public SetValue(BehaviourTree aTree, string aKeyVariableToChange,string aKeyValue) : base(aTree) 
    {
        key_variable = aKeyVariableToChange;
        key_value = aKeyValue;
    }

    public SetValue(BehaviourTree aTree, string aKeyVariableToChange, T aValue) : base(aTree)
    {
        key_variable = aKeyVariableToChange;
        m_value = aValue;
    }

    public override NodeState Execute()
    {
        //if the variable in blackboard that has the value is not set try set it through the member variable m_value
        if (key_value == null)
        {
            m_tree.SetData(key_variable, m_value);
        }
        else
        {
            //Get the value from the blackboard and then set
            m_tree.SetData(key_variable, (T)GetData(key_value));
        }
     
        return NodeState.Sucessful;
    }
}
