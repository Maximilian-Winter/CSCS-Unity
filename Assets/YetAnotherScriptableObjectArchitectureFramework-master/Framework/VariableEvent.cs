using System.Collections.Generic;
using OdinSerializer;
using UnityEngine;

public class VariableEvent<T> : SerializedScriptableObject
{
    private List<VariableEventListener<T>> listeners =
        new List<VariableEventListener<T>>();

    private int listenerCounter = 0;

    public int ListenerCounter
    {
        get => listenerCounter;
    }

    public void Raise(T variableEvent)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(variableEvent);
    }

    public void RegisterListener(VariableEventListener<T> listener)
    {
        listeners.Add(listener);
        listenerCounter = listeners.Count;
    }

    public void UnregisterListener(VariableEventListener<T> listener)
    {
        listeners.Remove(listener);
        listenerCounter = listeners.Count;
    }
}