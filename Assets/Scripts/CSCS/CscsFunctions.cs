using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SplitAndMerge;
using UnityEngine;

namespace CSCS
{
    public class CscsFunctions
    {
        public static void DefineScriptFunctions(GameObject unityEntityPrefab)
        {
            ParserFunction.RegisterFunction("CreateGameObject", new CreateCubeFunction(unityEntityPrefab));
            ParserFunction.RegisterFunction("CreateVector3", new CreateVector3Function());
            ParserFunction.RegisterFunction("DebugLog", new DebugLogFunction());
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
    
    class CreateCubeFunction: ParserFunction
    {
        private GameObject UnityEntityPrefab;
        
        public CreateCubeFunction(GameObject unityEntityPrefab) : base()
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