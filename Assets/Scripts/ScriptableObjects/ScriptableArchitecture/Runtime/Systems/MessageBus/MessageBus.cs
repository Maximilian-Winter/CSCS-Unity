using System;
using System.Collections.Generic;
using ScriptableObjects.ScriptableArchitecture.Framework;
using ScriptableObjects.ScriptableArchitecture.Systems.MessageBus;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Runtime.Systems.MessageBus
{

[CreateAssetMenu( menuName = "ScriptableSystems/MessageBus", fileName = "MessageBus", order = 0 )]
public class MessageBus : ScriptableSystem <MessageBus>
{
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
