using UnityEditor;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Framework.Utility.Editor
{

public static class CopyMesh
{
    #region Private

    [MenuItem( "Assets/CopySelectedMeshAndSaveItInSamePath" )]
    private static void CopySelectedMeshAndSaveItInSamePath()
    {
        Mesh mesh = Selection.activeObject as Mesh;
        Mesh newmesh = new Mesh();
        newmesh.vertices = mesh.vertices;
        newmesh.triangles = mesh.triangles;
        newmesh.uv = mesh.uv;
        newmesh.normals = mesh.normals;
        newmesh.colors = mesh.colors;
        newmesh.tangents = mesh.tangents;
        AssetDatabase.CreateAsset( newmesh, AssetDatabase.GetAssetPath( mesh ) + " copy.asset" );
    }

    #endregion
}

}
