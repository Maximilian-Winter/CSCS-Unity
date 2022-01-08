using UnityEngine;

namespace Utility
{

public static class LayerMaskExtention
{
    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}

}
