using System;
using System.Collections.Generic;
using OdinSerializer;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Framework
{

[Serializable]
public class VariableRevision : SerializedScriptableObject
{
    [SerializeField]
    private Dictionary < string, ScriptableData > Data = new Dictionary < string, ScriptableData >();

    public void LoadFromList( List < ScriptableBase > list )
    {
        Data.Clear();

        foreach ( ScriptableBase v in list )
        {
            Data.Add( v.Guid, v.GetScriptableData() );
        }
    }

    public void RestoreVariable( List < ScriptableBase > list )
    {
        foreach ( ScriptableBase v in list )
        {
            ScriptableData d;

            if ( Data.TryGetValue( v.Guid, out d ) )
            {
                v.LoadScriptableData( d );
            }
        }
    }
}

}
