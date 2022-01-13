using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ScriptableObjects.ScriptableArchitecture.Framework.Utility.Editor
{

public class SerializedPropertyUtility
{
    #region Public

    // A way to see everything a SerializedProperty object contains in case you don't
    // know what type is stored.
    public static void LogAllValues( SerializedProperty serializedProperty )
    {
        Debug.Log( "PROPERTY: name = " + serializedProperty.name + " type = " + serializedProperty.type );
        Debug.Log( "animationCurveValue = " + serializedProperty.animationCurveValue );
        Debug.Log( "arraySize = " + serializedProperty.arraySize );
        Debug.Log( "boolValue = " + serializedProperty.boolValue );
        Debug.Log( "boundsValue = " + serializedProperty.boundsValue );
        Debug.Log( "colorValue = " + serializedProperty.colorValue );
        Debug.Log( "depth = " + serializedProperty.depth );
        Debug.Log( "editable = " + serializedProperty.editable );
        Debug.Log( "enumNames = " + serializedProperty.enumNames );
        Debug.Log( "enumValueIndex = " + serializedProperty.enumValueIndex );
        Debug.Log( "floatValue = " + serializedProperty.floatValue );
        Debug.Log( "hasChildren = " + serializedProperty.hasChildren );
        Debug.Log( "hasMultipleDifferentValues = " + serializedProperty.hasMultipleDifferentValues );
        Debug.Log( "hasVisibleChildren = " + serializedProperty.hasVisibleChildren );
        Debug.Log( "intValue = " + serializedProperty.intValue );
        Debug.Log( "isAnimated = " + serializedProperty.isAnimated );
        Debug.Log( "isArray = " + serializedProperty.isArray );
        Debug.Log( "isExpanded = " + serializedProperty.isExpanded );
        Debug.Log( "isInstantiatedPrefab = " + serializedProperty.isInstantiatedPrefab );
        Debug.Log( "name = " + serializedProperty.name );
        Debug.Log( "objectReferenceInstanceIDValue = " + serializedProperty.objectReferenceInstanceIDValue );
        Debug.Log( "objectReferenceValue = " + serializedProperty.objectReferenceValue );
        Debug.Log( "prefabOverride = " + serializedProperty.prefabOverride );
        Debug.Log( "propertyPath = " + serializedProperty.propertyPath );
        Debug.Log( "propertyType = " + serializedProperty.propertyType );
        Debug.Log( "quaternionValue = " + serializedProperty.quaternionValue );
        Debug.Log( "rectValue = " + serializedProperty.rectValue );
        Debug.Log( "serializedObject = " + serializedProperty.serializedObject );
        Debug.Log( "stringValue = " + serializedProperty.stringValue );
        Debug.Log( "tooltip = " + serializedProperty.tooltip );
        Debug.Log( "type = " + serializedProperty.type );
        Debug.Log( "vector2Value = " + serializedProperty.vector2Value );
        Debug.Log( "vector3Value = " + serializedProperty.vector3Value );
    }

    public static void LogProperties( SerializedObject so, bool includeChildren = true )
    {
        // Shows all the properties in the serialized object with name and type
        // You can use this to learn the structure
        so.Update();
        SerializedProperty propertyLogger = so.GetIterator();

        while ( true )
        {
            Debug.Log( "name = " + propertyLogger.name + " type = " + propertyLogger.type );

            if ( !propertyLogger.Next( includeChildren ) )
            {
                break;
            }
        }
    }

    // variablePath may have a structure like this:
    // "meshData.Array.data[0].vertexColors"
    // So it uses FindProperty to get data from a specific field in an object array
    public static void SetSerializedProperty( Object obj, string variablePath, object variableValue )
    {
        SerializedObject so = new SerializedObject( obj );
        SerializedProperty sp = so.FindProperty( variablePath );

        if ( sp == null )
        {
            Debug.Log(
                "Error setting serialized property! Variable path: \"" +
                variablePath +
                "\" not found in object!" );

            return;
        }

        so.Update(); // refresh the data

        //SerializedPropertyType type = sp.propertyType; // get the property type
        Type valueType = variableValue.GetType(); // get the type of the incoming value

        if ( sp.isArray && valueType != typeof( string ) )
        {
            // serialized property is an array, except string which is also an array
            // assume the incoming value is also an array
            if ( !WriteSerializedArray( sp, variableValue ) )
            {
                return; // write the array
            }
        }
        else
        {
            // not an array
            if ( !WriteSerialzedProperty( sp, variableValue ) )
            {
                return; // write the value to the property
            }
        }

        so.ApplyModifiedProperties(); // apply the changes
    }

    #endregion

    #region Private

    private static bool WriteSerializedArray( SerializedProperty sp, object arrayObject )
    {
        Array[] array = ( Array[] ) arrayObject; // cast to array

        sp.Next( true ); // skip generic field
        sp.Next( true ); // advance to array size field

        // Set the array size
        if ( !WriteSerialzedProperty( sp, array.Length ) )
        {
            return false;
        }

        sp.Next( true ); // advance to first array index

        // Write values to array
        int lastIndex = array.Length - 1;

        for ( int i = 0; i < array.Length; i++ )
        {
            if ( !WriteSerialzedProperty( sp, array[i] ) )
            {
                return false; // write the value to the property
            }

            if ( i < lastIndex )
            {
                sp.Next( false ); // advance without drilling into children           
            }
        }

        return true;
    }

    private static bool WriteSerialzedProperty( SerializedProperty sp, object variableValue )
    {
        // Type the property and fill with new value
        SerializedPropertyType type = sp.propertyType; // get the property type

        if ( type == SerializedPropertyType.Integer )
        {
            int it = ( int ) variableValue;

            if ( sp.intValue != it )
            {
                sp.intValue = it;
            }
        }
        else if ( type == SerializedPropertyType.Boolean )
        {
            bool b = ( bool ) variableValue;

            if ( sp.boolValue != b )
            {
                sp.boolValue = b;
            }
        }
        else if ( type == SerializedPropertyType.Float )
        {
            float f = ( float ) variableValue;

            if ( sp.floatValue != f )
            {
                sp.floatValue = f;
            }
        }
        else if ( type == SerializedPropertyType.String )
        {
            string s = ( string ) variableValue;

            if ( sp.stringValue != s )
            {
                sp.stringValue = s;
            }
        }
        else if ( type == SerializedPropertyType.Color )
        {
            Color c = ( Color ) variableValue;

            if ( sp.colorValue != c )
            {
                sp.colorValue = c;
            }
        }
        else if ( type == SerializedPropertyType.ObjectReference )
        {
            Object o = ( Object ) variableValue;

            if ( sp.objectReferenceValue != o )
            {
                sp.objectReferenceValue = o;
            }
        }
        else if ( type == SerializedPropertyType.LayerMask )
        {
            int lm = ( int ) variableValue;

            if ( sp.intValue != lm )
            {
                sp.intValue = lm;
            }
        }
        else if ( type == SerializedPropertyType.Enum )
        {
            int en = ( int ) variableValue;

            if ( sp.enumValueIndex != en )
            {
                sp.enumValueIndex = en;
            }
        }
        else if ( type == SerializedPropertyType.Vector2 )
        {
            Vector2 v2 = ( Vector2 ) variableValue;

            if ( sp.vector2Value != v2 )
            {
                sp.vector2Value = v2;
            }
        }
        else if ( type == SerializedPropertyType.Vector3 )
        {
            Vector3 v3 = ( Vector3 ) variableValue;

            if ( sp.vector3Value != v3 )
            {
                sp.vector3Value = v3;
            }
        }
        else if ( type == SerializedPropertyType.Rect )
        {
            Rect r = ( Rect ) variableValue;

            if ( sp.rectValue != r )
            {
                sp.rectValue = r;
            }
        }
        else if ( type == SerializedPropertyType.ArraySize )
        {
            int aSize = ( int ) variableValue;

            if ( sp.intValue != aSize )
            {
                sp.intValue = aSize;
            }
        }
        else if ( type == SerializedPropertyType.Character )
        {
            int ch = ( int ) variableValue;

            if ( sp.intValue != ch )
            {
                sp.intValue = ch;
            }
        }
        else if ( type == SerializedPropertyType.AnimationCurve )
        {
            AnimationCurve ac = ( AnimationCurve ) variableValue;

            if ( sp.animationCurveValue != ac )
            {
                sp.animationCurveValue = ac;
            }
        }
        else if ( type == SerializedPropertyType.Bounds )
        {
            Bounds bounds = ( Bounds ) variableValue;

            if ( sp.boundsValue != bounds )
            {
                sp.boundsValue = bounds;
            }
        }
        else
        {
            Debug.Log( "Unsupported SerializedPropertyType \"" + type.ToString() + " encoutered!" );

            return false;
        }

        return true;
    }

    #endregion
}

}
