using System;
using System.Threading;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using Geomethod;
using Geomethod.Data;

//using ICSharpCode.SharpZipLib.Zip;

namespace Geomethod.GeoLib
{
/*	public class DefaultStyle
	{
		internal Style style=null;
		public DefaultStyle(){}
		public Pen Pen{get{return style!=null && style.pen!=null ? style.pen : Parameters.DefaultPen;}}
		public Brush Brush{get{return style!=null && style.brush!=null ? style.brush : Parameters.DefaultBrush;}}
		public TextStyle TextStyle{get{return style!=null && style.textStyle!=null ? style.textStyle : Parameters.DefaultTextStyle;}}
	}*/

	public class GLib: GComposite
    {
        #region Fields
        static int libCount=0;

		string defaultStyleStr="";
		IIndexer indexer;
		Scales scales;

		Colors colors;
		Images images;
		Layers layers;
		Views views;
		BgImages bgImages;
		IdGenerator idGenerator;
		CustomTables customTables;
		Selection selection=new Selection();

		int globalId;
        ConnectionFactory connFactory = null;
		Style defaultStyle=new Style();
		double unitMeasure=0.01;// [meter]
		bool visible=true;
		BitArray32 stateAttr = 0;

		Dictionary<int,GType> allTypes=null;
		Dictionary<int,GRange> allRanges = null;
        #endregion

        #region Properties
        public GConfig Config { get { return GConfig.Instance; } }
        public bool ReadOnly { get { return GetCommonAttr(CommonAttr.ReadOnly); } set { SetCommonAttr(CommonAttr.ReadOnly, value); UpdateAttr((int)TypeField.Attr); } }
		public bool Mirror { get { return attr[(int)LibAttr.Mirror]; } set { attr[(int)LibAttr.Mirror] = value; UpdateAttr((int)LibField.Attr); } }
		public Selection Selection{get{return selection;}}
		public bool Visible{get{return visible;}set{visible=value;}}
		protected override int NameFieldId{get{return (int)LibField.Name;}}
		protected override int StyleFieldId{get{return (int)LibField.Style;}}
		protected override int SMinFieldId{get{return (int)LibField.SMin;}}
		protected override int SMaxFieldId{get{return (int)LibField.SMax;}}
		public bool IsUpdated(LibField f) { return updateAttr[(int)f];}
		public override GComposite ParentComposite{get{return null;}}
		public override GLib Lib{get{return this;}}
		public Style DefaultStyle{get{return defaultStyle;}}
		public double UnitMeasure { get { return unitMeasure; } }
		public bool GetStateAttr(LibStateAttr a) {return stateAttr[(int)a]; }
		public bool IsChanged { get { return GetStateAttr(LibStateAttr.Changed); } }
		public bool AllObjectsLoaded { get { return GetStateAttr(LibStateAttr.AllObjectsLoaded); } }
		internal void SetStateAttr(LibStateAttr a,bool val){stateAttr[(int)a]=val;}
		internal void SetChanged() { SetStateAttr(LibStateAttr.Changed, true); }
		public void SetSaved() { SetStateAttr(LibStateAttr.Changed, false); }
		public int[] ScalesArray { get { return scales.Values; } set { scales.Values = value; UpdateAttr((int)LibField.Scales); } }
		public Layers Layers{get{return layers;}}
		public Views Views{get{return views;}}
		public BgImages BgImages{get{return bgImages;}}
        public ConnectionFactory ConnectionFactory { get { return connFactory; } set { connFactory = value; } }
		public bool HasDb{get{return connFactory!=null;}}
		public IIndexer Indexer{get{return indexer;}}
		public Scales Scales{get{return scales;}}
		public Colors Colors{get{return colors;}}
		public CustomTables CustomTables{get{return customTables;}}
		public Images Images{get{return images;}}
		public string DefaultStyleStr
		{
			get{return defaultStyleStr;}
			set
			{
				if(Lib.GetContext().SetStyle(value,ref defaultStyleStr,ref defaultStyle))
				{
					UpdateAttr((int)LibField.DefaultStyle);
				}
			}
		}
		public void SetBounds(Rect bounds)
		{
			if(this.bounds==bounds) return; 
			this.bounds=bounds;
			UpdateAttr((int)LibField.Code);
		}
		public ICollection<GRange> AllRanges{get{return allRanges.Values;}}
		public ICollection<GType> AllTypes{get{return allTypes.Values;}}

		internal void Register(GType type){if(allTypes==null) allTypes=new Dictionary<int,GType>(1<<10); allTypes.Add(type.Id,type);}
		internal void Register(GRange range){if(allRanges==null) allRanges=new Dictionary<int,GRange>(1<<16); allRanges.Add(range.Id,range);}
		internal void Unregister(GType type) { allTypes.Remove(type.Id); SetChanged(); SetStateAttr(LibStateAttr.AllObjectsLoaded,false);}
		internal void Unregister(GRange range) { allRanges.Remove(range.Id); SetChanged(); SetStateAttr(LibStateAttr.AllObjectsLoaded, false); }
		#endregion

		#region Construction
        public GLib(ConnectionFactory connFactory, Rect bounds, IIndexer indexer)
		{
			Init(connFactory);
			this.indexer= indexer==null ? new Indexer() : indexer;
			this.bounds=bounds;
			this.SMax =(int) (Math.Max(bounds.Width, bounds.Height) * unitMeasure*10);
			Scales.InitScales();
			updateAttr[Constants.updateAttrCreated]=true;
			SetChanged();
		}

        public GLib(ConnectionFactory connFactory)
		{
			Init(connFactory);
			if(HasDb) Load(BatchLevel.Object);
			SetStateAttr(LibStateAttr.Changed, false);
		}

       public GLib(ConnectionInfo connInfo)
        {
            Init(connInfo.CreateConnectionFactory());
//			foreach(string filePath in gisConnection.PreLoadFiles) LoadFile(filePath);
            if(HasDb) Load(BatchLevel.Object);
            foreach(string filePath in connInfo.GetFileList()) LoadFile(filePath);
            SetStateAttr(LibStateAttr.Changed, false);
        }

        void Init(ConnectionFactory connFactory)
		{
			globalId=libCount++;
			this.connFactory=connFactory;
			id=Constants.currentLib;
            int[] poolIds = { Constants.minRecordId};
            idGenerator = new IdGenerator(Constants.appPoolSize, Constants.poolSize, poolIds, connFactory);
			scales=new Scales(this);
			colors=new Colors(this);
			images=new Images(this);
			layers=new Layers(this);
			views=new Views(this);
			bgImages=new BgImages(this);
			customTables=new CustomTables(this);
		}
		#endregion

		#region Serialization
		public void Read(Context context, BinaryReader br)
		{
			id=br.ReadInt32();
//			UpdateGen(this);
			attr=br.ReadInt32();
			name=br.ReadString();
			context.SetStyle(br.ReadString(),ref styleStr,ref style);
			context.SetStyle(br.ReadString(),ref defaultStyleStr,ref defaultStyle);
			smin=br.ReadInt32();
			smax=br.ReadInt32();
			bounds=context.Buf.ReadRect(br);
			indexer=new Indexer(context.Buf.ReadIntArray(br));
			scales.Values=context.Buf.ReadIntArray(br);
			if(scales.Count==0) scales.InitScales();
		}

		public void Save(Context context)
		{
			if(context.ExportMode || updateAttr[Constants.updateAttrCreated])
			{
				GmCommand cmd=context.TargetConn.CreateCommandById("insertIntoGisLib");
				cmd.AddInt("Id",id);
				cmd.AddInt("Attr",attr);
				cmd.AddString("Name",name,MaxLength.Name);
				cmd.AddString("Style",styleStr,MaxLength.Style);

//      DZ      14.01.08
//                if( defaultStyleStr == "" )
//                    cmd.AddString( "DefaultStyle", /*defaultStyleStr*/ " ", MaxLength.Style );
//                else
				    cmd.AddString("DefaultStyle",defaultStyleStr,MaxLength.Style);
				cmd.AddInt("SMin",smin);
				cmd.AddInt("SMax",smax);
				cmd.AddBinary("Code",context.Buf.SetRect(bounds));
				cmd.AddBinary("IndexerCode",context.Buf.SetIntArray(indexer.ToIntArray()));
				cmd.AddBinary("Scales",context.Buf.SetIntArray(scales.Values));
				cmd.ExecuteNonQuery();
			}
			else if(updateAttr.NonEmpty)
			{
				GmCommand cmd=context.Conn.CreateCommand();
				string cmdText="";
				if(updateAttr[(int)LibField.Attr])
				{
					cmdText+="Attr= @Attr,";
					cmd.AddInt("Attr",attr);
				}
				if(updateAttr[(int)LibField.Name])
				{
					cmdText+="Name= @Name,";
					cmd.AddString("Name",name,MaxLength.Name);
				}
				if(updateAttr[(int)LibField.Style])
				{
					cmdText+="Style= @Style,";
					cmd.AddString("Style",styleStr,MaxLength.Style);
				}
				if(updateAttr[(int)LibField.DefaultStyle])
				{
					cmdText+="DefaultStyle= @DefaultStyle,";
					cmd.AddString("DefaultStyle",defaultStyleStr,MaxLength.Style);
				}
				if(updateAttr[(int)LibField.SMin])
				{
					cmdText+="SMin= @SMin,";
					cmd.AddInt("SMin",smin);
				}
				if(updateAttr[(int)LibField.SMax])
				{
					cmdText+="SMax= @SMax,";
					cmd.AddInt("SMax",smax);
				}
				if(updateAttr[(int)LibField.Code])
				{
					cmdText+="Code= @Code,";
					cmd.AddBinary("Code",context.Buf.SetRect(bounds));
				}
				if(updateAttr[(int)LibField.IndexerCode])
				{
					cmdText+="IndexerCode= @IndexerCode,";
					cmd.AddBinary("IndexerCode",context.Buf.SetIntArray(indexer.ToIntArray()));
				}
				if(updateAttr[(int)LibField.Scales])
				{
					cmdText+="Scales= @Scales,";
					cmd.AddBinary("Scales",context.Buf.SetIntArray(scales.Values));
				}
				Geomethod.StringUtils.RemoveLastChar(ref cmdText);
				cmd.CommandText="update gisLib set "+cmdText+" where Id= @Id";
				cmd.AddInt("Id",id);
				cmd.ExecuteNonQuery();
			}
			if(!context.ExportMode) updateAttr=0;

			if(context.Filter!=null && context.Filter.Includes(BatchLevel.Type))
			{
				if(types!=null) foreach(GType type in types) type.Save(context); 
			}

			if(context.ExportMode) idGenerator.Save(context.TargetConn);
			colors.Save(context);
			layers.Save(context);
			views.Save(context);
			bgImages.Save(context);
			customTables.Save(context);
		}
		public void Save(Filter filter)
		{
			using(Context context=GetContext())
			{
				context.Filter=filter;
				Save(context);
			}
		}
		public void Save(BatchLevel batchLevel){Save(new Filter(batchLevel));}

		void WriteLib(Context context, BinaryWriter bw)
		{
			// fields
			bw.Write((int)ClassId);
			id=Constants.currentLib;
			bw.Write(id);
			bw.Write(attr);
			bw.Write(name);
			bw.Write(styleStr);
			bw.Write(defaultStyleStr);
			bw.Write(smin);
			bw.Write(smax);
			context.Buf.WriteRect(bw,bounds);
			context.Buf.WriteIntArray(bw,indexer.ToIntArray());
			context.Buf.WriteIntArray(bw,scales.Values);
		}

		public void Write(Context context, BinaryWriter bw)
		{
			WriteLib(context, bw);
			bw.Write((int)ClassId.IdGenerator);
			idGenerator.Write(bw);
			// collections
			colors.Write(context,bw);
			layers.Write(context,bw);
			views.Write(context,bw);
			bgImages.Write(context,bw);
			// types
			if(context.Filter!=null && context.Filter.Includes(BatchLevel.Type))
			{
				if(types!=null) foreach(GType type in types) type.Write(context,bw); 
			}
			// custom tables
			customTables.Write(context,bw);
			bw.Write((int)ClassId.None);// end of stream
		}
		#endregion

		#region Load
		public void LoadFile(string filePath)
		{
			string absFilePath=Geomethod.PathUtils.AbsFilePath(filePath);
			string ext=System.IO.Path.GetExtension(filePath).ToLower();
			switch(ext)
			{
/*				case ".wdz":
				case ".zip":
					using(ZipInputStream zis = new ZipInputStream(File.OpenRead(absFilePath)))
					{
						ZipEntry zipEntry;
						while((zipEntry=zis.GetNextEntry())!=null) 
						{
							if(zipEntry.IsDirectory) continue;
							BinaryReader br=new BinaryReader(zis);
							Load(br);
						}
					}
					break;*/
				default:
					using(FileStream fs=File.OpenRead(absFilePath))
					{
						BinaryReader br=new BinaryReader(fs);
						Load(br);
					}
					break;
			}
		}

		public void Load(BinaryReader br)
		{
			using(Context context=GetContext())
			{
				bool cont=true;
				while(cont)
				{
					int cmd=br.ReadInt32();
					switch((ClassId)cmd)
					{
						case ClassId.Object: GObject.Create(context,br); break;
						case ClassId.Range: new GRange(context,br); break;
						case ClassId.Type: new GType(context,br); break;
						case ClassId.Color: colors.Add(new NamedColor(br)); break;
						case ClassId.View: views.Add(new View(context,br)); break;
						case ClassId.BgImage: bgImages.Add(new BgImage(context,br)); break;
						case ClassId.Layer: layers.Add(new Layer(context,br)); break;
						case ClassId.Lib: Read(context,br); break;
						case ClassId.IdGenerator: idGenerator.Read(br); break;
						case ClassId.CustomTables: customTables.Read(context,br); break;
						case ClassId.None: cont=false; break;
						default: throw new Exception("Unexpected command: "+cmd.ToString());
					}
				}
			}
			SortAll();
		}

		public void Load(BatchLevel level)
		{
			using(Context context=GetContext())
			{
				context.Filter=new Filter(level);
				Load(context);
			}
		}

		public GObject LoadObject(int objectId)
		{
			if(AllObjectsLoaded) throw new Exception("Object already loaded.");
			using(Context context=this.GetContext())
			{
				GmCommand cmd=context.Conn.CreateCommandById("selectAllFromGisObjectsWhereId");
				cmd.AddInt("Id",id);
				using(IDataReader dr=cmd.ExecuteReader())
				{
					if(dr.Read())
					{
						return GObject.Create(context,null,dr,false);
					}
				}
			}
			return null;
		}

		void LoadRanges(Context context)
		{
			if(allTypes==null) throw new Exception("Types not loaded.");
//      DZ      16.01.09
//			int count=(int)context.Conn.ExecuteScalarById("selectCountFromGisRanges");
            int count = Convert.ToInt32( context.Conn.ExecuteScalarById( "selectCountFromGisRanges" ) );
			if(allRanges==null) allRanges=new Dictionary<int,GRange>(count);
			using(IDataReader dr=context.Conn.ExecuteReaderById("selectAllFromGisRanges"))
			{
				while(dr.Read())
				{
					GRange range=new GRange(context,dr);
				}
			}
		}
		
		void LoadObjects(Context context)
		{
			if(allTypes==null) throw new Exception("Types not loaded");
			if(allRanges==null) throw new Exception("Ranges not loaded");
			if(HasDb)
			{
				foreach(GRange range in allRanges.Values) range.SetLoaded();
				using(IDataReader dr=context.Conn.ExecuteReaderById("selectAllFromGisObjects"))
				{
					while(dr.Read())
					{
						GObject.Create(context,null,dr,true);
					}
				}
			}
			SetStateAttr(LibStateAttr.AllObjectsLoaded, true);
		}

		void Load(Context context)
		{
			if(!HasDb) return;
			colors.Load(context);
			layers.Load(context);
			views.Load(context);
			bgImages.Load(context);
			LoadLib(context);
			if(context.Filter==null) return;
			if(context.Filter.Includes(BatchLevel.Type)) LoadTypes(context);
			if(context.Filter.Includes(BatchLevel.Range)) LoadRanges(context);			
			if(context.Filter.Includes(BatchLevel.Object)) LoadObjects(context);
			idGenerator.Load(context.Conn);
			customTables.Load(context);
		}

		void LoadLib(Context context)
		{
			int[] indexerCode;
			GmCommand cmd=context.Conn.CreateCommandById("selectAllFromGisLibWhereId");
			cmd.AddInt("Id",Constants.currentLib);
			using(IDataReader dr=cmd.ExecuteReader())
			{
				if(!dr.Read()) throw new GeoLibException("Lib record not found.");
				attr=dr.GetInt32((int)LibField.Attr);
				name=dr.GetString((int)LibField.Name);
//      DZ  16.01.09
//                context.SetStyle(dr.GetString((int)LibField.Style),ref styleStr,ref style);
                context.SetStyle(dr.IsDBNull( (int)LibField.Style ) ? "" : dr.GetString( (int)LibField.Style ), ref styleStr, ref style );
//				context.SetStyle(dr.GetString((int)LibField.DefaultStyle),ref defaultStyleStr,ref defaultStyle.style);
                context.SetStyle(dr.IsDBNull( (int)LibField.DefaultStyle ) ? "" : dr.GetString( (int)LibField.DefaultStyle ), ref defaultStyleStr, ref defaultStyle );
				smin=dr.GetInt32((int)LibField.SMin);
				smax=dr.GetInt32((int)LibField.SMax);
				bounds=context.Buf.GetRect(dr,(int)LibField.Code);
				indexerCode=context.Buf.GetIntArray(dr,(int)LibField.IndexerCode);
				scales.Values=context.Buf.GetIntArray(dr,(int)LibField.Scales);
			}
			if(scales.Count==0) scales.InitScales();
			id=Constants.currentLib;
			indexer=new Indexer(indexerCode);
		}

		void LoadTypes(Context context)
		{
//      DZ      16.01.09
//			int count=(int)context.Conn.ExecuteScalarById("selectCountFromGisTypes");
            int count = Convert.ToInt32( context.Conn.ExecuteScalarById( "selectCountFromGisTypes" ) );
			if(allTypes==null) allTypes=new Dictionary<int,GType>(count);
			using(IDataReader dr=context.Conn.ExecuteReaderById("selectAllFromGisTypes"))
			{
				while(dr.Read())
				{
					new GType(context,dr);
				}
			}
			foreach(GType type in allTypes.Values)
			{
				int parentId=type.ParentId;
				GType par = parentId>0 ? (GType)allTypes[parentId] : null;
				type.SetParent(par);
			}
			SortAll();
		}

		public void Clear()
		{
			SetStateAttr(LibStateAttr.Changed, false);
			SetStateAttr(LibStateAttr.AllObjectsLoaded, false);
			if(types!=null)
			{
				foreach(GType type in types) type.Clear();
				types.Clear();
				types=null;
			}
			if(allTypes!=null)
			{
				allTypes.Clear();
			}
			if(allRanges!=null)
			{
				allRanges.Clear();
			}
			colors.Clear();
			layers.Clear();
			views.Clear();
			bgImages.Clear();
			images.Clear();
			scales.Clear();
			selection.Clear();
			idGenerator.Clear();
			customTables.Clear();
			LocalDataStoreSlot slot=Thread.GetNamedDataSlot(globalId.ToString());
			Context context=Thread.GetData(slot) as Context;
			if(context!=null)
			{
				Thread.SetData(slot,null);
				context.Dispose();
				context=null;
			}
		}

		public void Unload()
		{
			if(types!=null)
			{
				foreach(GType type in types) type.Unload();
			}
		}
		#endregion

		#region IShapedObject
		public override ClassId ClassId{get{return ClassId.Lib;}}
		public override void Draw(Map map)
		{
			if(visible && Contains(map.Scale))
			{
				if(map.Includes(Id)) map.DrawRect(style,bounds);
				foreach(BgImage bi in bgImages) bi.Draw(map);
                if (map.Includes(BatchLevel.Type))
                {
                    if (types != null) foreach (GType type in types) type.Draw(map);
                }
                selection.Draw(map);
			}
		}
		#endregion

		#region Iterations

		public override void Visit(IVisitor vis)
		{
			if(vis.Visit(this))
			{
				foreach(GType type in types) type.Visit(vis);
			}
		}
		#endregion

		#region Utils
		public int GenerateId(ISerializableObject ser,ref BitArray32 updateAttr)
		{
			updateAttr[Constants.updateAttrCreated]=true;
			SetChanged();
			return idGenerator.GetId(ser.ClassId);
		}
//		public void UpdateGen(ISerializable ser){	if(!HasDb) idGenerator.Update(ser);}
		public Context GetContext()
		{
			LocalDataStoreSlot slot=Thread.GetNamedDataSlot(globalId.ToString());
			Context context=Thread.GetData(slot) as Context;
			if(context!=null)
			{
				if(context.Lib==this) context.ResetState();
				else
				{
					context.Dispose();
					context=null;
				}
			}
			if(context==null)
			{
				context=new Context(this);
				Thread.SetData(slot,context);
			}
			return context;
		}
		public GType GetType(int typeId){return allTypes==null? null: allTypes[typeId] as GType;}
		public GRange GetRange(int rangeId){return allRanges==null? null: allRanges[rangeId] as GRange;}
		public GObject GetObject(int objectId, int rangeId){GRange range=GetRange(rangeId); return range==null ? null : range.GetObject(objectId); }
		#endregion
	}
}