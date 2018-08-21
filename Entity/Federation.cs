using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using NLua; 

namespace SpaceGame.Entity
{
    public class Federation : NPC
    {

        public Federation(Vector2 position): base(position, Faction.Federation)
        {
            
        }
    }
}
