using System.Collections.Generic;
using OdinSerializer;

namespace ScriptableObjects.ScriptableArchitecture.Framework
{

public class VariableEvent < T > : SerializedScriptableObject
{
    private int listenerCounter = 0;

    private List < VariableEventListener < T > > listeners =
        new List < VariableEventListener < T > >();

    public int ListenerCounter => listenerCounter;

    public void Raise( T variableEvent )
    {
        for ( int i = listeners.Count - 1; i >= 0; i-- )
        {
            listeners[i].OnEventRaised( variableEvent );
        }
    }

    public void RegisterListener( VariableEventListener < T > listener )
    {
        listeners.Add( listener );
        listenerCounter = listeners.Count;
    }

    public void UnregisterListener( VariableEventListener < T > listener )
    {
        listeners.Remove( listener );
        listenerCounter = listeners.Count;
    }
}

}
