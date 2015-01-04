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
using System.IO;

namespace MiLib.CoreTypes
{
	public static class AssetManager
	{
		/// <summary>
		/// Private list of assets, the first asset in the list is guarenteed to 
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


		#region Properties
		/// <summary>
		/// Returns the percent of assets which have been loaded as an integer 0 to 100
		/// </summary>
		public static int PercentLoaded
		{
			get
			{
				return (100 * index) / assets.Count;
			}
		}

		/// <summary>
		/// Returns true if all loading is complete
		/// </summary>
		public static bool Loaded
		{
			get
			{
				return index >= assets.Count;
			}
		}
		#endregion

		#region AssetManager Methods
		/// <summary>
		/// Loads the next asset in the list then returns, advancing the index by one step
		/// 
		/// Currently supports:
		/// Texture2D loading from raw PNG's for greater speed
		/// Model
		/// SpriteFont
		/// SoundEffect
		/// Delegate (Calls a custom worker method)
		/// 
		/// Note: This is where you would implement any additional asset types
		/// </summary>
		/// <param name="game">The game underwhich you are running</param>
		/// <returns>True if all assets have been loaded</returns>
		public static bool LoadOne(Game game)
		{
			if (index >= assets.Count) return true;

			Asset nextAsset = assets[index];

			if (nextAsset.type == typeof(Texture2D))
			{
				data[nextAsset.key] = LoadTextureStream(game.GraphicsDevice, nextAsset.location, game);
			}
			else if (nextAsset.type == typeof(Model))
			{
				data[nextAsset.key] = game.Content.Load<Model>(nextAsset.location);
			}
			else if (nextAsset.type == typeof(SpriteFont))
			{
				data[nextAsset.key] = game.Content.Load<SpriteFont>(nextAsset.location);
			}
			else if (nextAsset.type == typeof(SoundEffect))
			{
				data[nextAsset.key] = game.Content.Load<SoundEffect>(nextAsset.location);
			}
			else if (nextAsset.type == typeof(Delegate))
			{
				nextAsset.assetLoader(game);
			}

			index++;

			return false;
		}

		/// <summary>
		/// Returns an asset by its key
		/// </summary>
		/// <typeparam name="T">The type of this asset</typeparam>
		/// <param name="key">The string based key of the asset</param>
		/// <returns>The asset object representing this key</returns>
		public static T Get<T>(string key)
		{
			return (T)data[key];
		}

		/// <summary>
		/// LoadTextureStream method to speed up loading Texture2Ds from pngs,
		/// as described in 
		/// http://jakepoz.com/jake_poznanski__speeding_up_xna.html
		/// 
		/// Notice that the blend states are created once and stored statically in the AssetManager
		/// </summary>
		/// <param name="graphics">Graphics device to use</param>
		/// <param name="loc">Location of the image, root of the path is in the Content folder</param>
		/// <returns>A Texture2D with premultiplied alpha</returns>
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
		#endregion

		#region Game Loading Methods

		/// <summary>
		/// This method does any additional loading required before the first screen is
		/// ever presented to the user. Currently, it loads the assets needed to display a splash screen
		/// </summary>
		/// <param name="game"></param>
		/*private static void FirstLoad(Game game)
		{
			data["Background"] = LoadTextureStream(game.GraphicsDevice, "Background");
			data["mainFont"] = game.Content.Load<SpriteFont>("mainFont");   
		}*/

		/// <summary>
		/// This is an example use of a Delegate asset type, where we want not only
		/// load the background music, but start playing it
		/// </summary>
		/// <param name="game"></param>
		private static void BackgroundMusic(Game game)
		{
			//Uncomment this code to play some background music in your game
			//Sorry, I can't include any because of copyrights ;)
			/*
            SoundEffect soundEffect = game.Content.Load<SoundEffect>("BackgroundSong");
            SoundEffectInstance instance = soundEffect.CreateInstance();
            instance.IsLooped = true;
            instance.Play();*/
		}

		/// <summary>
		/// Example method to simulate some loading delay
		/// </summary>
		/// <param name="game"></param>
		private static void SimulateDelay(Game game)
		{
			System.Threading.Thread.Sleep(2000);
		}

		/// <summary>
		/// Another example of a Delegate asset type, where we play a little sound to the user
		/// at then end of asset loading
		/// </summary>
		/// <param name="game"></param>
		private static void FinishedLoading(Game game)
		{
			//Uncomment this line to play an evil laugh at the end of loading
			//Get<SoundEffect>("EvilLaugh").CreateInstance().Play();
		}
		#endregion
	}
}

