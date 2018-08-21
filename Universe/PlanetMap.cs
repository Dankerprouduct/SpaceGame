using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SpaceGame.Universe
{
    public class PlanetMap
    {
        public List<Rectangle> Rooms;
        public int[,] tilemap;

        public PlanetMap()
        {
            Rooms = new List<Rectangle>();
        }

        public void Initialize(int width, int height)
        {
            tilemap = new int[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    
                    tilemap[x, y] = 2;
                }
            }
        }
                

        public int[,] GetMap()
        {
            return tilemap;
        }
        
    }
}
