using UnityEngine;
using UnityEngine.Events;

public class VariableEventListener<T> : MonoBehaviour
{
    public VariableEvent<T> Event;
    public UnityEvent<T> Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised(T variable)
    {
        Response.Invoke(variable);
    }
}