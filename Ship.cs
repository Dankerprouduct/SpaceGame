using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content; 

namespace SpaceGame
{
    public class Ship
    {
        private Vector2 position;
        private Vector2 velocity;
        private Vector2 direction;

        KeyboardState kbState;
        KeyboardState oldKbState;
        float rotation;
        Texture2D texture; 
        
        public Ship(Vector2 position)
        {
            this.position = position; 
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Ships/Ship2");
        }

        public void Update(GameTime gametime)
        {
            kbState = Keyboard.GetState();

            PlayerInput();

            oldKbState = kbState;
        }

        public Vector2 GetCenter()
        {
            return new Vector2(position.X + (texture.Width / 2), position.Y + (texture.Height / 2)); 
        }

        void PlayerInput()
        {
            // forward
            velocity *= .95f;
            position += velocity;
            float speed = 10; 
            if (kbState.IsKeyDown(Keys.W))
            {
                AddForce(rotation, speed); 
            }
            // right
            if (kbState.IsKeyDown(Keys.D))
            {
                rotation += .07f;
                AddForce(rotation, speed / 5);
            }
            // left
            if (kbState.IsKeyDown(Keys.A))
            {
                rotation -= .07f;
                AddForce(rotation, speed / 5);
            }

        }

        void AddForce(float rot, float force)
        {
            direction= new Vector2((float)Math.Cos(rot), (float)Math.Sin(rot));
            velocity += direction * force;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width / 1) , (int)(texture.Height / 1)), null, Color.White,
                rotation, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0f);

        }
    }
}
