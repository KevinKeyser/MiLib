using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace MiLib.UserInterface
{
    public class TextButton : Button
    {
        private int pad = 10;
        public virtual Color TextColor
        {
            get;
            set;
        }
        public virtual SpriteFont Font
        {
            get;
            set;
        }
        private string text = "";
        public virtual string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }
        public override Vector2 Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                if (Up.Width + value.X - pad >= Font.MeasureString(text).X)
                {
                    base.Size = new Vector2(value.X, Size.Y);
                }
                else
                {
                    base.Size = new Vector2(-Up.Width + Font.MeasureString(text).X + pad, Size.Y);
                }
                if (Up.Height + value.Y - pad >= Font.MeasureString(text).Y)
                {
                    base.Size = new Vector2(Size.X, value.Y);
                }
                else
                {
                    base.Size = new Vector2(Size.X, -Up.Width + Font.MeasureString(text).Y + pad);
                }
            }
        }

        public TextButton(Texture2D up, Texture2D down, Texture2D hover, Texture2D disabled, Color buttonColor, SpriteFont font, string text, Color textColor) :
            base(up, down, hover, disabled, buttonColor)
        {
            TextColor = textColor;
            Font = font;
            this.text = text;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(Font, text, position + new Vector2((Size.X) / 2, (Size.Y) / 2), TextColor, 0f, Font.MeasureString(text) / new Vector2(2), 1f, SpriteEffects.None, 0f);

        }
    }
}

