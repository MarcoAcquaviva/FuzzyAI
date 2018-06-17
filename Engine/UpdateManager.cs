using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMc
{
    static class UpdateManager
    {
        static List<IUpdatable> items;
        static List<IUpdatable> itemsToRemove;

        public static void Init()
        {
            items = new List<IUpdatable>();
            itemsToRemove = new List<IUpdatable>();
        }

        public static void AddItem(IUpdatable item)
        {
            items.Add(item);
        }

        public static void RemoveItem(IUpdatable item)
        {
            //items.Remove(item);
            itemsToRemove.Add(item);
        }
        public static void RemoveAll()
        {
            items.Clear();
        }

        public static void Update()
        {
            if (itemsToRemove.Count > 0)
            {
                for (int i = 0; i < itemsToRemove.Count; i++)
                {
                    items.Remove(itemsToRemove[i]);
                }
                itemsToRemove.Clear();
            }

            for (int i = 0; i < items.Count; i++)
            {
                items[i].Update();
            }
        }
    }
}
