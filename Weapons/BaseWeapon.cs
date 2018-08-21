using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLua;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input; 


namespace SpaceGame.Weapons
{
    public class BaseWeapon
    {
        public string hands; 
        public int id;
        public float fireRate;

        public string name; 
        public int clip;
        public int maxClip;

        public int ammo;
        public int maxAmmo;
        public float force; 

        float eTime = 0;

        private MouseState msState;
        private MouseState oldmsState;
        private KeyboardState kbState;
        private KeyboardState oldKbState;
        public int textureId;
        private int bulletId;
        private float bulletDampening; 

        public Vector2 position;
        public Vector2 tip; 
        public float rotation; 
        public enum FireType
        {
            Single,
            Burst,
            Rapid
        }

        public FireType fireType; 
        public BaseWeapon(string name, int id)
        {
            Lua lua = new Lua();
            this.name = name; 
            lua.DoFile("Scripts/Weapons/WeaponDictionary.lua");
            maxAmmo = (int)(double)lua[name + ".maxAmmo"];
            force = (float)(double)lua[name + ".force"]; 
            hands = (string)lua[name + ".Hands"]; 
            ammo = maxAmmo; 
            maxClip = (int)(double)lua[name + ".clipSize"];
            clip = maxClip;
            fireRate = (int)(double)lua[name + ".speed"];
            textureId = (int)(double)lua[name + ".textureId"];
            bulletId = (int)(double)lua[name + ".bulletId"];
            bulletDampening = (float)(double)lua[name + ".dampening"]; 
            fireType = FireType.Single;
            this.id = id;
        }

        public virtual void Update(GameTime gameTime, Vector2 position, float rotation)
        {
            
            msState = Mouse.GetState();
            kbState = Keyboard.GetState();
            eTime += gameTime.ElapsedGameTime.Milliseconds;
            this.position = position;
            this.rotation = rotation; 

            if(kbState.IsKeyDown(Keys.R) && oldKbState.IsKeyUp(Keys.R))
            {
                Reload();
            }

            switch (fireType)
            {
                case FireType.Single:
                    {
                        if (msState.LeftButton == ButtonState.Pressed && oldmsState.LeftButton == ButtonState.Released)
                        {
                            if(eTime >= fireRate)
                            {
                                //Utilities.Ray ray = new Utilities.Ray();
                                Vector2 dir = new Vector2((float)Math.Cos(rotation) * 100, (float)Math.Sin(rotation) * 100);
                                Fire();
                                eTime = 0; 
                            }
                        }
                        break;
                    }
                case FireType.Burst:
                    {
                        if (msState.LeftButton == ButtonState.Pressed && oldmsState.LeftButton == ButtonState.Released)
                        {
                            for(int i  = 0; i < 3; i++)
                            {
                                if (eTime >= fireRate)
                                {
                                    Fire();
                                    eTime = 0;
                                }
                            }
                        }
                        break;
                    }
                case FireType.Rapid:
                    {
                        if (msState.LeftButton == ButtonState.Pressed)
                        {
                            if (eTime >= fireRate)
                            {
                                Fire(); 
                                eTime = 0;
                            }
                        }
                        break;
                    }
            }
            oldKbState = kbState; 
            oldmsState = msState; 
        }
               
        public void AddAmmo(int val)
        {
            ammo += val; 
            if(ammo > maxAmmo)
            {
                ammo = maxAmmo; 
            }
        }

        public void Reload()
        {
            Console.WriteLine("Reloading " + clip + " | " + ammo);
            for (int i = 0; i < maxClip; i++)
            {
                if (ammo >= 0)
                {
                    if (clip == maxClip)
                    {
                        return;
                    }
                    ammo--;
                    clip++;
                    
                }
            }
        }

        public virtual void FireWeapon(float rotation)
        {
            this.rotation = rotation; 
            if (eTime >= fireRate)
            {
                Fire();                
                eTime = 0;
            }
            

        }

        public virtual void Fire()
        {
            if (ammo > 0 || clip > 0) // fires only if theres ammo in the clip or pool
            {
                Game1.planet.bulletManager.AddBullet(position, rotation, force, bulletDampening, bulletId); // adds new bullet to game world
                clip--; // subtracts from clip 

                if(clip <= 0) // if clip reaches 0 the reload
                {
                    Reload();                    
                }

            }
            
        }
        
        
    }
}
