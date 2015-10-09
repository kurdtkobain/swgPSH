using System;
using System.IO;

namespace swgHLSLStrip
{
    class Program
    {
        static void Main(string[] args)
        {
            if (File.Exists(args[0]))
            {
                var SLines = File.ReadAllLines(args[0]);
                if (SLines[0] == "ps.1.1" || SLines[0] == "ps.1.4" || SLines[0] == "ps.2.0")
                {
                    if (SLines[0] == "ps.1.1")
                    {
                        SLines[0] = "ps_1_1";
                    }
                    if (SLines[0] == "ps.1.4")
                    {
                        SLines[0] = "ps_1_4";
                    }
                    if (SLines[0] == "ps.2.0")
                    {
                        SLines[0] = "ps_2_0";
                    }
                    Console.WriteLine(SLines[0]);
                    File.WriteAllLines(args[0], SLines);
                }
            }
        }
    }
}
