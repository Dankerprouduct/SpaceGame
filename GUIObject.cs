using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 


namespace SpaceGame
{
    public class GUIObject
    {
        public GUIType guiType; 

        private Rectangle rectangle;
        public Vector2 position; 
        private Color color;
        public string text; 
        
        public enum GUIType
        {
            Box,
            BoxwString
        }
        
        public GUIObject(Rectangle rect, Color color)
        {
            this.rectangle = rect;
            this.color = color;
            this.guiType = GUIType.Box; 
        }

        public GUIObject(Rectangle rect, string text, Color color)
        {
            this.rectangle = rect;
            this.color = color;
            this.text = text;
            this.guiType = GUIType.BoxwString;
            this.position = new Vector2(rect.X, rect.Y); 
        }


        public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
        {
            spriteBatch.Draw(pixel, rectangle, color);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, Texture2D pixel, string text, Vector2 position)
        {
            spriteBatch.DrawString(spriteFont, text, position, Color.White); 
        }
    }
}
