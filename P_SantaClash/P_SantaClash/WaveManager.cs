using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace P_SantaClash
{
    public class WaveManager
    {
        private readonly Random _random = new Random();
        private readonly List<Enemy> _enemies;
        private readonly Texture2D _enemyTexture;

        private float _spawnTimer;
        private float _spawnInterval = 1.1f;

        private int _wave = 1;

        public int Wave => _wave;

        public WaveManager(List<Enemy> enemies, Texture2D enemyTexture)
        {
            _enemies = enemies;
            _enemyTexture = enemyTexture;
        }

        public void Reset()
        {
            _enemies.Clear();
            _spawnTimer = 0;
            _spawnInterval = 1.1f;
            _wave = 1;
        }

        public void Update(GameTime gameTime, Rectangle arena)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _spawnTimer -= dt;

            // Exemple simple : toutes les X secondes on ajoute un ennemi. Le rythme augmente avec les vagues.
            if (_spawnTimer <= 0)
            {
                SpawnEnemy(arena);
                _spawnTimer = _spawnInterval;

                // Une vague monte quand il y a déjà beaucoup d'ennemis (vivants) à l'écran.
                int aliveCount = _enemies.Count(e => e.IsAlive);
                if (aliveCount >= 8 + _wave * 2)
                {
                    _wave++;
                    _spawnInterval = MathF.Max(0.45f, _spawnInterval - 0.08f);
                }
            }
        }

        private void SpawnEnemy(Rectangle arena)
        {
            // Spawn sur les bords
            int side = _random.Next(4);
            Vector2 pos = side switch
            {
                0 => new Vector2(arena.Left - 20, _random.Next(arena.Top, arena.Bottom)),   // gauche
                1 => new Vector2(arena.Right + 20, _random.Next(arena.Top, arena.Bottom)),  // droite
                2 => new Vector2(_random.Next(arena.Left, arena.Right), arena.Top - 20),    // haut
                _ => new Vector2(_random.Next(arena.Left, arena.Right), arena.Bottom + 20)  // bas
            };

            // 2 types d'ennemis minimum
            Enemy e = (_random.NextDouble() < 0.5)
                ? new SlowEnemy(pos, _enemyTexture)
                : new FastEnemy(pos, _enemyTexture);

            _enemies.Add(e);
        }
    }
}
