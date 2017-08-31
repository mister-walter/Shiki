/// @author Andrew Walter
// Heavily inspired by http://www.willrmiller.com/?p=87

using System;
using System.Collections.Generic;

namespace Shiki.EventSystem {
    /// <summary>
    /// This is a "marker interface" to signal that a class represents a Game Event.
    /// </summary>
    public interface IGameEvent { }

    /// <summary>
    /// Global object for listening for events, and firing them.
    /// </summary>
    public class GameEventSystem {
        private static Dictionary<Type, Delegate> eventMap = new Dictionary<Type, Delegate>();

        /// <summary>
        /// Add a listener for an event. 
        /// The event to listen for is determined through the generic type parameter of the given Action
        /// </summary>
        /// <typeparam name="T">The IGameEvent to attach the given delegate to</typeparam>
        /// <param name="del">The Action to perform when the event occurs</param>
        public static void AttachDelegate<T>(Action<T> del) where T : IGameEvent
        {
            var evtType = typeof(T);
            if (!eventMap.ContainsKey(evtType)) {
                eventMap[evtType] = del;
            } else {
                eventMap[evtType] = Delegate.Combine(eventMap[evtType], del);
            }
        }

        /// <summary>
        /// Remove a listener from an event. 
        /// The event to remove from is determined through the generic type parameter of the given Action
        /// </summary>
        /// <typeparam name="T">The IGameEvent to remove the given delegate from</typeparam>
        /// <param name="del">The Action to remove from the event</param>
        public static void RemoveDelegate<T>(Action<T> del) where T : IGameEvent
        {
            var evtType = typeof(T);
            if (eventMap.ContainsKey(evtType))
            {
                var newDel = Delegate.Remove(eventMap[evtType], del);
                if (newDel == null)
                {
                    eventMap.Remove(evtType);
                } else
                {
                    eventMap[evtType] = newDel;
                }
            }
        }

        /// <summary>
        /// Causes an event to fire, causing any listening delegates to be run.
        /// </summary>
        /// <param name="evt">The event to fire. This is passed on to the delegates, so this is how to pass information to the delegates.</param>
        public static void FireEvent(IGameEvent evt)
        {
            var evtType = evt.GetType();
            if (eventMap.ContainsKey(evtType))
            {
                eventMap[evtType].DynamicInvoke(evt);
            }
        }
    }
}
