using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Project
{
    class StaticImage
    {
        Texture2D texture;
        Vector2 pos;
        Color ObjColor;

        public StaticImage(int x, int y, Color col, Texture2D img) //Setup
        {
            texture = img;
            ObjColor = col;
            pos = new Vector2(x, y);
        }

        public void DrawObject(SpriteBatch DrawFor) //Draw
        {
            DrawFor.Draw(texture, pos, ObjColor);
        }

        public void setAlignment(Alignment ali, GraphicsDevice gd) //ALign Item
        {
            int gdWidth = gd.Viewport.Width;
            int gdHeight = gd.Viewport.Height;
            Vector2 item = new Vector2(texture.Width, texture.Height);

            switch (ali)
            {
                case Alignment.Bottom:
                    pos.X = (gdWidth / 2) - (item.X / 2);
                    pos.Y = gdHeight - item.Y;
                    break;
                case Alignment.BottomLeft:
                    pos.X = 0;
                    pos.Y = gdHeight - item.Y;
                    break;
                case Alignment.BottomRight:
                    pos.X = gdWidth - item.X;
                    pos.Y = gdHeight - item.Y;
                    break;
                case Alignment.Center:
                    pos.X = (gdWidth / 2) - (item.X / 2);
                    pos.Y = (gdHeight / 2) - (item.Y / 2);
                    break;
                case Alignment.MiddleLeft:
                    pos.X = 0;
                    pos.Y = (gdHeight / 2) - (item.Y / 2);
                    break;
                case Alignment.MiddleRight:
                    pos.X = gdWidth - item.X;
                    pos.Y = (gdHeight / 2) - (item.Y / 2);
                    break;
                case Alignment.Top:
                    pos.X = (gdWidth / 2) - (item.X / 2);
                    pos.Y = 0;
                    break;
                case Alignment.TopLeft:
                    pos.X = 0;
                    pos.Y = 0;
                    break;
                case Alignment.TopRight:
                    pos.X = gdWidth - item.X;
                    pos.Y = 0;
                    break;
            }
        }

        public void modifyPosition(int x, int y)
        {
            pos.X += x;
            pos.Y += y;
        }

        public Texture2D getTexture()
        {
            return texture;
        }
    }
}
