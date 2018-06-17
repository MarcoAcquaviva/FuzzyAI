using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace FSMc
{
    class PowerUp : GameObject
    {
        public PowerUp(Vector2 position, string textureName="powerup", int width = 0, int height = 0) : base(position, textureName, DrawManager.Layer.Middleground, width, height)
        {
            this.sprite.pivot = new Vector2(Width / 2, Height / 2);
            RigidBody = new RigidBody(position, this,null,null,true);
            RigidBody.Type = (uint)PhysicsManager.ColliderType.PowerUp;
            RigidBody.SetCollisionMask((uint)PhysicsManager.ColliderType.Player | (uint)PhysicsManager.ColliderType.Enemy);
        }

        public override void OnCollide(GameObject other)
        {
            if(other is Actor)
            {
                SpawnManager.RemovePowerUp(this);
                this.Destroy();
            }
        }
    }
}
