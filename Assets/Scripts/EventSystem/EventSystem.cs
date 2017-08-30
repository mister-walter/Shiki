using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvent {}

public delegate void GameEventHandler(GameObject sender, object arg);

public class GameEventSystem {
    public static Dictionary<Type, List<GameEventHandler>> eventMap;
    public static void AddListener(IEvent evt, GameEventHandler handler)
    {
        var evtType = evt.GetType();
        if(!eventMap.ContainsKey(evtType))
        {
            eventMap[evtType] = new List<GameEventHandler>();
        }
        var listenerList = eventMap[evtType];
        listenerList.Add(handler);
    }
}
