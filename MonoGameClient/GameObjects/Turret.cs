using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Turrets
{
    public class Turret : DrawableGameComponent
    {
        public Projectile Projectile;
        Texture2D _tx;
        public Vector2 position;
        public float angle = 0f;
        Vector2 origin;

        //======================================================================================================

        public Turret(Texture2D texture, Game g,Vector2 pos) : base(g)
        {
            g.Components.Add(this);
            _tx = texture;
            position = pos;
            origin = new Vector2(_tx.Width/2 + position.X,_tx.Height / 2+position.Y);
        }

        public override void Update(GameTime gameTime)
        {
            origin = new Vector2(_tx.Width / 2 + position.X/2, _tx.Height / 2 + position.Y/2);
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = Game.Services.GetService<SpriteBatch>();
            if (sb == null) return;
            sb.Begin();
            sb.Draw(_tx, position, null, Color.White, angle, origin, 1f, SpriteEffects.None, 0);
            sb.End();
            base.Draw(gameTime);
        }
        //======================================================================================================

        //public override void Update(GameTime gametime, List<Sprite> sprites)
        //{
        //    _previousKey = _currentKey;
        //    _currentKey = Keyboard.GetState();


        //    if (_currentKey.IsKeyDown(Keys.A)) _rotation -= MathHelper.ToRadians(RotationSpeed);
        //    else if (_currentKey.IsKeyDown(Keys.D)) _rotation += MathHelper.ToRadians(RotationSpeed);

        //    Direction = new Vector2((float)Math.Cos(_rotation), (float)Math.Sin(_rotation));

        //    if (_currentKey.IsKeyDown(Keys.W))
        //        Position += Direction * Speed;

        //    if (_currentKey.IsKeyDown(Keys.Space) &&
        //            _previousKey.IsKeyUp(Keys.Space))
        //    {
        //        AddProjectile(sprites);
        //    }

        //}

        ////======================================================================================================

        //private void AddProjectile(List<Sprite>sprites)
        //    { 
        //                var projectile = Projectile.Clone() as Projectile;
        //                projectile.Direction = this.Direction;
        //                projectile.Position = this.Position;
        //                projectile.Speed = this.Speed * 2;
        //                projectile.Alive = 2f;

        //                sprites.Add(projectile);
        //     }

        //    }
    }
}


