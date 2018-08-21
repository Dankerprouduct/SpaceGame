using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SpaceGame.Entity
{
    public class Tile: BaseGameEntity
    {
        public Tile(Vector2 position, int id): base(position)
        {
            propertyID = id;
            rect = new Rectangle((int)position.X, (int)position.Y, 128, 128); 
        }
    }
}
