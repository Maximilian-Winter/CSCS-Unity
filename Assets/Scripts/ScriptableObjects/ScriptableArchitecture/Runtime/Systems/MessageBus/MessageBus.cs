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
    public int MaxMessageQueueLength = 500;
    
    
    
    [Serializable]
    public class RecipientsToMessageTypeQueuesDictionary : Dictionary<IMessageBusRecipient, Dictionary<Type, CscsMessageQueueObject>> {}
   
    [Serializable]
    public class TypesToRecipientsDictionary : Dictionary<Type, List<IMessageBusRecipient>> {}

    public RecipientsToMessageTypeQueuesDictionary RecipientsToMessages = 
        new RecipientsToMessageTypeQueuesDictionary();
   
    public TypesToRecipientsDictionary TypesToRecipients = 
        new TypesToRecipientsDictionary();
        
    private void Awake()
    {
        if ( Application.isEditor )
        {
            RecipientsToMessages = new RecipientsToMessageTypeQueuesDictionary();
            TypesToRecipients = 
                new TypesToRecipientsDictionary();
        }
    }

    public static void SubscribeToAllMessagesOfType<T> (IMessageBusRecipient recipient) where T : IMessage
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
                Instance.RecipientsToMessages[recipient].Add( type,  new CscsMessageQueueObject() );
            }
            else
            {
                Instance.RecipientsToMessages.Add( recipient, new Dictionary < Type, CscsMessageQueueObject >() );
                Instance.RecipientsToMessages[recipient].Add( type,  new CscsMessageQueueObject() );
            }
        }
        else
        {
            Instance.TypesToRecipients.Add( type, new List <IMessageBusRecipient>() );  
            Instance.TypesToRecipients[type].Add( recipient);
                
            Instance.RecipientsToMessages.Add( recipient, new Dictionary < Type, CscsMessageQueueObject >() );
            Instance.RecipientsToMessages[recipient].Add( type,  new CscsMessageQueueObject() );
        }
    }

    public static void PublishMessage(IMessage message, IMessageBusSender sender)
    {
        Type type = message.MessageType;
        if ( Instance.TypesToRecipients.ContainsKey( type ) )
        {
            foreach ( IMessageBusRecipient recipient in Instance.TypesToRecipients[type] )
            {
                if ( Instance.RecipientsToMessages[recipient][type].Count < Instance.MaxMessageQueueLength )
                {
                    Instance.RecipientsToMessages[recipient][type].Enqueue(message);
                    Debug.Log("MessageBus: " + (message as WelcomeMessage).WelcomeMessageContent);
                }
                else
                {
                    Instance.RecipientsToMessages[recipient][type].Clear();
                    Instance.RecipientsToMessages[recipient][type].Enqueue(message);
                }
                
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
