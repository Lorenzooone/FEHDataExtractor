using DSDecmp.Formats.Nitro;
using System;
using System.IO;
using System.Linq;

public class Decompression
{
    public static byte[] Open(string path)
    {
        if (File.Exists(path))
        {
            string ext = Path.GetExtension(path).ToLower();
            var yaz0 = false;
            using (var fs = File.OpenRead(path))
            {
                if (fs.Length > 4 && fs.ReadByte() == 'Y' && fs.ReadByte() == 'a' && fs.ReadByte() == 'z' && fs.ReadByte() == '0')
                {
                    yaz0 = true;
                }
            }
            if (yaz0)
            {
                var cmp = File.ReadAllBytes(path);
                return Decompress(cmp);
            }
            else if (ext == ".lz")
            {
                byte[] filedata = File.ReadAllBytes(path);
                if (filedata[0] == 0x13 && filedata[4] == 0x11) // "LZ13"
                {
                    filedata = filedata.Skip(4).ToArray();
                }
                else if (filedata[0] == 0x17 && filedata[4] == 0x11) // Fire Emblem Heroes "LZ17"
                {
                    var xorkey = BitConverter.ToUInt32(filedata, 0) >> 8;
                    xorkey *= 0x8083;
                    for (var i = 8; i < filedata.Length; i += 0x4)
                    {
                        BitConverter.GetBytes(BitConverter.ToUInt32(filedata, i) ^ xorkey).CopyTo(filedata, i);
                        xorkey ^= BitConverter.ToUInt32(filedata, i);
                    }
                    filedata = filedata.Skip(4).ToArray();
                }
                else if (filedata[0] == 0x4 && (BitConverter.ToUInt32(filedata, 0) >> 8) == filedata.Length - 4)
                {
                    var xorkey = BitConverter.ToUInt32(filedata, 0) >> 8;
                    xorkey *= 0x8083;
                    for (var i = 4; i < filedata.Length; i += 0x4)
                    {
                        BitConverter.GetBytes(BitConverter.ToUInt32(filedata, i) ^ xorkey).CopyTo(filedata, i);
                        xorkey ^= BitConverter.ToUInt32(filedata, i);
                    }
                    filedata = filedata.Skip(4).ToArray();
                    if (BitConverter.ToUInt32(filedata, 0) == filedata.Length)
                    {
                        return filedata;
                    }
                    filedata = null;
                }
                try
                {
                    return LZ11Decompress(filedata);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (filedata != null)
                    return filedata;
                else
                    Console.WriteLine("Error!");
                return null;
            }
            else
            {
                byte[] filedata = File.ReadAllBytes(path);
                return filedata;
            }
        }
        return null;
    }
    private static byte[] LZ11Decompress(byte[] compressed)
    {
        using (MemoryStream cstream = new MemoryStream(compressed))
        {
            using (MemoryStream dstream = new MemoryStream())
            {
                (new LZ11()).Decompress(cstream, compressed.Length, dstream);
                return dstream.ToArray();
            }
        }
    }
    public static byte[] Decompress(byte[] Data)
    {
        var leng = (uint)(Data[4] << 24 | Data[5] << 16 | Data[6] << 8 | Data[7]);
        byte[] Result = new byte[leng];
        int Offs = 16;
        int dstoffs = 0;
        while (true)
        {
            byte header = Data[Offs++];
            for (int i = 0; i < 8; i++)
            {
                if ((header & 0x80) != 0) Result[dstoffs++] = Data[Offs++];
                else
                {
                    byte b = Data[Offs++];
                    int offs = ((b & 0xF) << 8 | Data[Offs++]) + 1;
                    int length = (b >> 4) + 2;
                    if (length == 2) length = Data[Offs++] + 0x12;
                    for (int j = 0; j < length; j++)
                    {
                        Result[dstoffs] = Result[dstoffs - offs];
                        dstoffs++;
                    }
                }
                if (dstoffs >= leng) return Result;
                header <<= 1;
            }
        }
    }
}
