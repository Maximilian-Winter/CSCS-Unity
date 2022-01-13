using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Framework
{

public class ScriptableSingletonObject < T > : ScriptableBase where T : ScriptableBase
{
    private static T s_Instance;

    public static T Instance
    {
        get
        {
            if ( s_Instance == null )
            {
                T[] singeltons = Resources.FindObjectsOfTypeAll < T >();

                if ( singeltons.Length == 0 )
                {
                    Debug.LogErrorFormat(
                        "Scriptable Singleton Error! No instance of Type {0} found!",
                        typeof( T ).Name );
                }
                else if ( singeltons.Length > 1 )
                {
                    Debug.LogErrorFormat(
                        "Scriptable Singleton Error! More than one instance of Type {0} found!",
                        typeof( T ).Name );

                    s_Instance = singeltons[0];
                    s_Instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
                    ( s_Instance as ScriptableSingletonObject < T > ).InitializeSingleton();
                }
                else
                {
                    s_Instance = singeltons[0];
                    s_Instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
                    ( s_Instance as ScriptableSingletonObject < T > ).InitializeSingleton();
                }
            }

            return s_Instance;
        }
    }

    protected virtual void InitializeSingleton()
    {
    }
}

}
