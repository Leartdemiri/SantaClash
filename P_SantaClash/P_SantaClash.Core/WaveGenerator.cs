using System;
using System.Collections.Generic;
using System.Linq;

namespace P_SantaClash.Core
{
    public class WaveGenerator
    {
        private readonly Random _random;

        public WaveGenerator(int? seed = null)
        {
            _random = seed.HasValue ? new Random(seed.Value) : new Random();
        }

        /// <summary>
        /// Génère une vague : plus la vague est haute, plus il y a d'ennemis.
        /// Renvoie une liste d'EnemySpawnInfo (types).
        /// </summary>
        public List<EnemySpawnInfo> GenerateWave(int waveNumber)
        {
            if (waveNumber < 1) waveNumber = 1;

            int count = 5 + waveNumber * 2;

            // Exemple : répartition 60/40 qui se durcit avec la vague
            double fastChance = Math.Min(0.65, 0.30 + waveNumber * 0.03);

            var spawns = Enumerable.Range(0, count)
                .Select(_ =>
                {
                    bool fast = _random.NextDouble() < fastChance;
                    return new EnemySpawnInfo(fast ? EnemyType.Fast : EnemyType.Slow);
                })
                .ToList();

            return spawns;
        }

        public (int slow, int fast) CountByType(IEnumerable<EnemySpawnInfo> spawns)
        {
            var groups = spawns
                .GroupBy(s => s.Type)
                .ToDictionary(g => g.Key, g => g.Count());

            groups.TryGetValue(EnemyType.Slow, out int slow);
            groups.TryGetValue(EnemyType.Fast, out int fast);
            return (slow, fast);
        }
    }
}
