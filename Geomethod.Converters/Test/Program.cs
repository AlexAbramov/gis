using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Start test");
			try
			{
				//			TestClass tc = new TestClass();
//				MIFTestClass mtc = new MIFTestClass("str.mif");
//				MIFTestClass mtc2 = new MIFTestClass("tb.mif");
				//            MIFTestClass mtc3 = new MIFTestClass( "parks.mif" );

				TestShape ts = new TestShape( @"data\park.shp" );
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			Console.WriteLine("Test completed. Press any key to exit...");
			Console.ReadKey();
		}
	}
}
