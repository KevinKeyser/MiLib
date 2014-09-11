using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace MiLib.CoreTypes
{
	public static class InputManager
	{
		static MouseState mouseState;
		static KeyboardState keyState;
		static GamePadState[] gamePadState = new GamePadState[4];

		static MouseState lastMouseState;
		static KeyboardState lastKeyState;
		static GamePadState[] lastGamePadState = new GamePadState[4];

		static Vector2 mousePosition;
		static Vector2 lastMousePosition;
		public static Vector2 MousePosition
		{
			get
			{
				return mousePosition;
			}
		}

		public static Vector2 LastMoustPosition
		{
			get 
			{
				return LastMoustPosition;
			}
		}

		public static void Update ()
		{
			lastMouseState = mouseState;
			lastKeyState = keyState;
			lastMousePosition = mousePosition;

			mouseState = Mouse.GetState ();
			keyState = Keyboard.GetState ();
			mousePosition = new Vector2(mouseState.X, mouseState.Y);
			for (int i = 0; i < 4; i++) {
				lastGamePadState [i] = gamePadState [i];
				if (GamePad.GetState ((PlayerIndex)i).IsConnected) {
					gamePadState [i] = GamePad.GetState ((PlayerIndex)i); 
				}	
			}
		}

		#region GamePad Button
		public static bool IsButtonDown(PlayerIndex player, Buttons button)
		{
			return (gamePadState [(int)player].IsButtonDown (button));
		}

		public static bool IsButtonPressed(PlayerIndex player, Buttons button)
		{
			return (gamePadState [(int)player].IsButtonDown (button) && !lastGamePadState [(int)player].IsButtonDown (button));
		}

		public static bool IsButtonReleased(PlayerIndex player, Buttons button)
		{
			return (!gamePadState [(int)player].IsButtonDown (button) && lastGamePadState [(int)player].IsButtonDown (button));
		}

		public static bool IsButtonUp(PlayerIndex player, Buttons button)
		{
			return (gamePadState [(int)player].IsButtonDown (button));
		}
		#endregion
		#region Key
		public static bool IsKeyDown(Keys key)
		{
			return keyState.IsKeyDown (key);
		}

		public static bool IsKeyPressed(Keys key)
		{
			return keyState.IsKeyDown (key) && !lastKeyState.IsKeyDown (key);
		}

		public static bool IsKeyReleased(Keys key)
		{
			return !keyState.IsKeyDown (key) && lastKeyState.IsKeyDown (key);
		}

		public static bool IsKeyUp(Keys key)
		{
			return !keyState.IsKeyDown (key);
		}
		#endregion
		#region Mouse Movement
		public static bool IsDragged()
		{
			return ((mouseState.LeftButton == ButtonState.Pressed &&  mousePosition - lastMousePosition != Vector2.Zero) ? true : false);
		}

		public static bool IsDragged(Rectangle bounds)
		{
			return ((mouseState.LeftButton == ButtonState.Pressed &&  mousePosition - lastMousePosition != Vector2.Zero && bounds.Contains (new Point((int)mousePosition.X, (int)mousePosition.Y))) ? true : false);
		}

		public static Vector2 MouseDragAmount()
		{
			return ((mouseState.LeftButton == ButtonState.Pressed && mousePosition - lastMousePosition != Vector2.Zero) ?  mousePosition - lastMousePosition : Vector2.Zero); 
		}

		public static bool isMouseHovering(Rectangle bounds)
		{
            return bounds.Contains(new Point((int)mousePosition.X, (int)mousePosition.Y));
		}
		#endregion
		#region Mouse Button
		#region Left
		public static bool IsLeftDown(Rectangle bounds)
		{
            return mouseState.LeftButton == ButtonState.Pressed && bounds.Contains(new Point((int)mousePosition.X, (int)mousePosition.Y)); 
		}

		public static bool IsLeftClicked(Rectangle bounds)
		{
            return mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released && bounds.Contains(new Point((int)mousePosition.X, (int)mousePosition.Y)); 
		}

		public static bool IsLeftReleased(Rectangle bounds)
		{
            return mouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed && bounds.Contains(new Point((int)mousePosition.X, (int)mousePosition.Y)); 
		}

		public static bool IsLeftUp(Rectangle bounds)
		{
            return lastMouseState.LeftButton == ButtonState.Released && bounds.Contains(new Point((int)mousePosition.X, (int)mousePosition.Y)); 
		}
		#endregion
		#region Right
		public static bool IsRightDown(Rectangle bounds)
		{
            return mouseState.RightButton == ButtonState.Pressed && bounds.Contains(new Point((int)mousePosition.X, (int)mousePosition.Y)); 
		}

		public static bool IsRightClicked(Rectangle bounds)
		{
            return mouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released && bounds.Contains(new Point((int)mousePosition.X, (int)mousePosition.Y)); 
		}

		public static bool IsRightReleased(Rectangle bounds)
		{
            return mouseState.RightButton == ButtonState.Released && lastMouseState.RightButton == ButtonState.Pressed && bounds.Contains(new Point((int)mousePosition.X, (int)mousePosition.Y)); 
		}

		public static bool IsRightUp(Rectangle bounds)
		{
			return lastMouseState.RightButton == ButtonState.Released && bounds.Contains (new Point((int)mousePosition.X, (int)mousePosition.Y)); 
		}
		#endregion
		#region Middle
		public static bool IsMiddleDown(Rectangle bounds)
		{
			return mouseState.MiddleButton == ButtonState.Pressed && bounds.Contains (new Point((int)mousePosition.X, (int)mousePosition.Y)); 
		}

		public static bool IsMiddleClicked(Rectangle bounds)
		{
			return mouseState.MiddleButton == ButtonState.Pressed && lastMouseState.MiddleButton  == ButtonState.Released && bounds.Contains (new Point((int)mousePosition.X, (int)mousePosition.Y)); 
		}

		public static bool IsMiddleReleased(Rectangle bounds)
		{
			return mouseState.MiddleButton == ButtonState.Released && lastMouseState.MiddleButton == ButtonState.Pressed && bounds.Contains (new Point((int)mousePosition.X, (int)mousePosition.Y)); 
		}

		public static bool IsMiddleUp(Rectangle bounds)
		{
			return lastMouseState.MiddleButton == ButtonState.Released && bounds.Contains (new Point((int)mousePosition.X, (int)mousePosition.Y)); 
		}
		#endregion
		#endregion
	}
}

