using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SpaceGame.Weapons
{
    public class Bullet
    {
        public Vector2 position;
        Vector2 velocity;
        Vector2 direction;
        float drag;
        public Rectangle rect; 
        public float rotation; 
        public Point point;
        public int id;
        public bool Alive;
        public float fade;
        Vector2 initVelocity; 
        public Bullet()
        {
            Alive = false;
        }
        public void MakeBullet(Vector2 position, float rotation, float force, float drag, int id)
        {
            this.id = id;
            
            point = new Point((int)position.X, (int)position.Y);
            Alive = true; 
            this.position = position;
            this.drag = drag;
            this.rotation = rotation; 
            AddForce(rotation, force);

        }
        
        private void AddForce(float rotation, float force)
        {
            direction = new Vector2((float)Math.Cos(rotation),
                (float)Math.Sin(rotation));
            direction.Normalize();
            velocity = Vector2.Zero; 
            velocity += direction * force;
            initVelocity = velocity; 
        }

        public void Update()
        {
            fade = velocity.Length() / initVelocity.Length();
            rect = new Rectangle(point.X, point.Y,
                Game1.planet.bulletManager.textures[id].Width,
                Game1.planet.bulletManager.textures[id].Height);
            velocity *= drag;
            position += velocity;
            point = new Point((int)position.X, (int)position.Y); 
            
            if(velocity.Length() <= 3)
            {
                Alive = false;
            }
        }
    }
}
