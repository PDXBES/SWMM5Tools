using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace BesAsm.Toys.Swmm5.Swmm5InterfaceConverter
{
  public class Swmm5Converter
  {
    private TextReader reader;
    private TextWriter writer;
    private int catchmentCount;    
    private char[] delimiters = new char[] { ' ', '\t' };
    private double timeStep;
    private Dictionary<string, List<double>> flowDictionary = new Dictionary<string, List<double>>();
    private List<DateTime> timeSteps = new List<DateTime>();
   
    public Swmm5Converter()
    {
    }

    public void ConvertFile(string infile, string outfile)
    {
      reader = new StreamReader(infile);
      writer = new StreamWriter(outfile);
            
      Console.WriteLine("Reading Input: " + infile);

      ReadInput();
      reader.Close();

      Console.WriteLine("Writing Output File: " + outfile);
            
      WriteOutput();
      writer.Close();
      
      Console.WriteLine("Finished Writing");
    }

    void ReadInput()
    {      
      string[] inheader = reader.ReadLine().Split(delimiters);

      catchmentCount = inheader.Length - 2;
      var catchments = inheader.Skip(2);// as string[];//First two headers are "DATE" and "TIME", remaining are catchment names      

      foreach (string catchment in catchments)
      {
        flowDictionary.Add(catchment, new List<double>());
      }

      string line = reader.ReadLine();
      while (line != null)
      {
        string[] tokens = line.Split(delimiters) as string[];

        timeSteps.Add(DateTime.Parse(tokens[0] + " " + tokens[1]));

        int i = 2;
        foreach (string catchment in catchments)
        {
          flowDictionary[catchment].Add(Double.Parse(tokens[i]));
          i++;
        }
        line = reader.ReadLine();
      }
      return;
    }

    void WriteOutput()
    {
      Console.WriteLine("Writing Header");

      writer.WriteLine("SWMM5 Interface File");
      writer.WriteLine("Portland BES Explicit Model");
      writer.WriteLine(timeStep.ToString("0") + " - reporting time step in sec");
      writer.WriteLine("1 - number of constituents as listed below:");
      writer.WriteLine("FLOW CFS");
      writer.WriteLine(catchmentCount + " - number of nodes as listed below:");

      Console.WriteLine("Writing Catchment List");

      foreach (string catchment in flowDictionary.Keys)
      {
        writer.WriteLine(catchment);
      }

      writer.WriteLine("Node Year Mon Day Hr Min Sec FLOW");

      foreach (string catchment in flowDictionary.Keys)
      {
        Console.WriteLine("Writing Catchment: " + catchment);
        List<double> flows = flowDictionary[catchment];
        for (int i = 0; i < timeSteps.Count; i++)
        {
          writer.Write(catchment + " ");
          writer.Write(timeSteps[i].ToString("yyyy M d H m s "));
          writer.WriteLine(flows[i].ToString("0.000"));
        }
      }

      timeStep = timeSteps[1].Subtract(timeSteps[0]).TotalSeconds;

      return;
    }
  }
}
