using EventSystem;
using UnityEngine;

public class PlayerController
{
    GameObject m_owner;
    GameObject m_camera;
    EventManager m_eventManager;

    public PlayerController(GameObject aOwner, GameObject aCamera) 
    {
        m_owner = aOwner;
        m_camera = aCamera;

        m_eventManager = GameManager.instance.GetEventManager();
        m_eventManager.SubscribeToEvent(InputEvent.EventType(), HandleInputEvent);
        m_eventManager.SubscribeToEvent(AxisInputEvent.EventType(), HandleAxisInputEvent);
    }

    public void HandleInputEvent(IEvent gameEvent)
    {
        InputEvent inputEvent = (InputEvent)gameEvent;
        switch (inputEvent.GetAction())
        {
            case "Jump":
                m_eventManager.AddEvent(new JumpEvent(m_owner));
                break;
            case "Fire":
                m_eventManager.AddEvent(new WeaponFireEvent(m_owner));
                break;
        }
    }

    public void HandleAxisInputEvent(IEvent gameEvent)
    {
        AxisInputEvent inputEvent = (AxisInputEvent)gameEvent;
        switch (inputEvent.GetAction())
        {
            case "Movement":
                Vector2 value = inputEvent.GetValue();
                Vector3 direction = m_camera.transform.right * value.x + m_owner.transform.forward * value.y;

                m_eventManager.AddEvent(new MoveEvent(m_owner, direction));
                break;
        }
    }
    void SetCamera(GameObject aCamera) { m_camera = aCamera; }
}
