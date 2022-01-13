using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Framework.Utility
{

public class MathUtilities
{
    #region Public

    public static int GetAngleFromVector( Vector3 dir )
    {
        dir = dir.normalized;
        float n = Mathf.Atan2( dir.y, dir.x ) * Mathf.Rad2Deg;

        if ( n < 0 )
        {
            n += 360;
        }

        int angle = Mathf.RoundToInt( n );

        return angle;
    }

    public static int GetAngleFromVector180( Vector3 dir )
    {
        dir = dir.normalized;
        float n = Mathf.Atan2( dir.y, dir.x ) * Mathf.Rad2Deg;
        int angle = Mathf.RoundToInt( n );

        return angle;
    }

    public static float GetAngleFromVectorFloat( Vector3 dir )
    {
        dir = dir.normalized;
        float n = Mathf.Atan2( dir.y, dir.x ) * Mathf.Rad2Deg;

        if ( n < 0 )
        {
            n += 360;
        }

        return n;
    }

    public static float GetAngleFromVectorFloatXZ( Vector3 dir )
    {
        dir = dir.normalized;
        float n = Mathf.Atan2( dir.z, dir.x ) * Mathf.Rad2Deg;

        if ( n < 0 )
        {
            n += 360;
        }

        return n;
    }

    public static Vector3 GetVectorFromAngle( int angle )
    {
        // angle = 0 -> 360
        float angleRad = angle * ( Mathf.PI / 180f );

        return new Vector3( Mathf.Cos( angleRad ), Mathf.Sin( angleRad ) );
    }

    public static Vector3 GetVectorFromAngle( float angle )
    {
        // angle = 0 -> 360
        float angleRad = angle * ( Mathf.PI / 180f );

        return new Vector3( Mathf.Cos( angleRad ), Mathf.Sin( angleRad ) );
    }

    public static Vector3 GetVectorFromAngleInt( int angle )
    {
        // angle = 0 -> 360
        float angleRad = angle * ( Mathf.PI / 180f );

        return new Vector3( Mathf.Cos( angleRad ), Mathf.Sin( angleRad ) );
    }

    public static float Remap( float value, float from1, float to1, float from2, float to2 )
    {
        return ( value - from1 ) / ( to1 - from1 ) * ( to2 - from2 ) + from2;
    }

    #endregion
}

}
