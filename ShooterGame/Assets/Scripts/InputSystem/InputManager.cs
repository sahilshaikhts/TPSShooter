using EventSystem;
using System.Collections.Generic;
using UnityEngine;

namespace InputSystem
{
public class InputManager
{
    private bool m_inputEnabled = true;
    private List<InputDevice> m_inputDevices;

    public InputManager()
    {
        m_inputDevices=new List<InputDevice>();
        GameManager.instance.GetEventManager().SubscribeToEvent(ToggleInputEvent.EventType(), HandleToggleInputEvent);
    }

    public void Update()
    {
        if(m_inputEnabled)
        {
            foreach (InputDevice device in m_inputDevices)
            {
                device.Update();
            }
        }
    }

    public void HandleToggleInputEvent(IEvent aGameEvent)
    {
        ToggleInputEvent interruptEvent = (ToggleInputEvent)aGameEvent;
        m_inputEnabled = interruptEvent.GetState();
    }

    public void AddInputDevice(InputDevice aInputDevice)
    {
        if(m_inputDevices.Contains(aInputDevice))
        {
            Debug.LogWarning("The input device you're trying to add already exists");
            return;
        }
        m_inputDevices.Add(aInputDevice);
    }

    public void ClearInputDevices()
    {
        m_inputDevices.Clear();
    }

}
}
