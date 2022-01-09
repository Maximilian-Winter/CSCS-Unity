using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OdinSerializer;
using SplitAndMerge;
using UnityEngine;
using Utility;

namespace ScriptableObjects.ScriptableArchitecture.Framework
{

[Serializable]
public class ScriptableData
{
}

public class ScriptableBase : SerializedScriptableObject

{
    [ScriptableId]
    public string Guid;

    public virtual ScriptableData GetScriptableData()
    {
        return null;
    }

    public virtual void LoadScriptableData( ScriptableData data )
    {
    }
}

}
