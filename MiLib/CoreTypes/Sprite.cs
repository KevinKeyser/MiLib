using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MiLib.Interfaces;

namespace MiLib.CoreTypes
{
    class Sprite : ISprite
    {
        #region Variables
        public virtual Texture2D Texture { get; set; }

        public virtual Rectangle? Region { get; set; }

        protected Vector2 position;

        public virtual Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public virtual float X
        {
            get
            {
                return position.X;
            }
            set
            {
                position.X = value;
            }
        }

        public virtual float Y
        {
            get
            {
                return position.Y;
            }
            set
            {
                position.Y = value;
            }
        }

        public virtual Rectangle Bounds { get; set; }

        public virtual Color Color { get; set; }

        public virtual Rotation Rotation { get; set; }

        public virtual Vector2 Origin { get; set; }

        public virtual Vector2 Scale { get; set; }

        public virtual SpriteEffects Effect { get; set; }

        public virtual float Layer { get; set; }

        public virtual bool isUpdating { get; set; }

        public virtual bool isVisible { get; set; }
        #endregion Variables

        public Sprite(Texture2D texture, Vector2 position, Color color)
        {
            Texture = texture;
            Position = position;
            Color = color;
            Bounds = new Rectangle((int)(position.X - Origin.X), (int)(position.Y - Origin.Y),
                                   (int)(Texture.Width * Scale.X), (int)(Texture.Height * Scale.Y));
            Scale = Vector2.One;
            Rotation = new Rotation(0);
            isUpdating = true;
            isVisible = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (isUpdating)
            {
                Bounds.X = (int)(position.X - Origin.X);
                Bounds.Y = (int)(position.Y - Origin.Y);
                Bounds.Width = (int)(Texture.Width * Scale.X);
                Bounds.Height = (int)(Texture.Height * Scale.Y);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
            {
                spriteBatch.Draw(Texture, Position, Region, Color, Rotation.AsRadians(), Origin, Scale, Effect, Layer);
            }
        }
    }
}
