using Microsoft.Xna.Framework.Graphics;

namespace MiLib.Interfaces
{
    public interface IDraw
    {
        bool IsVisible { get; set; }

        void Render();

        void Draw(ICamera camera);
    }
}
