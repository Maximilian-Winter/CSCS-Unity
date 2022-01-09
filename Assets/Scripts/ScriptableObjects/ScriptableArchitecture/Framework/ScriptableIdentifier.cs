using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Framework
{

[CreateAssetMenu( menuName = "Variables/ScriptableIdentifier", fileName = "ScriptableIdentifier", order = 0 )]
public class ScriptableIdentifier : ScriptableBase
{
    public override ScriptableData GetScriptableData()
    {
        return null;
    }

    public override void LoadScriptableData( ScriptableData data )
    {
    }
}

}
