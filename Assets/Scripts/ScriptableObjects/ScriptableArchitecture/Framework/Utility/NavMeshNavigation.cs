using UnityEngine;
using UnityEngine.AI;

namespace ScriptableObjects.ScriptableArchitecture.Framework.Utility
{

public static class NavMeshNavigation
{
    #region Public

    public static Vector3 GetNearestPointOnTerrainNavMesh(
        Vector3 point,
        int navMeshAreaMask = ( int ) AreaMasks.WalkableMask )
    {
        Terrain terrain = TerrainUtility.GetClosestTerrain( point );

        if ( terrain != null )
        {
            float terrainHeight = terrain.SampleHeight( point );
            point.y = terrainHeight;

            NavMesh.SamplePosition( point, out NavMeshHit navHit, 0.1f, navMeshAreaMask );

            return navHit.position;
        }

        return Vector3.positiveInfinity;
    }

    public static Vector3 RandomNavSphere(
        Vector3 origin,
        float dist,
        int navMeshAreaMask = ( int ) AreaMasks.WalkableMask )
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition( randDirection, out navHit, dist, navMeshAreaMask );

        return navHit.position;
    }

    public static Vector3 RandomPointOnTerrainNavMesh(
        Vector3 origin,
        float range,
        int randomSeed,
        int navMeshAreaMask = ( int ) AreaMasks.WalkableMask )
    {
        Terrain startTerrain = TerrainUtility.GetClosestTerrain( origin );

        if ( startTerrain != null )
        {
            float terrainHeight = startTerrain.SampleHeight( origin );
            origin.y = terrainHeight;

            Random.InitState( ( int ) Time.time + randomSeed );
            Vector3 randDirection = Random.insideUnitSphere * range;
            Vector3 randomPos = randDirection + origin;

            Terrain targetTerrain = TerrainUtility.GetClosestTerrain( randomPos );

            if ( targetTerrain != null )
            {
                terrainHeight = targetTerrain.SampleHeight( randomPos );
                randomPos.y = terrainHeight;
                NavMesh.SamplePosition( randomPos, out NavMeshHit navHit, 0.1f, navMeshAreaMask );

                return navHit.position;
            }

            return Vector3.positiveInfinity;
        }

        return Vector3.positiveInfinity;
    }

    #endregion
}

}
