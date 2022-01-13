using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CSCS;
using SplitAndMerge;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Runtime.Systems.MessageBus
{

public class MessageBusProxyObject : Variable, ScriptObject
{
    public MessageBusProxyObject()
    {
        if ( MessageTypes == null )
        {
            MessageTypes = Assembly
                           .GetAssembly(typeof(Message))
                           .GetTypes()
                           .Where(t => t.IsSubclassOf(typeof(Message)));
        }
    }
    private static IEnumerable < Type > MessageTypes = null; 
    private static readonly List<string> s_properties = new()
    {
        "GetMessageQueue", "PublishMessage", "SubscribeToMessage"
    };
   
    public Task<Variable> GetProperty(string sPropertyName,
        List<Variable> args = null, ParsingScript script = null)
    {
        var newValue = Variable.EmptyInstance;
        var mre = new ManualResetEvent(false);
        CscsScriptingController.ExecuteInUpdate(() =>
        {
            switch (sPropertyName)
            {
                case "GetMessageQueue":
                    if (args != null && args.Count > 1 )
                    {
                        if(MessageBus.Instance.RecipientsToMessages.ContainsKey( args[0] ))
                        {
                            newValue = new Variable(MessageBus.Instance.RecipientsToMessages[args[0]][args[1].AsString()]);
                        }
                        else
                        {
                            Debug.Log( "Message Recipient not found!" );
                        }
                    }
                    break;
                case "SubscribeToMessage":
                    if (args != null && args.Count > 1 )
                    {
                        foreach ( Type messageType in MessageTypes )
                        {
                            if ( args[1].AsString() == messageType.Name  )
                            {
                                    
                                Variable variable = Utils.GetVariable(args[0].AsString(), script);
                                variable.MessageTypesToCallbackFunctions.Add(args[1].AsString(), new List<string>());
                                variable.MessageTypesToCallbackFunctions[args[1].AsString()].Add(args[0].AsString());
                                MessageBus.SubscribeToAllMessagesOfType( variable, messageType );
                            }
                        }
                            
                    }
                    break;
                default:
                    newValue = Variable.EmptyInstance;
                    break;
            }
            mre.Set();
        });

        mre.WaitOne();

        return Task.FromResult(newValue);
    }

    public Task<Variable> SetProperty(string sPropertyName, Variable argValue)
    {
        var newValue = Variable.EmptyInstance;
        /*
         switch (sPropertyName)
         {
            default:
               break;
         }
   */
        return Task.FromResult(newValue);
    }

    public List<string> GetProperties()
    {
        return s_properties;
    }
}

}
