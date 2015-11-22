using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiLib.Interfaces;
using MiLib.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiLib.CoreTypes
{
    public abstract class Screen
    {
        protected Color backcolor = Color.CornflowerBlue;

        public Color BackColor
        {
            get
            {
                return backcolor;
            }
            set
            {
                backcolor = value;
            }
        }


        protected List<ISprite> sprites = new List<ISprite>();
        protected List<UIComponent> uiComponents = new List<UIComponent>();

        public bool UIFocused { get; protected set; }

        public virtual List<ISprite> Sprites
        {
            get
            {
                return sprites;
            }
            set
            {
                sprites = value;
            }
        }

        public virtual List<UIComponent> UIComponents
        {
            get
            {
                return uiComponents;
            }
            set
            {
                uiComponents = value;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach(ISprite sprite in sprites)
            {
                sprite.Update(gameTime);
            }
            foreach (UIComponent uicomponent in uiComponents)
            {
                uicomponent.Update(gameTime);
                if(uiComponents is IFocusable && ((IFocusable)uiComponents).IsFocused)
                {
                    UIFocused = true;
                }
            }
        }

        public virtual void Render(SpriteBatch spriteBatch)
        {
            foreach (UIComponent uicomponent in uiComponents)
            {
                uicomponent.Render(spriteBatch);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach(ISprite sprite in sprites)
            {
                sprite.Draw(spriteBatch);
            }
            foreach (UIComponent uicomponent in uiComponents)
            {
                uicomponent.Draw(spriteBatch);
            }
        }

        public virtual void UnloadContent()
        {

        }
    }
}
