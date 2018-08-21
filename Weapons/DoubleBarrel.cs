using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SpaceGame.Weapons
{
    class DoubleBarrel: BaseWeapon
    {

        public DoubleBarrel(string name, int id): base(name, id)
        {
            fireType = FireType.Burst; 
        }

        public override void Update(GameTime gameTime, Vector2 position, float rotation)
        {
            base.Update(gameTime, position, rotation);
        }
    }
}
