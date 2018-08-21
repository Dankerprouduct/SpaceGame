using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using NLua;

namespace SpaceGame.Entity
{
    public class Player : BaseGameEntity
    {
        
        KeyboardState keyboardState;
        KeyboardState oldKbState;
        public MouseState mouseState;
        public MouseState oldMState;
        Texture2D texture;
        public Texture2D[] bodyTextures; 
        Lua lua;

        Systems.ParticleSystem playerParticleSystem;
        
        public Player(int id, Vector2 pos): base(pos)
        {
            maxVelocity = 5; 
            health = 100; 
            inventory = new Item.Inventory(5, 15);
            position = pos;
            velocity = Vector2.Zero;
            speed = 1;
            

        }

        #region Content Loading
        public void LoadContent(ContentManager content)
        {
            name = "Danker";
            texture = content.Load<Texture2D>("Player/TempSprite");
            bodyTextures = new Texture2D[3]; 
            bodyTextures[0] = content.Load<Texture2D>("Player/Head");
            bodyTextures[1] = content.Load<Texture2D>("Player/RightHand");
            bodyTextures[2] = content.Load<Texture2D>("Player/LeftHand");
            playerParticleSystem = new Systems.ParticleSystem(1000);
            playerParticleSystem.LoadContent(content);
            rightHand = new BodyPart()
            {
                Tag = "RightHand"
            };
            leftHand = new BodyPart()
            {
                Tag = "LeftHand"
            };

            lua = new Lua();
            CompileLua();
           // weapon = new Weapons.Pistol("Pistol", 0); 
        }

        public override void CompileLua()
        {
            
            lua.RegisterFunction("AddForce", this, GetType().GetMethod("AddForce"));
            lua.RegisterFunction("SetPosition", this, GetType().GetMethod("SetEntityPosition"));
            lua.RegisterFunction("GetPosition", this, GetType().GetMethod("GetEntityPosition"));
            lua.RegisterFunction("GetCenter", this, GetType().GetMethod("GetCenter"));
            lua.RegisterFunction("GetMouseDirection", this, GetType().GetMethod("GetMouseDirection"));
            lua.RegisterFunction("Physics", this, GetType().GetMethod("Physics"));
            lua.RegisterFunction("GetForce", this, GetType().GetMethod("GetForce"));

            lua.DoFile("Scripts/Player/Player.lua");

            lua.DoFile("Scripts/Animation/HandPositions.lua");

        }
        #endregion


        public override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            Movement();
            playerParticleSystem.Update();
            if (weapon1 != null)
            {
                weapon1.Update(gameTime, rightHand.position, rotation - MathHelper.ToRadians(90));
                
            }
            HandUpdate();

            if (Game1.planet.bulletManager.CheckCollision(rect))
            {
                TakeDamage(10);
            }

            oldMState = mouseState; 
            oldKbState = keyboardState; 
            
        }
                
        #region Input
        private void Movement()
        {
            velocity *= .85f;
            oldPosition = position;
            rotation = GetMouseDirection() + MathHelper.ToRadians(90);
            position.X += (int)velocity.X;
            if (Physics)
            {
                position.X = oldPosition.X;
                velocity.X = -velocity.X * .5f; 
            }

            position.Y += (int)velocity.Y;
            if (Physics)
            {
                position.Y = oldPosition.Y;
                velocity.Y = -velocity.Y * .5f;
            }



            if (keyboardState.IsKeyDown(Keys.W)) // move north
            {
                AddForce(MathHelper.ToRadians(270), speed);
                
            }
            else if(keyboardState.IsKeyDown(Keys.S)) // move south
            {
                AddForce(MathHelper.ToRadians(90), speed);
               
            }
            
            if (keyboardState.IsKeyDown(Keys.A)) // move west
            {
                AddForce(MathHelper.ToRadians(180), speed);
                
            }
            else if (keyboardState.IsKeyDown(Keys.D)) // move east
            {
                AddForce(MathHelper.ToRadians(0), speed);
               
            }

        }

        int animationIndex = 0;
        string currentAnimation = "Idle";
        private void HandUpdate()
        {
            
            if (keyboardState.IsKeyDown(Keys.D3) && oldKbState.IsKeyUp(Keys.D3))
            {
                lua.DoFile("Scripts/Animation/HandPositions.lua");
                animationIndex++;
                if(animationIndex > lua.GetTable("AnimationName").Values.Count)
                {
                    animationIndex = 1; 
                    
                }
                currentAnimation = (string)lua.GetTable("AnimationName")[animationIndex];
            }
            if (weapon1 != null)
            {
                currentAnimation = weapon1.hands;
            }
            else
            {
                currentAnimation = "Idle";
            }

            rightHand.position = Vec2ToEntitySpace(rightHand.position = new Vector2(
                (int)(double)lua[currentAnimation + "." + rightHand.Tag + ".X"],
                (int)(double)lua[currentAnimation + "." + rightHand.Tag + ".Y"]));            
            rightHand.Update();
            rightHand.center = new Vector2(rightHand.newPosition.X + (bodyTextures[1].Width / 2),
                rightHand.newPosition.Y + (bodyTextures[1].Height / 2));


            leftHand.position = Vec2ToEntitySpace(leftHand.position = new Vector2(
                (int)(double)lua[currentAnimation + "." + leftHand.Tag + ".X"] + bodyTextures[1].Width / 2,
                (int)(double)lua[currentAnimation + "." + leftHand.Tag + ".Y"] + bodyTextures[2].Height / 2));
            leftHand.center = new Vector2(leftHand.newPosition.X + (bodyTextures[2].Width / 2),
                leftHand.newPosition.Y + (bodyTextures[2].Height / 2));
            leftHand.Update();

            if (weapon1 != null)
            {
                weapon1.position = Vec2ToEntitySpace(
                    new Vector2(0, 0),
                    rightHand.center, rotation);
            }

        }
        

        public float GetMouseDirection()
        {
            Vector2 dir = Game1.cameraMousePosition - GetCenter();
            dir.Normalize();
            return (float)Math.Atan2((double)dir.Y, (double)dir.X);
        }


        public new bool Physics
        {
            get
            {
                rect = new Rectangle((int)position.X, (int)position.Y,
                    bodyTextures[0].Width, bodyTextures[0].Height);

                if (Game1.planet.CheckCollision(rect))
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region Drawing
        public void DrawPlayerGUI()
        {
            Rectangle healthRect = new Rectangle(GUIManager.screenBounds.X + 10, GUIManager.screenBounds.Y + 10, (int)(health * 2.5f), 25);
            GUIManager.DrawBox(healthRect, GUIManager.themes["RetroSpace1"].color1);

            // ammo 
            if (weapon1 != null)
            {
                for (int i = 0; i < weapon1.clip; i++)
                {
                    Rectangle gunRect = new Rectangle(healthRect.X + healthRect.X * (i) + (i * 3), healthRect.Y + healthRect.Height + 5, 10, 20);
                    GUIManager.DrawBox(gunRect, GUIManager.themes["RetroSpace1"].color3);
                }
            }


            /*
            for(int i = 0; i < 6; i++)
            {
                Rectangle HotbarRect = new Rectangle(
                    ((GUIManager.screenBounds.Width / 2) + i * 64) - ((6 * 64) / 2) + (i + 2),
                    GUIManager.screenBounds.Height - 100,
                    64, 64);
                GUIManager.DrawBox(HotbarRect, GUIManager.themes["RetroSpace2"].color1);
            }
            */
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bodyTextures[0], new Rectangle((int)GetCenter().X,(int)GetCenter().Y, bodyTextures[0].Width, bodyTextures[0].Height),
                new Rectangle(0,0, bodyTextures[0].Width, bodyTextures[0].Height),
                Color.White, rotation,
                new Vector2(bodyTextures[0].Width / 2, bodyTextures[0].Height / 2),
                SpriteEffects.None, 0f);

            spriteBatch.Draw(bodyTextures[1], rightHand.newPosition, Color.White);



            spriteBatch.Draw(bodyTextures[2], leftHand.newPosition, Color.White);


            if (weapon1 != null)
            {
                spriteBatch.Draw(Systems.BulletManager.gunTextures[weapon1.textureId],
                    new Rectangle(
                    (int)weapon1.position.X,
                    (int)weapon1.position.Y,
                    Systems.BulletManager.gunTextures[weapon1.textureId].Width,
                    Systems.BulletManager.gunTextures[weapon1.textureId].Height),
                    new Rectangle(0, 0,
                    Systems.BulletManager.gunTextures[weapon1.textureId].Width,
                    Systems.BulletManager.gunTextures[weapon1.textureId].Height),
                    Color.White, rotation,
                    new Vector2(
                    Systems.BulletManager.gunTextures[weapon1.textureId].Width / 2,
                    Systems.BulletManager.gunTextures[weapon1.textureId].Height / 2 + 2),
                    SpriteEffects.None, 0f);
            }

            playerParticleSystem.Draw(spriteBatch);
        }
    
        #endregion

    }
}
