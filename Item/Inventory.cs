using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NLua;
using Microsoft.Xna.Framework.Input;

namespace SpaceGame.Item
{
    public class Inventory
    {
        public int width;
        public int height;
        public BaseItem[] items;
        public Lua lua;
        public bool showInventory = false;


        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;
        MouseState mouseState;
        MouseState oldMouseState;
        enum InventoryType
        {
            Player,
            NPC
        }
        InventoryType inventoryType;
        public Inventory(int width, int height)
        {
            inventoryType = InventoryType.Player;
            this.width = width;
            this.height = height;
            items = new BaseItem[width * height];
            lua = new Lua();
            Initialize();

            AddItem(new Weapon("Pistol"));
            AddItem(new Weapon("Pistol"));
            AddItem(new Weapon("Pistol"));
            AddItem(new Weapon("Pistol"));
            AddItem(new Weapon("LaserGun"));
            AddItem(new Weapon("LMG"));
        }

        public Inventory(int size)
        {
            inventoryType = InventoryType.NPC;
            items = new BaseItem[size];
            lua = new Lua();
            Initialize();
        }

        public void Initialize()
        {
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new BaseItem("EMPTY", -1);
            }

        }

        public void Update()
        {
            if (inventoryType == InventoryType.Player)
            {
                keyboardState = Keyboard.GetState();
                mouseState = Mouse.GetState();
                if (keyboardState.IsKeyDown(Keys.Tab) && oldKeyboardState.IsKeyUp(Keys.Tab))
                {
                    showInventory = !showInventory;
                }
                oldKeyboardState = keyboardState;
                oldMouseState = mouseState;
            }
        }

        public void AddItem(BaseItem item)
        {
            for (int i = 0; i < items.Length; i++)
            {

                if (items[i].id == -1)
                {
                    items[i] = item;
                    Console.WriteLine("added item: " + item.name + " " + item.id);
                    return;
                }
            }
        }

        public void RemoveItem(BaseItem item)
        {
            for (int i = 0; i < items.Length; i++)
            {

                if (items[i].id == item.id)
                {
                    items[i] = new BaseItem("EMPTY", -1); 
                    return;
                }
            }
        }

        public void Draw()
        {
            if (inventoryType == InventoryType.Player)
            {
                if (showInventory)
                {
                    Rectangle screenBounds = new Rectangle(0, 0, Game1.ScreenWidth, Game1.ScreenHeight);
                    Rectangle inventoryTextbox = new Rectangle(868, 71 - 20, 164, 20);
                    GUIManager.DrawBoxWithString(
                        inventoryTextbox,
                        Color.White,
                        GUIManager.themes["RetroSpace2"].color5,
                        "Inventory");

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            int square = 32;
                            Rectangle rect = new Rectangle(
                                screenBounds.Width - (width * square) - 55 + (x * square) + x + 3,
                                screenBounds.Height - (height * square) - 55 + (y * square) + y + 3,
                                square, square);


                            if (rect.Contains(GUIManager.mousePoint))
                            {
                                GUIManager.DrawBox(rect, GUIManager.themes["RetroSpace2"].color4 * .5f);
                                // make item textbox to the left of the inventory


                                if (items[x * width + y].name != "EMPTY")
                                {
                                    Rectangle itemBox = new Rectangle(inventoryTextbox.X - 300, inventoryTextbox.Y, 300, 200);
                                    GUIManager.DrawBoxWithString(itemBox,
                                        Color.White, GUIManager.themes["RetroSpace2"].color1,
                                        items[x * width + y].name);

                                    if (mouseState.LeftButton == ButtonState.Pressed)
                                    {
                                        if (items[x * width + y].itemType == BaseItem.ItemType.Weapon)
                                        {

                                            Game1.player.ChangeWeapon(items[x * width + y].name);
                                            items[x * width + y] = new BaseItem("EMPTY", -1);
                                        }
                                    }

                                }
                            }
                            else
                            {
                                GUIManager.DrawBox(rect, GUIManager.themes["RetroSpace2"].color2 * .5f);
                            }

                            // this draws the gui icons
                            if (items[x * width + y].id != -1)
                            {
                                switch (items[x * width + y].itemType)
                                {
                                    case BaseItem.ItemType.Weapon:
                                        {
                                            GUIManager.DrawTexture(
                                            GUIManager.weaponGui[items[x * width + y].textureID],
                                            new Vector2(rect.X, rect.Y));
                                            break;
                                        }
                                    case BaseItem.ItemType.Consumable:
                                        {
                                            break;
                                        }
                                    case BaseItem.ItemType.Item:
                                        {
                                            break;
                                        }
                                    case BaseItem.ItemType.Gear:
                                        {
                                            break;
                                        }
                                }



                            }


                        }
                    }

                    DrawPlayerStats();

                }

            }
        }

        public void DrawPlayerStats()
        {
            if (inventoryType == InventoryType.Player)
            {
                Rectangle backgroundBox = new Rectangle(62, 101, 300, 460);
                GUIManager.DrawBox(backgroundBox, GUIManager.themes["RetroSpace2"].color2 * .5f);

                Rectangle titleBox = new Rectangle(62, 101, 300, 27);
                GUIManager.DrawBoxWithString(titleBox, Color.White, GUIManager.themes["RetroSpace2"].color5, Game1.player.name);

                Rectangle weapon1 = new Rectangle(81, 155, 81 + 34, 155 + 34);
                Rectangle weapon2 = new Rectangle(81, 207, 81 + 34, 207 + 34);
                Rectangle item1 = new Rectangle(81, 259, 81 + 34, 259 + 34);

                Rectangle helmet = new Rectangle(296, 155, 296 + 34, 155 + 34);
                Rectangle bodyArmor = new Rectangle(296, 207, 296 + 34, 207 + 34);
                Rectangle item2 = new Rectangle(296, 259, 296 + 34, 259 + 34);

                Rectangle characterRect = new Rectangle(121, 155, 121 + 169, 293);
                Rectangle characterbg = new Rectangle(121, 155, 169, 138);

                GUIManager.DrawString("Weapon1", "MenuFont", new Vector2(weapon1.X - 14, weapon1.Y - 14), Color.White);
                GUIManager.DrawBox(weapon1, 1, new Color(33, 38, 40));
                if (Game1.player.weapon1 != null)
                {
                    GUIManager.DrawTexture(GUIManager.weaponGui[Game1.player.weapon1.textureId], new Vector2(weapon1.X, weapon1.Y));
                    if (new Rectangle(weapon1.X, weapon1.Y, 32, 32).Contains(GUIManager.mousePoint))
                    {
                        GUIManager.DrawBox(new Rectangle(weapon1.X, weapon1.Y, 32, 32), GUIManager.themes["RetroSpace2"].color4 * .5f);
                        // mouse click to dequip
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            Game1.player.RemoveWeapon();
                        }
                    }
                }

                GUIManager.DrawString("Weapon2", "MenuFont", new Vector2(weapon2.X - 14, weapon2.Y - 14), Color.White);
                GUIManager.DrawBox(weapon2, 1, new Color(33, 38, 40));
                if (Game1.player.weapon2 != null)
                {
                    GUIManager.DrawTexture(GUIManager.weaponGui[Game1.player.weapon2.textureId], new Vector2(weapon2.X, weapon2.Y));
                    if (new Rectangle(weapon2.X, weapon2.Y, 32, 32).Contains(GUIManager.mousePoint))
                    {
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            Game1.player.RemoveWeapon();
                        }
                    }
                }


                GUIManager.DrawString("Item1", "MenuFont", new Vector2(item1.X, item1.Y - 14), Color.White);
                GUIManager.DrawBox(item1, 1, new Color(33, 38, 40));

                GUIManager.DrawString("Helmet", "MenuFont", new Vector2(helmet.X, helmet.Y - 14), Color.White);
                GUIManager.DrawBox(helmet, 1, new Color(33, 38, 40));


                GUIManager.DrawString("Body Armor", "MenuFont", new Vector2(bodyArmor.X, bodyArmor.Y - 14), Color.White);
                GUIManager.DrawBox(bodyArmor, 1, new Color(33, 38, 40));

                GUIManager.DrawString("Item2", "MenuFont", new Vector2(item2.X, item2.Y - 14), Color.White);
                GUIManager.DrawBox(item2, 1, new Color(33, 38, 40));

                GUIManager.DrawBox(characterbg, new Color(42, 62, 70));
                GUIManager.DrawBox(characterRect, 2, new Color(33, 38, 40));

                GUIManager.DrawString("Stats", new Vector2(characterbg.Center.X - 20, characterbg.Bottom + 10), Color.White);

                GUIManager.DrawString("Strength", new Vector2(81, 360), Color.White);
                GUIManager.DrawString("Dexterity", new Vector2(81, 390), Color.White);
                GUIManager.DrawString("Intelligence", new Vector2(81, 420), Color.White);
                GUIManager.DrawString("Wisdom", new Vector2(81, 450), Color.White);
                GUIManager.DrawString("Charisma", new Vector2(81, 480), Color.White);


                GUIManager.DrawString(Game1.player.strength.ToString(), new Vector2(181, 360), new Color(72, 207, 164));
                GUIManager.DrawString(Game1.player.dexterity.ToString(), new Vector2(181, 390), new Color(72, 207, 164));
                GUIManager.DrawString(Game1.player.intelligence.ToString(), new Vector2(181, 420), new Color(72, 207, 164));
                GUIManager.DrawString(Game1.player.wisdom.ToString(), new Vector2(181, 450), new Color(72, 207, 164));
                GUIManager.DrawString(Game1.player.charisma.ToString(), new Vector2(181, 480), new Color(72, 207, 164));

                GUIManager.DrawTexture(Game1.player.bodyTextures[0], new Vector2(characterbg.Center.X - 5, characterbg.Center.Y - 5));
            }
        }
    }
}
