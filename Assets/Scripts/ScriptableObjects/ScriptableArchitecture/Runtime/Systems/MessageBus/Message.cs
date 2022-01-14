using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSCS;
using ScriptableObjects.ScriptableArchitecture.Systems.MessageBus;
using SplitAndMerge;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Runtime.Systems.MessageBus
{
    public class WelcomeMessage: IMessage
    {
        public WelcomeMessage()
        {
            WelcomeMessageContent = "";
            MessageBusSender = null;
        }
        public WelcomeMessage(IMessageBusSender messageBusSender)
        {
            WelcomeMessageContent = "HelloWorld";
            MessageBusSender = messageBusSender;
        }
        
        public WelcomeMessage(IMessageBusSender messageBusSender, string welcomeMessage )
        {
            WelcomeMessageContent = welcomeMessage;
            MessageBusSender = messageBusSender;
        }

        public IMessage ConstructFromCscsVariable(IMessageBusSender messageBusSender, Variable variable)
        {
            WelcomeMessage message = new WelcomeMessage(messageBusSender, variable.GetProperty( "WelcomeMessage" ).AsString() );
            return message;
        }

        public Type MessageType
        {
            get => typeof(WelcomeMessage);
        }

        public IMessageBusSender MessageBusSender { get; set; }

        public string WelcomeMessageContent
        {
            get => m_WelcomeMessageContent;
            set => m_WelcomeMessageContent = value;
        }

        private static readonly List<string> s_properties = new()
        {
            "WelcomeMessage", "MessageSender"
        };

        private string m_WelcomeMessageContent = "Hi from C# To CSCS!";

        public Task<Variable> SetProperty(string name, Variable value)
        {
            var newValue = Variable.EmptyInstance;
            var mre = new ManualResetEvent(false);

            CscsScriptingController.ExecuteInUpdate(() =>
            {
                switch (name)
                {
                    default:
                        break;
                }

                mre.Set();
            });

            mre.WaitOne();
            return Task.FromResult(newValue);
        }

        public Task<Variable> GetProperty(string name, List<Variable> args = null, ParsingScript script = null)
        {
            var newValue = Variable.EmptyInstance;
            switch (name)
            {
                case "WelcomeMessage":
                    newValue = new Variable( WelcomeMessageContent );
                    break;
                case "MessageSender":
                    newValue = new Variable( MessageBusSender );
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