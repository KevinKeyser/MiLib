using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace MiLib.UserInterface
{
	public class Patch9Image
	{
		public Texture2D SplitImage{ get; set; }
		private Rectangle[] drawArea = new Rectangle[9];
		private Rectangle[] textureRegion = new Rectangle[9]; 
		public Color SplitColor{ get; set; }

		private Vector4 patches = Vector4.Zero;
		public Vector4 Patches {
			get {
				return patches;
			}
			set {
				if (value.X >= 0 && value.Y >= 0 && value.Z >= 0 && value.W >= 0 && value.X + value.Y <= SplitImage.Width && value.W + value.Z <= SplitImage.Height) {
					patches = value;

					textureRegion [0] = new Rectangle (0, 0, (int)Patches.X, (int)Patches.Z);
					textureRegion [1] = new Rectangle ((int)Patches.X, 0, SplitImage.Width - (int)(Patches.X + Patches.Y), (int)Patches.Z);
					textureRegion [2] = new Rectangle (SplitImage.Width - (int)Patches.Y, 0, (int)Patches.Y, (int)Patches.Z);

					textureRegion [3] = new Rectangle (0, (int)Patches.Z, (int)Patches.X, SplitImage.Height - (int)(Patches.Z + Patches.W));
					textureRegion [4] = new Rectangle ((int)Patches.X, (int)Patches.Z, SplitImage.Width - (int)(Patches.X + Patches.Y), SplitImage.Height - (int)(Patches.Z + Patches.W));
					textureRegion [5] = new Rectangle (SplitImage.Width - (int)Patches.X, (int)Patches.Z, (int)Patches.Y, SplitImage.Height - (int)(Patches.Z + Patches.W));

					textureRegion [6] = new Rectangle (0, SplitImage.Height - (int)Patches.W, (int)Patches.X, (int)Patches.W);
					textureRegion [7] = new Rectangle ((int)Patches.X, SplitImage.Height - (int)Patches.W, SplitImage.Width - (int)(Patches.X + Patches.Y), (int)Patches.W);
					textureRegion [8] = new Rectangle (SplitImage.Width - (int)Patches.Y, SplitImage.Height - (int)Patches.W, (int)Patches.Y, (int)Patches.W);

					Size = size;
				}
			}
		}

		private Vector2 size = Vector2.Zero;
		public Vector2 Size
		{ 
			get
			{
				return new Vector2(size.X + SplitImage.Width, size.Y + SplitImage.Height); 
			}
			set {
				size = value;

				drawArea [0] = new Rectangle ((int)(textureRegion [0].X + position.X), (int)(textureRegion [0].Y + position.Y), (int)(textureRegion [0].Width), 			(int)(textureRegion [0].Height));
				drawArea [1] = new Rectangle ((int)(textureRegion [1].X + position.X), (int)(textureRegion [1].Y + position.Y), (int)(textureRegion [1].Width + size.X), 	(int)(textureRegion [1].Height));
				drawArea [2] = new Rectangle ((int)(textureRegion [2].X + position.X + size.X), (int)(textureRegion [2].Y + position.Y), (int)(textureRegion [2].Width), 			(int)(textureRegion [2].Height));

				drawArea [3] = new Rectangle ((int)(textureRegion [3].X + position.X), (int)(textureRegion [3].Y + position.Y), (int)(textureRegion [3].Width), 			(int)(textureRegion [3].Height + size.Y));
				drawArea [4] = new Rectangle ((int)(textureRegion [4].X + position.X), (int)(textureRegion [4].Y + position.Y), (int)(textureRegion [4].Width + size.X), 	(int)(textureRegion [4].Height + size.Y));
				drawArea [5] = new Rectangle ((int)(textureRegion [5].X + position.X + size.X), (int)(textureRegion [5].Y + position.Y), (int)(textureRegion [5].Width), 			(int)(textureRegion [5].Height + size.Y));

				drawArea [6] = new Rectangle ((int)(textureRegion [6].X + position.X), (int)(textureRegion [6].Y + position.Y + size.Y), (int)(textureRegion [6].Width),				(int)(textureRegion [6].Height));
				drawArea [7] = new Rectangle ((int)(textureRegion [7].X + position.X), (int)(textureRegion [7].Y + position.Y + size.Y), (int)(textureRegion [7].Width + size.X), 	(int)(textureRegion [7].Height));
				drawArea [8] = new Rectangle ((int)(textureRegion [8].X + position.X + size.X), (int)(textureRegion [8].Y + position.Y + size.Y), (int)(textureRegion [8].Width), 			(int)(textureRegion [8].Height));
			}
		}

		private Vector2 position;
		public Vector2 Position
		{ 
			get
			{
				return position;
			}
			set 
			{
				position = value;
				Size = size;
			}
		}

		public Patch9Image (Vector4 patches, Texture2D splitImage, Vector2 position, Color color)
		{
			SplitColor = color;
			Position = position;
			SplitImage = splitImage;
			Patches = patches;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < 9; i++) 
			{
				spriteBatch.Draw (SplitImage, drawArea [i], textureRegion [i], SplitColor);
			}
		}
	}
}