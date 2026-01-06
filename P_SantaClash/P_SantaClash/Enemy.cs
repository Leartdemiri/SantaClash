using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace P_SantaClash
{
    public abstract class Enemy : GameObject
    {
        protected readonly Texture2D Texture;
        protected readonly Random Random = new Random();

        public int ContactDamage { get; protected set; } = 5;

        protected float Speed = 80f;
        protected float MaxAngleOffset = MathF.PI / 12f;

        public Rectangle Hitbox =>
            new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

        protected Enemy(Vector2 position, Texture2D texture, float speed, float maxAngleOffset, int contactDamage)
            : base(position, Vector2.Zero)
        {
            Texture = texture;
            Speed = speed;
            MaxAngleOffset = maxAngleOffset;
            ContactDamage = contactDamage;
        }

        /// <summary>
        /// Déplacement pseudo-aléatoire vers Santa (zigzag/rotation) comme dans le PDF.
        /// </summary>
        public void Update(GameTime gameTime, Vector2 santaPosition)
        {
            if (!IsAlive) return;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 dir = santaPosition - Position;
            if (dir == Vector2.Zero) return;
            dir.Normalize();

            float angleOffset = (float)(Random.NextDouble() - 0.5) * 2f * MaxAngleOffset;
            float cos = MathF.Cos(angleOffset);
            float sin = MathF.Sin(angleOffset);

            Vector2 dirRotated = new Vector2(
                dir.X * cos - dir.Y * sin,
                dir.X * sin + dir.Y * cos
            );

            Position += dirRotated * Speed * dt;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsAlive) return;
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }

    public sealed class SlowEnemy : Enemy
    {
        public SlowEnemy(Vector2 position, Texture2D texture)
            : base(position, texture, speed: 60f, maxAngleOffset: MathF.PI / 16f, contactDamage: 8) { }
    }

    public sealed class FastEnemy : Enemy
    {
        public FastEnemy(Vector2 position, Texture2D texture)
            : base(position, texture, speed: 120f, maxAngleOffset: MathF.PI / 10f, contactDamage: 4) { }
    }
}
