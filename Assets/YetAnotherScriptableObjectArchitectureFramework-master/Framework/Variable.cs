using System;
using System.Collections.Generic;
using OdinSerializer;
using ScriptableObjects.ScriptableArchitecture.Framework;
using UnityEngine;


public abstract class Variable<T> : BaseVariable
{
	[Serializable]
	private class ScriptableData<TV> : ScriptableData
	{
		public bool IsInitialized = false;
		public TV DefaultValue;
		public TV RuntimeValue;
	}

	public T DefaultValue;
	
	[OdinSerialize]
	protected T m_RuntimeValue;

	public VariableEvent<T> ChangedValueVariableEvent;
	
	public bool IsInitialized = false;

	public abstract T RuntimeValue();

	public void SetRuntimeValue( T val )
	{
		m_RuntimeValue = val;
	}

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

	public override ScriptableData GetScriptableData()
	{
		return new ScriptableData<T>
		{
			IsInitialized = IsInitialized,
			DefaultValue = DefaultValue,
			RuntimeValue = RuntimeValue()
		};
	}

	public override void LoadScriptableData(ScriptableData data)
	{
		IsInitialized =  ((ScriptableData<T>) data).IsInitialized;
		DefaultValue =  ((ScriptableData<T>) data).DefaultValue;
		m_RuntimeValue =  ((ScriptableData<T>) data).RuntimeValue;
	}
}
