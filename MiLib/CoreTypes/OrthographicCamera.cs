using Microsoft.Xna.Framework;
using MiLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiLib.CoreTypes
{
    public class OrthographicCamera : ICamera
    {
        Matrix projection;
        public Matrix Projection
        {
            get
            {
                return projection;
            }
        }


        Matrix view;
        public Matrix View
        {
            get
            {
                return view;
            }
        }

        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                view = Matrix.CreateLookAt(new Vector3(position, -10), new Vector3(position, 0), new Vector3((float)Math.Sin(rotation + Math.PI), (float)Math.Cos(rotation + Math.PI), 0));
            }
        }

        private float rotation;

        public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                view = Matrix.CreateLookAt(new Vector3(position, -10), new Vector3(position, 0), new Vector3((float)Math.Sin(rotation + Math.PI), (float)Math.Cos(rotation + Math.PI), 0));
            }
        }

        private float zoom;

        public float Zoom
        {
            get { return zoom; }
            set
            {
                zoom = value;
                projection = Matrix.CreateOrthographic(size.X * zoom, size.Y * zoom, 1, 100);
            }
        }

        private Vector2 size;

        public Vector2 Size
        {
            get { return size; }
            set
            {
                size = value;
                projection = Matrix.CreateOrthographic(size.X * zoom, size.Y * zoom, 1, 100);
            }
        }


        public OrthographicCamera(Vector2 size)
        {
            zoom = 1;
            rotation = 0;
            Size = size;
            Position = Vector2.Zero;
        }

        public Vector3 Unproject(Sprite sprite)
        {
            return WindowManager.GraphicsDevice.Viewport.Unproject(sprite.Position, projection, view, sprite.World) + new Vector3(size/2, 0);
        }

        public Vector3 Project(Vector3 position)
        {
            return WindowManager.GraphicsDevice.Viewport.Unproject(position, projection, view, Matrix.Identity);
        }
    }
}