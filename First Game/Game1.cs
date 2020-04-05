using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace First_Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D tyrestyTexture;

        const float tyrestyTopSpeed = 420f;
        const float tyrestyFriction = 0.85f;

        Vector2 tyrestyPosition;
        Vector2 tyrestyVelocity;
        float tyrestyAccel;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            tyrestyPosition = new Vector2(graphics.PreferredBackBufferWidth / 2,
                                          graphics.PreferredBackBufferHeight / 2);
            tyrestyVelocity = Vector2.Zero;
            tyrestyAccel = 70f;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            tyrestyTexture = Content.Load<Texture2D>("Tyresty");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();

            // Movement with arrow keys (speed * elapsedGameTime)
            if (kstate.IsKeyDown(Keys.Up))
                tyrestyVelocity.Y -= tyrestyAccel * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(Keys.Down))
                tyrestyVelocity.Y += tyrestyAccel * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(Keys.Left))
                tyrestyVelocity.X -= tyrestyAccel * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(Keys.Right))
                tyrestyVelocity.X += tyrestyAccel * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Capping the speed
            if (tyrestyVelocity.Y > tyrestyTopSpeed)
                tyrestyVelocity.Y = tyrestyTopSpeed;
            else if (tyrestyVelocity.Y < -tyrestyTopSpeed)
                tyrestyVelocity.Y = -tyrestyTopSpeed;

            if (tyrestyVelocity.X > tyrestyTopSpeed)
                tyrestyVelocity.X = tyrestyTopSpeed;
            else if (tyrestyVelocity.X < -tyrestyTopSpeed)
                tyrestyVelocity.X = -tyrestyTopSpeed;

            tyrestyPosition += tyrestyVelocity;

            tyrestyVelocity *= tyrestyFriction;

            // Constrict movement to only inside the window
            if (tyrestyPosition.Y > graphics.PreferredBackBufferHeight - tyrestyTexture.Height / 2)
            {
                tyrestyPosition.Y = graphics.PreferredBackBufferHeight - tyrestyTexture.Height / 2;
                tyrestyVelocity.Y = 0;
            }
            else if (tyrestyPosition.Y <= tyrestyTexture.Height / 2)
            {
                tyrestyPosition.Y = tyrestyTexture.Height / 2;
                tyrestyVelocity.Y = 0;
            }

            if (tyrestyPosition.X > graphics.PreferredBackBufferWidth - tyrestyTexture.Width / 2)
            {
                tyrestyPosition.X = graphics.PreferredBackBufferWidth - tyrestyTexture.Width / 2;
                tyrestyVelocity.X = 0;
            }
            else if (tyrestyPosition.X <= tyrestyTexture.Width / 2)
            {
                tyrestyPosition.X = tyrestyTexture.Width / 2;
                tyrestyVelocity.X = 0;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(tyrestyTexture, tyrestyPosition, null, Color.White, 0f, new Vector2(tyrestyTexture.Width / 2, tyrestyTexture.Height / 2), Vector2.One, SpriteEffects.None, 0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
