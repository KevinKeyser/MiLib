using Microsoft.Xna.Framework;
using MiLib.CoreTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiLib.Interfaces
{
    public interface ICamera
    {
        Matrix Projection { get; }
        Matrix View { get; }

        Vector3 Unproject(Sprite sprite);
        Vector3 Project(Vector3 position);
    }
}
