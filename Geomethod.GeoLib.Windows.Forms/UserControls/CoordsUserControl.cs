using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Geomethod.GeoLib.Windows.Forms
{
    public partial class CoordsUserControl : UserControl
    {
        List<IndexedPoint> indexedPoints=new List<IndexedPoint>();

        public Point[] GetPoints() { return null; }

        public void InitControl(Point[] points)
        {
            int index = 0;
            foreach (Point p in points) indexedPoints.Add(new IndexedPoint(++index, p));
            bindingSource.DataSource = indexedPoints;
        }

        public CoordsUserControl( )
        {
            InitializeComponent( );
            
        }
/*        public void DataBind( CoordDataTable cdt )
        {
            this.coordDataView.DataSource = (DataTable)cdt;
        }*/
    }

    public struct IndexedPoint
    {
        int x, y, index;
        public int Index { get { return index; } set { index = value; } }
        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public IndexedPoint(int index, Point p) { this.index = index; x = p.X; y = p.Y; }
    }

}
