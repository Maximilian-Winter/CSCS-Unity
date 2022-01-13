using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Framework.Utility.Editor
{

public class LODExtendedUtility
{
    #region Public

    //Get the LOD # of a selected GameObject. If the GameObject is not part of any LODGroup returns -1.
    public static int GetLODid( GameObject GO )
    {
        LODGroup LODGroupParent = GO.GetComponentInParent < LODGroup >();

        if ( LODGroupParent == null )
        {
            return -1;
        }

        LOD[] LODs = LODGroupParent.GetLODs();

        int index = Array.FindIndex(
            LODs,
            lod => lod.renderers.Where( renderer => renderer == GO.GetComponent < Renderer >() ).ToArray().Count() >
                   0 );

        return index;
    }

    public static int GetMaxLOD( LODGroup lodGroup )
    {
        return lodGroup.lodCount - 1;
    }

    //Return the LODGroup component with a renderer pointing to a specific GameObject. If the GameObject is not part of a LODGroup, returns null 
    public static LODGroup GetParentLODGroupComponent( GameObject GO )
    {
        LODGroup LODGroupParent = GO.GetComponentInParent < LODGroup >();

        if ( LODGroupParent == null )
        {
            return null;
        }

        LOD[] LODs = LODGroupParent.GetLODs();

        LOD[] FoundLOD = LODs.Where(
                                  lod => lod.renderers.Where( renderer => renderer == GO.GetComponent < Renderer >() ).
                                             ToArray().
                                             Count() >
                                         0 ).
                              ToArray();

        if ( FoundLOD != null && FoundLOD.Count() > 0 )
        {
            return LODGroupParent;
        }

        return null;
    }

    //Return the GameObject of the LODGroup component with a renderer pointing to a specific GameObject. If the GameObject is not part of a LODGroup, returns null.
    public static GameObject GetParentLODGroupGameObject( GameObject GO )
    {
        LODGroup LODGroup = GetParentLODGroupComponent( GO );

        return LODGroup == null ? null : LODGroup.gameObject;
    }

    //returns the currently visible LOD level of a specific LODGroup, from a specific camera. If no camera is define, uses the Camera.current.
    public static int GetVisibleLOD( LODGroup lodGroup, Camera camera = null )
    {
        LOD[] lods = lodGroup.GetLODs();
        float relativeHeight = GetRelativeHeight( lodGroup, camera ?? Camera.current );

        int lodIndex = GetMaxLOD( lodGroup );

        for ( int i = 0; i < lods.Length; i++ )
        {
            LOD lod = lods[i];

            if ( relativeHeight >= lod.screenRelativeTransitionHeight )
            {
                lodIndex = i;

                break;
            }
        }

        return lodIndex;
    }

    //returns the currently visible LOD level of a specific LODGroup, from a the SceneView Camera.
    public static int GetVisibleLODSceneView( LODGroup lodGroup )
    {
        Camera camera = SceneView.lastActiveSceneView.camera;

        return GetVisibleLOD( lodGroup, camera );
    }

    public static float GetWorldSpaceSize( LODGroup lodGroup )
    {
        return GetWorldSpaceScale( lodGroup.transform ) * lodGroup.size;
    }

    #endregion

    #region Private

    private static float DistanceToRelativeHeight( Camera camera, float distance, float size )
    {
        if ( camera.orthographic )
        {
            return size * 0.5F / camera.orthographicSize;
        }

        float halfAngle = Mathf.Tan( Mathf.Deg2Rad * camera.fieldOfView * 0.5F );
        float relativeHeight = size * 0.5F / ( distance * halfAngle );

        return relativeHeight;
    }

    private static float GetRelativeHeight( LODGroup lodGroup, Camera camera )
    {
        float distance =
            ( lodGroup.transform.TransformPoint( lodGroup.localReferencePoint ) - camera.transform.position ).magnitude;

        return DistanceToRelativeHeight( camera, distance / QualitySettings.lodBias, GetWorldSpaceSize( lodGroup ) );
    }

    private static float GetWorldSpaceScale( Transform t )
    {
        Vector3 scale = t.lossyScale;
        float largestAxis = Mathf.Abs( scale.x );
        largestAxis = Mathf.Max( largestAxis, Mathf.Abs( scale.y ) );
        largestAxis = Mathf.Max( largestAxis, Mathf.Abs( scale.z ) );

        return largestAxis;
    }

    #endregion
}

}
