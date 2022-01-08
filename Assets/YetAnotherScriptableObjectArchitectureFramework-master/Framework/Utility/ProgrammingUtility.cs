using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using OdinSerializer;
using UnityEngine;

namespace Utility
{

public static class ProgrammingUtility
{
    /// <summary>
    /// Alternative version of <see cref="Type.IsSubclassOf"/> that supports raw generic types (generic types without
    /// any type parameters).
    /// </summary>
    /// <param name="baseType">The base type class for which the check is made.</param>
    /// <param name="toCheck">To type to determine for whether it derives from <paramref name="baseType"/>.</param>
    public static bool IsSubclassOfRawGeneric( this Type toCheck, Type baseType )
    {
        while ( toCheck != typeof( object ) )
        {
            Type cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;

            if ( baseType == cur )
            {
                return true;
            }

            toCheck = toCheck.BaseType;
        }
        return false;
    }
    
    public static T OdinDeepClone<T>(this T a)
    {
        
        byte[] bytes = SerializationUtility.SerializeValue(a, DataFormat.Binary);
        return SerializationUtility.DeserializeValue<T>(bytes, DataFormat.Binary);
    }
    
    
    public static T OdinDeepCloneUnityObject<T>(this T a) where T : SerializedScriptableObject
    {
        T b = SerializedScriptableObject.CreateInstance<T>();
        using (MemoryStream ms = new MemoryStream())
        {
            UnitySerializationUtility.SerializeUnityObject(a, new BinaryDataWriter(ms, new SerializationContext()));
            UnitySerializationUtility.DeserializeUnityObject( b, new BinaryDataReader(ms, new DeserializationContext()) );
        }

        return b;
    }
    
    
    // Deep clone
    public static T DeepClone<T>(this T a)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, a);
            stream.Position = 0;
            return (T) formatter.Deserialize(stream);
        }
    }
}

}
