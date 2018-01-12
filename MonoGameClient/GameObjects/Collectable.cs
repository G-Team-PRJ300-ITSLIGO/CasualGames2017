using CameraNS;
using CommonData;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectables
{
    class Collectable : DrawableGameComponent
    {
        public Position position { get; set; }
        public Rectangle bounds { get; set; }
        public Texture2D texture { get; set; }
        public bool InCollision { get; set; }
        public CollectableData collectableData;
        public bool Visible = true;

        public Collectable(Game game, CollectableData cData, Texture2D Texture, Position userPosition) : base(game)
        {
            game.Components.Add(this);
            position = userPosition;
            texture = Texture;
            collectableData = cData;
            bounds = new Rectangle((int)this.position.X, (int)this.position.Y, this.texture.Width, this.texture.Height);

        }



        public bool collisionDetect()
        {
            SimplePlayerSprite otherSprite = (SimplePlayerSprite)Game.Components.FirstOrDefault(pl => pl.GetType() == typeof(SimplePlayerSprite));

            if (!Visible) return false;

            Rectangle otherBound = new Rectangle((int)otherSprite.Position.X, (int)otherSprite.Position.Y, otherSprite.Image.Width, otherSprite.Image.Height);
            if (bounds.Intersects(otherBound))
            {
                InCollision = true;
                return true;
            }
            else
            {
                InCollision = false;
                return false;
            }
        }

        public override void Update(GameTime gameTime)
        {

            if(Visible)
            {
                if (collisionDetect())
                {
                    SimplePlayerSprite otherSprite = (SimplePlayerSprite)Game.Components.FirstOrDefault(pl => pl.GetType() == typeof(SimplePlayerSprite));
                    Visible = false;
                    IHubProxy proxy = Game.Services.GetService<IHubProxy>();
                    proxy.Invoke("Collected", new Object[]
                    {
                    otherSprite.pData,
                    collectableData});

                }
            }
            
           base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sp = Game.Services.GetService<SpriteBatch>();
            SpriteFont font = Game.Services.GetService<SpriteFont>();

            if (sp == null) return;
            if (texture != null && Visible)
            {
                sp.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Camera.CurrentCameraTranslation);
                sp.Draw(texture, bounds, Color.White);
                sp.DrawString(font, collectableData.worth.ToString(), new Vector2(position.X + 20, position.Y - (texture.Height / 4)), Color.White);
                sp.End();
            }

            base.Draw(gameTime);
        }


    }
}
