using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MiLib.CoreTypes
{
    public enum FullScreenSwitchMode
    {
        Soft = 0,
        Hard = 1
    }

    public static class WindowManager
    {
        private static Game game;
        
        public static DisplayOrientation SupportedOrientations
        {
            get { return graphics.SupportedOrientations; }
            set { graphics.SupportedOrientations = value; }
        }

        public static bool IsMouseVisible
        {
            get { return game.IsMouseVisible; }
            set { game.IsMouseVisible = value; }
        }

        public static bool IsActive
        {
            get { return game.IsActive; }
        }

        public static TimeSpan InactiveSleepTime
        {
            get { return game.InactiveSleepTime; }
            set { game.InactiveSleepTime = value; }
        }

        private static SpriteBatch spriteBatch;

        public static SpriteBatch SpriteBatch
        {
            get { if (spriteBatch == null) spriteBatch = new SpriteBatch(GraphicsDevice); return spriteBatch; }
        }

        private static GraphicsDeviceManager graphics;

        public static GraphicsDevice GraphicsDevice
        {
            get { return graphics.GraphicsDevice; }
        }

        public static FullScreenSwitchMode SwitchMode
        {
            get { return graphics.HardwareModeSwitch ? FullScreenSwitchMode.Hard : FullScreenSwitchMode.Soft; }
        }

        public static int ScreenWidth
        {
            get { return graphics.GraphicsDevice.Viewport.Width; }
            set { graphics.PreferredBackBufferWidth = value; graphics.ApplyChanges(); }
        }

        public static int ScreenHeight
        {
            get { return graphics.GraphicsDevice.Viewport.Height; }
            set { graphics.PreferredBackBufferHeight = value; graphics.ApplyChanges(); }
        }

        public static Viewport Viewport
        {
            get { return graphics.GraphicsDevice.Viewport; }
            set { graphics.GraphicsDevice.Viewport = value; }
        }

        public static bool IsFullScreen
        {
            get { return graphics.IsFullScreen; }
            set { graphics.IsFullScreen = value; graphics.ApplyChanges(); }
        }
        
        public static GameWindow Window
        {
            get { return game.Window; }
        }

        public static void Initialize(Game game)
        {
            Initialize(game, FullScreenSwitchMode.Hard);
        }

        public static void Initialize(Game game, FullScreenSwitchMode switchMode)
        {
            graphics = new GraphicsDeviceManager(game) { HardwareModeSwitch = switchMode == FullScreenSwitchMode.Hard ? true : false };
            WindowManager.game = game;
        }

        public static void Exit()
        {
            graphics.Dispose();
            game.Exit();
        }

        public static void Dispose()
        {
            graphics.Dispose();
        }

    }
}
