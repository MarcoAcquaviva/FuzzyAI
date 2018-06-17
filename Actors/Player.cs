using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMc
{
    class Player : Actor
    {
        protected float shootDelay;
        protected int joystickIndex;
        

        protected float speed = 200.0f;

        protected bool isFirePressed;

        public KeyCode UP { get; set; }
        public KeyCode RIGHT { get; set; }
        public KeyCode LEFT { get; set; }
        public KeyCode DOWN { get; set; }
        public KeyCode FIRE { get; set; }


        public Player(string fileName, Vector2 spritePosition) : base(spritePosition, fileName)
        {
            //sprite.scale = new Vector2(0.3f, 0.3f);
            RigidBody.Type = (uint)PhysicsManager.ColliderType.Player;

            speed = 350;
            shootDelay = 0.45f;
            Nrg = 100;
            joystickIndex = 0;

            UP = KeyCode.Up;
            RIGHT = KeyCode.Right;
            DOWN = KeyCode.Down;
            LEFT = KeyCode.Left;
            FIRE = KeyCode.Space;
        }


        public void Input()
        {
            //shootCounter -= Game.DeltaTime;

            if (Game.NumJoysticks > 0)
            {
                Vector2 axis = Game.window.JoystickAxisLeft(joystickIndex);

                RigidBody.Velocity = axis * speed;

            }
            else
            {

                if (Game.window.GetKey(RIGHT))
                {
                    RigidBody.SetXVelocity(speed);
                }
                else if (Game.window.GetKey(LEFT))
                {
                    RigidBody.SetXVelocity(-speed);
                }
                else
                {
                    RigidBody.SetXVelocity(0);
                }

                if (Game.window.GetKey(UP))
                {
                    RigidBody.SetYVelocity(-speed);
                }
                else if (Game.window.GetKey(DOWN))
                {
                    RigidBody.SetYVelocity(speed);

                }
                else
                {
                    RigidBody.SetYVelocity(0);
                }

                if (Game.window.GetKey(FIRE))
                {
                    if (!isFirePressed)
                    {
                        isFirePressed = true;
                        Shoot();
                    }
                }
                else if (isFirePressed)
                {
                    isFirePressed = false;
                }
            }
        }

    }
}
