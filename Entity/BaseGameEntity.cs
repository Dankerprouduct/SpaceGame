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
    public class BaseGameEntity
    {
        public string name; 
        public int propertyID;
        public int mID;
        public static int mNextValidID = 0;

        public int cellIndex;
        public Point currentCell;

        public float rotation; 
        public Vector2 velocity;
        public Vector2 oldPosition;
        public Vector2 position;
        public Vector2 direction;
        public Rectangle rect;
        public int health;
        public float speed;
        public float maxVelocity;
        public int strength = 0;
        public int dexterity = 0;
        public int intelligence = 0;
        public int wisdom = 0; 
        public int charisma = 0;
        public Lua generalEntityLua;

        // body parts
        public struct BodyPart
        {
            public Vector2 newPosition;
            public Vector2 position;
            public Vector2 center;
            public string Tag;
            public void Update()
            {

                newPosition = Vector2.Lerp(newPosition, position, .55f);

            }

        }
        public BodyPart leftHand;
        public BodyPart rightHand;
        public BodyPart head; 

        //inventory stuff
        public Item.Inventory inventory;  
        public Weapons.BaseWeapon weapon1;
        public Weapons.BaseWeapon weapon2; 

        public void SetID()
        {
            mID = mNextValidID;
            mNextValidID++;
        }

        public BaseGameEntity()
        {
            SetID();
        }

        public BaseGameEntity(Vector2 position)
        {
            this.position = position;
            
            SetID();
        }


        /// <summary>
        /// gives player weapon and adds current one back to inventory
        /// </summary>
        /// <param name="name">name of weapon</param>s
        public void ChangeWeapon(string name)
        {
            if (weapon1 != null)
            {
                inventory.AddItem(new Item.Weapon(weapon1.name));
            }
            weapon1 = Weapons.WeaponHelper.GetWeapon(name);
        }


        // adds current weapon to inventory the empties hand
        public void RemoveWeapon()
        {
            if (weapon1 != null)
            {
                inventory.AddItem(new Item.Weapon(weapon1.name));
            }
            weapon1 = null;
        }


        public void AddForce(float rotation, float force)
        {
            direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            velocity += direction * force;
        }


        public float GetForce()
        {
            return velocity.Length();
        }

        public Vector2 GetCenter()
        {
            return new Vector2(position.X + (rect.Width / 2), position.Y + (rect.Height / 2));
        }

        public void SetPartitionCell(int x, int y)
        {
            currentCell = new Point(x, y);
        }

        public void SetEntityPosition(float X, float Y)
        {
            position = new Vector2((float)X, (float)Y);
        }

        public Vector2 Vec2ToEntitySpace(Vector2 pos)
        {
            
            Matrix rotationMatrix = Matrix.CreateRotationZ(rotation);

            Vector2 _pos = pos;

            return _pos = GetCenter() + Vector2.Transform(_pos, rotationMatrix);
            
            
        }

        public Vector2 Vec2ToEntitySpace(Vector2 pos1, Vector2 pos2, float rot)
        {

            Matrix rotationMatrix = Matrix.CreateRotationZ(rot);

            Vector2 _pos = pos1;

            return _pos = pos2 + Vector2.Transform(_pos, rotationMatrix);


        }

        public Vector2 GetEntityPosition()
        {
            return position;
        }

        public Vector2 GetEntityVelocity()
        {
            return velocity; 
        }

        public BaseGameEntity GetPlayer()
        {
            return Game1.player;
        }

        public float GetDistanceToEntity(BaseGameEntity target)
        {
            return Vector2.Distance(position, target.position);
        }

        public void TakeDamage(int val)
        {
            health -= val; 
        }

        #region Steering Behaviors
        
        public void AddSteeringForce(Vector2 steering)
        {
            velocity += steering;
        }


        public Vector2 Seek(Vector2 target)
        {

            Vector2 desiredVelocity = target - position;
            float distance = desiredVelocity.Length();
            desiredVelocity.Normalize();
            desiredVelocity *= maxVelocity;
            Vector2 steering = desiredVelocity - velocity;
            return steering;
        }

        public Vector2 Flee(Vector2 target)
        {

            Vector2 desiredVelocity = position - target;
            float distance = desiredVelocity.Length();
            desiredVelocity.Normalize();
            desiredVelocity *= maxVelocity;
            Vector2 steering = desiredVelocity - velocity;
            return steering;
        }

        public Vector2 Arrive(BaseGameEntity target, float slowingDistance)
        {
            Vector2 desiredVelocity = target.position - position;
            float distance = desiredVelocity.Length();

            if (distance < slowingDistance)
            {
                desiredVelocity.Normalize();
                desiredVelocity *= maxVelocity * (distance / slowingDistance);
            }
            else
            {
                desiredVelocity.Normalize();
                desiredVelocity *= maxVelocity;
            }
            Vector2 steering = desiredVelocity - velocity;
            return steering;
        }

        public Vector2 Pursue(BaseGameEntity target)
        {
            float distance = Vector2.Distance(target.position, position);
            float ahead = distance / 10;
            Vector2 futurePosition = target.position + target.velocity * ahead;
            return Seek(futurePosition);
        }

        public Vector2 Evade(BaseGameEntity target)
        {
            float distance = Vector2.Distance(target.position, position);
            float ahead = distance / 10;
            Vector2 futurePosition = target.position + target.velocity * ahead;
            return Flee(futurePosition);
        }

        public Vector2 Separation()
        {
            Vector2 steeringForce = Vector2.Zero;
            for(int i = 0; i < Game1.planet.cellSpacePartition.cells[cellIndex].members.Count; i++)
            {
                if (Game1.planet.cellSpacePartition.cells[cellIndex].members[i] != this)
                {
                    Vector2 toAgent = GetCenter() - Game1.planet.cellSpacePartition.cells[cellIndex].members[i].GetCenter();
                    Vector2 origanal = toAgent;
                    toAgent.Normalize();
                    steeringForce += toAgent / origanal.Length() * 5;
                }
            }
            return steeringForce;
        }
        
        #endregion

        public virtual bool Physics()
        {
            rect = new Rectangle((int)position.X, (int)position.Y,
                25, 25);
            if (Game1.planet.CheckCollision(rect))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual void Update(GameTime gameTime )
        {
            
        }

        public virtual void CompileLua()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

    }
}
