using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSCS;
using ScriptableObjects.ScriptableArchitecture.Systems.MessageBus;
using SplitAndMerge;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Runtime.Systems.MessageBus
{

public class CscsMessageQueueObject : Queue<IMessage>, ScriptObject
{
    private static readonly List<string> s_properties = new()
    {
        "DequeueMessage", "QueueLength"
    };

    public Task<Variable> SetProperty(string name, Variable value)
    {
        var newValue = Variable.EmptyInstance;
        /*switch (name)
        {
           default:
              break;
        }*/
        return Task.FromResult(newValue);
    }

    public Task<Variable> GetProperty(string name, List<Variable> args = null, ParsingScript script = null)
    {
        var newValue = Variable.EmptyInstance;
        switch (name)
        { 
            case "DequeueMessage":
                IMessage message = Dequeue();
                newValue = new Variable(message);
                break;
            case "QueueLength":
                if ( script != null )
                {
                    newValue = new Variable(Count);
                }
                break;
            default:
                break;
        }
              
        return Task.FromResult(newValue);
    }

    public List<string> GetProperties()
    {
        return s_properties;
    }
}

}
