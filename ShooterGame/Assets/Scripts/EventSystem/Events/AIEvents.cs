using UnityEngine;
namespace EventSystem
{
    public class AIMoveEvent : IEvent
    {
        GameObject m_gameObject;
        Vector3 m_direction;
        public AIMoveEvent(GameObject aGameObject, Vector3 direction) { m_direction = direction; m_gameObject = aGameObject; }
        public Vector3 GetDirection() { return m_direction; }
        public GameObject GetCallerGameObject() { return m_gameObject; }

        public string GetEventType() { return "AIMoveEvent"; }
        public static string EventType() { return "AIMoveEvent"; }
    }
}
