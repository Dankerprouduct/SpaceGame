using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame.Item
{
    public class Gear: BaseItem
    {
        public Gear(string name, int id): base(name, id)
        {
            SetItemType(ItemType.Gear);
        }

        public override void Update()
        {

            base.Update(); 
        }
    }
}
