using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace P_SantaClash
{
    public class Player : GameObject
    {
        private const float MoveSpeed = 200f;
        private const float ShootCooldown = 0.25f;

        private readonly Texture2D _texture;
        private readonly int _playerId;

        private float _shootTimer;

        public int PlayerId => _playerId;

        public int Score { get; private set; }
        public int ShotsFired { get; private set; }
        public int ShotsHit { get; private set; }

        public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);

        public Player(int playerId, Vector2 position, Texture2D texture) : base(position, Vector2.Zero)
        {
            _playerId = playerId;
            _texture = texture;
        }

        public float Accuracy => ShotsFired == 0 ? 0f : (float)ShotsHit / ShotsFired;

        public void AddKill()
        {
            Score++;
            ShotsHit++;
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_shootTimer > 0) _shootTimer -= dt;

            Vector2 move = Vector2.Zero;

            if (_playerId == 1)
            {
                GamePadState gp = GamePad.GetState(PlayerIndex.One);

                // Use D-pad for movement
                if (gp.DPad.Up == ButtonState.Pressed) move.Y -= 1;
                if (gp.DPad.Down == ButtonState.Pressed) move.Y += 1;
                if (gp.DPad.Left == ButtonState.Pressed) move.X -= 1;
                if (gp.DPad.Right == ButtonState.Pressed) move.X += 1;
            }
            else
            {
                KeyboardState k = Keyboard.GetState();
                if (k.IsKeyDown(Keys.W)) move.Y -= 1;
                if (k.IsKeyDown(Keys.S)) move.Y += 1;
                if (k.IsKeyDown(Keys.A)) move.X -= 1;
                if (k.IsKeyDown(Keys.D)) move.X += 1;
            }

            if (move != Vector2.Zero)
            {
                move.Normalize();
                Position += move * MoveSpeed * dt;
            }
        }

        public bool WantsToShoot()
        {
            if (_shootTimer > 0) return false;

            if (_playerId == 1)
            {
                GamePadState gp = GamePad.GetState(PlayerIndex.One);
                return gp.IsButtonDown(Buttons.A); 
            }
            else
            {
                KeyboardState k = Keyboard.GetState();
                return k.IsKeyDown(Keys.Space);
            }
        }

        public void MarkShotFired()
        {
            ShotsFired++;
            _shootTimer = ShootCooldown;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);
        }
    }
}
