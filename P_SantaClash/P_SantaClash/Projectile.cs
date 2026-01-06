using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace P_SantaClash
{
    public class Projectile : GameObject
    {
        private readonly Texture2D _texture;
        public int OwnerPlayerId { get; }

        public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);

        public Projectile(Vector2 position, Vector2 velocity, Texture2D texture, int ownerPlayerId) : base(position, velocity)
        {
            _texture = texture;
            OwnerPlayerId = ownerPlayerId;
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsAlive) return;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity * dt;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsAlive) return;
            spriteBatch.Draw(_texture, Position, Color.White);
        }
    }
}
