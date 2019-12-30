using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Text;
using System.IO;
using System.Collections;
using Geomethod;
using Geomethod.GeoLib;

namespace Geomethod.GeoLib.Windows.Forms
{
	public class FileFilter
	{
		// misc
		public const string htm="HTML (*.htm)|*.htm;*.html";
		// vector
		public const string wdr="WinMap Drawing (*.wdr)|*.wdr";
        public const string shp = "Shape files (*.shp)|*.shp";
        public const string mif = "MIF files (*.mif)|*.mif";
        public const string dxf = "AutoCAD DXF files (*.dxf)|*.dxf";
		public const string zip="Archives (*.zip)|*.zip";
        public static string SupportedVectorFormats
        {
            get
            {
                return Locale.Get( "_allvectorfiles" ) + "|*.wdr;*.shp;*.mif;*.dxf;*.zip";
            }
        }
		public static string VectorFilter{get{string[] items={SupportedVectorFormats,wdr,shp,mif,dxf,zip}; return GetString(items);}}
		// raster
		public const string gif="GIF (*.gif)|*.gif";
		public const string jpg="JPEG (*.jpg;*.jpeg;)|*.jpg;*.jpeg";
		public const string png="PNG (*.png)|*.png";
		public const string bmp="BITMAP (*.bmp;*.dib;*.rle)|*.bmp;*.dib;*.rle";
		public const string tif="TIFF (*.tif;*.tiff)|*.tif;*.tiff";
		public static string ImagesCollection{get{string[] ss={gif,jpg,png,bmp,tif};return GetString(ss);}}
		public static string SupportedImages{get{return Locale.Get("_allimages")+"|*.gif;*.jpg;*.jpeg;*.png;*.bmp;*.dib;*.rle;*.tif;*.tiff";}}
		public static string ImagesFilter{get{string[] items={SupportedImages,ImagesCollection};return FileFilter.GetString(items);}}
		// utils
		public static string GetString(string[] ss)
		{
			string res="";
			foreach(string s in ss)
			{
				if(res.Length>0) res+='|';
				res+=s;
			}
			return res;
		}
  }
}
