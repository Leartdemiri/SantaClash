using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace P_SantaClash
{
    /// <summary>
    /// Base POO imposée dans le sujet : position 2D, vitesse, IsAlive, Update(), Draw()
    /// </summary>
    public abstract class GameObject
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public bool IsAlive { get; set; } = true;

        protected GameObject(Vector2 position, Vector2 velocity)
        {
            Position = position;
            Velocity = velocity;
        }

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
