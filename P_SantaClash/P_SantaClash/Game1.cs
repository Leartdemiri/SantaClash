using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace P_SantaClash
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Parallaxe (2 couches)
        private Texture2D _background1;
        private Texture2D _background2;
        private float _bg1Offset;
        private float _bg2Offset;

        // Texture "pixel" (1x1)
        private Texture2D _pixel;
        private Texture2D santaTexture;
        private Texture2D _projectileTexture;

        // Entités
        private Santa _santa;
        private Player _p1;
        private Player _p2;

        private readonly List<Enemy> _enemies = new();
        private readonly List<Projectile> _projectiles = new();

        private WaveManager _waveManager;
        private readonly GameStateManager _state = new();

        // Arena de jeu
        private Rectangle _arena;

        // Fin de partie / stats
        private string _gameOverText = "";

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            _arena = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height); // zone de jeu
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Fond écran
            _background1 = Content.Load<Texture2D>("wallpaper");
            _background2 = _background1;

            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            // Santa au centre
            santaTexture = CreateSolidTexture(22, 22, Color.White);


            Vector2 santaPos = new Vector2(_arena.Width / 2f - santaTexture.Width / 2f, _arena.Height / 2f - santaTexture.Height / 2f);
            _santa = new Santa(santaPos, 100, santaTexture, targetPosition: new Vector2(_arena.Width / 2f, _arena.Height / 2f));

            // Joueurs (rectangles colorés via pixel "étiré")
            // On crée des textures dédiées pour avoir des tailles visibles.
            Texture2D p1Tex = CreateSolidTexture(22, 22, Color.Red);
            Texture2D p2Tex = CreateSolidTexture(22, 22, Color.Blue);
            Texture2D enemyTex = CreateSolidTexture(18, 18, Color.Green);
            Texture2D projTex = CreateSolidTexture(8, 8, Color.Yellow);

            _p1 = new Player(1, new Vector2(_arena.Width * 0.25f, _arena.Height * 0.70f), p1Tex);
            _p2 = new Player(2, new Vector2(_arena.Width * 0.75f, _arena.Height * 0.70f), p2Tex);

            _waveManager = new WaveManager(_enemies, enemyTex);

            // On stocke la texture projectile via un champ "hack" simple :
            _projectileTexture = projTex;

            UpdateWindowTitle();
        }

        private Texture2D CreateSolidTexture(int w, int h, Color color)
        {
            Texture2D t = new Texture2D(GraphicsDevice, w, h);
            Color[] data = Enumerable.Repeat(color, w * h).ToArray();
            t.SetData(data);
            return t;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (_state.State)
            {
                case GameState.Menu:
                    UpdateMenu();
                    break;

                case GameState.Playing:
                    UpdatePlaying(gameTime);
                    break;

                case GameState.GameOver:
                    UpdateGameOver();
                    break;
            }

            base.Update(gameTime);
        }

        private void UpdateMenu()
        {
            var k = Keyboard.GetState();
            var gp = GamePad.GetState(PlayerIndex.One);

            Window.Title = "Santa Clashs MENU (Enter / Start pour jouer)";
            if (k.IsKeyDown(Keys.Enter) || gp.Buttons.Start == ButtonState.Pressed)
            {
                StartNewGame();
            }
        }

        private void StartNewGame()
        {
            _state.StartGame();
            _enemies.Clear();
            _projectiles.Clear();
            _waveManager.Reset();

            // Reset Santa / joueurs
            _santa.ApplyDamage(-999999); // noop visuel, on ne veut pas d'overcomplication ici
            // on recrée proprement
            Vector2 center = new Vector2(_arena.Width / 2f, _arena.Height / 2f);
            _santa = new Santa(new Vector2(center.X - 16, center.Y - 16), 100, santaTexture, center);

            _p1 = new Player(1, new Vector2(_arena.Width * 0.25f, _arena.Height * 0.70f), CreateSolidTexture(22, 22, Color.Red));
            _p2 = new Player(2, new Vector2(_arena.Width * 0.75f, _arena.Height * 0.70f), CreateSolidTexture(22, 22, Color.Blue));

            _gameOverText = "";
            UpdateWindowTitle();
        }

        private void UpdatePlaying(GameTime gameTime)
        {
            // Parallaxe simple (défilement horizontal)
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _bg1Offset = (_bg1Offset + 20f * dt) % _arena.Width;
            _bg2Offset = (_bg2Offset + 40f * dt) % _arena.Width;

            _p1.Update(gameTime);
            _p2.Update(gameTime);
            _santa.Update(gameTime);

            ClampToArena(_p1);
            ClampToArena(_p2);
            ClampToArena(_santa);

            _waveManager.Update(gameTime, _arena);

            foreach (Enemy e in _enemies.Where(x => x.IsAlive))
                e.Update(gameTime, _santa.Position);

            foreach (Projectile p in _projectiles.Where(x => x.IsAlive))
                p.Update(gameTime);

            // Tir
            HandleShooting(_p1);
            HandleShooting(_p2);

            // ennemis dangereux (proches de Santa)
            List<Enemy> dangerousEnemies = _enemies
                .Where(e => e.IsAlive)
                .Where(e => Vector2.Distance(e.Position, _santa.Position) < 80f)
                .ToList();

            // Contact ennemis -> Santa
            foreach (Enemy e in dangerousEnemies)
            {
                if (e.Hitbox.Intersects(_santa.Hitbox))
                {
                    _santa.ApplyDamage(e.ContactDamage);
                    e.IsAlive = false; // "s'écrase" sur Santa
                }
            }

            // couples projectile/enemy en collision -> met en liste les projectiles et les enemies qui se touchent
            var hitPairs =
              (from proj in _projectiles
               where proj.IsAlive
               from enemy in _enemies
               where enemy.IsAlive && proj.Hitbox.Intersects(enemy.Hitbox)
               select (proj, enemy))
              .ToList();

            // tuer les deux instances
            foreach (var (proj, enemy) in hitPairs)
            {
                proj.IsAlive = false;
                enemy.IsAlive = false;

                if (proj.OwnerPlayerId == 1) _p1.AddKill();
                else _p2.AddKill();
            }

            // Nettoyage (hors écran ou morts)
            foreach (Projectile p in _projectiles.Where(p => p.IsAlive))
            {
                if (!_arena.Contains(p.Hitbox))
                    p.IsAlive = false;
            }

            _projectiles.RemoveAll(p => !p.IsAlive);
            _enemies.RemoveAll(e => !e.IsAlive && Vector2.Distance(e.Position, _santa.Position) > 500f);

            UpdateWindowTitle();

            if (!_santa.IsAlive)
            {
                BuildGameOverStats();
                _state.GameOver();
            }
        }

        private void HandleShooting(Player player)
        {
            if (!player.WantsToShoot()) return;

            player.MarkShotFired();

            // Direction de tir simple : vers le centre (Santa) mais inversée = on tire vers les ennemis autour
            Vector2 dir = player.Velocity;
            if (dir == Vector2.Zero) dir = new Vector2(0, -1);
            else dir.Normalize();

            Vector2 velocity = dir * 420f;
            Vector2 spawn = player.Position + new Vector2(10, 10);

            _projectiles.Add(new Projectile(spawn, velocity, _projectileTexture, player.PlayerId));
        }

        private void ClampToArena(GameObject obj)
        {
            // clamp position dans la fenêtre
            float x = MathHelper.Clamp(obj.Position.X, 0, _arena.Width - 1);
            float y = MathHelper.Clamp(obj.Position.Y, 0, _arena.Height - 1);
            obj.Position = new Vector2(x, y);
        }

        private void UpdateGameOver()
        {
            KeyboardState k = Keyboard.GetState();
            GamePadState gp = GamePad.GetState(PlayerIndex.One);

            Window.Title = $"Santa Clash GAME OVER | {_gameOverText} (R pour rejouer / Esc pour quitter)";
            if (k.IsKeyDown(Keys.R) || gp.Buttons.Start == ButtonState.Pressed)
                _state.GoToMenu();
        }

        private void BuildGameOverStats()
        {
            // classement final
            var ranking = new[]
                {
                    new { Player = _p1, Name = "Joueur 1 (Manette)" },
                    new { Player = _p2, Name = "Joueur 2 (Clavier)" },
                }
                .OrderByDescending(x => x.Player.Score)
                .ThenByDescending(x => x.Player.Accuracy)
                .ToList();

            var winner = ranking.First();

            _gameOverText =
                $"Gagnant: {winner.Name} | Scores: P1={_p1.Score} P2={_p2.Score} | " +
                $"Precision: P1={_p1.Accuracy:P0} P2={_p2.Accuracy:P0}";
        }

        private void UpdateWindowTitle()
        {
            if (_state.State != GameState.Playing) return;
            Window.Title = $"Santa Clash | Vie Santa: {_santa.CurrentHealth}/{_santa.MaxHealth} | " +
                           $"P1 Score: {_p1.Score} (Acc {_p1.Accuracy:P0}) | " +
                           $"P2 Score: {_p2.Score} (Acc {_p2.Accuracy:P0}) | Vague {_waveManager.Wave}";
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            DrawParallaxBackground();

            // Santa + joueurs + ennemis + projectiles
            _santa.Draw(_spriteBatch);
            _p1.Draw(_spriteBatch);
            _p2.Draw(_spriteBatch);

            foreach (Enemy e in _enemies.Where(x => x.IsAlive))
                e.Draw(_spriteBatch);

            foreach (Projectile p in _projectiles.Where(x => x.IsAlive))
                p.Draw(_spriteBatch);

            DrawHudBars();



            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawParallaxBackground()
        {
            // couche 1
            DrawTiled(_background1, _bg1Offset, Color.White * 0.85f);
            // couche 2 (plus rapide)
            DrawTiled(_background2, _bg2Offset, Color.White * 0.65f);
        }

        private void DrawTiled(Texture2D tex, float offset, Color color)
        {
            int w = _arena.Width;
            int h = _arena.Height;

            // on dessine 2 fois pour boucler
            Rectangle r1 = new Rectangle((int)-offset, 0, w, h);
            Rectangle r2 = new Rectangle((int)(w - offset), 0, w, h);

            _spriteBatch.Draw(tex, r1, color);
            _spriteBatch.Draw(tex, r2, color);
        }

        private void DrawHudBars()
        {
            // HUD sans texte : barres (vie Santa + 2 barres score)
            int pad = 10;
            int barW = 220;
            int barH = 14;

            // Vie Santa
            float healthRatio = _santa.MaxHealth == 0 ? 0 : (float)_santa.CurrentHealth / _santa.MaxHealth;
            DrawBar(new Rectangle(pad, pad, barW, barH), healthRatio, Color.DarkRed, Color.Red);

            // Scores (échelle arbitraire)
            float p1Ratio = MathHelper.Clamp(_p1.Score / 30f, 0, 1);
            float p2Ratio = MathHelper.Clamp(_p2.Score / 30f, 0, 1);

            DrawBar(new Rectangle(pad, pad + 22, barW, barH), p1Ratio, Color.DarkGray, Color.Red);
            DrawBar(new Rectangle(pad, pad + 44, barW, barH), p2Ratio, Color.DarkGray, Color.Blue);
        }

        private void DrawBar(Rectangle area, float ratio, Color back, Color fill)
        {
            _spriteBatch.Draw(_pixel, area, back);
            Rectangle filled = new Rectangle(area.X, area.Y, (int)(area.Width * ratio), area.Height);
            _spriteBatch.Draw(_pixel, filled, fill);
        }
    }
}
