using System;
using System.Collections.Generic;

public class UIEventSystem
{
    private static Dictionary<string, Action<object>> eventHandlers = 
        new Dictionary<string, Action<object>>();

    public static void Register(string eventName, Action<object> handler)
    {
        if (!eventHandlers.ContainsKey(eventName))
        {
            eventHandlers[eventName] = handler;
        }
        else
        {
            eventHandlers[eventName] += handler;
        }
    }

    public static void Unregister(string eventName, Action<object> handler)
    {
        if (eventHandlers.ContainsKey(eventName))
        {
            eventHandlers[eventName] -= handler;
        }
    }

    public static void Broadcast(string eventName, object data = null)
    {
        if (eventHandlers.TryGetValue(eventName, out var handler))
        {
            handler?.Invoke(data);
        }
    }
} 