using System;
using System.Collections.Generic;

using System.Threading;
using System.Threading.Tasks;
using ScriptableObjects.ScriptableArchitecture.Systems.MessageBus;
using SplitAndMerge;
using UnityEngine;
using ScriptableObjects.ScriptableArchitecture.Runtime.Systems.MessageBus;

namespace CSCS
{
    public class CscsFunctions
    {
        public static void DefineScriptFunctions(GameObject unityEntityPrefab)
        {
            ParserFunction.RegisterFunction("CreateGameApiProxyObject", new CreateGameApiProxyObjectFunction(unityEntityPrefab));
            ParserFunction.RegisterFunction("DebugLog", new DebugLogFunction());
            ParserFunction.RegisterFunction("NativeInvoke", new InvokeNativeFunction());
            ParserFunction.RegisterFunction("SubscribeToMessageBus", new SubscribeToMessageBus());
            ParserFunction.RegisterFunction("CreateVector3", new CreateVector3Function());
            /*ParserFunction.RegisterFunction("CreateCapsule", new CreateCapsuleFunction());
            ParserFunction.RegisterFunction("CreateTube", new CreateTubeFunction());*/
        }
        
        
    }

    namespace Unity
    {
       public enum GameObjectProxyType
       {
           New,
           Existing,
           Copy,
           DeepCopy
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
    
    public class SubscribeToMessageBus : ParserFunction
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

            Variable variable = Utils.GetVariable(varName, script);
            Utils.CheckNotNull(variable, m_name);
            SubscribeToMessageWithCallbackMethod(variable, strMessagType, strAction);

            return Variable.EmptyInstance;
        }
        public static void SubscribeToMessageWithCallbackMethod(Variable variable, string strMessageType, string strAction)
        {
            variable.MessageTypesToCallbackFunctions.Add(strMessageType, new List<string>());
            variable.MessageTypesToCallbackFunctions[strMessageType].Add(strAction);
            var mre = new ManualResetEvent(false);
            CscsScriptingController.ExecuteInUpdate(() => 
            {
                if (strMessageType == nameof(WelcomeMessage))
                {
                    MessageBus.SubscribeToAllMessagesOfType<WelcomeMessage>(variable);
                }
                else
                {
                    MessageBus.SubscribeToAllMessagesOfType<WelcomeMessage>(variable);
                }
                
                mre.Set();
            });
            mre.WaitOne();
        }
    }
    
    public class CreateGameApiProxyObjectFunction: ParserFunction
    {
        private GameObject UnityEntityPrefab;
        
        public CreateGameApiProxyObjectFunction(GameObject unityEntityPrefab) : base()
        {
            UnityEntityPrefab = unityEntityPrefab;
        }
        static Variable CreateGameApiProxyObject(GameObject unityEntityPrefab, List<Variable> args = null)
        {
            GameApiProxyObject myObject = new GameApiProxyObject(unityEntityPrefab);
            Variable newValue = new Variable (myObject);
            return newValue;
        }
        protected override Variable Evaluate(ParsingScript script)
        {
            Variable newValue = CreateGameApiProxyObject(UnityEntityPrefab);
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