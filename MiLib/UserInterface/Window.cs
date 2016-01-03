using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiLib.Interfaces;

namespace MiLib.UserInterface
{
    public class Window : UIComponent, IParent
    {
        public Color WindowColor;
        public Color ForegroundSelectionColor;
        public Color BackgroundSelectionColor;

        public Texture2D WindowTexture;
        public SpriteFont Font;

        public List<Label> Items;

        public int SelectedIndex = 0;
        public float X
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

        public float Y
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

        public Window(Vector2 position, Vector2 size, SpriteFont font, Texture2D windowTexture)
            : base(position, size)
        {
            Items = new List<Label>();
            Font = font;
            WindowTexture = windowTexture;
            WindowColor = Color.White;
            ForegroundSelectionColor = Color.White;
            BackgroundSelectionColor = Color.Blue;
        }

        public override void Update(GameTime gameTime)
        {
            if (isVisible)
            {
                foreach (Label label in Items)
                {
                    label.Update(gameTime);
                    label.Bounds = new Rectangle(label.Bounds.X, label.Bounds.Y, Bounds.Width, label.Bounds.Height);
                    label.IsSelected = false;
                    label.CenterText();
                }
                base.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                spriteBatch.Draw(WindowTexture, Bounds, WindowColor);
                foreach (Label label in Items)
                {
                    label.Draw(spriteBatch);
                }
                base.Draw(spriteBatch);
            }
        }

    }
}
