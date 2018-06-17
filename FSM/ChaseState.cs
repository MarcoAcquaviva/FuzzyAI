using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMc
{
    class ChaseState:State
    {
        protected float speed = 80.0f;

        public override void Enter()
        {
            machine.Owner.SetAdditiveTint(new Vector4(0.5f, 0.5f, 0, 0));//yellow
        }

        public override void Update()
        {
            machine.Owner.Rival = machine.Owner.GetBestPlayerToFight();

            if (machine.Owner.Rival != null && machine.Owner.Rival.IsActive)
            {
                Vector2 distance = machine.Owner.Rival.Position - machine.Owner.Position;
                if (distance.Length > machine.Owner.SightRadius)
                    machine.Owner.GoToPatrol();
                else if (distance.Length <= machine.Owner.AttackRadius)
                    machine.Switch((int)States.Attack);
                else
                {
                    machine.Owner.RigidBody.Velocity = distance.Normalized() * speed;
                }
            }
            else
            {
                machine.Owner.GoToPatrol();
            }
        }
    }
}
