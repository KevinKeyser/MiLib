using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

using MiLib.CoreTypes;

namespace MiLib.UserInterface
{
	public delegate void ButtonEvent(object sender, EventArgs e);

	public class Button
	{
		public Rectangle Bounds{ get; set; }

		protected Vector2 position;
		public Vector2 Position 
		{ 
			get
			{ 
				return position; 
			}
			set 
			{
				position = value;
				if (patch9 != null) {
					patch9.Position = value;
				}
			}
		}

		public Texture2D Up { protected get; set; }
		public Texture2D Down{ protected get; set; }
		public Texture2D Hover{ protected get; set; }
		public Texture2D Disabled{ protected get; set; }
		public bool IsDisabled{ get; set; }

		public Vector4 Patches
		{
			get
			{
				return patch9.Patches;
			}
			set 
			{
				patch9.Patches = value;
			}
		}

		public virtual Vector2 Size 
		{
			get
			{
				return patch9.Size;
			}
			set
			{
				if(value.X >= 0 && value.Y >= 0) patch9.Size = value;
			} 
		}

        public Color PatchColor
        {
            get
            {
                return patch9.SplitColor;
            }
            set
            {
                patch9.SplitColor = value;
            }
        }

		private Patch9Image patch9;

		#region Button Events
		/// <summary>
		/// Occurs when left button is held down
		/// </summary>
		public event ButtonEvent LeftDown;
		/// <summary>
		/// Occurs when left button is released.
		/// </summary>
		public event ButtonEvent LeftClicked;		
		/// <summary>
		/// Occurs when right button is held down
		/// </summary>
		public event ButtonEvent RightDown;
		/// <summary>
		/// Occurs when right button is released.
		/// </summary>
		public event ButtonEvent RightClicked;		
		/// <summary>
		/// Occurs when middle scrollwheel is held down
		/// </summary>
		public event ButtonEvent MiddleDown;
		/// <summary>
		/// Occurs when middle scrollwheel is released.
		/// </summary>
		public event ButtonEvent MiddleClicked;
		/// <summary>
		/// Occurs when mouse is hovering.
		/// </summary>
		public event ButtonEvent Hovered;
		/// <summary>
		/// Occurs when button is disabled and there is an attempt to clicked.
		/// </summary>
		public event ButtonEvent DisabledClicked;
		/// <summary>
		/// Occurs when mouse is down and dragging.
		/// </summary>
		public event ButtonEvent Drag;
		#endregion
		#region Event Invokes
		private void InvokeEvent(ButtonEvent myEvent)
		{
			if (myEvent != null) {
				myEvent.Invoke (null, null);
			}
		}

		private void InvokeEvent(ButtonEvent myEvent, object sender)
		{
			if (myEvent != null) {
				myEvent.Invoke (sender, null);
			}
		}

		private void InvokeEvent(ButtonEvent myEvent, object sender, EventArgs e)
		{
			if (myEvent != null) {
				myEvent.Invoke (sender, e);
			}
		}

		private void InvokeEvent(ButtonEvent myEvent, EventArgs e)
		{
			if (myEvent != null) {
				myEvent.Invoke (this, e);
			}
		}
		#endregion

		public Button (Texture2D up, Texture2D down, Texture2D hover, Texture2D disabled, Color buttonColor)
		{
			if(up == null)
			{
				Debug.WriteLine("Button - Texture2D up cannot be null");
			}
			else if ((up.Bounds != down.Bounds && down != null) || (up.Bounds != hover.Bounds && hover != null) || (up.Bounds != disabled.Bounds && disabled != null)) {
				Debug.WriteLine ("Button - Texture2D images must be the same size or null");
			}
			else
			{
				patch9 = new Patch9Image (Vector4.Zero, up, Vector2.Zero, buttonColor);
				IsDisabled = false;
				Up = up;
				Down = down;
				Hover = hover;
				Disabled = disabled;
			}
		}
	
		private void ChangeTexture(Texture2D texture)
		{
			if (texture != null) {
				patch9.SplitImage = texture;
			}
		}

		public void Update(GameTime gameTime)
		{
			if (InputManager.IsDragged(Bounds)) {
				InvokeEvent (Drag, InputManager.MouseDragAmount());
			}
			if (IsDisabled) {
				if (InputManager.IsLeftDown (Bounds) || InputManager.IsRightDown (Bounds) || InputManager.IsMiddleDown (Bounds)) {
					InvokeEvent (DisabledClicked);
				}
				ChangeTexture (Disabled);
			} else if (InputManager.IsLeftClicked (Bounds)) {
				ChangeTexture (Up);
				InvokeEvent (LeftClicked);
			} else if (InputManager.IsRightClicked (Bounds)) {
				InvokeEvent (RightClicked);
			} else if (InputManager.IsMiddleClicked (Bounds)) {
				InvokeEvent (MiddleClicked);
			} else if (InputManager.IsLeftDown (Bounds)) {
				ChangeTexture (Down);
				InvokeEvent (LeftDown);
			} else if (InputManager.IsRightDown (Bounds)) {
				InvokeEvent (RightDown);
			} else if (InputManager.IsMiddleDown (Bounds)) {
				InvokeEvent (MiddleDown);
			} else if (InputManager.isMouseHovering (Bounds)) {
				ChangeTexture (Hover);
				InvokeEvent (Hovered);
			} else {
				ChangeTexture (Up);
			}
			Bounds = new Rectangle ((int)Position.X, (int)Position.Y, (int)(Size.X), (int)(Size.Y));
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			patch9.Draw (spriteBatch);
		}
	}
}

