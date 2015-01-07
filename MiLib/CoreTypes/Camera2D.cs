using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiLib.CoreTypes
{
    public class Camera2D
    {
        Viewport view;
        float zoom;
        float rotation;
        Vector2 origin;
        Rectangle? limits;
        Vector2 position;

        public Viewport View
        {
            get
            {
                return view;
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                if (limits.HasValue)
                {
                    checkPosition();
                }
            }
        }
        public float Zoom
        {
            get
            {
                return zoom;
            }
            set
            {
                zoom = value;
                if (limits.HasValue)
                {
                    checkZoom();
                    checkPosition();
                }
                origin = new Vector2(view.Width / 2, view.Height / 2);
            }
        }
        public float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = MathHelper.ToRadians(value%360);
            }
        }
        public Rectangle? Limits
        {
            get
            {
                return limits;
            }
            set
            {
                if (value.HasValue)
                {
                    if (value.Value.Width >= view.Width &&
                        value.Value.Height >= view.Height)
                    {
                        limits = value;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("Limits set value of Height and Width must be greater than Viewport");
                    }
                }
                else
                {
                    limits = value;
                }
                checkZoom();
                checkPosition();
            }
        }

        public Camera2D(GraphicsDevice graphicsDevice)
        {
            zoom = 1f;
            view = graphicsDevice.Viewport;
            origin = new Vector2(view.Width / 2, view.Height / 2);
        }
        public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-position, 0.0f)) *
                Matrix.CreateScale(zoom, zoom, 1) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateTranslation(new Vector3(origin, 0.0f));
        }

        public Matrix GetViewMatrix(float layer)
        {
            return Matrix.CreateTranslation(new Vector3(-position * new Vector2(layer), 0.0f)) *
                Matrix.CreateScale(zoom, zoom, 1)*
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateTranslation(new Vector3(origin, 0.0f));
        }

        private void checkPosition()
        {
            if (limits.HasValue)
            {
                Vector2 cameraSize = new Vector2(view.Width/2, view.Height/2) / zoom;
                Vector2 limitWorldMin = new Vector2(limits.Value.Left, limits.Value.Top);
                Vector2 limitWorldMax = new Vector2(limits.Value.Right, limits.Value.Bottom);
                position = Vector2.Clamp(position, limitWorldMin + cameraSize, limitWorldMax - cameraSize);
            }
        }

        private void checkZoom()
        {
            if (limits.HasValue)
            {
                float minZoomX = (float)view.Width / limits.Value.Width;
                float minZoomY = (float)view.Height / limits.Value.Height;
                zoom = MathHelper.Max(zoom, MathHelper.Max(minZoomX, minZoomY));
            }
        }
    }
}
