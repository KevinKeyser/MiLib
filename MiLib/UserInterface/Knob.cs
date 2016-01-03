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
    public class Knob : UIComponent
    {
        private float percent;

        public float Percent
        {
            get { return percent; }
            set
            {
                percent = value;
                if (percent < 0)
                {
                    percent = 0;
                }
                else if (percent > 1)
                {
                    percent = 1;
                }
            }
        }


        public Knob(Rectangle bounds) : base(bounds)
        {
        }

        public Knob(Vector2 position, Vector2 size) : base(position, size)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.IsLeftDragged(new Rectangle((int)(position.X - bounds.Height/2), (int)(position.Y - bounds.Height / 2), bounds.Height, bounds.Height)))
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
            //Util.DrawPoint(spriteBatch, position, Color.White, bounds.Height);
            //Vector2 direction = new Vector2((float)Math.Sin(Math.PI * percent), -(float)Math.Cos( + Math.PI * percent));
            //Util.DrawLine(spriteBatch, position + direction * bounds.Height / 4, position + direction * bounds.Height/2, Color.Black, 5);
            base.Draw(spriteBatch);
        }
    }
}
