using Microsoft.Xna.Framework;

namespace P_SantaClash
{
    public interface IDamageable
    {
        void ApplyDamage(int amount);
        bool IsAlive { get; }
    }
}
