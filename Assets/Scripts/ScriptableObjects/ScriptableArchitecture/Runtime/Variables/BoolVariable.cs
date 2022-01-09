using UnityEngine;

[CreateAssetMenu(fileName = "boolVar",menuName ="Variables/Bool")]
public class BoolVariable : Variable<bool>
{
    public override bool RuntimeValue()
    {
        return m_RuntimeValue;
    }
}
