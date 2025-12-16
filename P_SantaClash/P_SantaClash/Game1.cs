using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace P_SantaClash
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D background;
        Texture2D santaTexture;
        Santa santa;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("wallpaper");
            santaTexture = Content.Load<Texture2D>("black_santa");
            Vector2 santaPosition = new Vector2(GraphicsDevice.Viewport.Width / 2 - santaTexture.Width/2, GraphicsDevice.Viewport.Height / 2 - santaTexture.Height/2);
            santa = new Santa(santaPosition, 100, santaTexture);


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            // basically, rectangle that starts from the corner that ends to the other corner which is the size of the viewport 
            Rectangle wallpaperSize = new Rectangle(0,0,GraphicsDevice.Viewport.Width,GraphicsDevice.Viewport.Height);




            _spriteBatch.Begin();
            _spriteBatch.Draw(background, wallpaperSize, Color.White);
            santa.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
