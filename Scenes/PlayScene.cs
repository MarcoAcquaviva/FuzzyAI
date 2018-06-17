using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMc
{
    class PlayScene : Scene
    {

        //public Player CurrentPlayer { get; private set; }

        public List<Player> Players { get; protected set; }
        public List<Enemy> Enemies { get; protected set; }

        public override void Start()
        {
            base.Start();
            GfxManager.Init();

            GfxManager.AddTexture("player1", Game.ASSETS_PATH + "player_1.png");
            GfxManager.AddTexture("player2", Game.ASSETS_PATH + "player_2.png");
            GfxManager.AddTexture("enemy0", Game.ASSETS_PATH + "enemy_0.png");
            GfxManager.AddTexture("enemy3", Game.ASSETS_PATH + "enemy_3.png");
            GfxManager.AddTexture("bg", Game.ASSETS_PATH + "hex_grid_green.png");
            GfxManager.AddTexture("bullet", Game.ASSETS_PATH + "fireball.png");
            GfxManager.AddTexture("powerup", Game.ASSETS_PATH + "heart.png");

            GfxManager.AddTexture("loadingBar_bar", Game.ASSETS_PATH + "loadingBar_bar.png");
            GfxManager.AddTexture("loadingBar_frame", Game.ASSETS_PATH + "loadingBar_frame.png");

            UpdateManager.Init();
            DrawManager.Init();
            PhysicsManager.Init();
            BulletManager.Init();
            //SpawnManager.Init();


            //Texture bgTExture = GfxManager.GetTexture("bg");

            //backGround = new GameObject(new Vector2(-bgTExture.Width, -600), "bg");
            //backGround.IsCullingAffected = false;
            //TilingTexture tl = new TilingTexture(backGround, bgTExture, 2);
            GameObject bg = new GameObject(Vector2.Zero, "bg", DrawManager.Layer.Background);

            Players = new List<Player>();
            Enemies = new List<Enemy>();


            Player player = new Player("player1", new Vector2(900, 600));
            Player player2 = new Player("player2", new Vector2(100, 100));

            player2.UP = KeyCode.W;
            player2.DOWN = KeyCode.S;
            player2.RIGHT = KeyCode.D;
            player2.LEFT = KeyCode.A;
            player2.FIRE = KeyCode.F;

            //Enemy enemy0 = new Enemy(new Vector2(100, 200), "enemy0");
            //Enemy enemy3 = new Enemy(new Vector2(500, 400), "enemy3");
            Load();

            Players = new List<Player>();

            Players.Add(player);
            Players.Add(player2);

        }
        public override void Draw()
        {
            DrawManager.Draw();
        }

        public override void Input()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i].IsActive)
                    Players[i].Input();
            }
        }

        public override void Update()
        {
            PhysicsManager.Update();
            UpdateManager.Update();
            PhysicsManager.CheckCollisions();
            SpawnManager.Update();
        }

        public override void OnExit()
        {
            UpdateManager.RemoveAll();
            DrawManager.RemoveAll();
            PhysicsManager.RemoveAll();
            GfxManager.RemoveAll();
        }

        private void Create()
        {
            string path = @"Config.txt";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Name,X,Y");
                    sw.WriteLine("enemy0,100,200");
                    sw.WriteLine("enemy3,500,400");
                }
            }
        }

        private Vector2 Position(string name)
        {
            string path = @"Config.txt";
            var lines = File.ReadAllLines(path);
            Vector2 position;

            for (int i = 0; i < lines.Length; i++)
            {
                var fields = lines[i].Split(',');
                for (int j = 0; j < fields.Length; j++)
                {
                    if (fields[0] == name)
                    {
                        float X = float.Parse(fields[1]);
                        float Y = float.Parse(fields[2]);
                        position = new Vector2(X, Y);
                        break;
                    }

                }
            }

            return Vector2.Zero;
        }

        private void Load()
        {
            try
            {
                string path = @"Config.txt";

                var lines = File.ReadAllLines(path);

                for (int i = 0; i < lines.Length; i++)
                {
                    var fields = lines[i].Split(',');
                    for (int j = 0; j < fields.Length; j++)
                    {
                        if (i != 0)
                        {
                            float X = float.Parse(fields[1]);
                            float Y = float.Parse(fields[2]);
                            Vector2 position = new Vector2(X, Y);
                            Enemy enemy = new Enemy(position, fields[0]);
                            Enemies.Add(enemy);
                        }
                    }
                }

            }
            catch (FileNotFoundException)
            {
                Create();
                Load();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
