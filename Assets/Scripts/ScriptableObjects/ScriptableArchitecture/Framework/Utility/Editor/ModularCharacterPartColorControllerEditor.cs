using UnityEditor;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Framework.Utility.Editor
{

[CustomEditor( typeof( ModularCharacterPartColorController ) )]
public class ModularCharacterPartColorControllerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ModularCharacterPartColorController myScript = ( ModularCharacterPartColorController ) target;

        if ( GUILayout.Button( "Save Colors" ) )
        {
            myScript.SaveColors();
        }
    }
}

}
