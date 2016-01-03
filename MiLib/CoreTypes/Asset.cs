using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MiLib.CoreTypes
{
	public delegate void AssetLoad(ContentManager Content);

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

