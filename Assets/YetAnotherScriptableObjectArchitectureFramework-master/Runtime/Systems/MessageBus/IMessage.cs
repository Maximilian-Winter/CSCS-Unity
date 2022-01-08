using System;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Systems.MessageBus
{



public interface IMessage
{
    Type MessageType
    {
        get;
    }
}

}
