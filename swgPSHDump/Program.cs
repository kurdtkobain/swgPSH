using System;
using System.IO;

namespace swgPSHDump
{
    class Program
    {
        static void Main(string[] args)
        {
            if (File.Exists(args[0]))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryReader br = new BinaryReader(File.OpenRead(args[0]));
                    br.ReadBytes(4);
                    int formL = br.ReadInt32();
                    br.ReadBytes(8);
                    int formPSHL = br.ReadInt32();
                    br.ReadBytes(8);
                    byte[] tmp = br.ReadBytes(4);
                    Array.Reverse(tmp);
                    int pshSRCL = BitConverter.ToInt32(tmp,0);
                    Console.WriteLine("SRC size:" + pshSRCL.ToString());
                    using (BinaryWriter bw = new BinaryWriter(ms))
                    {
                        bw.Write(br.ReadBytes(pshSRCL-1));
                    }
                    br.Close();
                    using (BinaryWriter bw = new BinaryWriter(File.Open(args[1], FileMode.Create)))
                    {
                        bw.Write(ms.ToArray());
                    }
                }
            }
        }
    }
}
