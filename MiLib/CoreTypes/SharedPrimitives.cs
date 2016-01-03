using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiLib.CoreTypes
{
    public static class SharedPrimitives
    {
        private static VertexBuffer plane;
        public static VertexBuffer Plane
        {
            get
            {
                if(plane == null)
                {
                    VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[4];

                    vertices[0] = new VertexPositionColorTexture(new Vector3(0f, 0f, 0), Color.White, new Vector2(1, 1));
                    vertices[1] = new VertexPositionColorTexture(new Vector3(1f, 0f, 0), Color.White, new Vector2(0, 1));
                    vertices[2] = new VertexPositionColorTexture(new Vector3(0f, 1f, 0), Color.White, new Vector2(1, 0));
                    vertices[3] = new VertexPositionColorTexture(new Vector3(1f, 1f, 0), Color.White, new Vector2(0, 0));

                    plane = new VertexBuffer(WindowManager.GraphicsDevice, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.WriteOnly);
                    plane.SetData<VertexPositionColorTexture>(vertices);
                }
                return plane;
            }
        }

    }
}
