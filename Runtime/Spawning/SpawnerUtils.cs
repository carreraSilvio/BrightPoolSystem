using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    public static class SpawnerUtils
    {
        public static Vector3 FetchSpawnPointPosition(SpawnPoint[] spawnPoints, SpawnDistanceType spawnDistance)
        {
            var targetIndex = 0;
            var distance = -1f;

            for (int i = 0; i < spawnPoints.Length; i++)
            {
                var sp = spawnPoints[i];
                if (spawnDistance == SpawnDistanceType.Far)
                {
                    if (sp.DistanceToPlayer >= distance)
                    {
                        distance = sp.DistanceToPlayer;
                        targetIndex = i;
                    }
                }
                else if (spawnDistance == SpawnDistanceType.Close)
                {
                    if (sp.DistanceToPlayer <= distance)
                    {
                        distance = sp.DistanceToPlayer;
                        targetIndex = i;
                    }
                }
            }

            var targetSpawnPoint = spawnPoints[targetIndex];
            targetSpawnPoint.MarkUse();
            return targetSpawnPoint.transform.position;
        }
    }

}
