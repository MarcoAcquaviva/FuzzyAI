using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMc
{
    class GameObject:IUpdatable,IDrawable, IActivable
    {
        protected Sprite sprite;
        protected Texture texture;
        protected DrawManager.Layer layer;

        public Vector2 Velocity {
            get { return (RigidBody != null ? RigidBody.Velocity : Vector2.Zero); }
            set { if (RigidBody != null) RigidBody.Velocity = value; }
        }

        public RigidBody RigidBody { get; protected set; }
        public virtual int Width { get { return (int)(sprite.Width * sprite.scale.X); } }
        public virtual int Height { get { return (int)(sprite.Height * sprite.scale.Y); } }
        public virtual Vector2 Position { get { return RigidBody!=null?RigidBody.Position:sprite.position; }
            set { sprite.position = value;
                if(RigidBody!=null)
                    RigidBody.Position = value;
            }
        }
        public float X { get { return sprite.position.X; } set { sprite.position.X = value; } }
        public float Y { get { return sprite.position.Y; } set { sprite.position.Y = value; } }

        public DrawManager.Layer Layer { get { return layer; } }

        public bool IsActive { get; set; }

        public float Rotation { get { return sprite.Rotation; } set { sprite.Rotation = value; } }

        public GameObject()
        {
            //position.X = 0;
            //position.Y = 0;
        }

        public GameObject(Vector2 spritePosition, string textureName, DrawManager.Layer drawLayer = DrawManager.Layer.Playground, int spriteWidth=0, int spriteHeight=0)
        {
            texture = GfxManager.GetTexture(textureName);
            sprite = new Sprite(spriteWidth>0 ? spriteWidth : texture.Width, spriteHeight>0 ? spriteHeight : texture.Height);
            sprite.position = spritePosition;
            layer = drawLayer;
            IsActive = true;
            UpdateManager.AddItem(this);
            DrawManager.AddItem(this);
        }

        
        public GameObject(Sprite spriteRef)
        {
            sprite = spriteRef;
        }

        public virtual void Destroy()
        {
            IsActive = false;
            UpdateManager.RemoveItem(this);
            DrawManager.RemoveItem(this);
            if (RigidBody != null)
            {
                PhysicsManager.RemoveItem(RigidBody);
            }
        }

        public bool IsMoving()
        {
            return (RigidBody != null && RigidBody.Velocity != Vector2.Zero);
        }

        public void Translate(float deltaX, float deltaY)
        {
            sprite.position.X += deltaX;
            sprite.position.Y += deltaY;
        }

        public void SetSprite(Sprite newSprite)
        {
            sprite = newSprite;
        }

        public Sprite GetSprite()
        {
            return sprite;
        }

        public virtual void Draw()
        {
            if(IsActive)
                sprite.DrawTexture(texture);
        }

        public virtual void Update()
        {
            if (IsActive && RigidBody !=null)
            {
                sprite.position = RigidBody.Position;
            }
        }

        public void SetAdditiveTint(Vector4 color)
        {
            sprite.SetAdditiveTint(color);

        }

        public virtual void LookAt(Vector2 position)
        {
            Vector2 direction = position - Position;

            sprite.Rotation = (float)Math.Atan2(direction.Y, direction.X);
        }

        public virtual void OnCollide(GameObject other)
        {

        }
    }
}
