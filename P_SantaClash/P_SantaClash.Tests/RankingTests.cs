using NUnit.Framework;
using P_SantaClash.Core;
using System.Collections.Generic;

namespace P_SantaClash.Tests
{
    public class RankingTests
    {
        [Test]
        public void RankPlayers_SortsByScoreDesc()
        {
            PlayerStats p1 = new PlayerStats(1);
            PlayerStats p2 = new PlayerStats(2);

            // p1 score 2
            p1.RegisterShot(); p1.RegisterHit();
            p1.RegisterShot(); p1.RegisterHit();

            // p2 score 1
            p2.RegisterShot(); p2.RegisterHit();

            List<PlayerStats> ranked = StatsService.RankPlayers(new List<PlayerStats> { p2, p1 });
            Assert.That(ranked[0].PlayerId, Is.EqualTo(1));
        }

        [Test]
        public void RankPlayers_TieBreakByAccuracy()
        {
            PlayerStats p1 = new PlayerStats(1);
            PlayerStats p2 = new PlayerStats(2);

            // même score : 2
            p1.RegisterShot(); p1.RegisterHit();
            p1.RegisterShot(); p1.RegisterHit(); // 2/2 => 100%

            p2.RegisterShot(); p2.RegisterHit();
            p2.RegisterShot();
            p2.RegisterHit(); // 2/3 => 66%

            List<PlayerStats> ranked = StatsService.RankPlayers(new List<PlayerStats> { p2, p1 });
            Assert.That(ranked[0].PlayerId, Is.EqualTo(1));
        }
    }
}
