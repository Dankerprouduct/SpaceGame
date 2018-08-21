using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NLua;
using Microsoft.Xna.Framework.Content; 
using Microsoft.Xna.Framework.Input; 

namespace SpaceGame
{
    public class GUIManager
    {
        static Lua lua;
        static List<GUIObject> guiObject;

        static SpriteBatch spriteBatch;
        public static MouseState mouseState;
        public static KeyboardState keyboardState;
        public static KeyboardState oldKeyboardState; 
        public static Point mousePoint;
        public static List<Texture2D> weaponGui = new List<Texture2D>();
        public static Rectangle screenBounds = new Rectangle(0, 0, Game1.ScreenWidth, Game1.ScreenHeight);
        public static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();
        public struct ColorTheme
        {
            public string name;
            public Color color1;
            public Color color2;
            public Color color3;
            public Color color4;
            public Color color5; 
        }

        public static Dictionary<string, ColorTheme> themes = new Dictionary<string, ColorTheme>(); 

        public static void Init()
        {
            lua = new Lua();
            guiObject = new List<GUIObject>();
            ColorTheme retroSpace = new ColorTheme()
            {
                name = "RetroSpace1",
                color1 = new Color(209, 53, 81),    // red 
                color2 = new Color(63, 17, 58),     // purple
                color3 = new Color(87, 205, 214),   // blue
                color4 = new Color(252, 208, 102),  // yellow
                color5 = new Color(252, 153, 96)    // orange
            };
            themes.Add(retroSpace.name, retroSpace);

            ColorTheme retroSpace2 = new ColorTheme()
            {
                name = "RetroSpace2",
                color1 = new Color(27, 65, 89),    // blue
                color2 = new Color(73, 139, 166),  // light blue
                color3 = new Color(242, 234, 121), // yellow
                color4 = new Color(242, 145, 61),  // orange
                color5 = new Color(242, 94, 61)    // dark orange
            };
            themes.Add(retroSpace2.name, retroSpace2);

            ColorTheme space = new ColorTheme()
            {
                name = "Space",
                color1 = new Color(26, 20, 38),     // dark blue
                color2 = new Color(34, 58, 89),     // tealish 
                color3 = new Color(36, 107, 115),   // lighter teal
                color4 = new Color(33, 166, 166),   // light blue
                color5 = new Color(65, 166, 137)    // sea green
            };
            themes.Add(space.name, space); 
        }

        public static void LoadContent(ContentManager content)
        {
            weaponGui.Add(content.Load<Texture2D>("GUI/Weapons/Pistol"));
            weaponGui.Add(content.Load<Texture2D>("GUI/Weapons/LMG"));
            weaponGui.Add(content.Load<Texture2D>("GUI/Weapons/Laser Gun"));
            weaponGui.Add(content.Load<Texture2D>("GUI/Weapons/Double Barrel"));                       
            weaponGui.Add(content.Load<Texture2D>("GUI/Weapons/Machine Gun"));            
            weaponGui.Add(content.Load<Texture2D>("GUI/Weapons/Revolver"));

            fonts.Add("MenuFont", content.Load<SpriteFont>("GUI/Fonts/MenuFont")); 
        }

        public static void Update()
        {

            mouseState = Mouse.GetState();
            mousePoint = new Point(mouseState.X, mouseState.Y);
            
        }

        public static void DrawLine(Vector2 p1, Vector2 p2, int thickness, Color color)
        {
            float angle = (float)Math.Atan2((p1.Y - p2.Y), (p1.X - p2.X));
            float dist = Vector2.Distance(p1, p2);

            spriteBatch.Draw(Game1.pixel, new Rectangle((int)p2.X, (int)p2.Y, (int)dist, thickness), null, color, angle, Vector2.Zero, SpriteEffects.None, 0f);
        }

        public static void DrawBox(Rectangle rectangle, int thickness, Color color)
        {

            // top
            DrawLine(new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.Width, rectangle.Y), thickness, color);
            // bottom
            DrawLine(new Vector2(rectangle.X, rectangle.Height), new Vector2(rectangle.Width, rectangle.Height), thickness, color);
            DrawLine(new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.X, rectangle.Height), thickness, color);
            // right
            DrawLine(new Vector2(rectangle.Width, rectangle.Y), new Vector2(rectangle.Width, rectangle.Height), thickness, color);

        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            GUIManager.spriteBatch = spriteBatch; 
            
        }
        
        public static void DrawBox(int x, int y, int w, int h, Color color)
        {
            //guiObject.Add(new GUIObject(new Rectangle(x, y, w, h), color));
            spriteBatch.Draw(Game1.pixel, new Rectangle(x, y, w, h), color);
            
        }

        public static void DrawBox(Rectangle rect, Color color)
        {
            //guiObject.Add(new GUIObject(new Rectangle(x, y, w, h), color));
            spriteBatch.Draw(Game1.pixel, rect, color);

        }

        public static void DrawBoxWithString(int x, int y, int w, int h, Color color1, Color color2, string text)
        {
            //MakeBox(x, y, w, h, color1);
            guiObject.Add(new GUIObject(new Rectangle(x,y,w,h), text, color2));
        }

        public static void DrawBoxWithString(Rectangle rect, Color textColor, Color boxColor, string text)
        {
            //MakeBox(x, y, w, h, color1);
            DrawBox(rect, boxColor);
            spriteBatch.DrawString(Game1.debugFont, text, new Vector2(rect.X + (rect.Width / 2) - (Game1.debugFont.MeasureString(text).X / 2) , rect.Y), textColor);
        }
              
        public static void DrawTextboxWHeading(string heading, string content, Color cHeading, Color cContent, Rectangle rect)
        {
            spriteBatch.DrawString(
                Game1.debugFont, 
                heading,
                new Vector2(rect.X + (rect.Width / 2) - (Game1.debugFont.MeasureString(heading).X / 2), rect.Y), 
                cHeading);
           
            // finish this later


        }

        public static void DrawString(string text, Vector2 position, Color color)
        {
            //MakeBox(x, y, w, h, color1);
           spriteBatch.DrawString(Game1.debugFont, text, position, color);
        }

        public static void DrawString(string text, Vector2 position, float size, Color color)
        {
            //MakeBox(x, y, w, h, color1);
            spriteBatch.DrawString(Game1.debugFont, text, position, color, 0f, Vector2.Zero, size, SpriteEffects.None, 0f);
        }

        public static void DrawString(string text, string font, Vector2 position, Color color)
        {
            //MakeBox(x, y, w, h, color1);
            spriteBatch.DrawString(fonts[font], text, position, color);
        }

        public static void DrawTexture(Texture2D texture, Vector2 position)
        {
            //MakeBox(x, y, w, h, color1);
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}

