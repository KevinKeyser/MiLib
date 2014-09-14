using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MiLib.CoreTypes;

namespace MiLib.Interfaces
{
    public interface ISprite : IPosition, IUpdate, IDraw
    {
        Vector2 Position { get; set; }

        Rectangle Bounds { get; set; }

        Color Color { get; set; }
        
        Rotation Rotation { get; set; }

        Vector2 Origin { get; set; }

        Vector2 Scale { get; set; }

        SpriteEffects Effect { get; set; }

        float Layer { get; set; }

    }
}
