using System;
using System.Drawing;
using System.Text;
using System.IO;
using System.Collections;
using System.Data;
using System.Reflection;
using Geomethod.Data;

namespace Geomethod.GeoLib
{
	public class MapCreator
    {
        #region Create GIS DB
        public static ConnectionInfo CreateGisDb(DbCreationProperties props)
		{
            string providerName=props.providerName;
			GmProviderFactory pr = GmProviders.Get(providerName);
			if (pr == null) throw new GeoLibException("MapCreator. Provider not found: "+providerName);
			string connStr = pr.CreateDatabase(props);
			UpdateDb(pr.CreateConnectionFactory(connStr));
            return new ConnectionInfo(props.dbName, providerName, connStr, "");
		}
        public static void UpdateDb(ConnectionFactory connectionFactory)
		{
			Assembly assembly = Assembly.GetAssembly(typeof(Geomethod.GeoLib.GLib));
            UpdateScripts.UpdateDb(connectionFactory, assembly, "Geomethod.GeoLib.Resources.UpdateScripts.sql");
        }
        #endregion

        #region Aux
        static int createTestObjectsCount = 0;
		public static void CreateTestObjects(GLib lib, int count)
		{
            createTestObjectsCount++;
			// Type
			GType type = new GType(lib, GeomType.Polygon);
            type.Name = "Polygon test"+createTestObjectsCount.ToString();
			type.StyleStr = "pc=blue;bc=lightgreen;";
			Rect bounds = lib.Bounds;
			int libSize = (int)lib.Bounds.MinSize;

			// Objects
			Random r = new Random();
			for (int n = 0; n < count; n++)
			{
				int maxSize = libSize / 10;
				int x = bounds.left + r.Next((int)bounds.Width - maxSize);
				int y = bounds.bottom + r.Next((int)bounds.Height - maxSize);
				Point[] points = new Point[3];
				for (int i = 0; i < points.Length; i++)
				{
					points[i].X = x + r.Next(maxSize);
					points[i].Y = y + r.Next(maxSize);
				}
				GPolygon polygon = new GPolygon(type, points);
				polygon.Caption = n.ToString();
			}
        }
        #endregion
    }
}
