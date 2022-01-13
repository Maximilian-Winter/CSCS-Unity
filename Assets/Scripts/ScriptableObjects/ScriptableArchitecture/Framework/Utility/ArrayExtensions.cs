using System;

namespace ScriptableObjects.ScriptableArchitecture.Framework.Utility
{

public static class ArrayExtensions
{
    #region Public

    public static void ForEach( this Array array, Action < Array, int[] > action )
    {
        if ( array.LongLength == 0 )
        {
            return;
        }

        ArrayTraverse walker = new ArrayTraverse( array );

        do
        {
            action( array, walker.Position );
        }
        while ( walker.Step() );
    }

    #endregion
}

}
