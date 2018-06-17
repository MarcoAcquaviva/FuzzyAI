using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace FSMc
{
    static class SpawnManager
    {
        static float spawnCounter;
        public static List<PowerUp> PowerUps { get; private set; }

        static SpawnManager()
        {
            spawnCounter = 10;
            PowerUps = new List<PowerUp>();
        }

        public static void AddPowerUp(PowerUp p)
        {
            PowerUps.Add(p);
        }

        public static void RemovePowerUp(PowerUp p)
        {
            PowerUps.Remove(p);
        }


        public static void Update()
        {
            spawnCounter -= Game.window.deltaTime;

            if (spawnCounter <= 0)
            {
                spawnCounter = RandomGenerator.GetRandom(8, 12);
                PowerUp p = new PowerUp(new Vector2(RandomGenerator.GetRandom(40, Game.window.Width - 40), RandomGenerator.GetRandom(40, Game.window.Height - 40)));
                AddPowerUp(p);
            }
        }
    }
}
