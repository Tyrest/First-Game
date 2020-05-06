using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace First_Game
{
    class Asteroid
    {
        public static Texture2D texture;

        private Vector2 position;
        private float size;
        private float angle;
        private float speed;

        /**
         * Constructor for the Asteroid class
         * spawn should be from 0 to 50
         * angle offset should be from -0.5 to 0.5
         */
        public Asteroid(Vector2 playerPosition, float spawn, float angleOffset, float size, float speed, float windowWidth, float windowHeight)
        {
            if (spawn < 16)
            {
                position.Y = -size / 2;
                position.X = spawn / 16 * windowWidth;
            }
            else if (spawn >= 16 && spawn < 25)
            {
                position.X = windowWidth + size / 2;
                position.Y = (spawn - 16) / 9 * windowHeight;
            }
            else if (spawn >= 25 && spawn < 41)
            {
                position.Y = windowHeight + size / 2;
                position.X = (spawn - 25) / 16 * windowWidth;
            }
            else
            {
                position.X = -size / 2;
                position.Y = (spawn - 41) / 9 * windowHeight;
            }

            this.size = size;
            this.speed = speed;

            angle = (float)Math.Atan((position.Y - playerPosition.Y) / (playerPosition.X - position.X)) + angleOffset;
            if (position.X > windowWidth / 2)
            {
                angle += (float) Math.PI;
            }
        }

        public void Update(GameTime gameTime)
        {
            position.X += speed * (float)Math.Cos(angle) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.Y -= speed * (float)Math.Sin(angle) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public bool CheckCollision(Vector2 playerPosition)
        {
            if (Math.Pow(playerPosition.X - position.X, 2) + Math.Pow(position.Y - playerPosition.Y, 2) <= Math.Pow(size / 2, 2))
            {
                return true;
            }
            return false;
        }

        public bool CheckOut(float windowWidth, float windowHeight)
        {
            if (position.Y > windowHeight + size ||
                position.Y < -size ||
                position.X > windowWidth + size ||
                position.X < -size)
            {
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, new Vector2(size / 2, size / 2), Vector2.One * size / texture.Width, SpriteEffects.None, 0f);
            //spriteBatch.Draw(texture, position, null, Color.White, 0f, new Vector2(32, 32), Vector2.One, SpriteEffects.None, 0f);
        }
    }
}