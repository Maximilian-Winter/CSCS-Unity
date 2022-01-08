using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Framework
{

public abstract class ScriptableSystem<T> : ScriptableSingletonObject<T> where T : ScriptableBase
{
    protected MonoBehaviour m_SystemUpdater;
    
    public virtual void IntitalizeSystem(MonoBehaviour systemUpdater)
    {
        m_SystemUpdater = systemUpdater;
    }
    public virtual void UpdateSystem(float deltaTime)
    {
    }
    public virtual void ShutdownSystem()
    {
    }

    protected override void InitializeSingleton()
    {
        
    }
    
    
}

}
