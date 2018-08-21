using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using NLua; 


namespace SpaceGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        // static
        public static int ScreenWidth = 1080; 
        public static int ScreenHeight = (ScreenWidth / 16) * 9;
        public static Random random = new Random("lele".GetHashCode());
        

        // universe
        public static Universe.SolarSystem system1; 
        public static Universe.Planet planet;
        public static Entity.Player player;
        public static Ship ship;

        // gui stuff
        public static Texture2D pixel; 
        public static Vector2 cameraMousePosition;
        public static SpriteFont debugFont;        
        public static Point mouseCords;
        float fps;
        
        // Input
        KeyboardState kbState;
        KeyboardState oldKbState;
        Utilities.LuaDebug luaDebug; 
        
        Camera camera;
        bool compiling = false;
        bool showdebug;


        public Game1()
        {

            graphics = new GraphicsDeviceManager(this)
            {
                GraphicsProfile = GraphicsProfile.HiDef
            };
            Content.RootDirectory = "Content"; // GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferWidth = ScreenWidth;
            IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            

        }
                
        protected override void Initialize()
        {
            camera = new Camera(GraphicsDevice.Viewport);
            
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GUIManager.Init();
            GUIManager.LoadContent(Content);

            debugFont = Content.Load<SpriteFont>("Font/Debug");
            pixel = Content.Load<Texture2D>("GUI/Pixel");

            planet = new Universe.Planet(Universe.Planet.PlanetClass.ClassA);
            planet.LoadContent(Content);
            system1 = new Universe.SolarSystem(8, 50000);
            system1.LoadContent(Content);
            ship = new Ship(Vector2.Zero);
            ship.LoadContent(Content);
            
            Point spawnPoint = planet.planetMap.Rooms[random.Next(0, planet.planetMap.Rooms.Count)].Center;
            
            player = new Entity.Player(0, new Vector2(spawnPoint.X * 128, spawnPoint.Y * 128));
            player.LoadContent(Content);
            planet.CompileEntityLua();
            Console.WriteLine("Spawned player at " + player.position + " " + player.cellIndex);
            luaDebug = new Utilities.LuaDebug();

            planet.cellSpacePartition.ClearCell(planet.cellSpacePartition.PositionToIndex(
                new Vector2(spawnPoint.X * 128, spawnPoint.Y * 128)));
        }
        
        protected override void UnloadContent()
        {
            
        }

        public void DrawLine(Vector2 p1, Vector2 p2, int thickness, Color color)
        {
            float angle = (float)Math.Atan2((p1.Y - p2.Y), (p1.X - p2.X));
            float dist = Vector2.Distance(p1, p2);

            spriteBatch.Draw(pixel, new Rectangle((int)p2.X, (int)p2.Y, (int)dist, thickness), null, color, angle, Vector2.Zero, SpriteEffects.None, 0f);
        }

        public void DrawBox(Rectangle rectangle, int thickness, Color color)
        {

            // top
            DrawLine(new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.Width, rectangle.Y), thickness, color);
            // bottom
            DrawLine(new Vector2(rectangle.X, rectangle.Height), new Vector2(rectangle.Width, rectangle.Height), thickness, color);
            DrawLine(new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.X, rectangle.Height), thickness, color);
            // right
            DrawLine(new Vector2(rectangle.Width, rectangle.Y), new Vector2(rectangle.Width, rectangle.Height), thickness, color);

        }

        protected override void Update(GameTime gameTime)
        {
            kbState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Game1 game = this;

            GUIManager.Update();
            

            if (kbState.IsKeyDown(Keys.OemTilde) && oldKbState.IsKeyUp(Keys.OemTilde))
            {
                luaDebug.showDebug = !luaDebug.showDebug; 
            }
            if (kbState.IsKeyDown(Keys.F3) && oldKbState.IsKeyUp(Keys.F3))
            {
                showdebug = !showdebug;
            }


            if (luaDebug.showDebug)
            {
                luaDebug.Update();

                if (kbState.IsKeyDown(Keys.Enter) && oldKbState.IsKeyUp(Keys.Enter))
                {
                    luaDebug.EnterCommand(luaDebug.text);
                    luaDebug.showDebug = false; 
                }
            }
            else
            {
                player.inventory.Update();

                camera.Update(ref game);
                if (!player.inventory.showInventory)
                {
                    planet.Update(gameTime);
                    player.Update(gameTime);
                    

                    if (kbState.IsKeyDown(Keys.Enter) && oldKbState.IsKeyUp(Keys.Enter))
                    {
                        luaDebug.EnterCommand(luaDebug.lastText);

                    }
                }
                
            }
            Vector2 mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y); 
            cameraMousePosition = Vector2.Transform(mousePosition, Matrix.Invert(camera.transform));
            mouseCords = new Point((int)cameraMousePosition.X / 128, (int)cameraMousePosition.Y / 128);

            oldKbState = kbState;
            base.Update(gameTime);
        }
                
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.transform);

            planet.Draw(spriteBatch);            
            ship.Draw(spriteBatch);

            if (!compiling)
            {                
                player.Draw(spriteBatch);                
            }

            if (showdebug)
            {
                // draws chunk boundaries
                DrawBox(new Rectangle((int)planet.tilePartition.cells[planet.tilePartition.mm].members[0].GetEntityPosition().X, (int)planet.tilePartition.cells[planet.tilePartition.mm].members[0].GetEntityPosition().Y,
                        (int)planet.tilePartition.cells[planet.tilePartition.mm].members[0].GetEntityPosition().X + (128 * 4 * 4), (int)planet.tilePartition.cells[planet.tilePartition.mm].members[0].GetEntityPosition().Y + (128 * 4 * 4)), 10, Color.Red);
            }
            // calculates fps
            fps = (int)(1 / (float)gameTime.ElapsedGameTime.TotalSeconds); 
            spriteBatch.End();
                       

            #region GUI
            // GUI STUFF
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            
            GUIManager.Draw(spriteBatch);
            player.inventory.Draw();
            player.DrawPlayerGUI(); 


            Rectangle screenBounds = new Rectangle(0, 0, Game1.ScreenWidth, Game1.ScreenHeight);
            GUIManager.DrawString("DEVELOPMENT BUILD", new Vector2(screenBounds.X + 20, screenBounds.Height - 20), Color.Gray);

            if (showdebug)
            {
                GUIManager.DrawString("FPS: " + fps, new Vector2(10, 10), Color.White);
                GUIManager.DrawString("Planet Class: " + planet.planetClass.ToString(), new Vector2(10, 30), Color.White);
                GUIManager.DrawString("Mouse Position: " + mouseCords.ToString() + " ,Chunk: " + planet.cellSpacePartition.PositionToIndex(cameraMousePosition).ToString(),
                    new Vector2(10, 50), Color.White);
                GUIManager.DrawString("Current Cell: " + planet.tilePartition.mm.ToString().ToString(), new Vector2(10, 70), Color.White);

                if (planet.cellSpacePartition.cells[planet.cellSpacePartition.mm].members != null)
                {
                    GUIManager.DrawString("Entities in Cell: " + planet.cellSpacePartition.cells[planet.cellSpacePartition.mm].members.Count.ToString().ToString(),
                        new Vector2(10, 90), Color.White);
                }
                GUIManager.DrawString("Active Paricles: " + planet.entityParticle.GetActiveParticles().ToString(),
                    new Vector2(10, 110), Color.White);
                GUIManager.DrawString("Active Bullets: " + planet.bulletManager.GetActiveBullets().ToString(),
                    new Vector2(10, 130), Color.White);

            }

            // draws lua console
            if (luaDebug.showDebug)
            {
                spriteBatch.Draw(pixel, new Rectangle(0, ScreenHeight - 24, ScreenWidth, 24), Color.Black * .5f); 
                spriteBatch.DrawString(debugFont, luaDebug.text, new Vector2(10, ScreenHeight - 20), Color.White);
            }
            
            spriteBatch.End();
            #endregion
                       

            base.Draw(gameTime);
        }
    }
}
