using NUnit.Framework;
using P_SantaClash.Core;

namespace P_SantaClash.Tests
{
    public class AccuracyTests
    {
        [Test]
        public void Accuracy_WhenZeroShots_ReturnsZero()
        {
            PlayerStats p = new PlayerStats(1);
            Assert.That(p.Accuracy(), Is.EqualTo(0.0));
        }

        [Test]
        public void Accuracy_NormalCase()
        {
            PlayerStats p = new PlayerStats(1);
            p.RegisterShot();
            p.RegisterShot();
            p.RegisterShot();
            p.RegisterHit(); // 1 hit / 3 shots
            Assert.That(p.Accuracy(), Is.EqualTo(1.0 / 3.0).Within(1e-9));
        }

        [Test]
        public void Score_IncrementsOnHit()
        {
            PlayerStats p = new PlayerStats(1);
            p.RegisterShot();
            p.RegisterHit();
            Assert.That(p.Score, Is.EqualTo(1));
        }
    }
}
