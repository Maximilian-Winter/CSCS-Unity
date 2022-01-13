using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CSCS.Unity;
using ScriptableObjects.ScriptableArchitecture.Runtime.Systems.MessageBus;
using SplitAndMerge;
using UnityEngine;

namespace CSCS
{
    public class GameApiProxyObject : Variable, ScriptObject
    {
        private static readonly List<string> s_properties = new()
        {
           "GetMessageBusProxy", "GetGameObjectProxy"
        };

        private CscsUnityEntity m_UnityEntity = null;

        private MessageBusProxyObject m_MessageBusProxyObject = new MessageBusProxyObject();
        public GameApiProxyObject(GameObject unityEntityPrefab)
        {
            var mre = new ManualResetEvent(false);
            CscsScriptingController.ExecuteInUpdate(() =>
            {
                m_UnityEntity = GameObject.Instantiate(unityEntityPrefab).GetComponent<CscsUnityEntity>();
                mre.Set();
            });

            mre.WaitOne();
        }

        public List<string> GetProperties()
        {
            return s_properties;
        }

        public Task<Variable> GetProperty(string sPropertyName,
            List<Variable> args = null, ParsingScript script = null)
        {
            var newValue = Variable.EmptyInstance;
            var mre = new ManualResetEvent(false);

            CscsScriptingController.ExecuteInUpdate(() =>
            {
                switch (sPropertyName)
                {
                    case "GetMessageBusProxy":
                        newValue = new Variable(new MessageBusProxyObject());
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
            var mre = new ManualResetEvent(false);

            CscsScriptingController.ExecuteInUpdate(() =>
            {
                switch (sPropertyName)
                {
                    default:
                        Debug.Log("CallSetDefault: " + sPropertyName);
                        break;
                }

                mre.Set();
            });

            mre.WaitOne();
            return Task.FromResult(newValue);
        }
    }
}