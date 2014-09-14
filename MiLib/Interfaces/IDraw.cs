using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiLib.Interfaces
{
    public interface IDraw
    {
        bool isVisible { get; set; }

        void Draw(SpriteBatch spriteBatch);
    }
}
