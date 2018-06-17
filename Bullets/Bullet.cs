using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMc
{
    class Bullet : GameObject
    {
        protected float ray;

        public BulletManager.BulletType Type { get; protected set; }

        public Bullet():this(Vector2.Zero,Vector2.Zero)
        {

        }

        public Bullet(Vector2 position, Vector2 velocity, String textureName="bullet") : base(position, textureName)
        {
            IsActive = false;
            Type = BulletManager.BulletType.StdBullet;

            sprite.pivot = new Vector2(sprite.Width / 2, sprite.Height / 2);
            ray = sprite.Width / 2;
          
            RigidBody = new RigidBody(sprite.position, this, null, null, true);
            RigidBody.Type = (uint)PhysicsManager.ColliderType.Bullet;
            RigidBody.SetCollisionMask((uint)(PhysicsManager.ColliderType.Player | PhysicsManager.ColliderType.Enemy));

            RigidBody.Velocity = new Vector2(velocity.X, velocity.Y);

            Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X);//not updated!

        }

        public override void Update()
        {
            base.Update();

            //Borders collisions check

            if (IsActive)
            {
                if (sprite.position.Y + ray <= 0)//TOP
                {
                    BulletManager.RestoreBullet(this);
                }
                else if (sprite.position.Y - ray >= Game.window.Height)//BOTTOM
                {
                    BulletManager.RestoreBullet(this);

                }

                else if (sprite.position.X - ray >= Game.window.Width)//RIGHT
                {
                    BulletManager.RestoreBullet(this);

                }
                else if (sprite.position.X + ray <= 0)//LEFT
                {
                    BulletManager.RestoreBullet(this);

                }
            }

        }


        public override void OnCollide(GameObject other)
        {

            if (other is Actor)
            {
                BulletManager.RestoreBullet(this);
            }
        }
    }
}
