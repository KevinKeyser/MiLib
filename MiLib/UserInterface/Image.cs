using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiLib.CoreTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiLib.UserInterface
{
    public class Image : UIComponent
    {
        public event EventHandler LeftClicked;
        public event EventHandler RightClicked;
        public event EventHandler<UIDraggedEventArgs> LeftDragged;
        public event EventHandler<UIDraggedEventArgs> RightDragged;

        private Texture2D texture;
        public Texture2D Texture
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;
                if(texture != null)
                {
                    Scale = scale;
                }
            }
        }

        public Rotation Rotation
        {
            get;
            set;
        }

        protected Vector2 origin;
        public Vector2 Origin
        {
            get
            {
                return origin;
            }
            set
            {
                origin = value;
                Position = position;
            }
        }

        public float LayerDepth
        {
            get;
            set;
        }

        public SpriteEffects Effects
        {
            get;
            set;
        }

        public Color Color
        {
            get;
            set;
        }

        private Vector2 scale;

        public Vector2 Scale
        {
            get { return scale; }
            set 
            { 
                scale = value;

                if (texture != null)
                {
                    if (sourceRectangle.HasValue)
                    {
                        size = new Vector2(sourceRectangle.Value.Width, sourceRectangle.Value.Height) * scale;
                        bounds.Width = (int)(sourceRectangle.Value.Width * scale.X);
                        bounds.Height = (int)(sourceRectangle.Value.Height * scale.Y);
                    }
                    else
                    {
                        size = new Vector2(Texture.Width, Texture.Height) * scale;
                        bounds.Width = (int)(texture.Width * scale.X);
                        bounds.Height = (int)(texture.Height * scale.Y);
                    }
                    /*if (parent is CameraPanel)
                    {
                        bounds.Width = (int)(bounds.Width * ((CameraPanel)parent).Zoom);
                        bounds.Height = (int)(bounds.Height * ((CameraPanel)parent).Zoom);
                    }*/
                    Position = position;
                }
            }
        }

        public override Vector2 Size
        {
            get { return base.Size; }
            set
            {
                base.Size = value;
                if (texture != null)
                {
                    if (sourceRectangle.HasValue)
                    {
                        scale = size / new Vector2(sourceRectangle.Value.Width, sourceRectangle.Value.Height);
                        bounds.Width = (int)(sourceRectangle.Value.Width * scale.X);
                        bounds.Height = (int)(sourceRectangle.Value.Height * scale.Y);
                    }
                    else
                    {
                        scale = size / new Vector2(Texture.Width, Texture.Height);
                        bounds.Width = (int)(texture.Width * scale.X);
                        bounds.Height = (int)(texture.Height * scale.Y);
                    }
                    /*if(parent is CameraPanel)
                    {
                        bounds.Width = (int)(bounds.Width * ((CameraPanel)parent).Zoom);
                        bounds.Height = (int)(bounds.Height * ((CameraPanel)parent).Zoom);
                    }*/
                    Position = position;
                }
            }
        }

        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                position = value;
                bounds.X = (int)(position.X - origin.X*scale.X);
                bounds.Y = (int)(position.Y - origin.Y*scale.Y);
                /*if(parent != null)
                {
                    bounds.X += (int)parent.X;
                    bounds.Y += (int)parent.Y;
                }*/
            }
        }
        public override Interfaces.IParent Parent
        {
            get
            {
                return base.Parent;
            }
            set
            {
                base.Parent = value;
                Scale = scale;
                Position = position;
            }
        }
        private Rectangle? sourceRectangle;

        public Rectangle? SourceRectangle
        {
            get { return sourceRectangle; }
            set { sourceRectangle = value; }
        }

        public Image(Texture2D texture, Vector2 position)
            : this(texture, position, null, Color.White, Vector2.One) { }

        public Image(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, Vector2 scale)
            : base(position, texture == null ? Vector2.Zero : new Vector2(texture.Width, texture.Height))
        {
            Texture = texture;
            Color = Color.White;
            this.sourceRectangle = sourceRectangle;
            Scale = scale;
            Rotation = new Rotation(0);
            Effects = SpriteEffects.None;
            LayerDepth = 0;
            Origin = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            if(LeftClicked != null && InputManager.IsLeftClicked() && bounds.Intersects(parent is CameraPanel ? (InputManager.MousePosition + ((CameraPanel)parent).CameraPosition - ((CameraPanel)parent).Position/2) : InputManager.MousePosition))
            {
                LeftClicked.Invoke(this, null);
            }
            if(LeftDragged != null && InputManager.IsLeftDragged())
            {
                LeftDragged.Invoke(this, new UIDraggedEventArgs(InputManager.MouseDragAmount()));
            }
            if (RightDragged != null && InputManager.IsRightDragged())
            {
                RightDragged.Invoke(this, new UIDraggedEventArgs(InputManager.MouseDragAmount()));
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, position, sourceRectangle, Color, Rotation.AsRadians(), origin, scale, Effects, LayerDepth);
            }
            base.Draw(spriteBatch);
        }
    }
}
