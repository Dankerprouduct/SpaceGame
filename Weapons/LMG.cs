using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SpaceGame.Weapons
{
    public class LMG: BaseWeapon
    {
        public LMG(string name, int id): base(name, id)
        {
            this.fireType = FireType.Rapid;
        }

        public override void Update(GameTime gameTime, Vector2 position, float rotation)
        {
            base.Update(gameTime, position, rotation);
        }
    }
}
