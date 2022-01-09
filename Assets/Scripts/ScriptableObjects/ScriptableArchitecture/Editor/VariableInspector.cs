using ScriptableVariablesAndReferences;
using UnityEditor;

[CustomEditor( typeof( BoolVariable ) )]
public class BoolVariableInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
    }
}

[CustomEditor( typeof( IntVariable ) )]
public class IntVariableInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
    }
}

[CustomEditor( typeof( GameObjectVariable ) )]
public class GameObjectVariableInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //Utility.SerializedPropertyUtility.LogProperties( serializedObject );
        /*
        SerializedProperty runtimeValue = serializedObject.FindProperty( "RuntimeValue" );
        EditorGUILayout.PropertyField(runtimeValue);*/
        DrawDefaultInspector();
    }
}