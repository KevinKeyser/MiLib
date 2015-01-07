using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiLib.Interfaces
{
    public interface IParent : IPosition
    {
        float X { get; set; }
        float Y { get; set; }
    }
}
