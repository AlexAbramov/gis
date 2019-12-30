using System;

namespace Geomethod.GeoLib
{
	public interface IIndexer
	{
		Rect GetIndex(Rect rect);
		void Init(int[] data);
		int[] ToIntArray();
	}

	public enum IndexingMethod{SS,DS};
	public enum IndexerField{Im,Inp,Dinp};

	public class Indexer: IIndexer
	{
		IndexingMethod im;
		int inp;
		int dinp;
		
		public int Inp{get{return inp;}}
		public int Dinp{get{return dinp;}}
		public IndexingMethod IndexingMethod{get{return im;}}

		public Indexer(IndexingMethod im,int inp,int dinp)
		{
			Init(im,inp,dinp);
		}
		public Indexer(): this(IndexingMethod.SS,16,6)
		{
		}
		public Indexer(int[] data)
		{
			Init(data);
		}
		public void Init(int[] data)
		{
			if(data.Length!=3) throw new Exception("Wrong indexer data.");
			Init((IndexingMethod)data[0],data[1],data[2]);
		}
		void Init(IndexingMethod im,int inp,int dinp)
		{
			if(inp<=0 || dinp<=0 || inp<dinp || inp>31 || dinp>31) throw new Exception(string.Format("Wrong parameters: inp={0} dinp={1}",inp,dinp));
			this.im=im;
			this.inp=inp;
			this.dinp=dinp;
		}

		public int[] ToIntArray()
		{
			int[] ar={(int)im,inp,dinp};
			return ar;
		}

		public Rect GetIndex(Rect rect)
		{
			if(!rect.IsNormalized) rect.Normalize();
			long size=rect.MaxSize;
			int index=inp;
			if(size>0)
			{
				while(size > 1<<(index-dinp))
				{
					index+=dinp;
					if(index>30) return Rect.Max;
				}
			}
			rect.left=rect.left>>index<<index;//inp
            rect.bottom = rect.bottom >> index << index;
			int s=1<<index;
			if(im==IndexingMethod.SS)
			{
				s+=1<<(index-dinp);
			}
			else
			{
				if(rect.right>rect.left+s || rect.top>rect.bottom+s) s<<=1;
			}
			rect.right=rect.left+s;
			rect.top=rect.bottom+s;
			if(rect.right<rect.left) rect.right=int.MaxValue;
			if(rect.top<rect.bottom) rect.top=int.MaxValue;
			return rect;
		}
		public override string ToString(){return string.Format("{{{0} {1} {2}}}",im,inp,dinp);}
	}
}
