using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame.Item
{
    class Item: BaseItem
    {
        public Item(string name, int id): base(name, id)
        {
            SetItemType(ItemType.Item);
        }
    }
}
