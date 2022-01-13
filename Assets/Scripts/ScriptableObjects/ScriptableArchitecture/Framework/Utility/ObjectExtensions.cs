﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace ScriptableObjects.ScriptableArchitecture.Framework.Utility
{

public static class ObjectExtensions
{
    private static readonly MethodInfo CloneMethod = typeof( object ).GetMethod(
        "MemberwiseClone",
        BindingFlags.NonPublic | BindingFlags.Instance );

    #region Public

    public static object Copy( this object originalObject )
    {
        return InternalCopy( originalObject, new Dictionary < object, object >( new ReferenceEqualityComparer() ) );
    }

    public static T Copy < T >( this T original )
    {
        return ( T ) Copy( ( object ) original );
    }

    public static bool IsPrimitive( this Type type )
    {
        if ( type == typeof( string ) )
        {
            return true;
        }

        return type.IsValueType & type.IsPrimitive;
    }

    #endregion

    #region Private

    private static void CopyFields(
        object originalObject,
        IDictionary < object, object > visited,
        object cloneObject,
        Type typeToReflect,
        BindingFlags bindingFlags =
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy,
        Func < FieldInfo, bool > filter = null )
    {
        foreach ( FieldInfo fieldInfo in typeToReflect.GetFields( bindingFlags ) )
        {
            if ( filter != null && filter( fieldInfo ) == false )
            {
                continue;
            }

            if ( IsPrimitive( fieldInfo.FieldType ) )
            {
                continue;
            }

            object originalFieldValue = fieldInfo.GetValue( originalObject );
            object clonedFieldValue = InternalCopy( originalFieldValue, visited );
            fieldInfo.SetValue( cloneObject, clonedFieldValue );
        }
    }

    private static object InternalCopy( object originalObject, IDictionary < object, object > visited )
    {
        if ( originalObject == null )
        {
            return null;
        }

        Type typeToReflect = originalObject.GetType();

        if ( IsPrimitive( typeToReflect ) )
        {
            return originalObject;
        }

        if ( visited.ContainsKey( originalObject ) )
        {
            return visited[originalObject];
        }

        if ( typeof( Delegate ).IsAssignableFrom( typeToReflect ) )
        {
            return null;
        }

        object cloneObject = CloneMethod.Invoke( originalObject, null );

        if ( typeToReflect.IsArray )
        {
            Type arrayType = typeToReflect.GetElementType();

            if ( IsPrimitive( arrayType ) == false )
            {
                Array clonedArray = ( Array ) cloneObject;

                clonedArray.ForEach(
                    ( array, indices ) => array.SetValue(
                        InternalCopy( clonedArray.GetValue( indices ), visited ),
                        indices ) );
            }
        }

        visited.Add( originalObject, cloneObject );
        CopyFields( originalObject, visited, cloneObject, typeToReflect );
        RecursiveCopyBaseTypePrivateFields( originalObject, visited, cloneObject, typeToReflect );

        return cloneObject;
    }

    private static void RecursiveCopyBaseTypePrivateFields(
        object originalObject,
        IDictionary < object, object > visited,
        object cloneObject,
        Type typeToReflect )
    {
        if ( typeToReflect.BaseType != null )
        {
            RecursiveCopyBaseTypePrivateFields( originalObject, visited, cloneObject, typeToReflect.BaseType );

            CopyFields(
                originalObject,
                visited,
                cloneObject,
                typeToReflect.BaseType,
                BindingFlags.Instance | BindingFlags.NonPublic,
                info => info.IsPrivate );
        }
    }

    #endregion
}

}
