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
using MiLib.CoreTypes;

namespace MiLibDemo
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        SpriteFont font;

        Vector2 mousePos;
        OrthographicCamera camera;
        Texture2D pixel;
        Effect effect;
        Texture2D image;
        Texture2D normal;
        Vector3 position = Vector3.Zero;
        float rotation = 0;
        Vector3 scale = Vector3.One;

        public Game1()
        {
            WindowManager.Initialize(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            WindowManager.IsMouseVisible = true;
            camera = new OrthographicCamera(new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            camera.Position = new Vector2(0, 0);
            
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
            font = Content.Load<SpriteFont>("font");
            effect = Content.Load<Effect>("NormalMap");
            effect.Parameters["TextureSampler"].SetValue(image);
            effect.Parameters["NormalSampler"].SetValue(normal);
            effect.Parameters["LightDirection"].SetValue(new Vector3(0, 0, 1));

            image = Content.Load<Texture2D>("DLight Trees");
            normal = Content.Load<Texture2D>("DLight Trees_NORMALS");   
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();
            mousePos = ms.Position.ToVector2();
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.W))
            {
                camera.Position += new Vector2(0, 1f);
            }
            if (ks.IsKeyDown(Keys.S))
            {
                camera.Position -= new Vector2(0, 1f);
            }
            if (ks.IsKeyDown(Keys.A))
            {
                camera.Position += new Vector2(1f, 0);
            }
            if (ks.IsKeyDown(Keys.D))
            {
                camera.Position -= new Vector2(1f, 0);
            }
            if (ks.IsKeyDown(Keys.E))
            {
                rotation += .1f;
            }
            if (ks.IsKeyDown(Keys.Q))
            {
                rotation -= .1f;
            }
            if(ks.IsKeyDown(Keys.OemPlus))
            {
                scale += new Vector3(1f);
            }
            if (ks.IsKeyDown(Keys.OemMinus))
            {
                scale -= new Vector3(1f);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);
            

            WindowManager.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, effect, null);

            
            WindowManager.SpriteBatch.DrawString(font, "text", Vector2.Zero, Color.Black);
            WindowManager.SpriteBatch.Draw(image, position.ToVector2(), null, Color.Lerp(Color.Green, Color.Transparent, .5f), rotation, Vector2.Zero, scale.ToVector2(), SpriteEffects.None, 1);

            WindowManager.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
