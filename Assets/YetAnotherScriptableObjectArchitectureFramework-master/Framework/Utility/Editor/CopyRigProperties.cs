using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CopyRigProperties : UnityEditor.Editor
{


    [MenuItem("Tools/Rescale Rig")]
    public static void RescaleRig()
    {
        var selected = Selection.gameObjects;
        try
        {
            var sourceRig = selected.First().transform;
            foreach (var t in sourceRig.GetComponentsInChildren<Transform>(true))
            {
                t.localPosition = t.localPosition / 100;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    [MenuItem("Tools/Rescale Bounds")]
    public static void RescaleBounds()
    {
        var selected = Selection.gameObjects;
        try
        {
            var source = selected.First().transform;
            foreach (var r in source.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                var b = r.localBounds;
                b.extents = b.extents / 100;
                b.center = b.center / 100;
                r.localBounds = b;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}
