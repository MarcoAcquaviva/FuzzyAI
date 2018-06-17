using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMc
{
    class AttackState : State
    {
        protected float nextShoot;
        protected float counter;
        const float WAIT = 2;
        public override void Enter()
        {
            machine.Owner.SetAdditiveTint(new Vector4(0.8f, 0, 0, 0));//red
            machine.Owner.Velocity = Vector2.Zero;
            nextShoot = 0.3f;
            counter = WAIT;
        }

        public override void Update()
        {
            machine.Owner.Rival = machine.Owner.GetBestPlayerToFight();

            if (machine.Owner.Rival != null && machine.Owner.Rival.IsActive)
            {
                Vector2 distance = machine.Owner.Rival.Position - machine.Owner.Position;
                if (distance.Length > machine.Owner.AttackRadius + 50.0f)
                    machine.Owner.GoToPatrol();
                else
                {
                    //look at enemy
                    float distanceRot = (float)Math.Atan2(distance.Y, distance.X);

                    machine.Owner.Rotation = distanceRot;

                    //attack!
                    nextShoot -= Game.window.deltaTime;
                    if (nextShoot <= 0.0f)
                    {
                        machine.Owner.Shoot();
                        nextShoot = RandomGenerator.GetRandom(1, 5);
                    }

                    //search for visible powerups
                    if (machine.Owner.Nrg / machine.Owner.MaxNrg < 0.5f)
                    {
                        counter -= Game.window.deltaTime;
                        if (counter <= 0)
                        {
                            counter = WAIT;
                            PowerUp p = machine.Owner.GetVisiblePowerUp();
                            if (p != null)
                            {
                                float fuzzyAttack = MathHelper.Clamp(machine.Owner.Nrg / machine.Owner.Rival.Nrg, 0.0f, 1.0f);
                                float fuzzyPowerUp = 1 - (p.Position - machine.Owner.Position).Length / machine.Owner.SightRadius;

                                if (fuzzyPowerUp > fuzzyAttack)
                                {
                                    machine.Owner.Target = p;
                                    machine.Switch((int)States.Recharge);
                                }

                            }
                        }
                    }


                }

            }
            else
            {
                machine.Owner.GoToPatrol();
            }
        }
    }
}
