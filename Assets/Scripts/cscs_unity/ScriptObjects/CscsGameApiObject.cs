using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ScriptableObjects.ScriptableArchitecture.Runtime.Systems.MessageBus;
using SplitAndMerge;
using UnityEngine;

namespace CSCS
{
    public class CscsGameApiObject : ScriptObject
    {
        private static readonly List<string> s_properties = new()
        {
           "GetMessageBusObject", "GetGameObjectApiObject", "GetUnityMathApiObject"
        };

        
        private CscsMessageBusObject m_CscsMessageBusObject = null;
        private CscsGameObjectApiObject m_CscsGameObjectApiObject = null;

        public CscsGameApiObject( GameObject unityCscsObjectPrefab )
        {
            m_CscsMessageBusObject = new CscsMessageBusObject();
            m_CscsGameObjectApiObject = new CscsGameObjectApiObject( unityCscsObjectPrefab );
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
                    case "GetMessageBusObject":
                        newValue = new Variable(m_CscsMessageBusObject);
                        break;
                    case "GetGameObjectApiObject":
                        
                        newValue = new Variable(m_CscsGameObjectApiObject);
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