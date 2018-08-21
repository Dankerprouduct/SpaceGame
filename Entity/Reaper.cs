using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SpaceGame.Entity
{
    public class Reaper: NPC
    {

        public Reaper(Vector2 position): base(position, Faction.Reapers)
        {

        }
    }
}
