using NUnit.Framework;
using P_SantaClash.Core;

namespace P_SantaClash.Tests
{
    public class CollisionTests
    {
        [Test]
        public void Intersects_WhenOverlapping_ReturnsTrue()
        {
            CoreRect a = new CoreRect(0, 0, 10, 10);
            CoreRect b = new CoreRect(5, 5, 10, 10);
            Assert.That(CollisionHelper.Intersects(a, b), Is.True);
        }

        [Test]
        public void Intersects_WhenSeparated_ReturnsFalse()
        {
            CoreRect a = new CoreRect(0, 0, 10, 10);
            CoreRect b = new CoreRect(20, 20, 5, 5);
            Assert.That(CollisionHelper.Intersects(a, b), Is.False);
        }

        [Test]
        public void Intersects_WhenTouchingEdge_ReturnsFalse_WithStrictAabb()
        {
            // Ici : Right == Left => pas de collision (strict)
            CoreRect a = new CoreRect(0, 0, 10, 10);
            CoreRect b = new CoreRect(10, 0, 10, 10);
            Assert.That(CollisionHelper.Intersects(a, b), Is.False);
        }
    }
}
