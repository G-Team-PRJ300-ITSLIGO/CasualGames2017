using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Sprites
{
    public class SimpleProjectile
    {
        public Texture2D Image;
        public Rectangle BoundingRect;
        public Vector2 Position;
        public float Speed;
        public Vector2 direction;
        public bool visible;

        public SimpleProjectile(Texture2D image, Vector2 pos, float s, float r)
        {
            Image = image;
            Position = pos;
            Speed = s;
            direction = new Vector2((float)Math.Cos(r),
                                  (float)Math.Sin(r));
            direction.Normalize();
            BoundingRect = new Rectangle((int)Position.X, (int)Position.Y, Image.Width, Image.Height);
        }

        public void Update(GameTime gameTime)
        {
            if (!visible) return;
            BoundingRect = new Rectangle((int)Position.X, (int)Position.Y, Image.Width, Image.Height);
            Position += direction * Speed;
        }
    }
}
