using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Text;
using System.IO;
using System.Collections;
using Geomethod;
using Geomethod.GeoLib;


namespace Geomethod.GeoLib.Converters
{
	public class HtmlGenerator
	{
		const string newline = "\r\n";
		StringBuilder sb = new StringBuilder(1 << 12);
		public HtmlGenerator()
		{
		}
        public void Generate(BufferedMap map, string filePath)
		{
			sb.Length = 0;
			Image image = map.Image;
			string imgFilePath = filePath + ".png";
			string imgFileName = Path.GetFileName(imgFilePath);
			image.Save(imgFilePath, ImageFormat.Png);
			ArrayList ar = new ArrayList(1 << 8);
			Hashtable ht = new Hashtable();
			ht.Add(typeof(GPolygon).Name, null);
			VisibleObjectsVisitor vis = new VisibleObjectsVisitor(map, ar, ht);
			map.Lib.Visit(vis);
			WriteLine("<html>");
			WriteLine("<head>");
			WriteLine("<script>");
			WriteLine("function onareaclick(objectName){alert(objectName);}");
			WriteLine("function showtip(text)");
			WriteLine("{");
			WriteLine("  if(document.all && document.readyState=='complete')");
			WriteLine("  {");
			WriteLine("    document.all.tooltip.innerHTML=text;");
			WriteLine("    document.all.tooltip.style.pixelLeft=event.clientX+document.body.scrollLeft+10;");
			WriteLine("    document.all.tooltip.style.pixelTop=event.clientY+document.body.scrollTop+10;");
			WriteLine("    document.all.tooltip.style.visibility='visible';");
			WriteLine("  }");
			WriteLine("}");
			WriteLine("function hidetip(){if (document.all) document.all.tooltip.style.visibility='hidden';}");
			WriteLine("</script>");
			WriteLine("</head>");
			WriteLine("<body>");
			WriteLine("<div id=tooltip style='color:#003399;position:absolute;visibility:hidden;background-color:Aliceblue;width=200;'></div>");
			WriteLine("<MAP name=Map0>");
			foreach (IShapedObject obj in vis.List)
			{
				WriteLine((GPolygon)obj, map);
			}
			WriteLine("</MAP>");
			WriteLine("<img width={0} height={1} src='{2}' useMap=#Map0 border=0>", image.Width, image.Height, imgFileName);
			WriteLine("</body>");
			WriteLine("</html>");
			using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
			{
				sw.Write(sb.ToString());
			}
		}
		void Write(string s) { sb.Append(s); }
		void Write(string s, params object[] args) { sb.AppendFormat(s, args); }
		void WriteLine(string s) { Write(s); Write(newline); }
		void WriteLine(string s, params object[] args) { Write(s, args); Write(newline); }
		void WriteLine(GPolygon obj, Map map)
		{
			Point[] pp = (Point[])obj.Points.Clone();
			map.WToG(pp);
			string objName = ToHtmlString(obj.Name);
			Write("<AREA href=javascript:onareaclick('{0}') onmouseover=showtip('{0}') onmouseout=hidetip() shape=POLY coords=", objName);
			for (int i = 0; i < pp.Length; i++)
			{
				if (i > 0) Write(",");
				Point p = pp[i];
				Write("{0},{1}", p.X, p.Y);
			}
			WriteLine(">", objName);
		}
		public static string ToHtmlString(string s)
		{
			s.Replace(" ", "&nbsp;");
			return s;
		}
	}
}
