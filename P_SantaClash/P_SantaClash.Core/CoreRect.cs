namespace P_SantaClash.Core
{
    /// <summary>
    /// Rectangle simple pour la logique métier (sans dépendre de MonoGame).
    /// </summary>
    public readonly struct CoreRect
    {
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }

        public int Left => X;
        public int Right => X + Width;
        public int Top => Y;
        public int Bottom => Y + Height;

        public CoreRect(int x, int y, int width, int height)
        {
            X = x; Y = y; Width = width; Height = height;
        }
    }
}
