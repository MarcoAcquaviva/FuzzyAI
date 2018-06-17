using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMc
{
    class PatrolingState:State
    {
        public float PatrolY { get; set; }
        public float MaxX { get; set; }
        public float MinX { get; set; }
        protected float speed = 55.0f;

        public override void Enter()
        {
            machine.Owner.SetAdditiveTint(new Vector4(0, 0, 0, 0));//black
        }


        public override void Update()
        {
            Player newEnemy = machine.Owner.GetBestPlayerToFight();

            if (newEnemy != null)
            {
                machine.Owner.Rival = newEnemy;
                this.machine.Switch((int)States.Chase);
                return;
            }

            float delta = this.machine.Owner.Position.Y - PatrolY;
            if (Math.Abs(delta) > 4.0f)//go to patrol Y
            {
                this.machine.Owner.RigidBody.SetYVelocity(Math.Sign(-delta) * speed);
            }
            else
            {//patroling...
                this.machine.Owner.RigidBody.SetYVelocity(0);

                if (this.machine.Owner.Position.X >= MaxX)
                    this.machine.Owner.RigidBody.SetXVelocity(-speed);
                else if (this.machine.Owner.Position.X <= MinX)
                    this.machine.Owner.RigidBody.SetXVelocity(speed);
                else if (!this.machine.Owner.IsMoving())
                    this.machine.Owner.RigidBody.SetXVelocity(speed);
            }
        }
    }
}
