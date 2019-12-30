using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Geomethod;

namespace Geomethod.GeoLib
{
    [XmlRootAttribute]
    public class GConfig : BaseConfig
    {
        public override int CurrentVersion{get { return 1; }}

		#region Static
        static GConfig instance;
        static GConfig()
        {
            instance = new GConfig();
            instance.styles.Init();
        }
        public static GConfig Instance { get {return instance; } }
		public static GConfig Load(string filePath) { return (GConfig)BaseConfig.DeserializeFile(typeof(GConfig), filePath); }
		#endregion

        #region Fields
        public GeometryConfig geometry = new GeometryConfig();
        public StylesConfig styles = new StylesConfig();
        #endregion

        #region Properties
        #endregion

		#region Construction
		public GConfig()
		{
		}
		#endregion

        public override object Clone()
        {
            GConfig obj = (GConfig)this.MemberwiseClone();
            obj.geometry = (GeometryConfig)this.geometry.Clone();
            obj.styles = (StylesConfig)this.styles.Clone();
            return obj;
        }
    }

    public class GeometryConfig
    {
        public int maxFormWidth = 1024;
        public int pointRadius = 2;
        public int selRadius = 5;
        public int minSizeVisible = 3;
        public int searchRadius = 10;
        public double searchRadiusCm = 0.25f;
        public GeometryConfig()
        {
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

}
