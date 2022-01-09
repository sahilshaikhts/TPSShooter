using System;
using System.Collections.Generic;
using UnityEngine;

namespace EventSystem
{
    public delegate void EventHandleMethod(IEvent aEvent);

    public class EventManager
    {
        Dictionary<string, EventHandleMethod> m_subsribers;
        List<IEvent>[] m_eventQueue;
        int m_currentNewEventQueueIndex = 0;

        public EventManager()
        {
            m_eventQueue = new List<IEvent>[2];
            m_eventQueue[0] = new List<IEvent>();
            m_eventQueue[1] = new List<IEvent>();

            m_subsribers = new Dictionary<string, EventHandleMethod>();
        }

        public void Update() { NotifySubsribers(); }

        void NotifySubsribers()
        {
            int queueBeingReleasedIndex = m_currentNewEventQueueIndex;
            m_currentNewEventQueueIndex = (m_currentNewEventQueueIndex == 0) ? 1 : 0;//swaps the queue index to the empty queue so any eventHandle function adding another event wont freeze the game

            foreach (var currentEvent in m_eventQueue[queueBeingReleasedIndex])
            {
                if (m_subsribers.ContainsKey(currentEvent.GetEventType()))
                {
                    EventHandleMethod handleMethod;

                    m_subsribers.TryGetValue(currentEvent.GetEventType(), out handleMethod);

                    if (handleMethod == null) Debug.LogError("No subsriber found for event type: " + m_eventQueue[0][0].GetEventType());

                    handleMethod(currentEvent);
                }
            }
            m_eventQueue[queueBeingReleasedIndex].Clear();
        }

        public void AddEvent(IEvent aEvent)
        {
            m_eventQueue[m_currentNewEventQueueIndex].Add(aEvent);
        }

        public void SubscribeToEvent(string aEventType, EventHandleMethod aEventHandleMethod)
        {
            if (m_subsribers.ContainsKey(aEventType))
            {
                m_subsribers[aEventType] += aEventHandleMethod;
            }
            else
            {
                m_subsribers.Add(aEventType, aEventHandleMethod);
            }
        }

    }

    public interface IEvent
    {
        /// <summary>
        /// Return class name as string ,and make sure to add "static string EventType()" ,returning the same string.
        /// </summary>
        public string GetEventType();
    }
}
