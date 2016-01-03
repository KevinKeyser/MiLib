using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace MiLib.CoreTypes
{
	public static class AssetManager
	{
		/// <summary>
		/// Private list of assets, the first asset in the list is guaranteed to 
		/// load before the body of any Update or Draw calls is made
		/// </summary>
		public static List<Asset> assets = new List<Asset>();


		/// <summary>
		/// Index storing which asset we are currently loading
		/// </summary>
		private static int index = 0;

		/// <summary>
		/// Map between asset keys and their loaded data
		/// </summary>
		private static Dictionary<string, object> data = new Dictionary<string, object>();

		/// <summary>
		/// Cache of the blendstates needed for the LoadTextureStream method
		/// </summary>
		public static BlendState blendAlpha = null, blendColor = null;

        
		/// <summary>
		/// Returns the percent of assets which have been loaded as an integer 0 to 100
		/// </summary>
		public static float PercentLoaded
		{
			get { return (float)index / assets.Count; }
		}

		/// <summary>
		/// Returns true if all loading is complete
		/// </summary>
		public static bool Loaded
		{
			get { return index >= assets.Count; }
		}

		public static bool LoadOne(ContentManager Content)
		{
			if (index >= assets.Count) return true;

			Asset nextAsset = assets[index];
            /*
            effect
            spritefont   ---same
            font texture ---same
            material     ---What represents this?
            model
            song
            soundeffect
            texture
            video
            */
            if(nextAsset.type == typeof(Effect))
            {
                data[nextAsset.key] = Content.Load<Effect>(nextAsset.location);
            }
            else if (nextAsset.type == typeof(SpriteFont))
            {
                data[nextAsset.key] = Content.Load<SpriteFont>(nextAsset.location);
            }
            else if (nextAsset.type == typeof(Model))
            {
                data[nextAsset.key] = Content.Load<Model>(nextAsset.location);
            }
            else if (nextAsset.type == typeof(Song))
            {
                data[nextAsset.key] = Content.Load<Song>(nextAsset.location);
            }
            else if (nextAsset.type == typeof(SoundEffect))
            {
                data[nextAsset.key] = Content.Load<SoundEffect>(nextAsset.location);
            }
            else if (nextAsset.type == typeof(Texture2D))
			{
                data[nextAsset.key] = Content.Load<Texture2D>(nextAsset.location);// LoadTextureStream(game.GraphicsDevice, nextAsset.location, game);
            }
            else if (nextAsset.type == typeof(Video))
            {
                data[nextAsset.key] = Content.Load<Video>(nextAsset.location);
            }
            else if (nextAsset.type == typeof(Delegate))
			{
				nextAsset.assetLoader(Content);
			}

			index++;

			return false;
		}
        
		public static T Get<T>(string key)
		{
			return (T)data[key];
		}
        
		private static Texture2D LoadTextureStream(GraphicsDevice graphics, string loc, Game game)
		{
			Texture2D file = null;
			RenderTarget2D result = null;

			using (Stream titleStream = TitleContainer.OpenStream(game.Content.RootDirectory + "/" + loc + ".png"))
			{
				file = Texture2D.FromStream(graphics, titleStream);
			}

			//Setup a render target to hold our final texture which will have premulitplied alpha values
			result = new RenderTarget2D(graphics, file.Width, file.Height);

			graphics.SetRenderTarget(result);
			graphics.Clear(Color.Black);

			//Multiply each color by the source alpha, and write in just the color values into the final texture
			if (blendColor == null)
			{
				blendColor = new BlendState();
				blendColor.ColorWriteChannels = ColorWriteChannels.Red | ColorWriteChannels.Green | ColorWriteChannels.Blue;

				blendColor.AlphaDestinationBlend = Blend.Zero;
				blendColor.ColorDestinationBlend = Blend.Zero;

				blendColor.AlphaSourceBlend = Blend.SourceAlpha;
				blendColor.ColorSourceBlend = Blend.SourceAlpha;
			}

			SpriteBatch spriteBatch = new SpriteBatch(graphics);
			spriteBatch.Begin(SpriteSortMode.Immediate, blendColor);
			spriteBatch.Draw(file, file.Bounds, Color.White);
			spriteBatch.End();

			//Now copy over the alpha values from the PNG source texture to the final one, without multiplying them
			if (blendAlpha == null)
			{
				blendAlpha = new BlendState();
				blendAlpha.ColorWriteChannels = ColorWriteChannels.Alpha;

				blendAlpha.AlphaDestinationBlend = Blend.Zero;
				blendAlpha.ColorDestinationBlend = Blend.Zero;

				blendAlpha.AlphaSourceBlend = Blend.One;
				blendAlpha.ColorSourceBlend = Blend.One;
			}

			spriteBatch.Begin(SpriteSortMode.Immediate, blendAlpha);
			spriteBatch.Draw(file, file.Bounds, Color.White);
			spriteBatch.End();

			//Release the GPU back to drawing to the screen
			graphics.SetRenderTarget(null);

			return result as Texture2D;
		}
    }
}

