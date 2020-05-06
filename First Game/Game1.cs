using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;

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
        const int numAsteroids = 8;

        Vector2 tyrestyPosition;
        Vector2 tyrestyVelocity;
        float tyrestyAccel;

        Asteroid [] asteroids = new Asteroid [numAsteroids]; // Hopefully enough space to hold everything

        Random rand = new Random();
        
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

            // ast1 = new Asteroid(tyrestyPosition, rand.Next(0, 50), rand.Next(-4, 4) / 8, rand.Next(64, 128), rand.Next(64, 128), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

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
            Asteroid.texture = Content.Load<Texture2D>("Asteroid");

            for (int i = 0; i < numAsteroids; i++)
            {
                float sizespeed = rand.Next(64);
                asteroids[i] = new Asteroid(tyrestyPosition, rand.Next(51), (rand.Next(16) - 8) / 16.0f, 32 + sizespeed, 192 - sizespeed, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            }
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

            for (int i = 0; i < numAsteroids; i++)
            {
                asteroids[i].Update(gameTime);
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

            for (int i = 0; i < numAsteroids; i++)
            {
                if (asteroids[i].CheckOut(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight))
                {
                    float sizespeed = rand.Next(64);
                    asteroids[i] = new Asteroid(tyrestyPosition, rand.Next(51), (rand.Next(16) - 8) / 16.0f, 32 + sizespeed, 192 - sizespeed, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                }

                if (asteroids[i].CheckCollision(tyrestyPosition))
                {
                    Exit();
                }

                asteroids[i].Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
