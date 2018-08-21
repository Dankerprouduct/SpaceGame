using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLua; 

namespace SpaceGame.Weapons
{
    public static class WeaponHelper
    {



        public static BaseWeapon GetWeapon(string name)
        {
            Lua lua = new Lua();
            lua.DoFile("Scripts/Weapons/WeaponDictionary.lua");
            int id = (int)(double)lua[name + ".id"];
            switch (id)
            {
                case 0:
                    {
                        return new DoubleBarrel("DoubleBarrel", id);                        
                    }
                case 1:
                    {
                        return new LMG("LMG", id);                         
                    }
                case 2:
                    {
                        return new Pistol("Pistol", id);
                    }
                case 3:
                    {
                        return new LaserGun("LaserGun", id);
                    }
                case 4:
                    {
                        return new MachineGun("MachineGun", id); 
                    }
                case 5:
                    {
                        return new Revolver("Revolver", id); 
                    }
                case 6:
                    {
                        return new SparkerCannon("SparkerCannon", id); 
                    }
                    
            }
            return new BaseWeapon("Pistol", 0); 
        }
        
    }
}
