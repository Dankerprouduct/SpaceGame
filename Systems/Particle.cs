using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework; 

namespace SpaceGame.Systems
{
    public class Particle
    {

        public bool alive;
        public Vector2 position;
        public Vector2 lastPosition; 
        public Vector2 direction;
        public Vector2 velocity;
        public float dampening;
        public int textureID;
        public float rotation;
        public float fade;
        public float width;
        public float height;
        public float size; 
        public float growth;
        public Rectangle rect; 
        private Vector2 initVelocity;

        public Particle()
        {
            alive = false; 
        }

        public void MakeParticle(Vector2 position, float rotation, float force, float dampening, int textureid, float growth)
        {
            width = ParticleSystem.textures[textureid].Width;
            height = ParticleSystem.textures[textureid].Height;
            alive = true;
            this.growth = growth;
            this.position = position; 
            this.direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            this.velocity = Vector2.Zero; 
            this.velocity += direction * force;
            this.dampening = dampening;
            this.textureID = textureid;
            initVelocity = velocity; 
        }

        public void Update()
        {
            width *= growth;
            height *= growth; 
            fade = velocity.Length() / initVelocity.Length();
            velocity *= dampening; 
            position += velocity;
            lastPosition = Vector2.Lerp(lastPosition, position, .01f); 
            rotation = (float)Math.Atan2(direction.Y, direction.X);
            
            lastPosition = position; 
            if (velocity.Length() < 3)
            {
                alive = false; 
            }

        }

    }
}
