using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Framework.Utility.Editor
{

public class BlendTreeCopyPaste : MonoBehaviour
{
    private const string workDir = "Assets/Editor/AnimTools/BlendTreeCopyPaste/";
    private const string filename = "btcopy_";
    private static int depth = 0;
    private static string treePath = "";
    private static string log = "";
    private static BlendTree useTree = null;

    private static void makeWorkDirIfDoesntExist()
    {
        string path = "Assets";
        string[] split = workDir.Split( '/' );

        for ( int i = 1; i < split.Length; i++ )
        {
            string p = split[i];
            string newpath = path + "/" + p;

            if ( !AssetDatabase.IsValidFolder( newpath ) )
            {
                AssetDatabase.CreateFolder( path, p );
            }

            path = newpath;
        }
    }

    //===========================================================
    private static void ClearConsole()
    {
        Type logEntries = Type.GetType( "UnityEditor.LogEntries, UnityEditor.dll" );
        MethodInfo clearMethod = logEntries.GetMethod( "Clear", BindingFlags.Static | BindingFlags.Public );
        clearMethod.Invoke( null, null );
    }

    //===========================================================
    public static bool isBlendTree( ChildMotion motion )
    {
        try
        {
            BlendTreeType treeType = ( motion.motion as BlendTree ).blendType;
        }
        catch
        {
            return false;
        }

        return true;
    }

    //===========================================================
    public static BlendTree getBlendTreeFromSelection()
    {
        BlendTree bt = useTree == null ? Selection.activeObject as BlendTree : useTree;

        if ( bt == null )
        {
            bt = ( Selection.activeObject as AnimatorState ).motion as BlendTree;
        }

        return bt;
    }

    //===========================================================
    public static string getLogPath()
    {
        return Application.dataPath.Substring( 0, Application.dataPath.Length - "Assets".Length ) + workDir + "log.txt";
    }

    //===========================================================
    [MenuItem( "AnimTools/Blend Tree/Copy Tree" )]
    private static void CopyBlendTree()
    {
        int notCopied = 0;

        // Get selected blendTree ...
        BlendTree bt = useTree == null ? getBlendTreeFromSelection() : useTree;

        if ( bt == null )
        {
            Debug.LogError( "BlendTreeCopy - Error: No selected blend tree" );

            return;
        }

        // Copy directory ...
        makeWorkDirIfDoesntExist();
        log = "";
        depth = -1;
        treePath = "btcopy_";
        CopyTreeRecursive( bt, 0 );

        // Save log
        ClearConsole();
        File.WriteAllText( getLogPath(), log );

        Debug.Log(
            "BlendTree copied!" +
            ( notCopied > 0 ? " (" + notCopied.ToString() + " child blend trees not copied)" : "" ) );
    }

    //===========================================================
    public static void CopyTreeRecursive( BlendTree t, int ichild )
    {
        string oldpath = treePath;

        treePath += depth.ToString() + "," + ichild.ToString() + "_";

        // Save 't's motions ...
        BlendTree tclone = Instantiate < BlendTree >( t );
        string fpath = workDir + filename + depth.ToString() + "," + ichild.ToString() + ".asset";
        AssetDatabase.CreateAsset( tclone, workDir + filename + depth.ToString() + "," + ichild.ToString() + ".asset" );
        log += fpath + "\n";

        // Save children (recursive) ...
        depth++;

        for ( int i = 0; i < t.children.Length; i++ )
        {
            if ( isBlendTree( t.children[i] ) )
            {
                CopyTreeRecursive( t.children[i].motion as BlendTree, i );
            }
        }

        depth--;
        treePath = oldpath;
    }

    //===========================================================
    [MenuItem( "AnimTools/Blend Tree/Paste" )]
    private static void PasteBlendTree()
    {
        try
        {
            BlendTree bt = useTree == null ? getBlendTreeFromSelection() : useTree;
            string[] lines = File.ReadAllLines( getLogPath() );
            List < BlendTree > trees = new List < BlendTree >();

            for ( int i = 0; i < lines.Length; i++ )
            {
                trees.Add( AssetDatabase.LoadAssetAtPath < BlendTree >( lines[i] ) );
            }

            for ( int i = 1; i < lines.Length; i++ )
            {
                string l = lines[i].Substring( ( workDir + filename ).Length );
                l = l.Substring( 0, l.Length - ".asset".Length );

                if ( l.Length == 0 )
                {
                    continue;
                }

                Debug.Log( l );
                string[] split = l.Split( ',' );
                int a = int.Parse( split[0] );
                int b = int.Parse( split[1] );
                trees[a].children[b].motion = trees[i];
            }

            pasteBlendTreeSettings( bt, trees[0] );
            ClearConsole();
            Debug.Log( "BlendTree pasted!" );
        }
        catch
        {
            Debug.LogError( "BlendTree - Error pasting!" );
        }
    }

    public static void pasteBlendTreeSettings( BlendTree bt, BlendTree paste )
    {
        bt.blendType = paste.blendType;
        bt.minThreshold = paste.minThreshold;
        bt.maxThreshold = paste.maxThreshold;
        bt.useAutomaticThresholds = paste.useAutomaticThresholds;
        bt.hideFlags = paste.hideFlags;
        bt.children = paste.children.Clone() as ChildMotion[];
        bt.blendParameter = paste.blendParameter;
        bt.blendParameterY = paste.blendParameterY;
    }

    public class Pair < T1, T2 >
    {
        public T1 First;
        public T2 Second;

        #region Public

        public Pair( T1 first, T2 second )
        {
            First = first;
            Second = second;
        }

        #endregion
    }
}

}
