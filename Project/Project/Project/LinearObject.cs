using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Project
{
    class LinearObject : FloatingObject
    {
        private Vector2 destPos;
        private Rectangle destBox;
        private int startWall;

        private const int dTol = 15;

        //FadeFunc
        public float objAlpha { get; set; }
        private float objAplhaInc = 0.1f;
        private double objFadeDelay;

        public LinearObject(int x, int y, int maxX, int maxY, Random RNG, float scal, Color cl, Texture2D txr)
            :base (x, y, RNG, scal, cl, txr)
        {
            //Setup Fixed
            objAlpha = 1f;
            objFadeDelay = 0.35;

            //Setup Start and Destination
            int tolerance = 50;
            startWall = RNG.Next(1, 5); //L R U D

            switch (startWall)
            {
                case 1:
                    //Left
                    pos = new Vector2(-tolerance, RNG.Next(-tolerance, maxY + tolerance));
                    destPos = new Vector2(maxX + tolerance, RNG.Next(-tolerance, maxY + tolerance));
                    break;
                case 2:
                    //Right
                    pos = new Vector2(maxX + tolerance, RNG.Next(-tolerance, maxY + tolerance));
                    destPos = new Vector2(-tolerance, RNG.Next(-tolerance, maxY + tolerance));
                    break;
                case 3:
                    //Up
                    pos = new Vector2(RNG.Next(-tolerance, maxX + tolerance), -tolerance);
                    destPos = new Vector2(RNG.Next(-tolerance, maxX + tolerance), maxY + tolerance);
                    break;
                case 4:
                    //Down
                    pos = new Vector2(RNG.Next(-tolerance, maxX + tolerance), maxY + tolerance);
                    destPos = new Vector2(RNG.Next(-tolerance, maxX + tolerance), -tolerance);
                    break;
            }

            destBox = new Rectangle((int)destPos.X, (int)destPos.Y, 1, 1);
        }

        public void UpdateObject()
        {
            //Debug.WriteLine(this.ToString() + " " + colSph.Center + " - " + (pos + (new Vector2(texture.Width, texture.Height) / 2)));

            //Location
            switch (startWall) //L R U D
            {
                case 1:
                    if (pos.X >= destPos.X - dTol)
                        this.objState = FloatObjState.Standby;
                    break;
                case 2:
                    if (pos.X <= destPos.X + dTol)
                        this.objState = FloatObjState.Standby;
                    break;
                case 3:
                    if (pos.Y >= destPos.Y - dTol)
                        this.objState = FloatObjState.Standby;
                    break;
                case 4:
                    if (pos.Y <= destPos.Y + dTol)
                        this.objState = FloatObjState.Standby;
                    break;
            }

            //Pos Update
            if (objState == FloatObjState.Active)
            {
                //Ptah Update
                velocity.X = this.destBox.Center.X - this.colSph.Center.X;
                velocity.Y = this.destBox.Center.Y - this.colSph.Center.Y;
                velocity.Normalize();

                velocity *= speed;

                pos += velocity;
            }

            rotation += speed / 25 * objScale;

            //Update Collision Sph
            UpdatateCollisionSph();

        } //End Update Obj

        public void FadeUpdate(GameTime gt)
        {
            objFadeDelay -= gt.ElapsedGameTime.TotalSeconds;
            if (objFadeDelay < 0)
            {
                objFadeDelay = 0.10;
                objAlpha -= objAplhaInc;
            }
        }

        public void UpdatateCollisionSph()
        {
            colSph.Center = new Vector3(pos, 0);
        }

        public Vector2 getPos()
        {
            return pos;
        }

        public void DrawObject(SpriteBatch DrawFor)
        {
            DrawFor.Draw(texture, pos, null, objColor * objAlpha, rotation, rotOrigion, objScale, SpriteEffects.None, 1);
        }
    }
}
