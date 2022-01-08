using UnityEngine;

namespace ScriptableVariablesAndReferences
{
    [CreateAssetMenu(fileName = "intVar",menuName ="Variables/Integer")]
    public class IntVariable : Variable<int>
    {
        public override int RuntimeValue()
        {
            return m_RuntimeValue;
        }
    }
}