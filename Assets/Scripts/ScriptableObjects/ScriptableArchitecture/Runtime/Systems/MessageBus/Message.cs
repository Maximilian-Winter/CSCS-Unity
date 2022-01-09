using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScriptableObjects.ScriptableArchitecture.Systems.MessageBus;
using SplitAndMerge;

namespace ScriptableObjects.ScriptableArchitecture.Runtime.Systems.MessageBus
{
    public class WelcomeMessage: Message
    {
        
        private static readonly List<string> s_properties = new()
        {
            "WelcomeMessage"
        };

        private string m_WelcomeMessage = "Hi from C# To CSCS!";
        
        public override Type MessageType
        {
            get => typeof(WelcomeMessage);
        }

        public override Task<Variable> GetProperty(string sPropertyName,
            List<Variable> args = null, ParsingScript script = null)
        {
            var newValue = Variable.EmptyInstance;

            switch (sPropertyName)
            {
                case "WelcomeMessage":
                    newValue = new Variable(m_WelcomeMessage);
                    break;
                default:
                    newValue = Variable.EmptyInstance;
                    break;
            }
      
            return Task.FromResult(newValue);
        }

        public override Task<Variable> SetProperty(string sPropertyName, Variable argValue)
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

        public override List<string> GetProperties()
        {
            return s_properties;
        }
    }
    
    public abstract class Message : IMessage, ScriptObject
    {
        public abstract Type MessageType { get; }
        public abstract Task<Variable> SetProperty(string name, Variable value);

        public abstract Task<Variable> GetProperty(string name, List<Variable> args = null,
            ParsingScript script = null);

        public abstract List<string> GetProperties();
    }
}