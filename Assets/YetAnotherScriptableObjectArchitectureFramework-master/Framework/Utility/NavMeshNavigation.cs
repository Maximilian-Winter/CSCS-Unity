using TerrainCustomTools;
using UnityEngine;
using UnityEngine.AI;

namespace NavMeshCustomTools
{
  
public enum AreaBits
{
    Walkable = 0,
    NotWalkable = 1,
    Jump = 2,
}

public enum AreaMasks : int
{
    WalkableMask = 1 << AreaBits.Walkable,
    NotWalkableMask = 1 << AreaBits.NotWalkable,
    JumpMask = 1 << AreaBits.Jump,
}
public static class NavMeshNavigation
{
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int navMeshAreaMask = ( int ) AreaMasks.WalkableMask) 
    {
        Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;
        randDirection += origin;
        UnityEngine.AI.NavMeshHit navHit;
        UnityEngine.AI.NavMesh.SamplePosition (randDirection, out navHit, dist, navMeshAreaMask);
        return navHit.position;
    }
    
    public static Vector3 RandomPointOnTerrainNavMesh(Vector3 origin, float range, int randomSeed, int navMeshAreaMask = ( int ) AreaMasks.WalkableMask ) 
    {
        Terrain startTerrain = TerrainUtility.GetClosestTerrain( origin);

        if ( startTerrain != null )
        {
            float terrainHeight = startTerrain.SampleHeight( origin );
            origin.y = terrainHeight;
        
            Random.InitState((int)Time.time + randomSeed);
            Vector3 randDirection = Random.insideUnitSphere * range;
            Vector3 randomPos = randDirection + origin;

            Terrain targetTerrain = TerrainUtility.GetClosestTerrain( randomPos );
            if ( targetTerrain != null )
            {
                terrainHeight = targetTerrain.SampleHeight( randomPos );
                randomPos.y = terrainHeight;
                NavMesh.SamplePosition (randomPos, out NavMeshHit navHit, 0.1f,  navMeshAreaMask);

                return navHit.position;
            }
            
            return Vector3.positiveInfinity;
        }
        return Vector3.positiveInfinity;
    }
    
    public static Vector3 GetNearestPointOnTerrainNavMesh(Vector3 point, int navMeshAreaMask = (int)AreaMasks.WalkableMask) 
    {
        Terrain terrain = TerrainUtility.GetClosestTerrain( point );

        if ( terrain != null )
        {
            float terrainHeight = terrain.SampleHeight( point );
            point.y = terrainHeight;
        
            UnityEngine.AI.NavMesh.SamplePosition (point, out NavMeshHit navHit, 0.1f, navMeshAreaMask);
            return navHit.position;
        }
        
        return Vector3.positiveInfinity;
    }
}

}