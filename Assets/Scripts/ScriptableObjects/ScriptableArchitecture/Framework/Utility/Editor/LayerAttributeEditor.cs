using UnityEditor;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Framework.Utility.Editor
{

[CustomPropertyDrawer( typeof( LayerAttribute ) )]
internal class LayerAttributeEditor : PropertyDrawer
{
    #region Unity Event Functions

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
        EditorGUI.BeginProperty( position, label, property );
        property.intValue = EditorGUI.LayerField( position, label, property.intValue );
        EditorGUI.EndProperty();
    }

    #endregion
}

}
