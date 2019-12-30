using System;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using Geomethod;
using Geomethod.Data;

namespace Geomethod.GeoLib
{
	public class GisConnection
    {
        #region Fields
        string name="";
		string providerName="";
		string connectionString="";
		string filePath="";
		string options="";
		ArrayList preLoadFiles=new ArrayList();
		ArrayList postLoadFiles=new ArrayList();
        #endregion

        #region Properties
        [XmlAttribute]
        public string Name { get { return name; } set { name = Geomethod.StringUtils.NotNullString(value).Trim(); } }
        [XmlAttribute]
        public string ProviderName{get{return providerName;}set{providerName=Geomethod.StringUtils.NotNullString(value).Trim();}}
        [XmlAttribute]
        public string ConnectionString{get{return connectionString;}set{connectionString=Geomethod.StringUtils.NotNullString(value).Trim();}}
        [XmlIgnore]
        public string FilePath{get{return filePath;}}
        [XmlAttribute]
        public string Options{get{return options;}set{options=Geomethod.StringUtils.NotNullString(value);ParseOptions();}}
		[XmlIgnore]
		public ICollection PreLoadFiles{get{return preLoadFiles;}}
		[XmlIgnore]
		public ICollection PostLoadFiles{get{return postLoadFiles;} }
        [XmlIgnore]
        public GmProviderFactory ProviderFactory { get { return providerName.Length > 0 ? GmProviders.Get(providerName) : null; } }
        [XmlIgnore]
        bool IsDbConnection { get { return GmProviders.HasProvider(providerName); } }
        #endregion

        #region Construction
        public GisConnection(){}
		public GisConnection(string name) { this.name = name; }
		public GisConnection(string name, string filePath)
		{
			Name = name;
			Options = "file=" + filePath;
		}
		public GisConnection(string name,string providerName,string connStr,string options)
		{
			Name=name;
			ProviderName=providerName;
			ConnectionString=connStr;
			Options=options;
        }
        #endregion

        #region Methods
        public GmConnection CreateConnection()
        {
            if (connectionString.Length == 0 || providerName.Length == 0) return null;
            GmProviderFactory prov = GmProviders.Get(providerName);
            return prov.CreateConnection(connectionString);
        }
        public ConnectionFactory CreateConnectionFactory()
		{
			if(connectionString.Length==0 || providerName.Length==0) return null;
			GmProviderFactory prov=GmProviders.Get(providerName);
			return prov.CreateConnectionFactory(connectionString);
	    }
		void ParseOptions()
		{
			filePath="";
			preLoadFiles.Clear();
			postLoadFiles.Clear();
			foreach(string p in options.Split(';'))
			{
				int i=p.IndexOf('=');
				if(i>=0) AddParam(p.Substring(0,i).Trim(),p.Substring(i+1).Trim());
				else AddParam(p.Trim(),null);
			}
		}
		void AddParam(string key,string val)
		{
			switch(key.ToLower())
			{
				case "preloadfile":
					if(val!=null && val.Length>0) preLoadFiles.Add(val);
					break;
				case "postloadfile":
					if(val!=null && val.Length>0) postLoadFiles.Add(val);
					break;
				case "file":
					if(val!=null && val.Length>0) filePath=val;
					break;
			}
		}
        #endregion
    }

}
