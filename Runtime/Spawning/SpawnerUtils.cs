using System;
using UnityEngine;

namespace BrightLib.Pooling.Runtime
{
    public static class SpawnerUtils
    {
        public static SpawnPoint FetchSpawnPoint(SpawnPoint[] spawnPoints, SpawnDistanceType spawnDistance)
        {
            int targetIndex;
            if (spawnDistance == SpawnDistanceType.Far)
            {
                targetIndex = FetchFarthestSpawnPoint(spawnPoints);
            }
            else
            {
                targetIndex = FetchClosestSpawnPoint(spawnPoints);
            }

            var targerSpawnPoint = spawnPoints[targetIndex];
            targerSpawnPoint.MarkUse();
            return targerSpawnPoint;
        }

        /// <summary>
        /// Returns the farthest spawnPoint fromn the player
        /// </summary>
        public static int FetchFarthestSpawnPoint(SpawnPoint[] spawnPoints, int ignoreIndex = -1)
        {
            var targetIndex = 0;
            var distance = -1f;

            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (i == ignoreIndex) continue;

                var sp = spawnPoints[i];
                if (sp.DistanceToPlayer >= distance)
                {
                    distance = sp.DistanceToPlayer;
                    targetIndex = i;
                }
            }

            return targetIndex;
        }

        /// <summary>
        /// Returns the closest spawnPoint fromn the player
        /// </summary>
        public static int FetchClosestSpawnPoint(SpawnPoint[] spawnPoints, int ignoreIndex = -1)
        {
            var targetIndex = 0;
            var distance = 9999f;

            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (i == ignoreIndex) continue;

                var sp = spawnPoints[i];
                if (sp.DistanceToPlayer <= distance)
                {
                    distance = sp.DistanceToPlayer;
                    targetIndex = i;
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
