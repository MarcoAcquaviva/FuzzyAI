using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace FSMc
{
    abstract class Actor:GameObject
    {
        protected float nrg;
        protected Bar bar;
        protected float shootSpeed;

        public float MaxNrg { get; protected set; }

        public float Nrg { get { return nrg; } set { SetNrg(value); } }

        public Actor(Vector2 spritePosition, string textureName, DrawManager.Layer drawLayer = DrawManager.Layer.Playground, int spriteWidth = 0, int spriteHeight = 0) : base(spritePosition, textureName, drawLayer, spriteWidth, spriteHeight)
        {
            sprite.pivot = new Vector2(Width / 2, Height / 2);
            MaxNrg = 100;
            shootSpeed = 100;
            Rotation = 1.57f;

            RigidBody = new RigidBody(sprite.position, this);
            RigidBody.SetCollisionMask((uint)PhysicsManager.ColliderType.Bullet);
            RigidBody.AddCollision((uint)PhysicsManager.ColliderType.PowerUp);

            bar = new Bar(new Vector2(Position.X - RigidBody.BoundingCircle.Ray, Position.Y - RigidBody.BoundingCircle.Ray), "loadingBar_bar", MaxNrg);
            bar.BarOffset = new Vector2(4, 4);
            bar.SetValue(Nrg);
            bar.IsActive = true;

        }

        public override void Update()
        {
            base.Update();
            if (Velocity.Length > 0)
            {
                Vector2 rotVector = new Vector2((float)Math.Cos(sprite.Rotation), (float)Math.Sin(sprite.Rotation));
                Vector2 velocityDir = Velocity.Normalized();

                if ((rotVector - velocityDir).Length > 0.01f)
                {
                    rotVector = Vector2.Lerp(rotVector, velocityDir, Game.DeltaTime*10);
                    sprite.Rotation = (float)Math.Atan2(rotVector.Y, rotVector.X);
                }
            }

            bar.Position=new Vector2(Position.X - bar.Width / 2, Position.Y - RigidBody.BoundingCircle.Ray - bar.Height * 2);
        }

        protected virtual void SetNrg(float newValue)
        {
            nrg = newValue;
            if (nrg > MaxNrg)
            {
                nrg = MaxNrg;
                
            }
            bar.SetValue(nrg);
        }

        public void Shoot()
        {
            Vector2 direction = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
            Vector2 position = Position + direction * (RigidBody.BoundingCircle.Ray + 20.0f);
            //Bullet b = new Bullet(position, direction * shootSpeed);
            Bullet b = BulletManager.GetBullet(BulletManager.BulletType.StdBullet);
            if (b != null)
            {
                b.Position = position;
                b.Velocity = direction * shootSpeed;
                b.Rotation = this.Rotation;
                b.IsActive = true;
            }
        }

        protected virtual void OnDie()
        {
            Nrg = 0;
            this.Destroy();
            this.bar.Destroy();
        }

        protected bool AddDmg(float dmg)
        {
            Nrg -= dmg;
            if (Nrg <= 0)
            {
                OnDie();
                return true;
            }

            return false;
        }

        public override void OnCollide(GameObject other)
        {
             if (other is Bullet)
            {
                this.AddDmg(5.0f);
            }
            else if (other is PowerUp)
            {
                Nrg += 25;
                if (Nrg > MaxNrg)
                    Nrg = MaxNrg;
                bar.SetValue(Nrg);
            }
        }
    }
}
