using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ScriptableObjects.ScriptableArchitecture.Runtime.Systems.MessageBus;
using SplitAndMerge;
using UnityEngine;

namespace CSCS
{

public class CscsFunctions
{
    #region Public

    public static void DefineScriptFunctions(GameObject UnityCscsObjectPrefab)
    {
        ParserFunction.RegisterFunction(
            "CreateGameApiObject",
            new CreateGameApiObjectFunction(UnityCscsObjectPrefab) );

        ParserFunction.RegisterFunction( "DebugLog", new DebugLogFunction() );
        ParserFunction.RegisterFunction( "InvokeNative", new InvokeNativeFunction() );
        ParserFunction.RegisterFunction( Constants.THIS, new ThisFunction() );
       // ParserFunction.AddAction( Constants.THIS + ".", new ThisDotFunction() );
        /*ParserFunction.RegisterFunction("CreateCapsule", new CreateCapsuleFunction());
        ParserFunction.RegisterFunction("CreateTube", new CreateTubeFunction());*/
    }

    #endregion
}

internal class DebugLogFunction : ParserFunction
{
    #region Protected

    protected override Variable Evaluate( ParsingScript script )
    {
        List < Variable > args = script.GetFunctionArgs();
        Variable newValue = new Variable( args );
        ManualResetEvent mre = new ManualResetEvent( false );

        CscsScriptingController.ExecuteInUpdate(
            () =>
            {
                foreach ( Variable variable in args )
                {
                    Debug.Log( variable.AsString() );
                }

                mre.Set();
            } );

        mre.WaitOne();

        return newValue;
    }

    #endregion
}

public class ThisFunction : ActionFunction
{
    #region Protected

    protected override Variable Evaluate( ParsingScript script )
    {
        return script.ClassInstance != null ? Utils.GetVariable( script.ClassInstance.InstanceName, script ) : null;
    }
    
    protected override Task < Variable > EvaluateAsync( ParsingScript script )
    {
        return script.ClassInstance != null ? Utils.GetVariableAsync( script.ClassInstance.InstanceName, script ) : null;
    }

    #endregion
}

public class CreateGameApiObjectFunction : ParserFunction
{
    private GameObject UnityCscsObjectPrefab;
    #region Protected

    public CreateGameApiObjectFunction( GameObject unityCscsObjectPrefab )
    {
        UnityCscsObjectPrefab = unityCscsObjectPrefab;
    }

    protected override Variable Evaluate( ParsingScript script )
    {
        CscsGameApiObject myObject = new CscsGameApiObject(UnityCscsObjectPrefab);
        Variable newValue = new Variable( myObject );
        return newValue;
    }

    #endregion
}

}
