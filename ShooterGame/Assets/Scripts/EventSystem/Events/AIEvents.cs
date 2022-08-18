using ShootingGame;
using UnityEngine;
namespace EventSystem
{
    public class AIMoveEvent : IEvent
    {
        Character m_character;
        Vector3 m_direction;
        public AIMoveEvent(Character aCharacter, Vector3 aDirection) { m_direction = aDirection; m_character = aCharacter; }
        public Vector3 GetDirection() { return m_direction; }
        public Character GetCaller() { return m_character; }

        public string GetEventType() { return "AIMoveEvent"; }
        public static string EventType() { return "AIMoveEvent"; }
    }

    public class AIShootTargetEvent : IEvent
    {
        Character m_character;
        Vector3 m_targetDirection;
        public AIShootTargetEvent(Character aCharacter, Vector3 aTargetDirection) { m_targetDirection = aTargetDirection; m_character = aCharacter; }
        public Vector3 GetDirection() { return m_targetDirection; }
        public Character GetCaller() { return m_character; }

        public string GetEventType() { return "AIShootTargetEvent"; }
        public static string EventType() { return "AIShootTargetEvent"; }
    }

    public class AILookTowardsEvent : IEvent
    {
        Character m_character;
        Vector3 m_lookDirection;
        public AILookTowardsEvent(Character aCharacter, Vector3 aTargetDirection) { m_lookDirection = aTargetDirection; m_character = aCharacter; }
        public Vector3 GetDirection() { return m_lookDirection; }
        public Character GetCaller() { return m_character; }

        public string GetEventType() { return "AILookTowardsEvent"; }
        public static string EventType() { return "AILookTowardsEvent"; }
    }
}
