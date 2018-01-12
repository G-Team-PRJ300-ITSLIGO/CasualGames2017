using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CommonData;
using Engine.Engines;
using Microsoft.Xna.Framework.Input;
using Microsoft.AspNet.SignalR.Client;

namespace Sprites
{
    public class SimplePlayerSprite :DrawableGameComponent
    {
        public Texture2D Image;
        public Vector2 Position;
        public Rectangle BoundingRect;
        public Rectangle DestRect;
        public bool Visible = true;
        public Color tint = Color.White;
		public PlayerData pData;
        public Vector2 previousPosition;		
        public int speed = 5;
        public float delay = 500;
        public float rotation;
        public float previousRoation;
        public Turret turret;
        public bool fired;
        public Vector2 origin;
        public Game g;


        // Constructor epects to see a loaded Texture
        // and a start position
        public SimplePlayerSprite(Game game, PlayerData data, Texture2D spriteImage,
                            Texture2D turretImage,Texture2D projectileImage,Point startPosition) :base(game)
        {
            g = game;
            pData = data;
            DrawOrder = 1;
            game.Components.Add(this);
            // Take a copy of the texture passed down
            Image = spriteImage;
            // Take a copy of the start position
            previousPosition = Position = (startPosition.ToVector2());
            // Calculate the bounding rectangle
            BoundingRect = new Rectangle((int)Position.X, (int)Position.Y, Image.Width, Image.Height);
            turret = new Turret(Position,turretImage,projectileImage, game);
            origin = new Vector2(Image.Width / 2, Image.Height / 2);

        }

        public override void Update(GameTime gameTime)
        {
            if (!Visible) return;
            turret.BoundingRect = new Rectangle((int)Position.X, (int)Position.Y, turret._tx.Width, turret._tx.Height);

            BoundingRect.X = BoundingRect.X + Image.Width / 2;
            BoundingRect.Y = BoundingRect.Y + Image.Height / 2;

            Vector2 direction = new Vector2((float)Math.Cos(rotation),
                                  (float)Math.Sin(rotation));
            direction.Normalize();
            turret.previousRotation = turret.rotation;
            if (InputEngine.IsKeyHeld(Keys.A))
                turret.rotation -= 0.05f;
            if (InputEngine.IsKeyHeld(Keys.D))
                turret.rotation += 0.05f;

            previousPosition = Position;
            if(InputEngine.IsKeyHeld(Keys.Up))
                Position += direction * speed;
            if (InputEngine.IsKeyHeld(Keys.Down))
                Position -= direction * speed;
            previousRoation = rotation;
            if (InputEngine.IsKeyHeld(Keys.Left))
                rotation -= 0.05f;
            if (InputEngine.IsKeyHeld(Keys.Right))
                rotation += 0.05f;
            delay -= gameTime.ElapsedGameTime.Milliseconds;

            if (InputEngine.IsKeyPressed(Keys.Space) && delay <= 0)
            {
                delay = 500;
                fired = true;
                turret.CreateProjectile(Position,pData.GamerTag);
                

            }
            if(turret.projectiles.Count > 0)
                foreach (SimpleProjectile p in turret.projectiles)
                {
                    p.Update(gameTime);
                    if (p.CollisionDetect(g))
                        p.visible = false;

                }
            //foreach (SimpleProjectile p in turret.projectiles)
            //{
            //    if (!p.visible)
            //        turret.projectiles.Remove(p);
            //    break;
            //}

            // if we have moved pull back the proxy reference and send a message to the hub
            if (Position != previousPosition || rotation != previousRoation || turret.rotation != turret.previousRotation)
            {
                pData.playerPosition = new Position { X = (int)Position.X, Y = (int)Position.Y,angle = rotation,TurretAngle = turret.rotation };
                IHubProxy proxy = Game.Services.GetService<IHubProxy>();
                proxy.Invoke("Moved", new Object[] 
                {
                    pData.playerID,
                    pData.playerPosition,
                });
            }
            if (fired)
            {
                pData.playerPosition.HasFired = true;
                IHubProxy proxy = Game.Services.GetService<IHubProxy>();
                proxy.Invoke("Fired", new Object[]
                {
                    pData.playerID,
                    pData.playerPosition
            });
                pData.playerPosition.HasFired = false;
                fired = false;
            }

            BoundingRect = new Rectangle((int)Position.X, (int)Position.Y, Image.Width, Image.Height);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sp = Game.Services.GetService<SpriteBatch>();
            if (sp == null) return;
            if (Image != null && Visible)
            {
                sp.Begin();
                sp.Draw(Image, BoundingRect, null, Color.White, rotation, origin, SpriteEffects.None, 0);
                if (turret.projectiles.Count > 0)
                    foreach (SimpleProjectile p in turret.projectiles)
                    {
                        if (p.visible)
                        sp.Draw(p.Image, p.BoundingRect, Color.White);
                    }
                sp.Draw(turret._tx, turret.BoundingRect, null, Color.White, turret.rotation, turret.origin, SpriteEffects.None, 1);
              
                sp.End();
            }

            base.Draw(gameTime);
        }



        
    }
}
