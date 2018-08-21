using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SpaceGame.Entity
{
    public class Rebel: NPC
    {

        public Rebel(Vector2 position, int id): base(position, Faction.Rebels, id)
        {
            
        }
    }
}
