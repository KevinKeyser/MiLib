using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiLib.UserInterface
{
    public class UIDraggedEventArgs : EventArgs
    {
        public Vector2 MoveAmount;

        public UIDraggedEventArgs(Vector2 moveAmount)
            : base ()
        {
            MoveAmount = moveAmount;
        }
    }
}
