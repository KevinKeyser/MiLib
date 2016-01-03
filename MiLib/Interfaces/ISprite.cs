using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiLib.CoreTypes;

namespace MiLib.Interfaces
{
    public interface ISprite : IUpdate, IDraw
    {
        Color Color { get; set; }

        Vector3 Position { get; set; }

        Vector3 Rotation { get; set; }

        Vector3 Origin { get; set; }

        Vector3 Scale { get; set; }
    }
}
