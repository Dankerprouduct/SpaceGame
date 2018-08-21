using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame.Item
{
    public class Consumable : BaseItem
    {
        public Consumable(string name, int id): base(name, id)
        {
            SetItemType(ItemType.Consumable);
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
