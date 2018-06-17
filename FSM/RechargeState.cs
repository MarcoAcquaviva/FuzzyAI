using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMc
{
    class RechargeState : State
    {
        protected float speed = 80.0f;

        public override void Enter()
        {
            machine.Owner.Rival = null;
        }

        public override void Update()
        {
            if (machine.Owner.Target != null && machine.Owner.Target.IsActive)
            {
                Vector2 distance = machine.Owner.Target.Position - machine.Owner.Position;
                if (distance.Length > machine.Owner.SightRadius)
                    machine.Owner.GoToPatrol();
                else
                {
                    machine.Owner.Velocity = distance.Normalized() * speed;
                }
            }
            else
            {
                machine.Owner.GoToPatrol();

            }
        }

        public override void Exit()
        {
            machine.Owner.Target = null;
        }
    }
}
