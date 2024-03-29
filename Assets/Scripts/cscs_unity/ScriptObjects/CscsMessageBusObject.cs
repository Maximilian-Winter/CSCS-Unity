﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CSCS;
using OdinSerializer.Utilities;
using ScriptableObjects.ScriptableArchitecture.Systems.MessageBus;
using SplitAndMerge;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Runtime.Systems.MessageBus
{

public class CscsMessageBusObject : ScriptObject
{
    [Serializable]
    public class TypeToString: Dictionary<string, Type> {}
    
    public CscsMessageBusObject()
    {
        if ( MessageTypesToString == null)
        {
            MessageTypesToString = new TypeToString();
            
            foreach (Type mytype in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                                          .Where(mytype => mytype .GetInterfaces().Contains(typeof(IMessage)))) 
            {
                MessageTypesToString.Add( mytype.Name, mytype );
            }
        }
    }
    private static TypeToString MessageTypesToString; 
    private static readonly List<string> s_properties = new()
    {
        "GetMessageQueue", "PublishMessage", "SubscribeToMessageType"
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
                        Variable eventRecipient = args[0];
                        if(MessageBus.Instance.RecipientsToMessages.ContainsKey( eventRecipient ))
                        {
                            if ( MessageTypesToString.ContainsKey( args[1].AsString() ) )
                            {
                                newValue = new Variable(MessageBus.Instance.RecipientsToMessages[eventRecipient][MessageTypesToString[ args[1].AsString()]]);
                                Debug.Log( "Peek : " + (MessageBus.Instance.RecipientsToMessages[eventRecipient][MessageTypesToString[ args[1].AsString()]].Peek() as WelcomeMessage).GetProperty( "WelcomeMessage" ).Result.AsString() );
                            }
                            else
                            {
                                Debug.Log( "Message Type not found!" );
                            }
                        }
                        else
                        {
                            Debug.Log( "Message Recipient not found!" );
                        }
                    }
                    break;
                case "PublishMessage":
                    if (args != null && args.Count > 1 )
                    {
                        if ( MessageTypesToString.ContainsKey( args[1].AsString() ) )
                        {
                            string varName = args[0].AsString();
                            Type messageType = MessageTypesToString[args[1].AsString()];

                            IMessage instantiatedObject = Activator.CreateInstance(messageType) as IMessage;
                            Variable messageVariable = Utils.GetVariable(varName, script);
                            instantiatedObject  = instantiatedObject.ConstructFromCscsVariable( messageVariable, messageVariable );
                            MessageBus.PublishMessage( instantiatedObject, messageVariable );
                        }
                            
                    }
                    break;
                case "SubscribeToMessageType":
                    if (args != null && args.Count > 1 )
                    {
                       
                        if ( MessageTypesToString.ContainsKey( args[1].AsString() ) )
                        {
                            string varName = args[0].AsString();
                            Type messageType = MessageTypesToString[args[1].AsString()];
                            
                            Variable eventVariable = Utils.GetVariable(varName, script);
                            eventVariable.MessageTypesToCallbackClassInstancesFunctions.Add(messageType, new List<string>());
                            eventVariable.MessageTypesToCallbackClassInstancesFunctions[messageType].Add(varName);
                            MessageBus.SubscribeToAllMessagesOfType( eventVariable, messageType );
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
