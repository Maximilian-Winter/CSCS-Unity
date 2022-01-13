using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Framework.Utility.Editor
{

public class CopyRigProperties : UnityEditor.Editor
{
    [MenuItem( "Tools/Rescale Rig" )]
    public static void RescaleRig()
    {
        GameObject[] selected = Selection.gameObjects;

        try
        {
            Transform sourceRig = selected.First().transform;

            foreach ( Transform t in sourceRig.GetComponentsInChildren < Transform >( true ) )
            {
                t.localPosition = t.localPosition / 100;
            }
        }
        catch ( Exception e )
        {
            Debug.LogError( e );
        }
    }

    [MenuItem( "Tools/Rescale Bounds" )]
    public static void RescaleBounds()
    {
        GameObject[] selected = Selection.gameObjects;

        try
        {
            Transform source = selected.First().transform;

            foreach ( SkinnedMeshRenderer r in source.GetComponentsInChildren < SkinnedMeshRenderer >( true ) )
            {
                Bounds b = r.localBounds;
                b.extents = b.extents / 100;
                b.center = b.center / 100;
                r.localBounds = b;
            }
        }
        catch ( Exception e )
        {
            Debug.LogError( e );
        }
    }
}

}
