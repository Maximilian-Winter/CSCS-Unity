using System;
using System.Collections.Generic;
using ScriptableObjects.ScriptableArchitecture.Framework.Utility;

namespace ScriptableObjects.ScriptableArchitecture.Framework
{

public class RuntimeSet < T > : ScriptableBase
{
    public List < T > Items = new List < T >();

    public void Add( T item )
    {
        if ( !Items.Contains( item ) )
        {
            Items.Add( item );
        }
    }

    public void Remove( T item )
    {
        if ( Items.Contains( item ) )
        {
            Items.Remove( item );
        }
    }

    public void CopyTo( List < T > list )
    {
        foreach ( T item in Items )
        {
            list.Add( item.OdinDeepClone() );
        }
    }

    public override ScriptableData GetScriptableData()
    {
        ScriptableData < T > data = new ScriptableData < T >();
        data.Items = new List < T >();
        CopyTo( data.Items );

        return data;
    }

    public override void LoadScriptableData( ScriptableData data )
    {
        ( ( ScriptableData < T > ) data ).CopyTo( Items );
    }

    [Serializable]
    private class ScriptableData < TV > : ScriptableData
    {
        public List < TV > Items;

        public void CopyTo( List < TV > list )
        {
            foreach ( TV item in Items )
            {
                list.Add( item.OdinDeepClone() );
            }
        }
    }
}

}
