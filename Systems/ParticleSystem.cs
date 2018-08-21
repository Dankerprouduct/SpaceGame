using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content; 
using NLua;

namespace SpaceGame.Systems
{
    public class ParticleSystem
    {
        Particle[] particles;
        public static List<Texture2D> textures = new List<Texture2D>();
        int maxPoolSize;

        public Lua particleEffects;
        

        public ParticleSystem(int poolSize)
        {
            particles = new Particle[poolSize];
            maxPoolSize = poolSize;
            for(int i = 0; i < particles.Length; i++)
            {
                particles[i] = new Particle();
            }
        }

        public void CallEffect(string name, Vector2 position)
        {
            particleEffects = new Lua();
            particleEffects.DoFile("Scripts/Particle/"+name+".lua");
            int intensity = (int)(double)particleEffects[name + ".intensity"];
            for (int i = 0; i < intensity; i++)
            {
                AddParticle(
                    position,
                    (float)(double)particleEffects[name + ".rotation"],
                    (float)(double)particleEffects[name + ".force"],
                    (float)(double)particleEffects[name + ".dampening"],
                    (int)(double)particleEffects[name + ".id"],
                    (float)(double)particleEffects[name + ".growth"]);
                particleEffects.DoFile("Scripts/Particle/" + name + ".lua");

            }
        }

        public void LoadContent(ContentManager content)
        {
            textures.Add(content.Load<Texture2D>("Particles/CryoParticle"));
            textures.Add(content.Load<Texture2D>("Particles/FireParticle"));
            textures.Add(content.Load<Texture2D>("Particles/Quills"));
            textures.Add(content.Load<Texture2D>("Particles/SmokeParticle"));
            textures.Add(content.Load<Texture2D>("Particles/SteamParticle"));
            textures.Add(content.Load<Texture2D>("Particles/WaterParticle"));
            textures.Add(content.Load<Texture2D>("Particles/BlueLaser"));

        }

        public void Update()
        {
            for(int i = 0; i < particles.Length; i++)
            {
                if (particles[i].alive)
                {
                    particles[i].Update();
                }
            }
        }
        
        public void AddParticle(Vector2 position, float rotation, float force, float dampening, int id, float growth)
        {
            
            for (int i = 0; i < particles.Length; i++)
            {
                if (!particles[i].alive)
                {
                    particles[i].MakeParticle(position, MathHelper.ToRadians(rotation) + MathHelper.ToRadians(Game1.random.Next(-2, 2)), force, dampening, id, growth);
                    return;
                }
            }
        }

        public int GetActiveParticles()
        {
            int active = 0;
            for (int i = 0; i < particles.Length; i++)
            {
                if (!particles[i].alive)
                {
                    active++;
                }
            }
            
            return maxPoolSize - active;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i].alive)
                {
                    
                    spriteBatch.Draw(textures[particles[i].textureID], 
                        new Rectangle(
                        (int)particles[i].lastPosition.X, 
                        (int)particles[i].lastPosition.Y,
                        (int)particles[i].width ,
                        (int)particles[i].height),
                        new Rectangle(0, 0, textures[particles[i].textureID].Width,
                        textures[particles[i].textureID].Height),
                        Color.White * particles[i].fade,
                        particles[i].rotation, 
                        new Vector2(textures[particles[i].textureID].Width / 2, 
                        textures[particles[i].textureID].Height / 2),
                        SpriteEffects.None, 0f);
                }
            }
        }


    }
}
