using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Collections;
using Geomethod;
using Geomethod.Data;

namespace Geomethod.GeoLib
{
	public class BgImage : IComparable, IShapedObject
	{
		const int intCodeCount=4;

		GLib lib;
		int id;
		BitArray32 attr=0;
		string filePath="";
		string styleStr="";
		Style style=null;
		string textAttr="";
		int smin=0;
		int smax=0;
		//code
		int x=0;
		int y=0;
	    float scale=0;// pixel scale
		float angle=0;
		BitArray32 updateAttr=0;

		public bool isChecked=false;
		ImageAttributes ia = new ImageAttributes();
		ColorMatrix cm = new ColorMatrix();
		string loadFilePath=null;
		Image image=null;
		protected void UpdateAttr(BgImageField f){updateAttr[(int)f]=true;lib.SetChanged();}
//		public bool IsUpdated(BgImageField f) { return updateAttr[(int)f]; }

		public int Id{get{return id;}}
		public string Name{get{return Path.GetFileName(filePath);}}
		public string FilePath
		{
			get{return filePath;}
			set
			{
				if(filePath==value) return; 
				filePath=Geomethod.PathUtils.TrimFilePath(value);				
				UpdateAttr(BgImageField.FilePath);
			}
		}
		public int SMin{get{return smin;}set{if(smin==value)return; smin=value; UpdateAttr(BgImageField.SMin);}}
		public int SMax{get{return smax;}set{if(smax==value)return; smax=value; UpdateAttr(BgImageField.SMax);}}
		public ClassId ClassId{get{return ClassId.BgImage;}}
		public string StyleStr
		{
			get{return styleStr;}
			set
			{
				if(lib.GetContext().SetStyle(value,ref styleStr,ref style))
				{
					UpdateAttr(BgImageField.Style);
				}
			}
		}
/*		public int Opacity
		{
			get{return opacity;}
			set
			{
				if(opacity==value)return; opacity=value; 
				cm.Matrix33=opacity*0.01f;
				ia.SetColorMatrix(cm);
				UpdateAttr(BgImageField.Opacity);
			}
		}*/
		public int X{get{return x;}set{if(x==value)return; x=value; UpdateAttr(BgImageField.Code);}}
		public int Y{get{return y;}set{if(y==value)return; y=value; UpdateAttr(BgImageField.Code);}}
		public float Scale{get{return scale;}set{if(scale==value)return; scale=value; UpdateAttr(BgImageField.Code);}}
		public float Angle{get{return angle;}set{if(angle==value)return; angle=value; UpdateAttr(BgImageField.Code);}}

		#region Construction
		public BgImage(Map map)
		{
			this.lib=map.Lib;
			id=lib.GenerateId(this,ref updateAttr);
			X=map.Pos.X;
			Y=map.Pos.Y;
			Scale=(float)map.PixelScale;
		}
	
		internal BgImage(Context context, IDataReader dr)
		{
			lib=context.Lib;
			id=dr.GetInt32((int)BgImageField.Id);
			attr=dr.GetInt32((int)BgImageField.Attr);
			context.SetStyle(dr.GetString((int)BgImageField.Style),ref styleStr,ref style);
			textAttr=dr.GetString((int)BgImageField.TextAttr);
			filePath=dr.GetString((int)BgImageField.FilePath);
			smin=dr.GetInt32((int)BgImageField.SMin);
			smax=dr.GetInt32((int)BgImageField.SMax);
			Init(context.Buf.GetIntArray(dr,(int)BgImageField.Code));
		}

		internal BgImage(Context context, BinaryReader br)
		{
			lib=context.Lib;
			id=br.ReadInt32();
//			lib.UpdateGen(this);
			attr=br.ReadInt32();
			context.SetStyle(br.ReadString(),ref styleStr,ref style);
			textAttr=br.ReadString();
			filePath=br.ReadString();
			smin=br.ReadInt32();
			smax=br.ReadInt32();
			Init(context.Buf.ReadIntArray(br));
		}
		#endregion

		void Init(int[] intArray)
		{
			int i=0;
			x=intArray[i++];
			y=intArray[i++];
			scale = Geomethod.BufferUtils.IntToFloat(intArray[i++]);
			angle = Geomethod.BufferUtils.IntToFloat(intArray[i++]);
		}

		int[] GetIntArray()
		{
			int[] intArray=new int[intCodeCount];
			int i=0;
			intArray[i++]=x;
			intArray[i++]=y;
			intArray[i++] = Geomethod.BufferUtils.FloatToInt(scale);
			intArray[i++] = Geomethod.BufferUtils.FloatToInt(angle);
			return intArray;
		}

		public int CompareTo(object obj)
		{
			BgImage bgImage= obj as BgImage;
			if(bgImage==null) return -1;
			return Name.CompareTo(bgImage.Name);
		}
			
		public void Remove(Context context)
		{
			if(id<0) return;
			GmCommand cmd=context.Conn.CreateCommandById("deleteFromGisBgWhereId");
			cmd.AddInt("Id",id);
			cmd.ExecuteNonQuery();
		}

		public void Save(Context context)
		{
			if(context.ExportMode || updateAttr[Constants.updateAttrCreated])
			{
				GmCommand cmd=context.TargetConn.CreateCommandById("insertIntoGisBg");
				cmd.AddInt("Id",id);
				cmd.AddInt("Attr",attr);
				cmd.AddString("Style",styleStr,MaxLength.Style);
				cmd.AddString("TextAttr",textAttr,MaxLength.TextAttr);
				cmd.AddString("FilePath",filePath,MaxLength.FilePath);
				cmd.AddInt("SMin",smin);
				cmd.AddInt("SMax",smax);
				context.Buf.SetIntArray(cmd.AddBinary("Code"),GetIntArray());
				cmd.ExecuteNonQuery();
			}
			else if(updateAttr.NonEmpty)
			{
				GmCommand cmd=context.Conn.CreateCommand();
				string cmdText="";
				if(updateAttr[(int)BgImageField.Attr])
				{
					cmdText+="Attr= @Attr,";
					cmd.AddInt("Attr",attr);
				}
				if(updateAttr[(int)BgImageField.Style])
				{
					cmdText+="Style= @Style,";
					cmd.AddString("Style",styleStr,MaxLength.Style);
				}
				if(updateAttr[(int)BgImageField.TextAttr])
				{
					cmdText+="TextAttr= @TextAttr,";
					cmd.AddString("TextAttr",textAttr,MaxLength.TextAttr);
				}
				if(updateAttr[(int)BgImageField.FilePath])
				{
					cmdText+="FilePath= @FilePath,";
					cmd.AddString("FilePath",filePath,MaxLength.FilePath);
				}
				if(updateAttr[(int)BgImageField.SMin])
				{
					cmdText+="SMin= @SMin,";
					cmd.AddInt("SMin",smin);
				}
				if(updateAttr[(int)BgImageField.SMax])
				{
					cmdText+="SMax= @SMax,";
					cmd.AddInt("SMax",smax);
				}
				if(updateAttr[(int)BgImageField.Code])
				{
					cmdText+="Code= @Code,";
					context.Buf.SetIntArray(cmd.AddBinary("Code"),this.GetIntArray());
				}
				Geomethod.StringUtils.RemoveLastChar(ref cmdText);
				cmd.CommandText="update gisBg set "+cmdText+" where Id= @Id";
				cmd.AddInt("Id",id);
				cmd.ExecuteNonQuery();
			}
			if(!context.ExportMode) updateAttr=0;
		}

		public void Write(Context context, BinaryWriter bw)
		{
			bw.Write((int)ClassId);
			bw.Write(id);
			bw.Write(attr);
			bw.Write(styleStr);
			bw.Write(textAttr);
			bw.Write(filePath);
			bw.Write(smin);
			bw.Write(smax);
			context.Buf.WriteIntArray(bw,GetIntArray());
		}

		public void Draw(Map map)
		{
			if(isChecked)
			{
				if(loadFilePath!=filePath)
				{
					LoadImage();
				}
				if(image!=null)
				{
					map.DrawImage(image,ia,new Point(x,y),scale,angle);
				}
			}
		}

		public void DrawSelected(Map map)
		{
			Rect bounds=Bounds;
			if(!map.Intersects(bounds)) return;
            map.DrawPolygon(lib.Config.styles.selStyle, bounds.Points);
		}

		public void LoadImage()
		{
			if(loadFilePath!=filePath)
			{
				loadFilePath=filePath;
				try
				{
					image=Bitmap.FromFile(Geomethod.PathUtils.AbsFilePath(filePath));
				}
				catch//(Exception ex)
				{
//					Log.Exception(ex,false);
				}
			}
		}
		#region IShapedObject Members

		public bool IsVisibleOnMap(Map map)
		{
			return map.Intersects(Bounds);
		}

        public Point[] GetPoints()
        {
            if (image != null)
            {
                int dx=(int)(image.Width*scale*0.5*Math.Cos(angle));
                int dy=(int)(image.Height*scale*0.5*Math.Sin(angle));
                Point[] points = { new Point( x + dx, y + dy ), new Point( x - dx, y + dy ), new Point( x - dx, y - dy ), new Point( x + dx, y - dy ) };
                return points;
            }
            return Rect.Null.Points;
        }

		public Rect Bounds
		{
			get
			{
                if (image != null)
                {
                    return new Rect(GetPoints());
                }
				return Rect.Null;
			}
		}

		public IShapedObject Parent{get{return null;}}
		public bool GetCommonAttr(CommonAttr a) { return attr[(int)a]; }
		#endregion
	}
}
