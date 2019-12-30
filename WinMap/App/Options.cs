using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Geomethod;

namespace WinMap
{
	public enum Language{Autodetect,English,Russian}

	[XmlRootAttribute(ElementName = "Options")]
	public class Options
	{
		public static string FilePath{get{return PathUtils.BaseDirectory+"Options\\Options.xml";}}
		Language language=Language.Autodetect;
		[XmlIgnore]
		bool changed=false;

//		[XmlElement(ElementName = "LogFileName")]
//		public string logFilePath=CommonLib.Utils.BaseDirectory+"WinMap.log";

		public Language Language{get{return language;}set{language=value;changed=true;}}
		public bool Changed{get{return changed;}}
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

		public Options()
		{
		}

		public static Options Load()
		{
			Options res;
			XmlSerializer xs = new XmlSerializer(typeof(Options));
			using(TextReader reader = new StreamReader(FilePath))
			{
				res=(Options)xs.Deserialize(reader);
			}
			res.changed=false;
			return res;
		}

		public void Save()
		{
			XmlSerializer xs = new XmlSerializer(typeof(Options));
			using(TextWriter writer = new StreamWriter(FilePath))
			{
				xs.Serialize(writer, this);
			}
			changed=false;
		}
	}
}
