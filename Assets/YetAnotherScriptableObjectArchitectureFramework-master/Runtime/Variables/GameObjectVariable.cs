using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableVariablesAndReferences
{
    [CreateAssetMenu(fileName = "gameObjectVar",menuName ="Variables/GameObject")]
    public class GameObjectVariable : Variable<GameObject>
    {
        public override GameObject RuntimeValue()
        {
            return m_RuntimeValue;
        }
    }
}
