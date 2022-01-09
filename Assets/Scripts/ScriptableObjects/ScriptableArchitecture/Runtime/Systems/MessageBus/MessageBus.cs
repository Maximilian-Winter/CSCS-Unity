using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSCS;
using ScriptableObjects.ScriptableArchitecture.Framework;
using ScriptableObjects.ScriptableArchitecture.Systems.MessageBus;
using SplitAndMerge;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Runtime.Systems.MessageBus
{

   
    [CreateAssetMenu( menuName = "ScriptableSystems/MessageBus", fileName = "MessageBus", order = 0 )]
    public class MessageBus : ScriptableSystem <MessageBus>, ScriptObject
    {
        [Serializable]
        public class SenderTypeAssociation
        {
            public IMessageBusSender Sender;
            public Type Type;
            public IMessageBusRecipient Recipient;
        }

        public class MessageQueue : Queue<Message>, ScriptObject
        {
            private static readonly List<string> s_properties = new()
            {
                "DequeMessage"
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
                var mre = new ManualResetEvent(false);
                CscsScriptingController.ExecuteInUpdate(() =>
                {
                    switch (name)
                    { 
                        case "DequeMessage":
                            newValue = new Variable(Dequeue());
                            break;
                        default:
                            break;
                    }

                    mre.Set();
                });

                mre.WaitOne();
              
                return Task.FromResult(newValue);
            }

            public List<string> GetProperties()
            {
                return s_properties;
            }
        }
   
        public class RecipientsToMessageQueuesDictionary : Dictionary<IMessageBusRecipient, MessageQueue> {}
   
        [Serializable]
        public class TypesToRecipientsDictionary : Dictionary<Type, List<IMessageBusRecipient>> {}

        public RecipientsToMessageQueuesDictionary RecipientsToMessages = 
            new RecipientsToMessageQueuesDictionary();
   
        public TypesToRecipientsDictionary TypesToRecipients = 
            new TypesToRecipientsDictionary();
   
   

        private void Awake()
        {
            if ( Application.isEditor )
            {
                RecipientsToMessages = new RecipientsToMessageQueuesDictionary();
                TypesToRecipients = 
                    new TypesToRecipientsDictionary();
            }
        }

        public static void SubscribeToAllMessagesOfType<T> (IMessageBusRecipient recipient) where T : Message
        {
            Variable rec = recipient as Variable;
            Type type = typeof( T );
            if (  Instance.TypesToRecipients.ContainsKey( type ) )
            {
                Instance.TypesToRecipients[type].Add( recipient );
            }
            else
            {
                Instance.TypesToRecipients.Add( type, new List <IMessageBusRecipient>() );  
                Instance.TypesToRecipients[type].Add( recipient);
            }
      
            Instance.RecipientsToMessages.Add(recipient, new MessageQueue());
        }

        public static void PublishMessage(Message message, IMessageBusSender sender)
        {
            Type type = message.MessageType;
            if ( Instance.TypesToRecipients.ContainsKey( type ) )
            {
                foreach ( IMessageBusRecipient recipient in Instance.TypesToRecipients[type] )
                {
                    Instance.RecipientsToMessages[recipient].Enqueue(message);
                    recipient.ReceiveMessage( message  );
                }
            }
        }
   
        public static void UnsubcribeFromMessages<T>(IMessageBusRecipient recipient)
        {
            Type type = typeof( T );
            if ( Instance.TypesToRecipients.ContainsKey( type ) )
            {
                Instance.RecipientsToMessages.Remove(recipient);
                Instance.TypesToRecipients[type].Remove( recipient );
            }
        }
   

        private static readonly List<string> s_properties = new()
        {
            "GetMessageQueue", "PublishMessage"
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
                        newValue = new Variable(Instance.RecipientsToMessages[args[0]]);
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