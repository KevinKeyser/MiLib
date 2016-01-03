using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiLib.UserInterface
{
    public class TextCharacter
    {
        public string character = "";
        public Color color = Color.Black;
        public SpriteFont font;

        public TextCharacter(SpriteFont font, string character, Color color)
        { this.character = character; this.font = font; this.color = color; }
    }
}
