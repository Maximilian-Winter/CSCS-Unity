using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SplitAndMerge;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Systems.MessageBus
{



public interface IMessage : ScriptObject
{
    IMessage ConstructFromCscsVariable( IMessageBusSender messageBusSender, Variable variable );

    Type MessageType
    {
        get;
    }
    
    IMessageBusSender MessageBusSender
    {
        get;
    }

    abstract Task < Variable > ScriptObject.SetProperty( string name, Variable value );

    abstract Task < Variable > ScriptObject.GetProperty( string name, List < Variable > args, ParsingScript script );

    abstract List < string > ScriptObject.GetProperties();
}

}
