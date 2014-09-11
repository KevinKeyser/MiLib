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
	public delegate void AssetLoad(Game game);

	public class Asset
	{
		public string key, location;
		public Type type;
		public AssetLoad assetLoader;

		public Asset(string key, string location, Type type)
		{
			this.key = key;
			this.location = location;
			this.type = type;
		}

		public Asset(AssetLoad assetLoader)
		{
			this.assetLoader = assetLoader;
			this.type = typeof(Delegate);
		}
	}
}

