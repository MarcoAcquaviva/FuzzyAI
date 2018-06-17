using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMc
{
    static class PhysicsManager
    {
        public enum ColliderType: uint { Player=1, Enemy=2, Bullet=4, PowerUp = 8}
        static List<RigidBody> items;
        static List<RigidBody> itemsToRemove;

        public static void Init()
        {
            items = new List<RigidBody>();
            itemsToRemove = new List<RigidBody>();
        }


        public static void AddItem(RigidBody item)
        {
            items.Add(item);
        }

        public static void RemoveItem(RigidBody item)
        {
            //items.Remove(item);
            itemsToRemove.Add(item);
        }
        public static void RemoveAll()
        {
            items.Clear();
        }

        private static void DeleteItemsToRemove()
        {
            if (itemsToRemove.Count > 0)
            {
                for (int i = 0; i < itemsToRemove.Count; i++)
                {
                    items.Remove(itemsToRemove[i]);
                }
                itemsToRemove.Clear();
            }

        }

        public static void Update()
        {
            DeleteItemsToRemove();

            for (int i = 0; i < items.Count; i++)
            {
                if(items[i].GameObject.IsActive)
                    items[i].Update();
            }
        }

        public static void CheckCollisions()
        {
            DeleteItemsToRemove();

            for (int i = 0; i < items.Count-1; i++)
            {
                if(items[i].GameObject.IsActive && items[i].IsCollisionsAffected)
                {
                    for (int j = i+1; j < items.Count; j++)
                    {
                        if(items[j].GameObject.IsActive && items[j].IsCollisionsAffected)
                        {
                            bool checkFirst = items[i].CheckCollisionWith(items[j]);
                            bool checkSecond = items[j].CheckCollisionWith(items[i]);

                            if ((checkFirst || checkSecond) && items[i].Collides(items[j]))
                            {
                                if(checkFirst)
                                    items[i].GameObject.OnCollide(items[j].GameObject);
                                if(checkSecond)
                                    items[j].GameObject.OnCollide(items[i].GameObject);
                            }
                        }
                    }
                }
            }
        }
    }
}
