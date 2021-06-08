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

        public long Offset(long index)
        {
            if (_data.Length < index)
                _data.SetLength(index);
            
            long r = 0;
            long offset = index % _data.Length;

            r = offset < 0 ? _data.Length - offset : offset;

            return r;
        }

        public int this[int index]
        {
            get
            {
                _data.Position = Offset(index);
                return _data.ReadByte();
            }

            set
            {
                _data.Position = Offset(index);
                _data.WriteByte((byte)value);
            }
        }

        public byte[] this[int index, int length]
        {
            get
            {
                long position = Offset(index);

                byte[] r = new byte[length];

                _data.Position = position;
                _data.Read(r, 0, length);

                return r;
            }
        }

        public ByteWrite(string filename)
        {
            _data = new MemoryStream();
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                fs.CopyTo(_data);
            }
        }

        public ByteWrite(int init_size=-1)
        {
            _data = new MemoryStream();

            if (init_size > 0)
                _data.SetLength(init_size);
        }

        public void Write(int offset, byte[] data, int size)
        {
            _data.Position = offset;
            _data.Write(data, 0, size);
        }

        public string ReadASCIIString(int offset, int length)
        {
            return ASCIIEncoding.ASCII.GetString(this[offset, length]).Replace("\0", string.Empty);
        }

        public Int16 ReadInt16(int offset)
        {
            return BitConverter.ToInt16(this[offset, 2], 0);
        }

        public int ReadInt32(int offset)
        {
            return BitConverter.ToInt32(this[offset, 4], 0);
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
        
        public void Fill(int offset, int size)
        {
            int s = (int)Math.Max(_data.Length,offset+size);
            _data.SetLength(s);
        }

        public void Save(string filename)
        {
            File.WriteAllBytes(filename, _data.ToArray());
        }
    }
}
