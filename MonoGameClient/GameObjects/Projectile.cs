using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Turrets
{
    public class Projectile : DrawableGameComponent
    {
        private float _timer;
        private float Alive = 3f;
        private float angle;
        bool visible = true;
        private int Speed = 4;
        private Vector2 Direction;
        private Vector2 Position;

        public Projectile (Texture2D texture,float t,Game g,float a,Vector2 pos) : base(g)
        {
            _timer = 0f;
            Alive = t;
            angle = a;

        }

        public override void Update(GameTime gametime)
        {
            if (!visible) return;
            _timer += (float)gametime.ElapsedGameTime.TotalSeconds;

            if (_timer > Alive)
                visible = false;

            Position += Direction * Speed;
            base.Update(gametime);
        }
    }
}
