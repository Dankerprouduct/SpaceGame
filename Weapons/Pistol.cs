using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceGame.Weapons
{
    public class Pistol: BaseWeapon
    {
        public Pistol(string name, int id): base(name, id)
        {
            fireType = FireType.Single;
        }

        public override void Update(GameTime gameTime, Vector2 position, float rotation)
        {
            
            base.Update(gameTime, position, rotation);
        }
        
    }
}
