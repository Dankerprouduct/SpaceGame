using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace SpaceGame.Systems
{
    public class BulletManager
    {

        Weapons.Bullet[] bullets;
        public List<Texture2D> textures;
        public static List<Texture2D> gunTextures;
        public Vector2 position;                 
        public BulletManager(int pool)
        {
            bullets = new Weapons.Bullet[pool];
            for(int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = new Weapons.Bullet();
            }
        }

        public void LoadContent(ContentManager content)
        {
            textures = new List<Texture2D>()
            {
                content.Load<Texture2D>("Weapons/Ammo/BetterBullet"),
                content.Load<Texture2D>("Weapons/Ammo/Bullet"),
                content.Load<Texture2D>("Weapons/Ammo/ElectroBall")
            };

            gunTextures = new List<Texture2D>()
            {
                content.Load<Texture2D>("Weapons/Pistol"),
                content.Load<Texture2D>("Weapons/Pistol"),
                content.Load<Texture2D>("Weapons/Laser")              
        };
        }

        public void AddBullet(Vector2 position, float rotation, float force, float drag, int id)
        {
            
            for(int i = 0; i < bullets.Length; i++)
            {
                if (!bullets[i].Alive)
                {
                    bullets[i].MakeBullet(position, rotation, force, drag, id);
                    return;
                }
                
            }
        }

        public void Update()
        {
            for(int i = 0; i < bullets.Length; i++)
            {
                if (bullets[i].Alive)
                {
                    bullets[i].Update();
                }
            }
        }

        public bool CheckCollision(Rectangle rect)
        {
            for(int i = 0; i < bullets.Length; i++)
            {
                if (bullets[i].Alive)
                {
                    if (rect.Contains(bullets[i].point))
                    {
                        bullets[i].Alive = false; 
                        return true;
                    }
                    
                }
            }
            return false; 
        }

        public int GetActiveBullets()
        {
            int active = 0;
            for (int i = 0; i < bullets.Length; i++)
            {
                if (!bullets[i].Alive)
                {
                    active++;
                }
            }

            return active;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                if (bullets[i].Alive)
                {
                    spriteBatch.Draw(textures[bullets[i].id],
                        bullets[i].rect,
                        new Rectangle(0, 0, textures[bullets[i].id].Width, textures[bullets[i].id].Height),
                        Color.White * bullets[i].fade,
                        bullets[i].rotation,
                        new Vector2(
                            textures[bullets[i].id].Width / 2,
                            textures[bullets[i].id].Height / 2),
                        SpriteEffects.None,
                        0f);
                }
            }
        }
    }
}
