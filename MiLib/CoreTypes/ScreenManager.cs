using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiLib.CoreTypes
{
    public static class ScreenManager
    {
        static Dictionary<string, Screen> screens = new Dictionary<string, Screen>();

        static Screen currentScreen;
        public static Screen CurrentScreen
        {
            get
            {
                return currentScreen;
            }
        }
        public static bool AddScreen(string name, Screen screen)
        {
            if (screens.ContainsKey(name))
            {
                return false;
            }
            screens.Add(name, screen);
            return true;
        }
        public static bool RemoveScreen(string name)
        {
            if (screens.ContainsKey(name))
            {
                screens[name].UnloadContent();
                screens.Remove(name);
                return true;
            }
            return false;
        }

        public static bool SetScreen(string name)
        {
            if(screens.ContainsKey(name))
            {
                currentScreen = screens[name];
                return true;
            }
            return false;
        }

        public static void UnloadContent()
        {
            string[] names = screens.Keys.ToArray();
            for (int i = 0; i < names.Length; i++)
            {
                screens[names[i]].UnloadContent();
                screens.Remove(names[i]);
            }
        }

        public static void Update(GameTime gameTime)
        {
            if(currentScreen != null)
            {
                currentScreen.Update(gameTime);
            }
        }

        public static void Render(SpriteBatch spriteBatch)
        {
            if (currentScreen != null)
            {
                currentScreen.Render(spriteBatch);
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if(currentScreen != null)
            {
                currentScreen.Draw(spriteBatch);
            }
        }
    }
}
