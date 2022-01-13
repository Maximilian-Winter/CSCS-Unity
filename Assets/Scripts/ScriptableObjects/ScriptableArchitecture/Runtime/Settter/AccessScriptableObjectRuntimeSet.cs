using System;
using System.Collections.Generic;
using OdinSerializer;
using ScriptableObjects.ScriptableArchitecture.Framework;
using ScriptableVariablesAndReferences;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Settter
{

public class
    AccessScriptableObjectRuntimeSet : MonoBehaviour
{
    public ScriptableRuntimeSet ScriptableRuntimeSet;
    public bool InstantiateLocalRuntimeSetOnEnable;
    public bool IsInitialized = false;

    protected void OnEnable()
    {
        if ( Application.isPlaying && !IsInitialized )
        {
            if ( InstantiateLocalRuntimeSetOnEnable && IsInitialized == false )
            {
                ScriptableRuntimeSet = CopyObjectsToNewSet();
            }

            IsInitialized = true;
        }
        else
        {
            IsInitialized = false;
        }
    }

    public ScriptableRuntimeSet CopyObjectsToNewSet()
    {
        ScriptableRuntimeSet b = ScriptableObject.CreateInstance<ScriptableRuntimeSet>();
        b.name = this.name;
        List < ScriptableBase > temp = new List < ScriptableBase >();
        b.Items = new List < ScriptableBase >();
        foreach ( ScriptableBase item in ScriptableRuntimeSet.Items )
        {
            Type type = item.GetType();
            /*if ( type == typeof( AgentWorldStateIntVariable ) )
            {
                temp.Add( (AgentWorldStateIntVariable)ScriptableObject.CreateInstance(type) );
                AgentWorldStateIntVariable tempItem = ( AgentWorldStateIntVariable ) item;
                
                temp[^1].LoadScriptableData( tempItem.GetScriptableData() );
                
                temp[^1].Guid = Guid.NewGuid().ToString();
            }*/
        }

        b.Items = temp;
        
        b.Guid = Guid.NewGuid().ToString();
        
        return b;
    }
    
}

}
