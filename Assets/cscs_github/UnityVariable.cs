using System;
using System.Collections.Generic;
using ScriptableObjects.ScriptableArchitecture.Systems.MessageBus;

namespace SplitAndMerge
{
    public class UnityVariable : Variable, IMessageBusRecipient
    {
        public Dictionary<string, List<string>> MessageTypesToCallbackFunctions = new Dictionary<string, List<string>>();
        
        public void ReceiveMessage<T>(T message) where T : IMessage
        {
            string body = "IncomingMessage[\""+ typeof(T).Name +"\"][\"Hallo, CSCS From MessageBus!\"]";
            if (MessageTypesToCallbackFunctions.ContainsKey(typeof(T).Name))
            {
                List<string> callbackFunctions = MessageTypesToCallbackFunctions[typeof(T).Name];
                foreach (string callbackFunction in callbackFunctions)
                {
                    body += string.Format("{0}({1}", callbackFunction, "IncomingMessage");
                    body += ");";
                }
            
                ParsingScript tempScript = new ParsingScript(body);
                tempScript.Execute();
            }

        }
    }
}