using NUnit.Framework;
using P_SantaClash.Core;
using System.Linq;

namespace P_SantaClash.Tests
{
    public class WaveTests
    {
        [Test]
        public void GenerateWave_Wave1_HasExpectedCount()
        {
            WaveGenerator gen = new WaveGenerator(seed: 123);
            List<EnemySpawnInfo> wave = gen.GenerateWave(1);
            Assert.That(wave.Count, Is.EqualTo(7)); // 5 + 1*2
        }

        [Test]
        public void GenerateWave_Wave5_HasExpectedCount()
        {
            WaveGenerator gen = new WaveGenerator(seed: 123);
            List<EnemySpawnInfo> wave = gen.GenerateWave(5);
            Assert.That(wave.Count, Is.EqualTo(15)); // 5 + 5*2
        }

        [Test]
        public void CountByType_SumsToTotal()
        {
            WaveGenerator gen = new WaveGenerator(seed: 42);
            List<EnemySpawnInfo> wave = gen.GenerateWave(3);
            var (slow, fast) = gen.CountByType(wave);
            Assert.That(slow + fast, Is.EqualTo(wave.Count));
        }

        [Test]
        public void EnemiesByType_GroupBy_ReturnsBothKeysSometimes()
        {
            WaveGenerator gen = new WaveGenerator(seed: 7);
            List<EnemySpawnInfo> wave = gen.GenerateWave(4);
            Dictionary<EnemyType, int> dict = StatsService.EnemiesByType(wave);

            // Au minimum : la somme vaut le total
            Assert.That(dict.Values.Sum(), Is.EqualTo(wave.Count));
        }
    }
}
