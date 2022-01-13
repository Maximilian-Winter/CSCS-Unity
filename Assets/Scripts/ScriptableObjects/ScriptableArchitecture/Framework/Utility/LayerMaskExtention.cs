using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Framework.Utility
{

public static class LayerMaskExtention
{
    #region Public

    public static bool Contains( this LayerMask mask, int layer )
    {
        return mask == ( mask | ( 1 << layer ) );
    }

    #endregion
}

}
