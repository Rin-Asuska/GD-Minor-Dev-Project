using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project
{
    enum ExplosionState
    {
        Active,
        Expended
    }

    class Explosion : FloatingObject
    {
        public float objAlpha { get; set; }
        private float objAplhaInc = 0.1f;
        private double objFadeDelay, timeout;

        private Boolean maxedFade;

        ExplosionState objState;

        public Explosion(int x, int y, double Fadedelay, Random RNG, float scal, Color cl, Texture2D txr)
            : base(x, y, RNG, scal, cl, txr)
        {
            pos = new Vector2(x + RNG.Next(-15, 16), y + RNG.Next(-15, 16));

            //Setup Alpha Timeout
            objAlpha = 0f;
            objFadeDelay = Fadedelay;
            timeout = Fadedelay;
            objAplhaInc = 0.2f + (RNG.Next(0, 6) / 10f);

            maxedFade = false;

            objState = ExplosionState.Active;
        }

        public void FadeUpdate(GameTime gt)
        {
            objFadeDelay -= gt.ElapsedGameTime.TotalSeconds;
            if (objFadeDelay < 0)
            {
                objFadeDelay = timeout;
                objAlpha += objAplhaInc;
            }

            if ((objAlpha >= 1f) && (!maxedFade))
            {
                objAplhaInc = -objAplhaInc;
                maxedFade = true;
            }

            if (maxedFade && objAlpha <= 0f)
            {
                objState = ExplosionState.Expended;
            }

        }

        public void DrawObject(SpriteBatch DrawFor)
        {
            DrawFor.Draw(texture, pos, null, objColor * objAlpha, rotation, rotOrigion, objScale, SpriteEffects.None, 1);
        }
    }
}
