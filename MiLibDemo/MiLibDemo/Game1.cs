using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using MiLib.Collision;

namespace MiLibDemo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
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

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            shape1 = new RectangleOBB(new Vector2(100), new Vector2(200), new Vector2(200), GraphicsDevice);
            shape1.Debug = true;
            shape2 = new RectangleOBB(new Vector2(100, 100), new Vector2(100, 100), new Vector2(150), GraphicsDevice);
            shape2.Debug = true;

            font = Content.Load<SpriteFont>("font");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState ks = Keyboard.GetState();

            if(ks.IsKeyDown(Keys.W))
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

            if(ks.IsKeyDown(Keys.OemPlus))
            {
                shape2.Scale += .1f;
            }
            else if(ks.IsKeyDown(Keys.OemMinus))
            {
                shape2.Scale -= .1f;
            }

            if(shape2.Intersects(shape1))
            {
                iscollide = true;
            }
            else
            {
                iscollide = false;
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            shape1.Draw(spriteBatch);
            shape2.Draw(spriteBatch);

            spriteBatch.DrawString(font, iscollide.ToString(), Vector2.Zero, Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
