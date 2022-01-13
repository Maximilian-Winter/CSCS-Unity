using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects.ScriptableArchitecture.Framework
{

public class VariableEventListener < T > : MonoBehaviour
{
    public VariableEvent < T > Event;
    public UnityEvent < T > Response;

    private void OnEnable()
    {
        Event.RegisterListener( this );
    }

    private void OnDisable()
    {
        Event.UnregisterListener( this );
    }

    public void OnEventRaised( T variable )
    {
        Response.Invoke( variable );
    }
}

}
