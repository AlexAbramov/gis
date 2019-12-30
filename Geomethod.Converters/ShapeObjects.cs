using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.IO;


namespace Geomethod.Converters
{

	public	enum	ShapeUnit: uint
	{
		NullShape	= 0,
			
		Point		= 1,
		Arc			= 2,
		PolyLine	= 3,
		Polygon		= 5,
		MultiPoint	= 8,

		PointZ		= 11,
		PolyLineZ	= 13,
		PolygonZ	= 15,
		MultiPointZ	= 18,
		PointM		= 21,
		PolyLineM	= 23,
		PolygonM	= 25,
		MultiPointM	= 28,
		MultiPatch	= 31,

	 
	}

	public	class	Boundary
	{
		public	double	minx;
		public	double	miny;
		public	double	maxx;
		public	double	maxy;

		public	Boundary( BinaryReader br )
		{
			minx = br.ReadDouble();
			miny = br.ReadDouble();
			maxx = br.ReadDouble();
			maxy = br.ReadDouble();
		}
		public	void	Write( StreamWriter sw )
		{
			sw.WriteLine( minx + " \t" + miny + " \t" + maxx + " \t" + maxy );
		}

	}

	public	class	ShPoint
	{
		public	double	X;
		public	double	Y;

		public	ShPoint( double X, double Y )
		{
			this.X = X;
			this.Y = Y;
		}
		public	ShPoint( BinaryReader br )
		{
			this.X = br.ReadDouble();
			this.Y = br.ReadDouble();
		}
	}


	public		abstract	class	ShapeObject
	{
		public	static	uint	cnt;
		uint	cur = 0;

		public	ShapeObject( BinaryReader br )
		{
			cur = cnt++;
		}
	
		public	ShapeObject( )
		{
		}

	}

	public	class	ShapePoint: ShapeObject
	{
		public	ShPoint	point;

		public	ShapePoint( BinaryReader br ): base( br )
		{
			point	= new  ShPoint( br );
		}
	}
		
	public	class	ShapePointGroup: ShapeObject
	{
		public	Boundary	bound;
		uint	numPoints;
		public	ShPoint[]	points;

		public	ShapePointGroup( BinaryReader br ): base( br )
		{
			bound = new Boundary( br );
			uint numParts = br.ReadUInt32();
			numPoints = br.ReadUInt32();
			points	= new ShPoint[ numPoints ];
			for( int j = 0; j < numParts; j++ )
				points[ j ] = new ShPoint( br );
		}
	}

	public	class	ShapeArc: ShapeObject
	{
		public	Boundary	bound;
		public	uint		numPoints;
		public	ShPoint[]	points;

		public	ShapeArc( BinaryReader br ): base( br )
		{
			bound = new Boundary( br );
			uint numParts = br.ReadUInt32();
			numPoints = br.ReadUInt32();
			points	= new ShPoint[ numPoints ];
			for( int j = 0; j < numParts; j++ )
				br.ReadUInt32();

			for( int j = 0; j < numPoints; j++ )
				points[ j ] = new ShPoint( br );
		}
	}

	public	class	ShapePolygon: ShapeObject
	{
		public	Boundary	bound;
		public	uint		numParts;
		public	uint[]		parts;		
		public	uint		numPoints;
		public	ShPoint[]	points;

		public	ShapePolygon( BinaryReader br ): base( br )
		{
			bound = new Boundary( br );
			numParts = br.ReadUInt32();
			numPoints = br.ReadUInt32();

			parts	= new uint[ numParts ];
			for( int j = 0; j < numParts; j++ )
				parts[ j ] = br.ReadUInt32();

			points	= new ShPoint[ numPoints ];
			for( int j = 0; j < numPoints; j++ )
				points[ j ] = new ShPoint( br );
		}
	}
	
	public	class	ShapePolyline: ShapeObject
	{
		public	Boundary	bound;
		public	uint		numParts;
		public	uint[]		parts;		
		public	uint		numPoints;
		public	ShPoint[]	points;

		public	ShapePolyline( BinaryReader br ): base( br )
		{
			bound = new Boundary( br );
			numParts = br.ReadUInt32();
			numPoints = br.ReadUInt32();

			parts	= new uint[ numParts ];
			for( int j = 0; j < numParts; j++ )
				parts[ j ] = br.ReadUInt32();

			points	= new ShPoint[ numPoints ];
			for( int j = 0; j < numPoints; j++ )
				points[ j ] = new ShPoint( br );
		}
	}
}
