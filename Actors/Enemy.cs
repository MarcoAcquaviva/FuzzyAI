using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace FSMc
{
    class Enemy : Actor
    {

        protected StateMachine fsm;

        public Player Rival;
        public GameObject Target;
        public float SightRadius { get; protected set; }
        public float AttackRadius { get; protected set; }
        public float HalfConeAngle { get; protected set; }

        public List<Player> VisiblePlayers { get; protected set; }

        public Enemy(Vector2 spritePosition, string textureName, DrawManager.Layer drawLayer = DrawManager.Layer.Playground, int spriteWidth = 0, int spriteHeight = 0) : base(spritePosition, textureName, drawLayer, spriteWidth, spriteHeight)
        {

            RigidBody.Type = (uint)PhysicsManager.ColliderType.Enemy;

            fsm = new StateMachine(this);
            PatrolingState ps = new PatrolingState();
            ps.PatrolY = Position.Y;
            ps.MaxX = Game.window.Width - 120;
            ps.MinX = 120;

            fsm.RegisterState((int)States.Patroling, ps);
            fsm.RegisterState((int)States.Chase, new ChaseState());
            fsm.RegisterState((int)States.Attack, new AttackState());
            fsm.RegisterState((int)States.Recharge, new RechargeState());

            fsm.Switch((int)States.Patroling);
            SightRadius = 400;
            AttackRadius = 200;
            HalfConeAngle = MathHelper.DegreesToRadians(30);//60 degrees cone
            VisiblePlayers = new List<Player>();
            Nrg = 100;
        }

        public void GoToPatrol()
        {
            Rival = null;
            //change state to Patrol
            this.fsm.Switch((int)States.Patroling);

        }

        protected bool CheckVisiblePlayers()
        {
            bool returnVal = false;
            VisiblePlayers.Clear();
            foreach (Player p in ((PlayScene)Game.CurrentScene).Players)
            {
                if (!p.IsActive)
                    continue;

                Vector2 distance = p.Position - this.Position;

                if (distance.Length <= SightRadius)//he's quite near
                {
                    Vector2 distDirection = distance.Normalized();
                    Vector2 rotationDirection = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
                    float deltaAngle = (float)Math.Acos(Vector2.Dot(distDirection, rotationDirection));

                    if (deltaAngle <= HalfConeAngle)//he's inside my field of view
                    {
                        VisiblePlayers.Add(p);
                        returnVal = true;
                    }
                }
            }

            return returnVal;
        }

        

        protected Player GetPlayerToFight()
        {
            int bestIndex = -1;
            float maxVal = float.MinValue;

            if (VisiblePlayers.Count() == 1)//the only one
                return VisiblePlayers[0];

            float fuzzySum;

            for (int i = 0; i < VisiblePlayers.Count(); i++)
            {
                fuzzySum = 0;

                Vector2 distance = VisiblePlayers[i].Position - this.Position;

                float fuzzDistance = 1 - (distance.Length / SightRadius);//near is best
                fuzzySum += fuzzDistance;

                float fuzzNrg = 1 - (VisiblePlayers[i].Nrg / VisiblePlayers[i].MaxNrg);//dying is best
                fuzzySum += fuzzNrg;

                Vector2 playerToEnemyDir = -distance.Normalized();
                Vector2 playerSightDir = new Vector2((float)Math.Cos(VisiblePlayers[i].Rotation), (float)Math.Sin(VisiblePlayers[i].Rotation));

                float deltaAngle = (float)Math.Acos(Vector2.Dot(playerSightDir, playerToEnemyDir));
                float fuzzAngle = 1 - (deltaAngle / (float)Math.PI);//the one looking at me is best

                fuzzySum += fuzzAngle;

                if (fuzzySum > maxVal)
                {
                    maxVal = fuzzySum;
                    bestIndex = i;
                }
            }

            return bestIndex != -1 ? VisiblePlayers[bestIndex] : null;
        }


        public PowerUp GetVisiblePowerUp()
        {
            PowerUp powerUp = null;
            float minDistance = float.MaxValue;
            foreach (PowerUp p in SpawnManager.PowerUps)
            {
                if (!p.IsActive)
                    continue;

                Vector2 distance = p.Position - this.Position;

                if (distance.Length <= SightRadius)//is visible
                {
                    if (distance.Length < minDistance)
                        powerUp = p;
                }
            }

            return powerUp;
        }

        public Player GetBestPlayerToFight()
        {
            if (CheckVisiblePlayers())
            {
                return GetPlayerToFight();
            }

            return null;
        }

        public override void Update()
        {
            base.Update();
            fsm.Run();
        }
    }
}
