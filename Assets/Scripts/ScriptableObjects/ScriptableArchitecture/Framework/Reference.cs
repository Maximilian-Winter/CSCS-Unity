using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Reference<T>
{
    public bool UseConstant = true;
    public T ConstantValue;
    public Variable<T> Variable;

    public Reference()
    {
        
    }

    public Reference(T value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public T Value
    {
        get
        {
            if ( Application.isPlaying )
            {
                return UseConstant ? ConstantValue : Variable.RuntimeValue();
            }
            return UseConstant ? ConstantValue : Variable.DefaultValue;
        }
    }
    
    public static implicit operator Reference<T>(T value)
    {
        return new Reference<T>(value);
    }

    public static implicit operator T(Reference<T> reference)
    {
        return reference.Value;
    }
}
