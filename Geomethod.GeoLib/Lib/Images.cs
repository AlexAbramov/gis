using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using Geomethod;

namespace Geomethod.GeoLib
{
	/// <summary>
	/// Summary description for Images.
	/// </summary>
	public class Images: IImageIndexer
	{
		static readonly string [] imageExtensions={".png",".gif",".jpg",".jpeg",".bmp"};
		GLib lib;
		Dictionary<string, Image> images = new Dictionary<string, Image>(Constants.imagesHashtableSize);
		public Images(GLib lib)
		{
			this.lib=lib;
		}
		public Image GetImage(string name)// throwable
		{
			if(images.ContainsKey(name)) return images[name] as Image;
			Image image=null;
			try
			{
				string filePath=Geomethod.PathUtils.BaseDirectory+"Images\\"+name;
				if(File.Exists(filePath))
				{
					image=Image.FromFile(filePath);
				}
				else if(!Path.HasExtension(name))
				{
					foreach(string ext in imageExtensions)
					{
						string path=filePath+ext;
						if(File.Exists(filePath))
						{
							image=Image.FromFile(filePath);
						}
					}
				}
			}
			finally
			{
				images[name]=image;
			}
			return image;
		}
		public void Clear()
		{
			images.Clear();
		}
		public int Count{get{return images==null ? 0 : images.Count;}}
	}
}
