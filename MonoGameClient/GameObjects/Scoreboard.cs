﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CommonData;
using Engine.Engines;

namespace GameComponentNS
{
   public class Scoreboard : DrawableGameComponent
    {
        public List<PlayerData> players = new List<PlayerData>();
        SpriteBatch batch;
        SpriteFont font;
        Vector2 position;
        public bool visible = false;

        public Scoreboard(List<PlayerData> p,SpriteBatch sb,SpriteFont sf,Vector2 pos,Game g) : base(g)
        {
            g.Components.Add(this);
            players = p;
            batch = sb;
            font = sf;
            position = pos;
        }
        public override void Update(GameTime gameTime)
        {
            if (InputEngine.IsKeyHeld(Keys.Tab))
            {
                visible = true;
            }
            else visible = false;
            players = players.OrderBy(o => o.Score).ToList();
            base.Update(gameTime);

        }

        public override void Draw(GameTime gameTime)
        {
            if (!visible) return;
            batch.Begin(SpriteSortMode.BackToFront, BlendState.Additive);
            for (int i = 0; i < players.Count; i++)
            {
                PlayerData temp = players.ElementAt(i);
                batch.DrawString(font, string.Format("{0,-15}{1,0}", temp.GamerTag, temp.Score.ToString()), new Vector2(100, 100 + (20 * i)), Color.White);
            }
            batch.End();
            base.Update(gameTime);
        }

    }
}
