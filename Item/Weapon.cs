using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLua; 

namespace SpaceGame.Item
{
    public class Weapon: BaseItem
    {
        public int clipSize;
        public int maxAmmo;
        public int barrelSpeed; 
        public Weapon(string name): base(name)
        {
            SetItemType(ItemType.Weapon);
            Lua lua = new Lua();
            lua.DoFile("Scripts/Weapons/WeaponDictionary.lua");
            clipSize = (int)(double)lua[name + ".clipSize"]; 
            maxAmmo = (int)(double)lua[name + ".maxAmmo"];
            barrelSpeed = (int)(double)lua[name + ".speed"];
            textureID = (int)(double)lua[name + ".textureId"];
            id = (int)(double)lua[name + ".id"];
            SetId(id); 

        }

        public void FireWeapon()
        {

        }

        public override void Update()
        {

            base.Update();
        }
    }
}
