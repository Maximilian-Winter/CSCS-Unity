using System;
using SplitAndMerge;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Systems.MessageBus
{



public interface IMessage : ScriptObject
{
    Type MessageType
    {
        get;
    }
}

}
