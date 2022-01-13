using ScriptableObjects.ScriptableArchitecture.Framework;
using UnityEngine;

namespace ScriptableVariablesAndReferences
{
    [CreateAssetMenu(fileName = "TransformVariable",menuName ="Variables/TransformVariable")]
    public class TransformVariable : Variable<Transform>
    {
        public override Transform RuntimeValue()
        {
            return m_RuntimeValue;
        }
    }
}