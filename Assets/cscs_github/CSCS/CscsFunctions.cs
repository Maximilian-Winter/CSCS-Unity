using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ScriptableObjects.ScriptableArchitecture.Systems.MessageBus;
using SplitAndMerge;
using UnityEngine;

namespace CSCS
{
    public class CscsFunctions
    {
        public static void DefineScriptFunctions(GameObject unityEntityPrefab)
        {
            ParserFunction.RegisterFunction("CreateGameObject", new CreateGameObjectFunction(unityEntityPrefab));
            ParserFunction.RegisterFunction("DebugLog", new DebugLogFunction());
            ParserFunction.RegisterFunction("NativeInvoke", new InvokeNativeFunction());
            ParserFunction.RegisterFunction("AddMessageBusCallback", new AddMessageBusCallbackFunction());
            ParserFunction.RegisterFunction("CreateVector3", new CreateVector3Function());
            /*ParserFunction.RegisterFunction("CreateCapsule", new CreateCapsuleFunction());
            ParserFunction.RegisterFunction("CreateTube", new CreateTubeFunction());*/
        }
        
        
    }

    namespace Unity
    {
       public enum Archetype
       {
           Empty,
           StaticMesh,
           AnimatedMesh
       }
       
    }
    class DebugLogFunction: ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List <Variable> args = script.GetFunctionArgs();
            Variable newValue = new Variable(args);
            ManualResetEvent mre = new ManualResetEvent (false);

            CscsScriptingController.ExecuteInUpdate(() => 
            {
                foreach (Variable variable in args)
                {
                   Debug.Log(variable.AsString());
                }
                mre.Set();
            });

            mre.WaitOne();
            return newValue;
        }
    }
    
    public class AddMessageBusCallbackFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            //string varName = Utils.GetToken(script, Constants.NEXT_ARG_ARRAY);
            string varName = Utils.GetItem(script).AsString();
            Utils.CheckNotEmpty(script, varName, m_name);
            script.MoveForwardIf(Constants.NEXT_ARG);

            Variable actionValue = Utils.GetItem(script);
            string strAction = actionValue.AsString();
            script.MoveForwardIf(Constants.NEXT_ARG);
            
            Variable messageTypeValue = Utils.GetItem(script);
            string strMessagType= messageTypeValue.AsString();
            script.MoveForwardIf(Constants.NEXT_ARG);

            UnityVariable unityVar = Utils.GetVariable(varName, script) as UnityVariable;
            CscsScriptingController.ExecuteInUpdate(() => 
            {
                Debug.Log(varName + " " + strAction + " " + strMessagType);
            });

            Utils.CheckNotNull(unityVar, m_name);
            AddAction(unityVar, strMessagType, strAction);

            return Variable.EmptyInstance;
        }
        public static void AddAction(UnityVariable unityVar, string strMessagType, string strAction)
        {
            unityVar.MessageTypesToCallbackFunctions.Add(strMessagType, new List<string>());
            unityVar.MessageTypesToCallbackFunctions[strMessagType].Add(strAction);
            
            var mre = new ManualResetEvent(false);
            CscsScriptingController.ExecuteInUpdate(() =>
            {
                MessageBus.SubscribeToAllMessagesOfType<IMessage>(unityVar);
                    mre.Set();
            });

            mre.WaitOne();
        }
    }
    
    public class CreateGameObjectFunction: ParserFunction
    {
        private GameObject UnityEntityPrefab;
        
        public CreateGameObjectFunction(GameObject unityEntityPrefab) : base()
        {
            UnityEntityPrefab = unityEntityPrefab;
        }
        static Variable CreateEntityOfType(string sPrimitiveType, GameObject unityEntityPrefab, List<Variable> args = null)
        {
            EntityScriptObject myObject = new EntityScriptObject(unityEntityPrefab);
            Variable newValue = new Variable (myObject);
            return newValue;
        }
        protected override Variable Evaluate(ParsingScript script)
        {
            List <Variable> args = script.GetFunctionArgs();
            string sPrimitiveType = Utils.GetSafeString(args, 0, "Cube");
            Variable newValue = CreateEntityOfType(sPrimitiveType, UnityEntityPrefab);
            return newValue;
        }
    }

    class CreateVector3Function: ParserFunction
    {
        private GameObject UnityEntityPrefab;
        
        static Variable CreateEntityOfType(List<Variable> args = null)
        {
            Variable newValue = new Variable (Variable.VarType.ARRAY_NUM);
            if (args.Count == 3)
            {
                newValue.AddVariable(args[0], 0);
                newValue.AddVariable(args[1], 1);
                newValue.AddVariable(args[2], 2);
            }
            else
            {
                newValue.AddVariable(new Variable(0.0), 0);
                newValue.AddVariable(new Variable(0.0), 1);
                newValue.AddVariable(new Variable(0.0), 2);
            }
            return newValue;
        }
        protected override Variable Evaluate(ParsingScript script)
        {
            List <Variable> args = script.GetFunctionArgs();
            Variable newValue = CreateEntityOfType(args);
            return newValue;
        }
    }
}