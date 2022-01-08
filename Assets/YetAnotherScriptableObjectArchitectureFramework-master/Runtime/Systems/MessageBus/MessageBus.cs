using System;
using System.Collections.Generic;
using ScriptableObjects.ScriptableArchitecture.Framework;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Systems.MessageBus
{
   public class WelcomeMessage: IMessage
   {
      public Type MessageType
      {
         get => typeof(WelcomeMessage);
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

   [Serializable]
   public class TypesToRecipientsDictionary : Dictionary<Type, List<IMessageBusRecipient>> {}
   
   [Serializable]
   public class SendersToRecipientsDictionary : Dictionary<IMessageBusSender, List<IMessageBusRecipient>> {}
   
   public TypesToRecipientsDictionary TypesToRecipients = 
      new TypesToRecipientsDictionary();
   
   public SendersToRecipientsDictionary SendersToRecipients = 
      new SendersToRecipientsDictionary();
   
   public List <SenderTypeAssociation> SenderTypeAssociationToRecipients = 
      new List <SenderTypeAssociation>();

   private void Awake()
   {
      if ( Application.isEditor )
      {
         TypesToRecipients = 
            new TypesToRecipientsDictionary();
         SendersToRecipients = 
            new SendersToRecipientsDictionary();
         SenderTypeAssociationToRecipients = 
            new List <SenderTypeAssociation>();
      }
   }

   public static void SubscribeToAllMessagesFromSender(IMessageBusSender sender, IMessageBusRecipient recipient)
   {
      if (  Instance.SendersToRecipients.ContainsKey( sender ) )
      {
         Instance.SendersToRecipients[sender].Add( recipient );
      }
      else
      {
         Instance.SendersToRecipients.Add( sender, new List <IMessageBusRecipient>() );  
         Instance.SendersToRecipients[sender].Add( recipient );
      }
   }
   
   public static void SubscribeToAllMessagesOfType<T> (IMessageBusRecipient recipient) where T : IMessage
   {
      Type type = typeof( T );
      Debug.Log( "New Subscriber to Type: " + type + " Recipient: " + recipient.GetType() );
      if (  Instance.TypesToRecipients.ContainsKey( type ) )
      {
         Instance.TypesToRecipients[type].Add( recipient );
      }
      else
      {
         Instance.TypesToRecipients.Add( type, new List <IMessageBusRecipient>() );  
         Instance.TypesToRecipients[type].Add( recipient);
      }

      WelcomeMessage welcomeMessage = new WelcomeMessage();
      recipient.ReceiveMessage( welcomeMessage );
   }
   
   public static void SubscribeToAllMessagesOfTypeFromSender <T>(IMessageBusSender sender, IMessageBusRecipient recipient) where T : IMessage
   {
      Type type = typeof( T );
      SenderTypeAssociation senderTypeAssociation = new SenderTypeAssociation();
      senderTypeAssociation.Recipient = recipient;
      senderTypeAssociation.Sender = sender;
      senderTypeAssociation.Type = type;
      
      Instance.SenderTypeAssociationToRecipients.Add( senderTypeAssociation );
   }
   
   public static void PublishMessage(IMessage message, IMessageBusSender sender)
   {
      Type type = message.MessageType;

      Debug.Log( "New Message of Type: " + type + " Published by: " + sender.GetType() );
      
      if ( Instance.TypesToRecipients.ContainsKey( type ) )
      {
         foreach ( IMessageBusRecipient recipient in Instance.TypesToRecipients[type] )
         {
            recipient.ReceiveMessage( message  );
         }
      }
      if(sender == null)
         return;
      
      if ( Instance.SendersToRecipients.ContainsKey( sender ) )
      {
         foreach ( IMessageBusRecipient recipient in Instance.SendersToRecipients[sender] )
         {
            recipient.ReceiveMessage( message );
         }
      }

      foreach ( SenderTypeAssociation senderTypeAssociation in Instance.SenderTypeAssociationToRecipients )
      {
         if ( senderTypeAssociation.Type == type && senderTypeAssociation.Sender == sender )
         {
            senderTypeAssociation.Recipient.ReceiveMessage( message );
         }
      }
   }
   
   public static void UnsubcribeFromMessages<T>(IMessageBusRecipient recipient)
   {
      Type type = typeof( T );
      if ( Instance.TypesToRecipients.ContainsKey( type ) )
      {
         Instance.TypesToRecipients[type].Remove( recipient );
      }
   }
   
   public static void UnsubcribeFromAllMessagesFromSender(IMessageBusSender sender, IMessageBusRecipient recipient)
   {
      if ( Instance.SendersToRecipients.ContainsKey( sender ) )
      {
         Instance.SendersToRecipients[sender].Remove( recipient );
      }
   }
   
   public static void UnsubcribeFromMessagesOfTypeFromSender<T>(IMessageBusSender sender, IMessageBusRecipient recipient)
   {
      Type type = typeof( T );
      SenderTypeAssociation senderTypeAssociationTemp = null;
      foreach ( SenderTypeAssociation senderTypeAssociation in Instance.SenderTypeAssociationToRecipients )
      {
         if ( senderTypeAssociation.Type == type && senderTypeAssociation.Sender == sender && senderTypeAssociation.Recipient == recipient )
         {
            senderTypeAssociationTemp = senderTypeAssociation;
            break;
         }
      }
      
      Instance.SenderTypeAssociationToRecipients.Remove( senderTypeAssociationTemp );
   }
}

}
