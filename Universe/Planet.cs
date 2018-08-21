using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;
using NLua;

namespace SpaceGame.Universe
{
    public class Planet
    {
        public static List<Texture2D> tileTextures;
        public Vector2 position;
        public PlanetClass planetClass;
        private MapGenerator mapGenerator;
        public PlanetMap planetMap;
        Lua lua;
        public static Texture2D tempMonster; 
        List<Entity.BaseGameEntity> entities = new List<Entity.BaseGameEntity>();

        public Systems.CellSpacePartition cellSpacePartition;
        public Systems.CellSpacePartition tilePartition;
        public Systems.ParticleSystem entityParticle;
        public Systems.BulletManager bulletManager; 

        public enum PlanetClass
        {
            ClassA, // LARGE    
            ClassB, // MEDIUM
            ClassC, // SMALL
            ClassD  // TINY
        }

        public Planet(PlanetClass pClass)
        {
            planetClass = pClass;

            CompileLua();
            SetPlanetClass(planetClass);
            GenerateLevel(0);
        }

        private void SetPlanetClass(PlanetClass p_class)
        {
            planetClass = p_class;
            planetMap = new PlanetMap();
            entities = new List<Entity.BaseGameEntity>();
            entityParticle = new Systems.ParticleSystem(500);
            bulletManager = new Systems.BulletManager(500); 

            switch (p_class)
            {
                case PlanetClass.ClassA:
                    {

                        cellSpacePartition = new Systems.CellSpacePartition((int)(double)lua["ClassA.Width"], (int)(double)lua["ClassA.Height"], 4);
                        tilePartition = new Systems.CellSpacePartition((int)(double)lua["ClassA.Width"], (int)(double)lua["ClassA.Height"], 4);
                        mapGenerator = new MapGenerator(
                            (int)(double)lua["ClassA.Width"],
                            (int)(double)lua["ClassA.Height"],
                            (int)(double)lua["ClassA.MaxRooms"],
                            (int)(double)lua["ClassA.RoomMaxSize"],
                            (int)(double)lua["ClassA.RoomMinSize"]);
                        planetMap = mapGenerator.CreateMap();
                        break;
                    }
                case PlanetClass.ClassB:
                    {
                        cellSpacePartition = new Systems.CellSpacePartition((int)(double)lua["ClassB.Width"], (int)(double)lua["ClassB.Height"], 4);
                        tilePartition = new Systems.CellSpacePartition((int)(double)lua["ClassB.Width"], (int)(double)lua["ClassB.Height"], 4);
                        mapGenerator = new MapGenerator(
                            (int)(double)lua["ClassB.Width"],
                            (int)(double)lua["ClassB.Height"],
                            (int)(double)lua["ClassB.MaxRooms"],
                            (int)(double)lua["ClassB.RoomMaxSize"],
                            (int)(double)lua["ClassB.RoomMinSize"]);
                        planetMap = mapGenerator.CreateMap();
                        break;
                    }
                case PlanetClass.ClassC:
                    {
                        cellSpacePartition = new Systems.CellSpacePartition((int)(double)lua["ClassC.Width"], (int)(double)lua["ClassC.Height"], 4);
                        tilePartition = new Systems.CellSpacePartition((int)(double)lua["ClassC.Width"], (int)(double)lua["ClassC.Height"], 4);
                        mapGenerator = new MapGenerator(
                            (int)(double)lua["ClassC.Width"],
                            (int)(double)lua["ClassC.Height"],
                            (int)(double)lua["ClassC.MaxRooms"],
                            (int)(double)lua["ClassC.RoomMaxSize"],
                            (int)(double)lua["ClassC.RoomMinSize"]);
                        planetMap = mapGenerator.CreateMap();
                        break;
                    }
                case PlanetClass.ClassD:
                    {
                        cellSpacePartition = new Systems.CellSpacePartition((int)(double)lua["ClassD.Width"], (int)(double)lua["ClassD.Height"], 4);
                        tilePartition = new Systems.CellSpacePartition((int)(double)lua["ClassD.Width"], (int)(double)lua["ClassD.Height"], 4);
                        mapGenerator = new MapGenerator(
                            (int)(double)lua["ClassD.Width"],
                            (int)(double)lua["ClassD.Height"],
                            (int)(double)lua["ClassD.MaxRooms"],
                            (int)(double)lua["ClassD.RoomMaxSize"],
                            (int)(double)lua["ClassD.RoomMinSize"]);
                        planetMap = mapGenerator.CreateMap();
                        break;
                    }

            }

            for(int y = 0; y < planetMap.tilemap.GetLength(1); y++)
            {
                for(int x = 0; x < planetMap.tilemap.GetLength(0); x++)
                {
                    tilePartition.AddEntity(new Entity.Tile(new Vector2(x * 128, y * 128), planetMap.tilemap[x, y]));
                }
            }

            lua.DoFile("Scripts/Planets/Generation.lua");

        }

        

        #region Lua

        public void CompileLua()
        {
            lua = new Lua();
            lua.RegisterFunction("SetTile", this, GetType().GetMethod("SetTile"));
            lua.RegisterFunction("GetRooms", this, GetType().GetMethod("GetRooms"));
            lua.RegisterFunction("GetRandomRoomLocation", this, GetType().GetMethod("GetRandomRoomLocation"));
            lua.RegisterFunction("GetRoomNumber", this, GetType().GetMethod("GetRoomNumber"));
            lua.RegisterFunction("SpawnEntity", this, GetType().GetMethod("SpawnEntity"));
            lua.DoFile("Scripts/Planets/PlanetType.lua");
            SetPlanetClass(planetClass);

            Point point = GetRandomRoomLocation(GetRooms()[0]); 
            
        }

        public void CompileEntityLua()
        {
            
        }

        public List<Rectangle> GetRooms()
        {
            return planetMap.Rooms;
        }

        public int GetRoomNumber()
        {
            return planetMap.Rooms.Count;
        }

        public Point GetRandomRoomLocation(Rectangle rect)
        {
            int x = Game1.random.Next(rect.X + 1, rect.X + rect.Width - 1);
            int y = Game1.random.Next(rect.Y + 1, rect.Y + rect.Height - 1);
            return new Point(x, y);
        }

        public void SetTile(int x, int y, int id)
        {
            planetMap.tilemap[x, y] = id;

        }

        public void SpawnEntity(int x, int y, int id)
        {
            Console.WriteLine("spawining entity " + id + " at " + x + " " + y);
            if (cellSpacePartition != null)
            {
                cellSpacePartition.AddEntity(new Entity.Monster(new Vector2(x * 128, y * 128)));
            }
        }

        #endregion

        public void GenerateLevel(int _level)
        {
            Console.WriteLine("Generated level: " + _level);
        }
        
        static int[,] LoadMapData(string name)
        {

            List<string> maps = LoadMaps(name);
            string mapName = maps[Game1.random.Next(0, maps.Count)];
            string path = name + mapName + ".txt";

            // Width and height of our tile array
            int width = 0;
            int height = File.ReadLines(path).Count();

            StreamReader sReader = new StreamReader(path);
            string line = sReader.ReadLine();
            string[] tileNo = line.Split(',');

            width = tileNo.Count();

            // Creating a new instance of the tile map
            sReader.Close();
            int[,] map = new int[width, height];

            // Re-initialising sReader
            sReader = new StreamReader(path);

            for (int y = 0; y < height; y++)
            {
                line = sReader.ReadLine();
                tileNo = line.Split(',');
                // Console.WriteLine(line); 
                for (int x = 0; x < width; x++)
                {
                    map[x, y] = Convert.ToInt32(tileNo[x]);
                }
            }

            return map;

        }
        
        static List<string> LoadMaps(string contentFolder)
        {
            //Load directory info, abort if none
            DirectoryInfo dir = new DirectoryInfo(contentFolder);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();
            //Init the resulting list
            List<string> result = new List<string>();

            //Load all files that matches the file filter
            FileInfo[] files = dir.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);

                result.Add(key);
            }
            //Return the result
            return result;
        }
        
        public void LoadContent(ContentManager content)
        {

            tempMonster = content.Load<Texture2D>("Entity/Sparker");
            tileTextures = new List<Texture2D>();
            Dictionary<string, Texture2D> texs = LoadContent<Texture2D>(content, "Tiles");
            foreach (KeyValuePair<string, Texture2D> entry in texs)
            {
                // do something with entry.Value or entry.Key
                tileTextures.Add(entry.Value);
            }
            
            entityParticle.LoadContent(content);
            bulletManager.LoadContent(content);

        }

        public static Dictionary<String, T> LoadContent<T>(ContentManager contentManager, string contentFolder)
        {
            //Load directory info, abort if none
            DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory + "\\" + contentFolder);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();
            //Init the resulting list
            Dictionary<String, T> result = new Dictionary<String, T>();

            //Load all files that matches the file filter
            FileInfo[] files = dir.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);

                result[key] = contentManager.Load<T>(contentFolder + "/" + key);
            }
            //Return the result
            return result;
        }

        public void Update(GameTime gameTime)
        {

            tilePartition.Update(gameTime);
            cellSpacePartition.Update(gameTime);
            entityParticle.Update();
            bulletManager.Update();

        }

        public bool CheckCollision(Rectangle rect)
        {
            
            return tilePartition.CheckCollision(rect); 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            tilePartition.DrawTiles(spriteBatch);
            cellSpacePartition.Draw(spriteBatch);
            entityParticle.Draw(spriteBatch);
            bulletManager.Draw(spriteBatch);
            
        }

        

    }
}
