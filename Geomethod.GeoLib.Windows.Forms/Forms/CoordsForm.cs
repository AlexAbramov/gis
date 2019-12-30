using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Geomethod.GeoLib.Windows.Forms
{
    public partial class CoordsForm : Form
    {
        public CoordsForm(Point[] points )
        {
            InitializeComponent( );
            ucCoords.InitControl(points);
        }

        private void CoordDataTableForm_Load(object sender, EventArgs e)
        {

        }
    }

/*    public class CoordDataTable : DataTable
    {
        public CoordDataTable()
        {
            DataColumn cnt = new DataColumn("Cnt", typeof(int));
            DataColumn cx = new DataColumn("X", typeof(int));
            DataColumn cy = new DataColumn("Y", typeof(int));
            Columns.AddRange(new DataColumn[] { cnt, cx, cy });
        }
        public void Init(Point[] points)
        {
            foreach (Point p in points)
            {
                DataRow r = cdt.NewRow();
                r["Cnt"] = 0;
                r["X"] = point.X;
                r["Y"] = point.Y;
                base.Rows.Add(r);
            }
        }

                public CoordDataTable( GPolygon gobj )
                    : this( )
                {
                    for( int i = 0; i < gobj.Points.Length; i++ )
                    {
                        DataRow r = NewRow( );
                        r[ "Cnt" ] = i;
                        r[ "X" ] = gobj.Points[ i ].X;
                        r[ "Y" ] = gobj.Points[ i ].Y;
                        Rows.Add( r );
                    }
  
                }
         
    }*/

}