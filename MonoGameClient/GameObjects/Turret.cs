using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Engines;
using Microsoft.Xna.Framework.Input;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Sprites
{
  public class Turret
    {

        public Texture2D _tx;
        public float rotation = 0.0f;
        public float previousRotation;
        public Vector2 origin;
        //public SimpleProjectile projectile = null;

        public Rectangle BoundingRect;

        public Turret(Vector2 p,Texture2D t,Game g)
        {
            _tx = t;
           BoundingRect = BoundingRect = new Rectangle((int)p.X, (int)p.Y, _tx.Width, _tx.Height);
            origin = new Vector2(_tx.Width / 2, _tx.Height / 2);
            previousRotation = rotation;
        }

        //public void CreateProjectile()
        //{
        //    SimpleProjectile temp = new SimpleProjectile(_tx, origin, 5f, rotation);
        //    projectile = temp;
        //}

    }
}
