using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SplitAndMerge;
using UnityEngine;

namespace CSCS
{
    public class CscsGameObject: ScriptObject
    {
        private GameObject UnityCscsObjectPrefab;
        private static readonly List<string> s_properties = new()
        {
           "Position", "Rotation", "Scale", "Translate", "RotateAround", "GetUnityEntity"
        };

        private CscsUnityEntity m_UnityEntity = null;
        public static Variable GetVariableFromVector3(Vector3 aVector)
        {
            Variable newValue = new Variable (Variable.VarType.ARRAY_NUM);
            newValue.AddVariable(new Variable(aVector.x), 0);
            newValue.AddVariable(new Variable(aVector.y), 1);
            newValue.AddVariable(new Variable(aVector.z), 2);
            return newValue;
        }

        public static Vector3 GetVector3FromVariable(Variable aVector)
        {
            if (aVector.GetSize() >= 3)
            {
                var newValue = new Vector3
                {
                    x = aVector.GetValue(0).AsFloat(),
                    y = aVector.GetValue(1).AsFloat(),
                    z = aVector.GetValue(2).AsFloat(),
                };
                return newValue;
            }
            else
            {
                var newValue = new Vector3
                {
                    x = (float)0.0,
                    y = (float)0.0,
                    z = (float)0.0,
                };
                return newValue;
            }
            
           
        }
        
        public static Variable GetVariableFromQuaternion(Quaternion aQuaternion)
        {
            Variable newValue = new Variable (Variable.VarType.ARRAY_NUM);
            newValue.AddVariable(new Variable(aQuaternion.x), 0);
            newValue.AddVariable(new Variable(aQuaternion.y), 1);
            newValue.AddVariable(new Variable(aQuaternion.z), 2);
            newValue.AddVariable(new Variable(aQuaternion.w), 3);
            return newValue;
        }

        public static Quaternion GetQuaternionFromVariable(Variable aQuaternion)
        {
            if (aQuaternion.GetSize() >= 4)
            {
                var newValue = new Quaternion
                {
                    x = aQuaternion.GetValue(0).AsFloat(),
                    y = aQuaternion.GetValue(1).AsFloat(),
                    z = aQuaternion.GetValue(2).AsFloat(),
                    w = aQuaternion.GetValue(3).AsFloat(),
                };
                return newValue;
            }
            else
            {
                var newValue = Quaternion.identity;
                return newValue;
            }
        }
        
        public CscsGameObject()
        {
        }

        public CscsGameObject(GameObject unityEntityPrefab)
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
                    case "Position":
                        newValue = new Variable(GetPositionProperty());
                        break;
                    case "Rotation":
                        newValue = new Variable(GetRotationProperty());
                        break;
                    case "Scale":
                        newValue = new Variable(GetScaleProperty());
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
                    case "Position":
                        newValue = SetPositionProperty(GetVector3FromVariable(argValue));
                        break;
                    case "Rotation":
                        newValue = SetRotationProperty(GetQuaternionFromVariable(argValue));
                        break;
                    case "Scale":
                        newValue = SetScaleProperty(GetVector3FromVariable(argValue));
                        break;
                    case "Translate":
                        Debug.Log("CallSetTranslate");
                        newValue = Translate(argValue);
                        break;
                    case "RotateAround":
                        newValue = RotateAround(argValue);
                        break;
                    default:
                        Debug.Log("CallSetDefault: " + sPropertyName);
                        break;
                }

                mre.Set();
            });

            mre.WaitOne();
            return Task.FromResult(newValue);
        }

        public Variable GetPositionProperty()
        {
            var myVector3 = m_UnityEntity.transform.position;
            return GetVariableFromVector3(myVector3);
        }

        public Variable SetPositionProperty(Vector3 aVector3)
        {
            m_UnityEntity.transform.position = aVector3;
            return Variable.EmptyInstance;
        }
        
        public Variable GetScaleProperty()
        {
            var myVector3 = m_UnityEntity.transform.localScale;
            return GetVariableFromVector3(myVector3);
        }

        public Variable SetScaleProperty(Vector3 aVector3)
        {
            m_UnityEntity.transform.localScale = aVector3;
            return Variable.EmptyInstance;
        }
        
        public Variable GetRotationProperty()
        {
            Quaternion quaternion = m_UnityEntity.transform.rotation;
            return GetVariableFromQuaternion(quaternion);
        }

        public Variable SetRotationProperty(Quaternion aQuaternion)
        {
            m_UnityEntity.transform.rotation = aQuaternion;
            return Variable.EmptyInstance;
        }

        public Variable Translate(Variable vectorVariable)
        {
            var aVector3 = GetVector3FromVariable(vectorVariable);
            m_UnityEntity.transform.Translate(aVector3);
            return Variable.EmptyInstance;
        }
        
        public Variable RotateAround(Variable vectorVariable)
        {
            var aVector3 = GetVector3FromVariable(vectorVariable);
            m_UnityEntity.transform.RotateAround(aVector3,vectorVariable.GetValue(3).AsFloat());
            return Variable.EmptyInstance;
        }
    }
}