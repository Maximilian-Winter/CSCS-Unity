using System;
using ScriptableObjects.ScriptableArchitecture.Runtime.Systems.MessageBus;
using ScriptableObjects.ScriptableArchitecture.Systems.MessageBus;
using UnityEngine;

namespace CSCS
{
    public class CscsUnityEntity : MonoBehaviour, IMessageBusSender
    {
        [ContextMenu("SendMessage")]
        public void SendMessage()
        {
            MessageBus.PublishMessage(new WelcomeMessage(), this);
        }
        public Type MessageBusSenderType { get => typeof(CscsUnityEntity); }
    }
}