using System;
using System.Collections.Generic;
using ScriptableObjects.ScriptableArchitecture.Framework;
using ScriptableObjects.ScriptableArchitecture.Systems.MessageBus;
using SplitAndMerge;

namespace CSCS
{

public class UnityEventVariable : Variable, IMessageBusRecipient, IMessageBusSender
{
    public Dictionary<Type, List<string>> MessageTypesToCallbackClassInstancesFunctions = new Dictionary<Type, List<string>>();

    public UnityEventVariable() :  base()
    {
    }
    
    public UnityEventVariable(object obj) :  base(obj)
    {
    }
    
    public override Variable Clone()
    {
        //Variable newVar = new Variable();
        //newVar.Copy(this);
        UnityEventVariable newVar = (UnityEventVariable)EmptyInstance;
            
        newVar = (UnityEventVariable)base.Clone();

        newVar.MessageTypesToCallbackClassInstancesFunctions = new Dictionary<Type, List<string>>(MessageTypesToCallbackClassInstancesFunctions) ;

        return newVar;
    }
    
    public override Variable DeepClone()
    {
        //Variable newVar = new Variable();
        //newVar.Copy(this);
        UnityEventVariable newVar = (UnityEventVariable)EmptyInstance;
            
        newVar = (UnityEventVariable)base.DeepClone();

        newVar.MessageTypesToCallbackClassInstancesFunctions = new Dictionary<Type, List<string>>(MessageTypesToCallbackClassInstancesFunctions) ;

        return newVar;
    }
    
    public void ReceiveMessage<T>(T message) where T : IMessage
    {
        string body = "";
            
        if (MessageTypesToCallbackClassInstancesFunctions.ContainsKey(message.MessageType))
        {
            List<string> callbackFunctions = MessageTypesToCallbackClassInstancesFunctions[message.MessageType];
            foreach (string callbackInstances in callbackFunctions)
            {
                body += $"{callbackInstances}.ReceiveMessage();\n";
            }
            CscsScriptingController.AddScriptToQueue(body);
        }

    }

}

}
