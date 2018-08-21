using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SpaceGame.Utilities
{
    public static class NPCSpawner
    {
        
        
        public static Entity.NPC SpawnNPC(Entity.NPC.Faction faction, Vector2 position, int level)
        {
            switch (faction)
            {
                case Entity.NPC.Faction.Federation:
                    {
                        return new Entity.Federation(position, level); 
                        
                    }
                case Entity.NPC.Faction.Rebels:
                    {
                        return new Entity.Rebel(position, level); 
                        
                    }
                case Entity.NPC.Faction.TheAcquisitors:
                    {
                        return new Entity.Acquisitor(position, level); 
                    }
                case Entity.NPC.Faction.TheNewOrder:
                    {
                        return new Entity.TheNewOrder(position, level);
                    }
                case Entity.NPC.Faction.Reapers:
                    {
                        return new Entity.Reaper(position, level); 
                    }
                default:
                    {
                        return new Entity.NPC(position, faction, 0); ; 
                    }
            }
            
        }
    }
}
