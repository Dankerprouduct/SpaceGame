using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using NLua;

namespace SpaceGame.Universe
{
    public class SolarSystem
    {       
        
        public static List<Texture2D> tileTextures = new List<Texture2D>();
        public static List<Texture2D> planetTextures = new List<Texture2D>();
        Random rand = new Random();
        float range;
        
        struct PlanetPosition
        {
            public Vector2 position;
            public int id; 
        }

        PlanetPosition[] planets; 

        public SolarSystem(int planet_Ammount, float range)
        {
            this.range = range; 
            planets = new PlanetPosition[planet_Ammount];
            
        }

        public void Initialize()
        {
            
            for(int i = 0; i < planets.Length; i++)
            {
                float angle = Game1.random.Next(0, 360);
                planets[i].position = new Vector2((float)Math.Cos(angle) * Game1.random.Next((int)range / 5, (int)range),
                    (float)Math.Sin(angle) * Game1.random.Next((int)range / 5, (int)range));

                rand = new Random(i);
                planets[i].id = rand.Next(0, planetTextures.Count);
            }
         
        }

        public void ExecuteLuaCommand(string command)
        {
            Lua lua = new Lua();
            lua.RegisterFunction("Compile", this, GetType().GetMethod("Compile"));
            lua.RegisterFunction("SetPlayerPosition", this, GetType().GetMethod("SetPlayerPosition"));
            lua.RegisterFunction("SpawnEntity", this, GetType().GetMethod("SpawnEntity"));
            lua.RegisterFunction("ClearCell", this, GetType().GetMethod("ClearCell"));
            try
            {
                lua.DoString(command);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString()); 
            }
        }
        public void SpawnEntity(int type)
        {
            Game1.planet.SpawnEntity(Game1.mouseCords.X, Game1.mouseCords.Y, type);
        }
        public void ClearCell(int x)
        {
            Game1.planet.cellSpacePartition.ClearCell(x);
        }
        public void SetPlayerPosition(int x, int y)
        {
            Game1.player.SetEntityPosition(x * 128, y * 128); 
        }
        public void Compile(int val)
        {
            switch (val)
            {
                case 0: // system
                    {
                        Initialize();
                        break;
                    }
                case 1: // planet
                    {
                        Game1.planet.CompileLua();
                        break;
                    }
                case 2: // player
                    {
                        Point spawnPoint = Game1.planet.planetMap.Rooms[Game1.random.Next(0, Game1.planet.planetMap.Rooms.Count)].Center;
                        Game1.player.SetEntityPosition(spawnPoint.X * 128, spawnPoint.Y * 128);
                        Game1.player.CompileLua();
                        break;
                    }
                case 3: // all
                    {
                        Game1.player.CompileLua();
                        Game1.planet.CompileLua();
                        Point spawnPoint = Game1.planet.planetMap.Rooms[Game1.random.Next(0, Game1.planet.planetMap.Rooms.Count)].Center;
                        Game1.player.SetEntityPosition(spawnPoint.X * 128, spawnPoint.Y * 128);
                        break;
                    }
            }
        }

        public void LoadContent(ContentManager content)
        {
            planetTextures.Add(content.Load<Texture2D>("Planets/Planet1"));
            planetTextures.Add(content.Load<Texture2D>("Planets/Blueplanet"));
            planetTextures.Add(content.Load<Texture2D>("Planets/GreenPlanet"));
            planetTextures.Add(content.Load<Texture2D>("Planets/OrangePlanet"));

            Initialize(); 
        }

        public void Update()
        {

        }
        
        public void Draw(SpriteBatch spritBatch)
        {
            for(int i = 0; i < planets.Length; i++)
            {
                spritBatch.Draw(planetTextures[planets[i].id], planets[i].position, Color.White); 
            }
        }
    }
}
