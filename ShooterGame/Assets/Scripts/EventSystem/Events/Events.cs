using ShootingGame;
using UnityEngine;

namespace EventSystem
{
public class MoveEvent : IEvent
{
    Character m_gameObject;
    Vector3 m_direction;
    public MoveEvent(Character aGameObject,Vector3 direction) { m_direction = direction; m_gameObject = aGameObject; }
    public Vector3 GetDirection() { return m_direction; }
    public Character GetCallerGameObject() { return m_gameObject; }

    public string GetEventType() { return "MoveEvent"; }
    public static string EventType() { return "MoveEvent"; }
}

public class JumpEvent : IEvent
{
    Character m_callerCharacter;
    public JumpEvent(Character aCallerCharacter) { m_callerCharacter = aCallerCharacter; }
    public Character GetCallerCharacter() { return m_callerCharacter; }

    public string GetEventType() { return "JumpEvent"; }
    public static string EventType() { return "JumpEvent"; }
}

public class RotateEvent : IEvent
{
    Character m_callerCharacter;
    Vector3 m_amount;

    public RotateEvent(Character aCallerCharacter, Vector3 aAmount) { m_callerCharacter = aCallerCharacter; m_amount = aAmount; }
    public Character GetCallerCharacter() { return m_callerCharacter; }

    public string GetEventType() { return "RotateEvent"; }
    public static string EventType() { return "RotateEvent"; }
}

public class SetPlayerRotationEvent : IEvent
{
    Vector3 m_rotation;

    public SetPlayerRotationEvent(Vector3 aRotation) {m_rotation = aRotation; }

    public Vector3 GetRotation() { return m_rotation; }

    public string GetEventType() { return "SetPlayerRotationEvent"; }
    public static string EventType() { return "SetPlayerRotationEvent"; }
}

public class InputEvent : IEvent
{
    string m_action;
    public InputEvent(string action) { m_action = action; }
    public string GetAction() { return m_action; }

    public string GetEventType() { return "InputEvent"; }
    public static string EventType() { return "InputEvent"; }
}

public class AxisInputEvent : IEvent
{
    string m_action;
    Vector2 m_value;

    public AxisInputEvent(string action, Vector2 value){ m_action = action; m_value = value; }

    public string GetAction() { return m_action; }
    public Vector2 GetValue() { return m_value; }

    public string GetEventType() { return "AxisInputEvent"; }
    public static string EventType() { return "AxisInputEvent"; }
}

public class ToggleInputEvent : IEvent
{
    bool m_value;
    public ToggleInputEvent(bool value) { m_value = value; }
    public bool GetState() { return m_value; }

    public string GetEventType() { return "ToggleInputEvent"; }
    public static string EventType() { return "ToggleInputEvent"; }
}

public class WeaponDrawEvent : IEvent
{
    Character m_callerCharacter;

    public WeaponDrawEvent(Character aCallerCharacter) { m_callerCharacter = aCallerCharacter; }
    public Character GetCallerCharacter() { return m_callerCharacter; }

    public string GetEventType() { return "WeaponDrawEvent"; }
    public static string EventType() { return "WeaponDrawEvent"; }
}

public class WeaponFireEvent : IEvent
{
    Character m_callerCharacter;

    public WeaponFireEvent(Character aCallerCharacter) { m_callerCharacter = aCallerCharacter; }
    public Character GetCallerCharacter() { return m_callerCharacter; }

    public string GetEventType() { return "WeponFireEvent"; }
    public static string EventType() { return "WeponFireEvent"; }
}
public class PlayerAboutToShootEvent : IEvent
{
    Character m_playersTarget;

    public PlayerAboutToShootEvent(Character aPlayersTarget) { m_playersTarget = aPlayersTarget; }
    public Character GetPlayersTarget() { return m_playersTarget; }

    public string GetEventType() { return "PlayerAboutToShootEvent"; }
    public static string EventType() { return "PlayerAboutToShootEvent"; }
}

}
