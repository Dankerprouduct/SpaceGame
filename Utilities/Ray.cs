using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SpaceGame.Utilities
{
    public class Ray
    {

        public Ray()
        {
                
        }

        public bool Intersects(Vector2 origin, Vector2 target)
        {
            Point p0 = new Point((int)origin.X, (int)origin.Y);

            Point p1 = new Point((int)target.X, (int)target.Y);

            bool steep = Math.Abs(p1.Y - p0.Y) > Math.Abs(p1.X - p0.X);

            if (steep)
            {
                Point tempPoint = new Point(p0.X, p0.Y);
                p0 = new Point(tempPoint.Y, tempPoint.X);

                tempPoint = p1;
                p1 = new Point(tempPoint.Y, tempPoint.X);
            }

            int deltaX = (int)Math.Abs(p1.X - p0.X);
            int deltaY = (int)Math.Abs(p1.Y - p0.Y);
            int error = 0;
            int deltaError = deltaY;
            int yStep = 0;
            int xStep = 0;
            int x = p0.X;
            int y = p0.Y;

            if (p0.Y < p1.Y)
            {
                yStep = 4;
            }
            else
            {
                yStep = -4;
            }

            if (p0.X < p1.X)
            {
                xStep = 4;
            }
            else
            {
                yStep = -4;
            }

            int tmpX = 0;
            int tmpY = 0;

            while (x != p1.X)
            {
                x += xStep;
                error += deltaError;

                if ((2 * error) > deltaX)
                {
                    y += yStep;
                    error -= deltaX;
                }

                if (steep)
                {
                    tmpX = y;
                    tmpY = x;
                }

                if (Game1.planet.cellSpacePartition.cells[Game1.planet.cellSpacePartition.PositionToIndex(new Vector2(tmpX, tmpY))].members != null)
                {
                    for (int i = 0; i < Game1.planet.cellSpacePartition.cells[Game1.planet.cellSpacePartition.PositionToIndex(new Vector2(tmpX, tmpY))].members.Count; i++)
                    {
                        Point _p = new Point(tmpX, tmpY);
                        if (Game1.planet.cellSpacePartition.cells[Game1.planet.cellSpacePartition.PositionToIndex(new Vector2(tmpX, tmpY))].members[i].rect.Contains(_p))
                        {
                            return true;
                        }

                    }
                }

            }
            return false; 
        }
    }
}
