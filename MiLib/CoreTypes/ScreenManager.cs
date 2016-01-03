using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiLib.CoreTypes
{
    public static class ScreenManager
    {
        static OrthographicCamera camera = new OrthographicCamera(new Vector2(WindowManager.ScreenWidth, WindowManager.ScreenHeight));

        static Dictionary<String, Screen> screens = new Dictionary<string, Screen>();
        public static bool IsDebug = false;

        private static String currentScreen;
        public static String CurrentScreen
        {
            get { return currentScreen; }
            set
            {
                currentScreen = value;
                if (!screens.ContainsKey(value))
                    throw new KeyNotFoundException("\"" + value + "\" was not found in screens dictionary");
            }
        }

        public static bool AddScreen(String name, Screen screen)
        {
            if (screens.ContainsKey(name))
            {
                return false;
            }
            screens.Add(name, screen);
            return true;
        }

        public static Screen GetScreen(String name)
        {
            return screens[name];
        }

        public static bool RemoveScreen(String name)
        {
            if (screens.ContainsKey(name))
            {
                screens[name].UnloadContent();
                screens.Remove(name);
                return true;
            }
            return false;
        }
        
        public static void Update(GameTime gameTime)
        {
            if (currentScreen != null && screens[currentScreen].IsUpdating)
            {
                screens[currentScreen].Update(gameTime);
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (currentScreen != null && screens[currentScreen].IsVisible)
            {
                screens[currentScreen].Render();
                screens[currentScreen].Draw(camera);
            }
        }
    }
}
