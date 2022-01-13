using OdinSerializer;
using ScriptableObjects.ScriptableArchitecture.Framework.Utility;

namespace ScriptableObjects.ScriptableArchitecture.Framework
{

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
