using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiLib.CoreTypes;

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
                if (texture != null)
                {
                    Scale = scale;
                }
            }
        }

        public float Rotation
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

        public SpriteEffects SpriteEffect
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
                    size = new Vector2(sourceRectangle.Width, sourceRectangle.Height) * scale;
                    bounds.Width = (int)(sourceRectangle.Width * scale.X);
                    bounds.Height = (int)(sourceRectangle.Height * scale.Y);
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
                    scale = size / new Vector2(sourceRectangle.Width, sourceRectangle.Height);
                    bounds.Width = (int)(sourceRectangle.Width * scale.X);
                    bounds.Height = (int)(sourceRectangle.Height * scale.Y);
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
                bounds.X = (int)(position.X - origin.X * scale.X);
                bounds.Y = (int)(position.Y - origin.Y * scale.Y);
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
        private Rectangle sourceRectangle;

        public Rectangle SourceRectangle
        {
            get { return sourceRectangle; }
            set { sourceRectangle = value; }
        }

        public Image(Texture2D texture, Vector2 position)
            : this(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, Vector2.One)
        { }

        public Image(Texture2D texture, Vector2 position, Rectangle sourceRectangle, Color color, Vector2 scale)
            : base(position, texture == null ? Vector2.Zero : new Vector2(texture.Width, texture.Height))
        {
            Texture = texture;
            Color = Color.White;
            this.sourceRectangle = sourceRectangle;
            Scale = scale;
            Rotation = 0;
            SpriteEffect = SpriteEffects.None;
            LayerDepth = 0;
            Origin = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            if (LeftClicked != null && InputManager.IsLeftClicked() && bounds.Contains(InputManager.MousePosition))
            {
                LeftClicked.Invoke(this, null);
            }
            if (LeftDragged != null && InputManager.IsLeftDragged())
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
                spriteBatch.Draw(Texture, position, sourceRectangle, Color, Rotation, origin, scale, SpriteEffect, LayerDepth);
            }
            base.Draw(spriteBatch);
        }
    }
}
