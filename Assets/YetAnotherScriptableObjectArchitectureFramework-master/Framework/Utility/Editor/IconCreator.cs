using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;

public class IconCreator : EditorWindow
{
    Mesh mPreviewMesh;
    Material mPreviewMaterial;
    PreviewRenderUtility mPreviewRenderUtility;


    [MenuItem("Tools/IconCreator")]
    static void Setup()
    {
        GetWindow<IconCreator>();
    }

    void OnGUI()
    {
        if (mPreviewRenderUtility == null)
        {
            mPreviewRenderUtility = new PreviewRenderUtility 
            {
                camera =
                {
                    farClipPlane = 500, clearFlags = CameraClearFlags.SolidColor, backgroundColor = Color.red,
                    transform = {position = new Vector3(0, 0, -10)}
                }
            };

            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var meshFilter = go.GetComponent<MeshFilter>();
            mPreviewMesh = meshFilter.sharedMesh;
            mPreviewMaterial = go.GetComponent<MeshRenderer>().sharedMaterial;

            DestroyImmediate(go);
        }

        var drawRect = new Rect(0, 0, 150, 150);
        
        mPreviewRenderUtility.BeginPreview(drawRect, GUIStyle.none);

        InternalEditorUtility.SetCustomLighting(mPreviewRenderUtility.lights, new Color(0.6f, 0.6f, 0.6f, 1f));

        mPreviewRenderUtility.DrawMesh(mPreviewMesh, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(30, 45, 0), Vector3.one), mPreviewMaterial, 0);
        mPreviewRenderUtility.camera.clearFlags = CameraClearFlags.SolidColor;
        mPreviewRenderUtility.camera.backgroundColor = Color.red;
        mPreviewRenderUtility.camera.Render();
        var texture = mPreviewRenderUtility.EndPreview();

        InternalEditorUtility.RemoveCustomLighting();

        GUI.Box(drawRect, texture);
    }
    
    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width*height];
 
        for(int i = 0; i < pix.Length; i++)
            pix[i] = col;
 
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
 
        return result;
    }
}