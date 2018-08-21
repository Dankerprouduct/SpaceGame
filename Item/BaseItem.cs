using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame.Item
{
    public class BaseItem
    {
        public int id;
        public int textureID;
        public string name;
        public enum ItemType
        {
            Weapon,
            Item,
            Consumable,
            Gear
        }
        public ItemType itemType; 
        public BaseItem(string _name, int _id)
        {
            this.name = _name;
            this.id = _id;
        }
        public BaseItem(string _name)
        {
            this.name = _name; 
            
        }

        public void SetId(int _id)
        {
            this.id = _id; 
        }

        public void SetItemType(ItemType itemT)
        {
            itemType = itemT;
        }
        public virtual void Update()
        {

        }

        public virtual void Draw()
        {

        }
    }
}
