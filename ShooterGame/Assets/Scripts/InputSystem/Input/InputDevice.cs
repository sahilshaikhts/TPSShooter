using EventSystem;
using UnityEngine;

namespace InputSystem
{
[System.Serializable]
public class InputDevice
{
[SerializeField]private KeyBindings m_keyBindings;

public void Update()
{
    foreach (InputKey input in m_keyBindings.Input)
    {
        switch (input.m_inputKeyType)
        {
            case InputKeyType.AxisInput:
                {
                    Vector3 axisInput = input.GetAxisInput();
                    if (axisInput != Vector3.zero) 
                    { 
                        BrodcastAxisInputEvent(input.m_action, axisInput);
                    }
                    break;
                }
            default:
                if (input.Performed())
                    BrodcastInputEvent(input.m_action);
                break;
        }
    }
}

private void BrodcastInputEvent(string action) {GameManager.instance.GetEventManager().AddEvent(new InputEvent(action));}
private void BrodcastAxisInputEvent(string action,Vector3 value) {GameManager.instance.GetEventManager().AddEvent(new AxisInputEvent(action, value));}

public void SetKeyBindings(KeyBindings aKeyBindings) { m_keyBindings = aKeyBindings; }
public KeyBindings GetKeyBindings() { return m_keyBindings; }
}
}
