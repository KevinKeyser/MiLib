using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiLib.UserInterface
{
    public class Panel : UIComponent, IParent
    {
        protected Color backColor;

        public virtual Color BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }

        protected List<UIComponent> components;
        public List<UIComponent> Components
        {
            get
            {
                return components;
            }
        }

        protected RenderTarget2D render;

        protected bool isRendered;

        protected Vector2 oldSize;
        public bool IsRendered
        {
            get { return isRendered; }
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

        public Panel(Rectangle bounds, Color backColor)
            : base(bounds)
        {
            this.backColor = backColor;
            components = new List<UIComponent>();
            oldSize = new Vector2(bounds.Width, bounds.Height);
        }

        public Panel(Vector2 position, Vector2 size, Color backColor)
            : base(position, size)
        {
            this.backColor = backColor;
            components = new List<UIComponent>();
            oldSize = size;
        }

        public void Add(UIComponent component)
        {
            component.Parent = this;
            components.Add(component);
        }

        public void Remove(UIComponent component)
        {
            component.Parent = null;
            components.Remove(component);
        }

        public void Remove(int index)
        {
            components[index].Parent = null;
            components.RemoveAt(index);
        }

        public void Clear()
        {
            components.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < components.Count; i++ )
            {
                components[i].Update(gameTime);
            }
            base.Update(gameTime);
        }

        public void Render(SpriteBatch spriteBatch)
        {
            foreach (Panel panel in components.OfType<Panel>())
            {
                panel.Render(spriteBatch);
            }

            if (render == null || oldSize != Size)
            {
                oldSize = Size;
                render = new RenderTarget2D(spriteBatch.GraphicsDevice, (int)Size.X, (int)Size.Y);
            }
            spriteBatch.GraphicsDevice.SetRenderTarget(render);

            spriteBatch.GraphicsDevice.Clear(backColor);
            spriteBatch.Begin();

            foreach (UIComponent component in components)
            {
                component.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            isRendered = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(!isRendered)
            {
                throw new Exception("Panel - Draw(SpriteBatch spriteBatch) : Must render panel before drawing");
            }
            spriteBatch.Draw(render, bounds, Color.White);
            isRendered = false;
        }
    }
}
