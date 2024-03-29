﻿using System;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Framework.Utility
{

[CustomPropertyDrawer( typeof( ScriptableIdAttribute ) )]
public class ScriptableObjectIdDrawer : PropertyDrawer
{
    #region Unity Event Functions

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
        GUI.enabled = false;

        if ( string.IsNullOrEmpty( property.stringValue ) )
        {
            property.stringValue = Guid.NewGuid().ToString();
        }

        EditorGUI.PropertyField( position, property, label, true );
        GUI.enabled = true;
    }

    #endregion
}

}
