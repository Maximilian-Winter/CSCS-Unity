using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ModularCharacterPartColorController))]
public class ModularCharacterPartColorControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ModularCharacterPartColorController myScript = (ModularCharacterPartColorController)target;
        if (GUILayout.Button("Save Colors"))
        {
            myScript.SaveColors();
        }
    }
}
