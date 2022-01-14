using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScriptableObjects.ScriptableArchitecture.Runtime.Systems.MessageBus;
using ScriptableObjects.ScriptableArchitecture.Systems.MessageBus;
using SplitAndMerge;
using UnityEngine;

namespace CSCS
{
    public class CscsUnityEntity : MonoBehaviour, IMessageBusSender, IMessageBusRecipient, ScriptObject
    {
        public string WelcomeMessage = "";
        private void Awake()
        {
            MessageBus.SubscribeToAllMessagesOfType<WelcomeMessage>( this );
        }

        [ContextMenu("SendMessage")]
        public void SendMessage()
        {
            MessageBus.PublishMessage(new WelcomeMessage(this, WelcomeMessage), this);
        }

        public Task < Variable > SetProperty( string name, Variable value )
        {
            return null;
        }

        public Task < Variable > GetProperty( string name, List < Variable > args = null, ParsingScript script = null )
        {
            return null;
        }

        public List < string > GetProperties()
        {
            return null;
        }

        public void ReceiveMessage < T >( T message ) where T : IMessage
        {
            Debug.Log( (message as WelcomeMessage).WelcomeMessageContent);
        }
    }
}