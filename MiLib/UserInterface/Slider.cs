using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiLib.CoreTypes;

namespace MiLib.UserInterface
{
    public class Slider : UIComponent
    {
        private float percent;

        public float Percent
        {
            get { return percent; }
            set
            {
                percent = value;
                if(percent < 0)
                {
                    percent = 0;
                }
                else if(percent > 1)
                {
                    percent = 1;
                }
            }
        }
            

        public Slider(Rectangle bounds) : base(bounds)
        {
            
        }

        public Slider(Vector2 position, Vector2 size) : base(position, size)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if(InputManager.IsLeftDragged(new Rectangle((int)(bounds.Width * percent - bounds.Height / 2 + position.X), (int)(position.Y), bounds.Height, bounds.Height)))
            {
                Percent += InputManager.MouseDragAmount().X / bounds.Width; 
            }
            base.Update(gameTime);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Util.DrawRectangle(spriteBatch, bounds, Color.Red);
            //Util.DrawPoint(spriteBatch, position + new Vector2(0, bounds.Height/2) + new Vector2(bounds.Width * percent,0), Color.White, bounds.Height);
            base.Draw(spriteBatch);
        }
    }
}
