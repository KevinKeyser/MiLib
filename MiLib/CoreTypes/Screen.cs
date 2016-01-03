using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiLib.Interfaces;
using MiLib.UserInterface;

namespace MiLib.CoreTypes
{
    public class Screen : Sprite
    {
        protected Color backColor = Color.CornflowerBlue;

        public Color BackColor
        {
            get
            {
                return backColor;
            }
            set
            {
                backColor = value;
            }
        }

        public ICamera Camera;

        public UIComponent UIFocused { get; protected set; }

        protected Dictionary<string, UIComponent> uiComponents;
        protected Dictionary<string, Sprite> sprites;

        public Screen(String name)
            : base(name)
        {
            uiComponents = new Dictionary<string, UIComponent>();
            sprites = new Dictionary<string, Sprite>();
            BackColor = Color.CornflowerBlue;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Sprite sprite in sprites.Values)
            {
                sprite.Update(gameTime);
            }

            foreach (UIComponent uiComponent in uiComponents.Values)
            {
                if (uiComponent.IsEnabled)
                {
                    uiComponent.Update(gameTime);
                    if (uiComponents is IFocusable && ((IFocusable)uiComponents).IsFocused)
                    {
                        UIFocused = uiComponent;
                    }
                }
            }
        }

        public override void Render()
        {
            foreach (UIComponent uiComponent in uiComponents.Values)
            {
                if (IsVisible)
                {
                    uiComponent.Render(WindowManager.SpriteBatch);
                }
            }
        }

        public override void Draw(ICamera camera)
        {
            foreach (Sprite sprite in sprites.Values)
            {/*
                if (sprite.IsVisible)
                {*/
                sprite.Draw(Camera);
                // }
            }
            foreach (UIComponent component in uiComponents.Values)
            {
                if (component.IsVisible)
                {
                    component.Draw(WindowManager.SpriteBatch);
                }
            }
        }

        public virtual void UnloadContent()
        {

        }
    }
}
