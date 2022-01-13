using System.Collections.Generic;

namespace ScriptableObjects.ScriptableArchitecture.Framework.Utility
{

public class ReferenceEqualityComparer : EqualityComparer < object >
{
    #region Public

    public override bool Equals( object x, object y )
    {
        return ReferenceEquals( x, y );
    }

    public override int GetHashCode( object obj )
    {
        if ( obj == null )
        {
            return 0;
        }

        return obj.GetHashCode();
    }

    #endregion
}

}
