using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Project
{
    class PointItem : LinearObject
    {
        public int rewardPTs;

        //AOE Circle Effect
        public Texture2D AOECircle;
        private Vector2 AOEOrigion;

        public PointItem(int x, int y, int maxX, int maxY, int rwdPoints, Random RNG, float scal, Color cl, Texture2D txr, Texture2D txrAOERing)
            : base(x, y, maxX, maxY, RNG, scal, cl, txr)
        {
            rewardPTs = rwdPoints;
            speed = speed + (float)RNG.NextDouble();

            //Setup AOE Circle
            AOECircle = txrAOERing;
            AOEOrigion = new Vector2(AOECircle.Width/2, AOECircle.Height/2);
        }

        public void isHitRemove()
        {
            colSph.Radius = AOECircle.Width / 2;
        }

        public void DrawObject(SpriteBatch DrawFor) //Draw Object
        {
            DrawFor.Draw(texture, pos, null, objColor * objAlpha, MathHelper.PiOver4, rotOrigion, objScale, SpriteEffects.None, 1);
            if (objState == FloatObjState.FadeOut)
            {
                DrawFor.Draw(AOECircle, pos, null, objColor * objAlpha, 0, AOEOrigion, objScale, SpriteEffects.None, 1);
            }
        }
    }
}
