using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Framework
{

[CreateAssetMenu( fileName = "ScriptableTag", menuName = "Variables/ScriptableTag" )]
public class ScriptableTag : ScriptableBase
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
