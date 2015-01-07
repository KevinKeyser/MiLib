using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiLib.CoreTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiLib.UserInterface
{
    public class CameraPanel : Panel
    {
        
        protected Camera2D camera;
        public Camera2D Camera
        {
            get
            {
                return camera;
            }
        }
        private float zoom;
        public float Zoom
        {
            get
            {
                return zoom;
            }
            set
            {
                zoom = value;
                if (camera != null)
                {
                    camera.Zoom = value;
                }
                foreach (UIComponent component in components)
                {
                    component.Parent = this;
                }
            }
        }

        private Vector2 cameraPosition;
        public Vector2 CameraPosition
        {
            get
            {
                return cameraPosition;
            }
            set
            {
                cameraPosition = value;
                if (camera != null)
                {
                    camera.Position = value;
                }
            }
        }

        public override float X
        {
            get
            {
                return base.X;// -CameraPosition.X;// + camera.View.Width// / 2;// / camera.Zoom;
            }
            set
            {
                base.X = value;// +CameraPosition.X;// - camera.View.Width// / 2;// / camera.Zoom;
            }
        }

        public override float Y
        {
            get
            {
                return base.Y - CameraPosition.Y  +camera.View.Height / 2;// / camera.Zoom;
            }
            set
            {
                base.Y = value + CameraPosition.Y -camera.View.Height / 2;// / camera.Zoom;
            }
        }

        public CameraPanel(Rectangle bounds, Color backColor)
            : base(bounds, backColor)
        {
            this.backColor = backColor;
            components = new List<UIComponent>();
            oldSize = new Vector2(bounds.Width, bounds.Height);
        }

        public CameraPanel(Vector2 position, Vector2 size, Color backColor)
            : base(position, size, backColor)
        {
            this.backColor = backColor;
            components = new List<UIComponent>();
            oldSize = size;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(camera == null)
            {
                camera = new Camera2D(spriteBatch.GraphicsDevice);
                camera.Zoom = zoom;
                camera.Position = cameraPosition;
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Texture, null, null, null, null, null, camera.GetViewMatrix());
            base.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin();
        }
    }
}
