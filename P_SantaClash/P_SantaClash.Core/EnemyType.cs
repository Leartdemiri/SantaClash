namespace P_SantaClash.Core
{
    public enum EnemyType
    {
        Slow,
        Fast
    }

    public readonly record struct EnemySpawnInfo(EnemyType Type);
}
