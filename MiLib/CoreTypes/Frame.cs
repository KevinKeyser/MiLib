using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiLib.CoreTypes
{
    public class Frame
    {
        private Rectangle sourceRectangle;

        public Rectangle SourceRectangle
        {
            get { return sourceRectangle; }
            set { sourceRectangle = value; }
        }

        private Vector2 origin;

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }
        public Frame(Rectangle rectangle)
            : this(rectangle, Vector2.Zero)
        { }

        public Frame(Rectangle sourceRectangle, Vector2 origin)
        {
            this.sourceRectangle = sourceRectangle;
            this.origin = origin;
        }

        public override string ToString()
        {
            return string.Format("Frame: {0}, {1}", sourceRectangle, origin);
        }

    }
}
