namespace P_SantaClash.Core
{
    public class PlayerStats
    {
        public int PlayerId { get; }
        public int ShotsFired { get; private set; }
        public int ShotsHit { get; private set; }
        public int Score { get; private set; }

        public PlayerStats(int playerId)
        {
            PlayerId = playerId;
        }

        public void RegisterShot() => ShotsFired++;
        public void RegisterHit()
        {
            ShotsHit++;
            Score++;
        }

        public double Accuracy()
        {
            if (ShotsFired == 0) return 0.0;
            return (double)ShotsHit / ShotsFired;
        }
    }
}
