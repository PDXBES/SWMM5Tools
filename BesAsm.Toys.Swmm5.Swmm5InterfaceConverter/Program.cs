using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BesAsm.Toys.Swmm5.Swmm5InterfaceConverter
{
  class Program
  {
    static void Main(string[] args)
    {
      string infile = args[0];
      string outfile = args[1];

      Swmm5Converter converter = new Swmm5Converter();
      converter.ConvertFile(infile, outfile);
    }


  }
}
