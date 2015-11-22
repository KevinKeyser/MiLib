using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiLib.UserInterface
{
    public class ListBox : UIComponent
    {
        List<object> objects;

        public ListBox(Rectangle bounds)
            : base(bounds)
        {
        }

        public ListBox(Vector2 position, Vector2 size)
            : base(position, size)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
