using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace P_SantaClash
{
    public class Santa
    {
        public Vector2 Position { get; set; }
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public Texture2D texture;

        // => so it uses actual class stuff and (int) to convert from float to int 
        public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height); // position of the santa to the size of the texture of santa


        public Santa(Vector2 position, int maxHealth, Texture2D texture)
        {
            Position = position;
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            this.texture = texture;
        }

        public void TakeDamage(int dmg){

            if(this.CurrentHealth <= 0){
                this.CurrentHealth = 0; // so that it doesnt show bugged
                return;
            }else{
                this.CurrentHealth -= dmg;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(this.CurrentHealth <= 0){
                return; // so that it doesnt draw if dead
            }

            spriteBatch.Draw(texture, Position, Color.White);
        }
    }
}
