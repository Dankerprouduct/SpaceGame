using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using NLua; 

namespace SpaceGame.Entity
{
    public class NPC: BaseGameEntity
    {

        
        public enum Faction // temp faction names
        {
            TheNewOrder, 
            TheAcquisitors, 
            Federation,
            Rebels,
            Reapers
        }
        public Faction faction;
        public int likeness;
        public int credits;
        
        public int level; // faction rank + (kills / 2)
        public Lua npcLua;
        private int id;

        public NPC(Vector2 position, Faction faction): base(position)
        {
            this.faction = faction;
            inventory = new Item.Inventory(5);
            InitializeNPCLua(); 
        }

        public NPC(Vector2 position, Faction faction, int id) : this(position, faction)
        {
            this.id = id;
        }

        public void InitializeNPCLua()
        {
            npcLua = new Lua();

            // NPC Specific functions
            npcLua.RegisterFunction("SetHealth", this, GetType().GetMethod("SetHealth"));
            npcLua.RegisterFunction("GetHealth", this, GetType().GetMethod("GetHealth"));
            npcLua.RegisterFunction("GiveCredits", this, GetType().GetMethod("GiveCredits"));
            npcLua.RegisterFunction("GiveItem", this, GetType().GetMethod("GiveItem"));
            npcLua.RegisterFunction("GiveArmor", this, GetType().GetMethod("GiveArmor"));
            npcLua.RegisterFunction("GetLevel", this, GetType().GetMethod("GetLevel"));
            npcLua.RegisterFunction("SetLevel", this, GetType().GetMethod("SetLevel"));
            npcLua.RegisterFunction("GetFaction", this, GetType().GetMethod("GetFaction"));
            npcLua.RegisterFunction("SetFaction", this, GetType().GetMethod("SetFaction"));

            // BaseGameEntity functions
            npcLua.RegisterFunction("SetID", this, GetType().GetMethod("SetID"));
            npcLua.RegisterFunction("ChangeWeapon", this, GetType().GetMethod("ChangeWeapon"));
            npcLua.RegisterFunction("RemoveWeapon", this, GetType().GetMethod("RemoveWeapon"));
            npcLua.RegisterFunction("AddForce", this, GetType().GetMethod("AddForce"));
            npcLua.RegisterFunction("GetForce", this, GetType().GetMethod("GetForce"));
            npcLua.RegisterFunction("GetCenter", this, GetType().GetMethod("GetCenter"));
            npcLua.RegisterFunction("SetPartitionCell", this, GetType().GetMethod("SetPartitionCell"));
            npcLua.RegisterFunction("Vec2ToEntitySpace", this, GetType().GetMethod("Vec2ToEntitySpace"));
            npcLua.RegisterFunction("GetEntityPosition", this, GetType().GetMethod("GetEntityPosition"));
            npcLua.RegisterFunction("GetEntityVelocity", this, GetType().GetMethod("GetEntityVelocity"));
            npcLua.RegisterFunction("GetPlayer", this, GetType().GetMethod("GetPlyer"));
            npcLua.RegisterFunction("GetDistanceToEntity", this, GetType().GetMethod("GetDistanceToEntity"));
            npcLua.RegisterFunction("TakeDamage", this, GetType().GetMethod("TakeDamage"));
            npcLua.RegisterFunction("AddSteeringForce", this, GetType().GetMethod("AddSteeringForce"));
            npcLua.RegisterFunction("Seek", this, GetType().GetMethod("Seek"));
            npcLua.RegisterFunction("Flee", this, GetType().GetMethod("Flee"));
            npcLua.RegisterFunction("Arrive", this, GetType().GetMethod("Arrive"));
            npcLua.RegisterFunction("Pursue", this, GetType().GetMethod("Pursue"));
            npcLua.RegisterFunction("Evade", this, GetType().GetMethod("Evade"));
            npcLua.RegisterFunction("Separation", this, GetType().GetMethod("Separation")); 

            

        }

        public void SetHealth(int ammount)
        {
            health = ammount; 
        }

        public int GetHealth(int ammount)
        {
            return ammount; 
        }

        public void GiveCredits(int ammount)
        {
            credits += ammount; 
            
        }

        /// <summary>
        /// Adds Item to inventory
        /// </summary>
        public void GiveItem(string name, int id)
        {
            inventory.AddItem(new Item.Item(name, id));
        }

        /// <summary>
        /// Adds Weapon to inventory
        /// </summary>
        public void GiveWeapon(string name)
        {
            inventory.AddItem(new Item.Weapon(name));
        }

        /// <summary>
        /// Adds Armor to inventory
        /// </summary>
        public void GiveArmor(string name, int id)
        {
            inventory.AddItem(new Item.Gear(name, id)); 
        }

        public int GetLevel()
        {
            return level; 
        }

        public void SetLevel(int _level)
        {
            level = _level;
        }
        
        public Faction GetFaction()
        {
            return faction; 
        }       
        
        public void SetFaction(Faction faction)
        {
            this.faction = faction;
        }
    }
}
