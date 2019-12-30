using System;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using Geomethod;
using Geomethod.Data;

namespace Geomethod.GeoLib
{
	[XmlRootAttribute(ElementName = "Root")]
	public class GisConnections
	{
		[XmlArray(ElementName = "GisConnections")]
		[XmlArrayItem(typeof(GisConnection))]
		public ArrayList gisConnections=new ArrayList();

		public int Count{get{return gisConnections.Count;}}
		public GisConnection GetItem(int index){return (GisConnection)gisConnections[index];}

		public GisConnections(){}

		public static GisConnections Load(string filePath)
		{
			XmlSerializer xs = new XmlSerializer(typeof(GisConnections));
			using(TextReader reader = new StreamReader(filePath))
			{
				return (GisConnections)xs.Deserialize(reader);
			}
		}

		public void Save(string filePath)
		{
			XmlSerializer xs = new XmlSerializer(typeof(GisConnections));
			using(TextWriter writer = new StreamWriter(filePath))
			{
				xs.Serialize(writer, this);
			}
		}
		public bool HasName(string name)
		{
			foreach(GisConnection gisConnection in gisConnections)
			{
				if(gisConnection.Name==name) return true;
			}
			return false;
		}

		public void Add(GisConnection gisConnection)
		{
			gisConnections.Add(gisConnection);
		}

		public void Remove(GisConnection gisConnection)
		{
			gisConnections.Remove(gisConnection);
		}

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			// TODO:  Add GisConnections.GetEnumerator implementation
			
			return gisConnections.GetEnumerator();
		}

		#endregion
	}
}
