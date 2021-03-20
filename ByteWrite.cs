using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace GaloreWare.IO
{
    public class ByteWrite
    {
        MemoryStream _data;

        public ByteWrite(int init_size=-1)
        {
            _data = init_size == -1 ? new MemoryStream() : new MemoryStream(init_size);
        }

        public void Write(int offset, byte[] data, int size)
        {
            _data.Position = offset;
            _data.Write(data, 0, size);
        }

        public void WriteASCII(int offset, string str, int size)
        {
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes(str);
            Write(offset, buffer, size);
        }

        public void WriteInt16(int offset, short value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            Write(offset, buffer, 2);
        }

        public void WriteInt32(int offset,int value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            Write(offset, buffer, 4);
        }

        public void Save(string filename)
        {
            File.WriteAllBytes(filename, _data.ToArray());
        }
    }
}
