using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NLua; 


namespace SpaceGame.Entity
{
    public class Monster : BaseGameEntity
    {
        Lua lua;

        Weapons.BaseWeapon gun; 
        public Monster(Vector2 position) : base(position)
        {
            rect = new Rectangle((int)position.X, (int)position.Y, 32, 32);
            rotation = MathHelper.ToRadians(Game1.random.Next(0,360));
            gun = new Weapons.SparkerCannon("SparkerCannon", 6); 
            maxVelocity = 5;
            health = 50;
            lua = new Lua();
            lua.RegisterFunction("SetPosition", this, GetType().GetMethod("SetEntityPosition"));
            lua.RegisterFunction("GetCenter", this, GetType().GetMethod("GetCenter"));
            lua.RegisterFunction("GetForce", this, GetType().GetMethod("GetForce"));
            lua.RegisterFunction("AddForce", this, GetType().GetMethod("AddForce"));

            lua.RegisterFunction("Seek", this, GetType().GetMethod("Seek"));
            lua.RegisterFunction("Flee", this, GetType().GetMethod("Flee"));
            lua.RegisterFunction("Arrive", this, GetType().GetMethod("Arrive"));
            lua.RegisterFunction("Pursue", this, GetType().GetMethod("Pursue"));
            lua.RegisterFunction("Evade", this, GetType().GetMethod("Evade"));
            lua.RegisterFunction("AddSteeringForce", this, GetType().GetMethod("AddSteeringForce"));

            lua.RegisterFunction("GetPlayer", this, GetType().GetMethod("GetPlayer"));
            lua.RegisterFunction("DistanceTo", this, GetType().GetMethod("GetDistanceToEntity"));
            lua.RegisterFunction("GetVelocity", this, GetType().GetMethod("GetEntityVelocity"));
            lua.RegisterFunction("UpdatePhysics", this, GetType().GetMethod("UpdatePhysics"));
            lua.RegisterFunction("GetMembers", this, GetType().GetMethod("GetMembers"));
            lua.RegisterFunction("FireAt", this, GetType().GetMethod("FireAt"));
            lua.RegisterFunction("Separation", this, GetType().GetMethod("Separation"));
            lua.DoFile("Scripts/Entity/MonsterAI.lua");
        }
        public override void CompileLua()
        {

            lua = new Lua();
            lua.RegisterFunction("SetPosition", this, GetType().GetMethod("SetEntityPosition"));
            lua.RegisterFunction("GetCenter", this, GetType().GetMethod("GetCenter"));
            lua.RegisterFunction("GetForce", this, GetType().GetMethod("GetForce"));
            lua.RegisterFunction("AddForce", this, GetType().GetMethod("AddForce"));

            lua.RegisterFunction("Seek", this, GetType().GetMethod("Seek"));
            lua.RegisterFunction("Flee", this, GetType().GetMethod("Flee"));
            lua.RegisterFunction("Arrive", this, GetType().GetMethod("Arrive"));
            lua.RegisterFunction("Pursue", this, GetType().GetMethod("Pursue"));
            lua.RegisterFunction("Evade", this, GetType().GetMethod("Evade"));
            lua.RegisterFunction("GetMembers", this, GetType().GetMethod("GetMembers"));
            lua.RegisterFunction("AddSteeringForce", this, GetType().GetMethod("AddSteeringForce"));

            lua.RegisterFunction("GetPlayer", this, GetType().GetMethod("GetPlayer"));
            lua.RegisterFunction("DistanceTo", this, GetType().GetMethod("DistanceTo"));
            lua.RegisterFunction("GetVelocity", this, GetType().GetMethod("GetEntityVelocity"));
            lua.RegisterFunction("UpdatePhysics", this, GetType().GetMethod("UpdatePhysics"));
            lua.RegisterFunction("GetCurrentCellIndex", this, GetType().GetMethod("GetCurrentCellIndex"));
            lua.RegisterFunction("FireAt", this, GetType().GetMethod("FireAt"));
            lua.RegisterFunction("Separation", this, GetType().GetMethod("Separation"));
            cellIndex = GetCurrentCellIndex();
            lua.DoFile("Scripts/Entity/MonsterAI.lua");
            
        }

        public override void Update(GameTime gameTime)
        {
            if(cellIndex != GetCurrentCellIndex())
            {
                // switch cells
                Game1.planet.cellSpacePartition.ChangeCell(this);
                cellIndex = GetCurrentCellIndex(); 
            }

            velocity *= .85f;
            if (Vector2.Distance(GetPlayer().position, position) <= (float)(double)lua["radius"])
            {
                lua.GetFunction("Update").Call();
                
                gun.Update(gameTime, GetCenter(), rotation);
                //AddSteeringForce(Pursue(GetPlayer()));

            }
            

        }

        public List<BaseGameEntity> GetMembers()
        {
            return Game1.planet.cellSpacePartition.cells[cellIndex].members;
        }

        public void FireAt(BaseGameEntity ent, int id )
        {
            Vector2 direction = ent.GetCenter() - GetCenter();
            direction.Normalize();
            float rotation = (float)Math.Atan2(direction.Y, direction.X);
            this.rotation = rotation; 
            gun.FireWeapon(rotation);
            //Game1.planet.bulletManager.AddBullet(GetCenter(), rotation, 50, .5f, 0); 
        }

        public int GetCurrentCellIndex()
        {
            return Game1.planet.cellSpacePartition.PositionToIndex(this);

        }
        
        public void UpdatePhysics()
        {
            oldPosition = position; 
            position.X += (int)velocity.X;
            if (Physics())
            {
                position.X = oldPosition.X;
                velocity.X = -velocity.X;
            }

            position.Y += (int)velocity.Y;
            if (Physics())
            {
                position.Y = oldPosition.Y;
                velocity.Y = -velocity.Y;
            }

            if (Game1.planet.bulletManager.CheckCollision(rect))
            {
                rect = new Rectangle((int)position.X, (int)position.Y,
                25,
                25);

                TakeDamage(10);
                if(health <= 0)
                {
                    Game1.planet.entityParticle.CallEffect("Explosion1", GetCenter()); 
                    Game1.planet.cellSpacePartition.cells[cellIndex].RemoveEntity(this); 
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.pixel, new Rectangle((int)rect.X - (health / 2), (int)GetCenter().Y - 45,
                health, 3), Color.Red);
                        
        }

    }
}
