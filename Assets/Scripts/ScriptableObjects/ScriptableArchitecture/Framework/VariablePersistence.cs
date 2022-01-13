using System.IO;
using OdinSerializer;
using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Framework
{

public class VariablePersistence : MonoBehaviour
{
    private const string SavePath = "Temp/save.savefile";
    public VariableManager VariableManager;

    private void Start()
    {
        if ( VariableManager == null )
        {
            Debug.LogError( "No variable manager assigned!" );

            return;
        }
#if UNITY_EDITOR
        VariableManager.RefreshList();
#endif
    }

    [ContextMenu( "Save" )]
    public void Save()
    {
#if UNITY_EDITOR
        VariableManager.RefreshList();
#endif
        VariableRevision revision = ScriptableObject.CreateInstance < VariableRevision >();
        revision.LoadFromList( VariableManager.Variables );
        FileStream file = File.Create( SavePath );

        UnitySerializationUtility.SerializeUnityObject(
            revision,
            new JsonDataWriter( file, new SerializationContext() ) );

        file.Close();
    }

    [ContextMenu( "Load" )]
    public void Load()
    {
        if ( File.Exists( SavePath ) )
        {
            FileStream file = File.Open( SavePath, FileMode.Open );
            VariableRevision revision = ScriptableObject.CreateInstance < VariableRevision >();

            UnitySerializationUtility.DeserializeUnityObject(
                revision,
                new JsonDataReader( file, new DeserializationContext() ) );

            revision.RestoreVariable( VariableManager.Variables );
            file.Close();
        }
    }
}

}
