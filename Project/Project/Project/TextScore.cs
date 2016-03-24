using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project
{
    class TextScore : Text
    {
        public TextScore(int x, int y, Color col, String str, SpriteFont sf) //Text Handler for Score
            :base (x, y, col, str, sf)
        {

        }

        public void DrawObject(SpriteBatch DrawFor, int score1)
        {
            DrawFor.DrawString(font, text + score1, pos, colr);
        }

        public void setAlignment(Alignment ali, int offset, int score, GraphicsDevice gd)
        {
            int gdWidth = gd.Viewport.Width;
            int gdHeight = gd.Viewport.Height;
            Vector2 fontMeasure = font.MeasureString(text + score + "  ");

            switch (ali)
            {
                case Alignment.Bottom:
                    pos.X = gdWidth - (fontMeasure.X / 2);
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
                    pos.X = gdWidth - (fontMeasure.X / 2);
                    pos.Y = gdHeight - (fontMeasure.Y / 2);
                    break;
                case Alignment.MiddleLeft:
                    pos.X = 0 + offset;
                    pos.Y = gdHeight - (fontMeasure.Y / 2);
                    break;
                case Alignment.MiddleRight:
                    pos.X = gdWidth - fontMeasure.X - offset;
                    pos.Y = gdHeight - (fontMeasure.Y / 2);
                    break;
                case Alignment.Top:
                    pos.X = gdWidth - (fontMeasure.X / 2);
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
    }
}
