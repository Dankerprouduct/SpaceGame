using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework; 


namespace SpaceGame.Universe
{
    public class Tile
    {
        public Vector2 position;
        public int id; 

        public Tile(Vector2 position, int id)
        {
            this.position = position;
            this.id = id; 
        }

        public void ChangeTile(int id)
        {
            this.id = id; 
        }
    }
}
