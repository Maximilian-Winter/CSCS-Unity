using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SplitAndMerge;
using UnityEngine;

namespace CSCS
{
    public class CscsGameObjectApiObject: ScriptObject
    {
        private GameObject UnityCscsObjectPrefab;
        private static readonly List<string> s_properties = new()
        {
            "GetGameObject", "CopyGameObject", "NewGameObject", "GetChildGameObject" 
        };

        public CscsGameObjectApiObject( GameObject unityCscsObjectPrefab )
        {
            UnityCscsObjectPrefab = unityCscsObjectPrefab;
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
                    case "NewGameObject":
                        newValue = new Variable(new CscsGameObject( UnityCscsObjectPrefab ));
                        break;

                    default:
                        Debug.Log("CallGetDefault: " + sPropertyName);
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