using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using MiLib.Collision;

namespace MiLibDemo
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RectangleOBB shape1;
        RectangleOBB shape2;
        SpriteFont font;
        bool iscollide = false;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            shape1 = new RectangleOBB(new Vector2(100), new Vector2(100), new Vector2(150, 150), GraphicsDevice);
            shape1.Debug = true;
            shape2 = new RectangleOBB(new Vector2(100), new Vector2(100), new Vector2(150), GraphicsDevice);
            shape2.Debug = true;

            font = Content.Load<SpriteFont>("font");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.W))
            {
                shape2.Position += new Vector2(0, -1);
            }
            if (ks.IsKeyDown(Keys.S))
            {
                shape2.Position += new Vector2(0, 1);
            }
            if (ks.IsKeyDown(Keys.A))
            {
                shape2.Position += new Vector2(-1, 0);
            }
            if (ks.IsKeyDown(Keys.D))
            {
                shape2.Position += new Vector2(1, 0);
            }
            if (ks.IsKeyDown(Keys.E))
            {
                shape2.Rotation += new MiLib.CoreTypes.Rotation(5, MiLib.CoreTypes.AngleMeasure.Degrees);
            }
            if (ks.IsKeyDown(Keys.Q))
            {
                shape2.Rotation += new MiLib.CoreTypes.Rotation(-5, MiLib.CoreTypes.AngleMeasure.Degrees);
            }

            if (shape2.Intersects(shape1))
            {
                iscollide = true;
            }
            else
            {
                iscollide = false;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            shape1.Draw(spriteBatch);
            shape2.Draw(spriteBatch);

            spriteBatch.DrawString(font, iscollide.ToString(), Vector2.Zero, Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
