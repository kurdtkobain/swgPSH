using System;
using System.IO;

namespace swgPSH
{
    class Program
    {

        static void Main(string[] args)
        {
                if (args.Length == 3)
                {
                    if (File.Exists(args[0]) && File.Exists(args[1]))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            byte[] FORM = new byte[] { 0x46, 0x4F, 0x52, 0x4D };
                            byte[] PSHFORM = new byte[] { 0x50, 0x53, 0x48, 0x50, 0x46, 0x4F, 0x52, 0x4D };
                            byte[] PSRC = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x50, 0x53, 0x52, 0x43 };
                            byte[] PEXE = new byte[] { 0x50, 0x45, 0x58, 0x45 };
                            using (BinaryWriter bw = new BinaryWriter(ms))
                            {
                                bw.BaseStream.Position = 0x14;
                                bw.Write(PSRC);
                                BinaryReader br = new BinaryReader(File.OpenRead(args[0]));
                                byte[] bytespsrc = BitConverter.GetBytes((int)br.BaseStream.Length + 1);
                                if(BitConverter.IsLittleEndian)
                                    Array.Reverse(bytespsrc);
                                bw.Write(bytespsrc);
                                bw.Write(br.ReadBytes((int)br.BaseStream.Length));
                                bw.Write((byte)0x00);
                                br.Close();
                                bw.Write(PEXE);
                                br = new BinaryReader(File.OpenRead(args[1]));
                                byte[] bytespexe = BitConverter.GetBytes((int)br.BaseStream.Length);
                                if(BitConverter.IsLittleEndian)
                                    Array.Reverse(bytespexe);
                                bw.Write(bytespexe);
                                bw.Write(br.ReadBytes((int)br.BaseStream.Length));
                                br.Close();
                                int tmp = (int)bw.BaseStream.Length - 0x14;
                                bw.BaseStream.Position = 0x08;
                                bw.Write(PSHFORM);
                                byte[] bytespshform = BitConverter.GetBytes(tmp);
                                if(BitConverter.IsLittleEndian)
                                    Array.Reverse(bytespshform);
                                bw.Write(bytespshform);
                                byte[] bytesform = BitConverter.GetBytes((int)bw.BaseStream.Length - 0x08);
                                if(BitConverter.IsLittleEndian)
                                    Array.Reverse(bytesform);
                                bw.BaseStream.Position = 0;
                                bw.Write(FORM);
                                bw.Write(bytesform);
                            }
                            BinaryWriter filebw = new BinaryWriter(File.Open(args[2], FileMode.Create));
                            filebw.Write(ms.ToArray());
                            filebw.Close();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Shader source or compiled shader does not exist.");
                    }
                }
                else
                {
                    Console.WriteLine("Usage: swgPSH <shader source> <compiled shader> <output PSH file>");
                }
            }
        }
    }
