using Microsoft.Xna.Framework;

namespace MiLib.Interfaces
{
    public interface IUpdate
    {
        bool IsUpdating { get; set; }

        void Update(GameTime gameTime);
    }
}
