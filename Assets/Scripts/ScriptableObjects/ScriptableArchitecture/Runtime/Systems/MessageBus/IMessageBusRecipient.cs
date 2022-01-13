using System;
using ScriptableObjects.ScriptableArchitecture.Runtime.Systems.MessageBus;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Systems.MessageBus
{
public interface IMessageBusRecipient
{
    public void ReceiveMessage < T >( T message ) where T : IMessage;
}

}
