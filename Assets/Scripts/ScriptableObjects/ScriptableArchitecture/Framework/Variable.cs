using System;
using OdinSerializer;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Framework
{

public abstract class Variable < T > : BaseVariable
{
    public T DefaultValue;

    public VariableEvent < T > ChangedValueVariableEvent;

    public bool IsInitialized = false;

    [OdinSerialize]
    protected T m_RuntimeValue;

    protected void OnEnable()
    {
        if ( Application.isPlaying )
        {
            if ( !IsInitialized )
            {
                m_RuntimeValue = DefaultValue;
                IsInitialized = true;
            }
        }
        else
        {
            IsInitialized = false;
        }
    }

    public abstract T RuntimeValue();

    public void SetRuntimeValue( T val )
    {
        m_RuntimeValue = val;
    }

    public override ScriptableData GetScriptableData()
    {
        return new ScriptableData < T >
        {
            IsInitialized = IsInitialized, DefaultValue = DefaultValue, RuntimeValue = RuntimeValue()
        };
    }

    public override void LoadScriptableData( ScriptableData data )
    {
        IsInitialized = ( ( ScriptableData < T > ) data ).IsInitialized;
        DefaultValue = ( ( ScriptableData < T > ) data ).DefaultValue;
        m_RuntimeValue = ( ( ScriptableData < T > ) data ).RuntimeValue;
    }

    [Serializable]
    private class ScriptableData < TV > : ScriptableData
    {
        public bool IsInitialized = false;
        public TV DefaultValue;
        public TV RuntimeValue;
    }
}

}
