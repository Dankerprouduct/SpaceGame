using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SpaceGame.Weapons
{
    public class MachineGun: BaseWeapon
    {
        public MachineGun(string name, int id): base(name, id)
        {
            fireType = FireType.Rapid;
        }

        public override void Update(GameTime gameTime, Vector2 position, float rotation)
        {
            base.Update(gameTime, position, rotation);
        }
    }
}
