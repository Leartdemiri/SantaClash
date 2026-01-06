using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace P_SantaClash
{
    public class Santa : GameObject, IDamageable
    {
        public int MaxHealth { get; }
        public int CurrentHealth { get; private set; }

        private readonly Texture2D _texture;
        private readonly Vector2 _targetPosition;
        private readonly Random _random = new Random();

        private float _attractionStrength = 50f;
        private float _noiseStrength = 20f;
        private float _damping = 0.6f;

        public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);

        public Santa(Vector2 position, int maxHealth, Texture2D texture, Vector2? targetPosition = null) : base(position, Vector2.Zero)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            _texture = texture;
            _targetPosition = targetPosition ?? position; // centre de la map
        }
        

        // Gestions des dégats
        public void ApplyDamage(int amount) => TakeDamage(amount);
        public void TakeDamage(int dmg)
        {
            if (!IsAlive) return;

            CurrentHealth -= dmg;
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                IsAlive = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsAlive) return;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds; 

            // Mouvement chaotique

            // bougers vers cible
            Vector2 toTarget = _targetPosition - Position;
            if (toTarget != Vector2.Zero)
                toTarget.Normalize();

            // bruit aléatoire
            float noiseX = (float)(_random.NextDouble() - 0.5f);
            float noiseY = (float)(_random.NextDouble() - 0.5f);
            Vector2 noise = new Vector2(noiseX, noiseY);
            if (noise != Vector2.Zero)
                noise.Normalize();

            // vitesse et accelerations
            Vector2 acceleration = toTarget * _attractionStrength + noise * _noiseStrength;

            // lent? plus vite.
            if (Velocity.Length() < 15f)
                Velocity += noise * 30f;

            Velocity *= _damping; // amortir
            Position += Velocity * dt; 
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsAlive) return;
            spriteBatch.Draw(_texture, Position, Color.White);
        }
    }
}
