using UnityEngine;

namespace ScriptableObjects.ScriptableArchitecture.Framework.Utility
{

public static class TerrainUtility
{
    #region Public

    public static Terrain GetClosestTerrain( Vector3 worldPos )
    {
        Terrain[] terrains = Terrain.activeTerrains;

        if ( terrains.Length == 0 )
        {
            return null;
        }

        if ( terrains.Length == 1 )
        {
            return terrains[0];
        }

        for ( int i = 0; i < terrains.Length; i++ )
        {
            Terrain terrain = terrains[i];
            Bounds terrainBounds = terrain.terrainData.bounds;
            Vector3 terrainWorldPosition = terrain.transform.position;

            if ( worldPos.x >= terrainBounds.min.x + terrainWorldPosition.x )
            {
                if ( worldPos.x <= terrainBounds.max.x + terrainWorldPosition.x )
                {
                    if ( worldPos.z >= terrainBounds.min.z + terrainWorldPosition.z )
                    {
                        if ( worldPos.z <= terrainBounds.max.z + terrainWorldPosition.z )
                        {
                            return terrains[i];
                        }
                    }
                }
            }
        }

        return null;
    }

    #endregion
}

}
