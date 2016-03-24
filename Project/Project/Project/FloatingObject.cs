using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project
{
    enum FloatObjState
    {
        Active,
        Standby,
        Hit,
        FadeOut,
        HitRemove
    }

    class FloatingObject
    {
        protected Vector2 pos, velocity, rotOrigion;
        protected Vector3 posCenter;
        protected Texture2D texture;
        protected float speed, rotation;
        
        public FloatObjState objState { get; set; }
        public Color objColor { get; set; }

        public BoundingSphere colSph;
        protected float SphScale, objScale;

        public FloatingObject(int x, int y, Random RNG, float scal, Color cl, Texture2D txr) //Class Init
        {
            objScale = scal;
            SphScale = 1.20f * scal;

            //Load Texture
            texture = txr;
            
            //Set Color
            objColor = cl;

            //Speed
            speed = (float)RNG.NextDouble();

            //Render Center
            posCenter = new Vector3(-100, -100, 0);

            colSph = new BoundingSphere(new Vector3(pos, 0), txr.Width / 2);
            colSph.Radius *= SphScale;

            //Vel
            velocity = Vector2.Zero;

            //Others
            rotOrigion = new Vector2(txr.Width / 2, txr.Height / 2);
            rotation = 0;
        }

        //Draw Normal With Rotation
        public void DrawObject(SpriteBatch DrawFor)
        {
            DrawFor.Draw(texture, pos, null, objColor, rotation, rotOrigion, objScale, SpriteEffects.None, 1);


            /* DEBUG SECTION - Shows Destination Rectangles
            switch (objState)
            {
                case MineState.Active:
                    DrawFor.Draw(texture, pos, Color.White * 0.4f);
                    DrawFor.Draw(texture, destPos, Color.Red * 0.3f);
                    break;
                case MineState.Standby:
                    DrawFor.Draw(texture, pos, Color.Red);
                    DrawFor.Draw(texture, destPos, Color.Red * 0.3f);
                    break;
            }*/
        }

        //End Class
    }
}
