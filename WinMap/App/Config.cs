using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Geomethod;

namespace WinMap
{
	[XmlRootAttribute]
	public class Config: BaseConfig
	{
        public override int CurrentVersion { get { return 1; } }
        public static string FilePath { get { return PathUtils.BaseDirectory + "Config\\Config.xml"; } }
        #region Static
        public static Config Load() { return (Config)BaseConfig.DeserializeFile(typeof(Config), FilePath); }
        #endregion

        #region Fields
		public string defaultFileName = "Map";
        public int testObjectCount = 100;
        public int debug = 1;
        #endregion
        //		bool changed=false;

//		[XmlElement(ElementName = "LogFileName")]
//		public string logFilePath=CommonLib.Utils.BaseDirectory+"WinMap.log";

//		public Language Language { get { return language; } set { language = value; changed = true; } }
//		public string DefaultFileName { get { return defaultFileName; } set { defaultFileName = value; changed = true; } }
//		public bool Changed{get{return changed;}}
//		[XmlIgnore]
/*		public StringDictionary serverInstances = new StringDictionary();
		public string[] XmlServerInstances
		{
			get
			{
				StringCollection keys = new StringCollection(serverInstances.Keys);
				StringCollection vals = new StringCollection(serverInstances.Values);
				StringCollection items = new StringCollection();
				for(int i=0;i<keys.Count;i++)
				{
					items.Add(keys[i]);
					items.Add(vals[i]);
				}
				return (string[])items.ToArray(typeof(string));
			}
			set
			{
				ArrayList items=new ArrayList(value);
				for(int i=0;i<items.Count-1;i+=2)
				{
					serverInstances.Add(items[i],items[i+1]);
				}
			}
		}*/

		public Config()
		{
		}

		public void Save(){base.Serialize(FilePath,false);}

        public override object Clone()
        {
            var obj = (Config)this.MemberwiseClone();
            return obj;
        }
	}
}
