using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp.MapCreation;
using NLua;
using Microsoft.Xna.Framework;

namespace SpaceGame.Universe
{
    public class MapGenerator
    {

        private readonly int width;
        private readonly int height;
        private readonly int maxRooms;
        private readonly int roomMaxSize;
        private readonly int roomMinSize;
        private readonly PlanetMap map; 


        public MapGenerator (int width, int height, int maxRooms, int roomMaxSize, int roomMinSize)
        {
            this.width = width;
            this.height = height;
            this.maxRooms = maxRooms;
            this.roomMaxSize = roomMaxSize;
            this.roomMinSize = roomMinSize;

            map = new PlanetMap(); 
        }

        public PlanetMap CreateMap()
        {
            map.Initialize(width, height); 

            for(int r = maxRooms; r > 0; r--)
            {
                int roomWidth = Game1.random.Next(roomMinSize, roomMaxSize);
                int roomHeight = Game1.random.Next(roomMinSize, roomMaxSize);
                int roomXPosition = Game1.random.Next(0, width - roomWidth - 1);
                int roomYPosition = Game1.random.Next(0, height - roomHeight - 1);

                var newRoom = new Rectangle(roomXPosition, roomYPosition, roomWidth, roomHeight);

                bool nRoomIntersects = map.Rooms.Any(room => newRoom.Intersects(room));

                if (!nRoomIntersects)
                {
                    // create room
                    map.Rooms.Add(newRoom);
                }
                              

            }

            foreach (Rectangle room in map.Rooms)
            {
                CreateRoom(room);
            }


            for (int r = 1; r < map.Rooms.Count; r++)
            {
                // For all remaing rooms get the center of the room and the previous room
                int previousRoomCenterX = map.Rooms[r - 1].Center.X;
                int previousRoomCenterY = map.Rooms[r - 1].Center.Y;
                int currentRoomCenterX = map.Rooms[r].Center.X;
                int currentRoomCenterY = map.Rooms[r].Center.Y;

                // Give a 50/50 chance of which 'L' shaped connecting hallway to tunnel out
                if (Game1.random.Next(1, 2) == 1)
                {
                    CreateHorizontalTunnel(previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
                    CreateVerticalTunnel(previousRoomCenterY, currentRoomCenterY, currentRoomCenterX);
                }
                else
                {
                    CreateVerticalTunnel(previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
                    CreateHorizontalTunnel(previousRoomCenterX, currentRoomCenterX, currentRoomCenterY);
                }
            }



            return map;
        }

        private void CreateHorizontalTunnel(int xStart, int xEnd, int yPosition)
        {
            for(int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
            {
                map.tilemap[x, yPosition] = 1; 
            }
        }

        private void CreateVerticalTunnel(int yStart, int yEnd, int xPositon)
        {
            for(int y = Math.Min(yStart, yEnd); y < Math.Max(yStart, yEnd); y++)
            {
                map.tilemap[xPositon, y] = 1; 
            }
        }

        public void CreateRoom(Rectangle room)
        {
            for(int x = room.Left + 1; x < room.Right; x++)
            {
                for(int y = room.Top + 1; y < room.Bottom; y++)
                {
                    map.tilemap[x, y] = 1; 
                }
            }
        }



    }
}
