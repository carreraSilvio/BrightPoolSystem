using System.Collections.Generic;

namespace BrightLib.Pooling.Runtime
{
    public static class SpawnerUtils
    {
        public static SpawnPoint FetchSpawnPoint(SpawnPoint[] spawnPoints, SpawnDistanceType spawnDistance)
        {
            int targetIndex;
            if (spawnDistance == SpawnDistanceType.Far)
            {
                targetIndex = FetchFarthestSpawnPoint(spawnPoints, new List<int>());
            }
            else
            {
                targetIndex = FetchClosestSpawnPoint(spawnPoints, new List<int>());
            }

            var targerSpawnPoint = spawnPoints[targetIndex];
            targerSpawnPoint.MarkUse();
            return targerSpawnPoint;
        }

        /// <summary>
        /// Returns the farthest spawnPoint fromn the player
        /// </summary>
        public static int FetchFarthestSpawnPoint(SpawnPoint[] spawnPoints, List<int> ignoreIndex)
        {
            var targetIndex = 0;
            var distance = -1f;

            for (int spawnPointIndex = 0; spawnPointIndex < spawnPoints.Length; spawnPointIndex++)
            {
                if (ignoreIndex.Contains(spawnPointIndex)) continue;

                var sp = spawnPoints[spawnPointIndex];
                if (sp.DistanceToPlayer >= distance)
                {
                    distance = sp.DistanceToPlayer;
                    targetIndex = spawnPointIndex;
                }
            }

            return targetIndex;
        }

        /// <summary>
        /// Returns the closest spawnPoint fromn the player
        /// </summary>
        public static int FetchClosestSpawnPoint(SpawnPoint[] spawnPoints, List<int> ignoreIndex)
        {
            var targetIndex = 0;
            var distance = 9999f;

            for (int spawnPointIndex = 0; spawnPointIndex < spawnPoints.Length; spawnPointIndex++)
            {
                if (ignoreIndex.Contains(spawnPointIndex)) continue;

                var sp = spawnPoints[spawnPointIndex];
                if (sp.DistanceToPlayer <= distance)
                {
                    distance = sp.DistanceToPlayer;
                    targetIndex = spawnPointIndex;
                }
            }

            return targetIndex;
        }

        /// <summary>
        /// Returns a random spawn point
        /// </summary>
        public static int FetchRandomSpawnPoint(SpawnPoint[] spawnPoints)
        {
            var targetIndex = UnityEngine.Random.Range(0, spawnPoints.Length - 1);

            return targetIndex;
        }
    }

}
