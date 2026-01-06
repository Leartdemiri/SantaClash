using System.Collections.Generic;
using System.Linq;

namespace P_SantaClash.Core
{
    public static class StatsService
    {
        /// <summary>
        /// Classement final : score desc, précision desc.
        /// </summary>
        public static List<PlayerStats> RankPlayers(IEnumerable<PlayerStats> players)
        {
            return players
                .OrderByDescending(p => p.Score)
                .ThenByDescending(p => p.Accuracy())
                .ToList();
        }

        /// <summary>
        /// Exemple de LINQ : combien d'ennemis par type.
        /// </summary>
        public static Dictionary<EnemyType, int> EnemiesByType(IEnumerable<EnemySpawnInfo> spawns)
        {
            return spawns
                .GroupBy(s => s.Type)
                .ToDictionary(g => g.Key, g => g.Count());
        }
    }
}
