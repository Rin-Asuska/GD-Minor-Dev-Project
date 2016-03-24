using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Project
{
    enum Move
    {
        Up,
        Down,
        Left,
        Right
    }

    class Player
    {
        public Vector2 pos;
        private Vector3 posCenter;
        public Texture2D texture;
        //private Rectangle colBox;
        public BoundingSphere colSph;
        private Color objColor;
        float spd = 1.5f;
        public int score { get; set; }
        public float scoreMultiplier { get; set; }
        private int moveBorderTop, moveBorderBottom, moveBorderLeft, moveBorderRight;

        public Player(int x, int y, Color clr, Texture2D txr) //Class Init
        {
            texture = txr;
            pos = new Vector2(x, y);
            posCenter = new Vector3(x, y, 0);
            objColor = clr;
            score = 0;
            scoreMultiplier = 1.0f;
            SetMoveBorders(0, 0, 0, 0);
            
            //colBox = new Rectangle(x, y, txr.Width, txr.Height);
            colSph = new BoundingSphere(new Vector3(pos, 0), txr.Width / 2 + 0.5f);

            //Update Collision
            UpdatateCollisionSph();
        }

        //Movements Updates
        public void UpdateObject(float moveX, float moveY)
        {
            //Movements Etc
            if (pos.X >= moveBorderLeft && pos.X <= moveBorderRight)
                pos.X += spd * moveX;
            if (pos.Y >= moveBorderTop && pos.Y <= moveBorderBottom)
                pos.Y += spd * -moveY;

            //Update Collision
            UpdatateCollisionSph();
        }
        public void UpdateObject(Move move)
        {
            Debug.WriteLine(this.ToString() + " " + colSph.Center + " - " + (pos + (new Vector2(texture.Width, texture.Height) / 2)));
            //Movements Etc
            switch (move)
            {
                case Move.Left:
                    if (pos.X >= moveBorderLeft)
                        pos.X -= spd;
                    break;
                case Move.Right:
                    if (pos.X <= moveBorderRight)
                        pos.X += spd;
                    break;
                case Move.Up:
                    if (pos.Y >= moveBorderTop)
                        pos.Y -= spd;
                    break;
                case Move.Down:
                    if (pos.Y <= moveBorderBottom)
                        pos.Y += spd;
                    break;
            }

            //Update Collision
            UpdatateCollisionSph();
        } //END UpdateObject

        public void ResetObj(int x, int y)
        {
            pos.X = x;
            pos.Y = y;

            //Update Collision
            UpdatateCollisionSph();
        }

        public void UpdatateCollisionSph()
        {
            //Update Collision METHOD
            posCenter.X = pos.X + (texture.Width / 2f);
            posCenter.Y = pos.Y + (texture.Height / 2f);
            colSph.Center = posCenter;
        }

        public void SetMoveBorders(int left, int right, int up, int down)
        {
            moveBorderLeft = left;
            moveBorderRight = right - texture.Width;
            moveBorderTop = up;
            moveBorderBottom = down - texture.Height;
        }

        public void DrawObject(SpriteBatch DrawFor)
        {
            DrawFor.Draw(texture, pos, objColor);
        }

        //End Class Player
    }
}
