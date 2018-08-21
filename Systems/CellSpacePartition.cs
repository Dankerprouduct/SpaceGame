using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 

namespace SpaceGame.Systems
{
    public class CellSpacePartition
    {

        public struct Cell
        {
            public List<Entity.BaseGameEntity> members;
            
            public void AddEntity(Entity.BaseGameEntity ent)
            {
                if (members != null)
                {
                    members.Add(ent);
                }
                else
                {
                    members = new List<Entity.BaseGameEntity>
                    {
                        ent
                    };
                }
            }

            public void RemoveEntity(Entity.BaseGameEntity ent)
            {
                if (members != null)
                {
                    for (int i = 0; i < members.Count; i++)
                    {
                        if(members[i].mID == ent.mID)
                        {
                            members.RemoveAt(i);
                        }
                    }
                }
            }

            public void Update(GameTime gameTime)
            {
                if (members != null)
                {
                    for (int i = 0; i < members.Count; i++)
                    {
                        members[i].Update(gameTime);
                    }
                }
            }
                        

            public void Draw(SpriteBatch spriteBatch)
            {
                if (members != null)
                {
                    for (int i = 0; i < members.Count; i++)
                    {

                        spriteBatch.Draw(
                            Universe.Planet.tempMonster,
                            members[i].rect,
                            new Rectangle(0, 0, Universe.Planet.tempMonster.Width, Universe.Planet.tempMonster.Height),
                            Color.White,
                            members[i].rotation,
                            new Vector2(Universe.Planet.tempMonster.Width / 2, Universe.Planet.tempMonster.Height / 2),
                            SpriteEffects.None, 
                            0f);
                        members[i].Draw(spriteBatch);
                    }
                }
            }
        }

        private int partitionSize;
        public int cellLength;
        private int numCellsX;
        private int numCellsY;

        public Cell[] cells;

        public int mm; 

        public CellSpacePartition(int cellsX, int cellsY, int partitionSize)
        {
            cellsX = cellsX / partitionSize;
            cellsY = cellsY / partitionSize;
            this.partitionSize = partitionSize;


            int cellIndex = cellsX * cellsY;
            cellLength = cellIndex;
            cells = new Cell[cellIndex];
            Console.WriteLine("CELLS X: " + cellsX);
            Console.WriteLine("CELLS Y: " + cellsY);
            Console.WriteLine("CELL Length " + cellIndex);
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = new Cell();
            }

            numCellsX = cellsX;
            numCellsY = cellsY;
        }

        public void Update(GameTime gameTime)
        {
            mm = PositionToIndex(Game1.player);
            //Vector2 pos = Game1.player.position;
            cells[mm].Update(gameTime); 
            // left right
            if(mm - 40 >= 0)
            {
                cells[mm - 40].Update(gameTime);
            }
            if(mm + 40 < cellLength)
            {
                cells[mm + 40].Update(gameTime);
                
            }
            // corners
            if (mm - 40 - 1>= 0)
            {
                cells[mm - 40 - 1].Update(gameTime);
            }

            if (mm - 40 + 1 >= 0)
            {
                cells[mm - 40 + 1].Update(gameTime);
            }

            if (mm + 40 + 1< cellLength)
            {
                cells[mm + 40 + 1].Update(gameTime);

            }
            if (mm + 40 - 1 < cellLength)
            {
                cells[mm + 40 - 1].Update(gameTime);

            }
            // up down
            if (mm - 1 >= 0)
            {
                cells[mm - 1].Update(gameTime);
            }
            if (mm + 1 < cellLength)
            {
                cells[mm + 1].Update(gameTime);
            }
        }

        public void ClearCell(int x)
        {
            cells[x].members = new List<Entity.BaseGameEntity>();
        }

        public bool CheckCollision(Rectangle rect)
        {
            for (int i = 0; i < cells[mm].members.Count; i++)
            {
                if (cells[mm].members[i].propertyID == 2)
                {
                    if (cells[mm].members[i].rect.Intersects(rect))
                    {
                        return true;
                    }
                }
            }
            if (mm - 1 >= 0)
            {
                if (cells[mm - 1].members != null)
                {
                    for (int i = 0; i < cells[mm - 1].members.Count; i++)
                    {
                        if (cells[mm - 1].members[i].propertyID == 2)
                        {
                            if (cells[mm - 1].members[i].rect.Intersects(rect))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            if (mm + 1 < cellLength)
            {
                if (cells[mm + 1].members != null)
                {
                    for (int i = 0; i < cells[mm + 1].members.Count; i++)
                    {
                        if (cells[mm + 1].members[i].propertyID == 2)
                        {
                            if (cells[mm + 1].members[i].rect.Intersects(rect))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            if (mm - 40 >= 0)
            {
                if (cells[mm - 40].members != null)
                {
                    for (int i = 0; i < cells[mm - 40].members.Count; i++)
                    {
                        if (cells[mm - 40].members[i].propertyID == 2)
                        {
                            if (cells[mm - 40].members[i].rect.Intersects(rect))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            if (mm + 40 < cellLength)
            {
                if (cells[mm + 40].members != null)
                {
                    for (int i = 0; i < cells[mm].members.Count; i++)
                    {
                        if (cells[mm + 40].members[i].propertyID == 2)
                        {
                            if (cells[mm + 40].members[i].rect.Intersects(rect))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            
            cells[mm].Draw(spriteBatch);

            if (mm - 40 >= 0)
            {
                cells[mm - 40].Draw(spriteBatch);
            }
            if (mm + 40 < cellLength)
            {
                cells[mm + 40].Draw(spriteBatch);

            }
            // corners
            if (mm - 40 - 1 >= 0)
            {
                cells[mm - 40 - 1].Draw(spriteBatch);
            }

            if (mm - 40 + 1 >= 0)
            {
                cells[mm - 40 + 1].Draw(spriteBatch);
            }

            if (mm + 40 + 1 < cellLength)
            {
                cells[mm + 40 + 1].Draw(spriteBatch);

            }
            if (mm + 40 - 1 < cellLength)
            {
                cells[mm + 40 - 1].Draw(spriteBatch);

            }

            if (mm - 1 >= 0)
            {
                cells[mm - 1].Draw(spriteBatch); 
            }
            if (mm + 1 < cellLength)
            {
                cells[mm + 1].Draw(spriteBatch);
            }
        }  

        public void DrawTiles(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < cells[mm].members.Count; i++)
            {
                spriteBatch.Draw(Universe.Planet.tileTextures[cells[mm].members[i].propertyID],
                    cells[mm].members[i].position, Color.White); 
            }

            if (mm - 40 >= 0)
            {
                if (cells[mm - 40].members != null)
                {
                    for (int i = 0; i < cells[mm - 40].members.Count; i++)
                    {
                        spriteBatch.Draw(Universe.Planet.tileTextures[cells[mm - 40].members[i].propertyID],
                            cells[mm - 40].members[i].position, Color.White);
                    }
                }
            }
            if (mm + 40 < cellLength)
            {
                if (cells[mm + 40].members != null)
                {
                    for (int i = 0; i < cells[mm + 40].members.Count; i++)
                    {
                        spriteBatch.Draw(Universe.Planet.tileTextures[cells[mm + 40].members[i].propertyID],
                            cells[mm + 40].members[i].position, Color.White);
                    }
                }
            }
            if (mm - 1 >= 0)
            {
                if (cells[mm - 1].members != null)
                {
                    for (int i = 0; i < cells[mm - 1].members.Count; i++)
                    {
                        spriteBatch.Draw(Universe.Planet.tileTextures[cells[mm - 1].members[i].propertyID],
                            cells[mm - 1].members[i].position, Color.White);
                    }
                }

            }
            if (mm + 1 < cellLength)
            {
                if (cells[mm + 1].members != null)
                {
                    for (int i = 0; i < cells[mm + 1].members.Count; i++)
                    {
                        spriteBatch.Draw(Universe.Planet.tileTextures[cells[mm + 1].members[i].propertyID],
                            cells[mm + 1].members[i].position, Color.White);
                    }
                }
            }

            

            // corners
            if (mm - 40 + 1 >= 0)
            {
                if (cells[mm - 40 + 1].members != null)
                {
                    for (int i = 0; i < cells[mm - 40 + 1].members.Count; i++)
                    {
                        spriteBatch.Draw(Universe.Planet.tileTextures[cells[mm - 40 + 1].members[i].propertyID],
                            cells[mm - 40 + 1].members[i].position, Color.White);
                    }
                }
            }
            

            if (mm + 40 + 1 < cellLength)
            {
                if (cells[mm + 40 + 1].members != null)
                {
                    for (int i = 0; i < cells[mm + 40 + 1].members.Count; i++)
                    {
                        spriteBatch.Draw(Universe.Planet.tileTextures[cells[mm + 40 + 1].members[i].propertyID],
                            cells[mm + 40 + 1].members[i].position, Color.White);
                    }
                }

            }


            if (mm + 40 - 1 < cellLength)
            {
                if (cells[mm + 40 - 1].members != null)
                {
                    for (int i = 0; i < cells[mm + 40 - 1].members.Count; i++)
                    {
                        spriteBatch.Draw(Universe.Planet.tileTextures[cells[mm + 40 - 1].members[i].propertyID],
                            cells[mm + 40 - 1].members[i].position, Color.White);
                    }
                }

            }

            if (mm - 40 - 1 >= 0)
            {
                if (cells[mm - 40 - 1].members != null)
                {
                    for (int i = 0; i < cells[mm - 40 - 1].members.Count; i++)
                    {
                        
                        spriteBatch.Draw(Universe.Planet.tileTextures[cells[mm - 40 - 1].members[i].propertyID],
                            cells[mm - 40 - 1].members[i].position, Color.White);
                            
                    }
                }

            }



        }

        public void AddEntity(Entity.BaseGameEntity entity)
        {
            int cellX = (int)(entity.GetEntityPosition().X / (partitionSize * 128) / partitionSize);
            int cellY = (int)(entity.GetEntityPosition().Y / (partitionSize * 128) / partitionSize);

            //Console.WriteLine("Adding entity to partition: "+ cellX + " " + cellY); 
            entity.SetPartitionCell(cellX, cellY);

            cells[PositionToIndex(entity)].AddEntity(entity);

        }

        public void ChangeCell(Entity.BaseGameEntity ent)
        {
            //Console.WriteLine()
            if (cells[ent.cellIndex].members != null)
            {
                cells[ent.cellIndex].members.Remove(ent);
                int i = PositionToIndex(ent);
                if (i >= 0 && i < cellLength)
                {
                    cells[PositionToIndex(ent)].AddEntity(ent);
                }
            }
        }

        public int PositionToIndex(Entity.BaseGameEntity ent)
        {
            int cellX = (int)(ent.GetEntityPosition().X / (partitionSize * 128) / partitionSize);
            int cellY = (int)(ent.GetEntityPosition().Y / (partitionSize * 128) / partitionSize);

            return cellX * numCellsX + cellY;
        }

        public int PositionToIndex(Vector2 pos)
        {
            int cellX = (int)(pos.X / (partitionSize * 128) / partitionSize);
            int cellY = (int)(pos.Y / (partitionSize * 128) / partitionSize);

            return cellX * numCellsX + cellY;
        }


    }
}
