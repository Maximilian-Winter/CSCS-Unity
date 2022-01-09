using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CSCS;
using ScriptableObjects.ScriptableArchitecture.Framework;
using ScriptableObjects.ScriptableArchitecture.Systems.MessageBus;
using SplitAndMerge;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Runtime.Systems.MessageBus
{

public class MessageBusProxyObject : ScriptObject
{
    public MessageBusProxyObject()
    {
        MessageTypes = Assembly
                       .GetAssembly(typeof(Message))
                       .GetTypes()
                       .Where(t => t.IsSubclassOf(typeof(Message)));
    }
    private IEnumerable < Type > MessageTypes; 
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
                        newValue = new Variable(MessageBus.Instance.RecipientsToMessages[args[0]][args[1].AsString()]);
                    }
                    break;
                case "SubscribeToMessage":
                    if (args != null && args.Count > 1 )
                    {
                        foreach ( Type messageType in MessageTypes )
                        {
                            Debug.Log( args[1].AsString() + "==" + messageType.Name );
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
   
[CreateAssetMenu( menuName = "ScriptableSystems/MessageBus", fileName = "MessageBus", order = 0 )]
public class MessageBus : ScriptableSystem <MessageBus>
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
   
    public class RecipientsToMessageQueuesDictionary : Dictionary<IMessageBusRecipient, Dictionary<string, MessageQueue>> {}
   
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
        Type type = typeof( T );
        SubscribeToAllMessagesOfType( recipient, type );
    }
        
    public static void SubscribeToAllMessagesOfType(IMessageBusRecipient recipient, Type type)
    {
        if (  Instance.TypesToRecipients.ContainsKey( type ) )
        {
            Instance.TypesToRecipients[type].Add( recipient );

            if (  Instance.RecipientsToMessages.ContainsKey( recipient ) )
            {
                Instance.RecipientsToMessages[recipient].Add( type.Name,  new MessageQueue() );
            }
            else
            {
                Instance.RecipientsToMessages.Add( recipient, new Dictionary < string, MessageQueue >() );
                Instance.RecipientsToMessages[recipient].Add( type.Name,  new MessageQueue() );
            }
        }
        else
        {
            Instance.TypesToRecipients.Add( type, new List <IMessageBusRecipient>() );  
            Instance.TypesToRecipients[type].Add( recipient);
                
            Instance.RecipientsToMessages.Add( recipient, new Dictionary < string, MessageQueue >() );
            Instance.RecipientsToMessages[recipient].Add( type.Name,  new MessageQueue() );
        }
    }

    public static void PublishMessage(Message message, IMessageBusSender sender)
    {
        Type type = message.MessageType;
        if ( Instance.TypesToRecipients.ContainsKey( type ) )
        {
            foreach ( IMessageBusRecipient recipient in Instance.TypesToRecipients[type] )
            {
                Instance.RecipientsToMessages[recipient][type.Name].Enqueue(message);
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
}

}
