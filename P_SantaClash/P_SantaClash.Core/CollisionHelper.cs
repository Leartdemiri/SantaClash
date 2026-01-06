namespace P_SantaClash.Core
{
    public static class CollisionHelper
    {
        public static bool Intersects(CoreRect a, CoreRect b)
        {
            // AABB
            return a.Left < b.Right &&
                   a.Right > b.Left &&
                   a.Top < b.Bottom &&
                   a.Bottom > b.Top;
        }
    }
}
