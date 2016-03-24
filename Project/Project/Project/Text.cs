using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Project
{
    enum Alignment
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        MiddleLeft,
        MiddleRight,
        Top,
        Bottom,
        Center
    }

    class Text
    {
        protected SpriteFont font;
        protected Vector2 pos;
        public String text { get; set; }
        public Color colr { get; set; }

        public Text(int x, int y, Color col, String str, SpriteFont sf) //Text Handler Class
        {
            pos = new Vector2(x, y);
            text = str;
            font = sf;
            colr = col;
        }

        public void DrawObject(SpriteBatch DrawFor)
        {
            DrawFor.DrawString(font, text, pos, colr);
        }

        public void setAlignment(Alignment ali, int offset, GraphicsDevice gd)
        {
            int gdWidth = gd.Viewport.Width;
            int gdHeight = gd.Viewport.Height;
            Vector2 fontMeasure = font.MeasureString(text);

            switch (ali)
            {
                case Alignment.Bottom:
                    pos.X = (gdWidth / 2) - (fontMeasure.X / 2);
                    pos.Y = gdHeight - fontMeasure.Y - offset;
                    break;
                case Alignment.BottomLeft:
                    pos.X = 0 + offset;
                    pos.Y = gdHeight - fontMeasure.Y - offset;
                    break;
                case Alignment.BottomRight:
                    pos.X = gdWidth - fontMeasure.X - offset;
                    pos.Y = gdHeight - fontMeasure.Y - offset;
                    break;
                case Alignment.Center:
                    pos.X = (gdWidth / 2) - (fontMeasure.X / 2);
                    pos.Y = (gdHeight / 2) - (fontMeasure.Y / 2);
                    break;
                case Alignment.MiddleLeft:
                    pos.X = 0 + offset;
                    pos.Y = (gdHeight / 2) - (fontMeasure.Y / 2);
                    break;
                case Alignment.MiddleRight:
                    pos.X = gdWidth - fontMeasure.X - offset;
                    pos.Y = (gdHeight / 2) - (fontMeasure.Y / 2);
                    break;
                case Alignment.Top:
                    pos.X = (gdWidth / 2) - (fontMeasure.X / 2);
                    pos.Y = 0 + offset;
                    break;
                case Alignment.TopLeft:
                    pos.X = 0 + offset;
                    pos.Y = 0 + offset;
                    break;
                case Alignment.TopRight:
                    pos.X = gdWidth - fontMeasure.X - offset;
                    pos.Y = 0 + offset;
                    break;
            }
        }

        public void modifyPosition(int x, int y)
        {
            pos.X += x;
            pos.Y += y;
        }
    }
}
