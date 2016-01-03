using System;
using Microsoft.Xna.Framework;

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
