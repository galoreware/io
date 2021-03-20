using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GaloreWare.IO
{
    public class ByteAccess
    {
        BufferedStream _data;

        public long Length { get { return _data.Length; } }

        public long Offset(long index)
        {
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

        public ByteAccess(string filename)
        {
            _data = new BufferedStream(new FileStream(filename, FileMode.Open, FileAccess.Read));

        }

        public string GetASCIIString(int offset, int length)
        {
            return ASCIIEncoding.ASCII.GetString(this[offset, length]).Replace("\0", string.Empty);
        }
        
        public int GetInt16(int offset)
        {
            return BitConverter.ToInt16(this[offset,2], 0);
        }

        public int GetInt32(int offset)
        {
            return BitConverter.ToInt32(this[offset,4], 0);
        }

        public void SaveOffset(string filename, int offset, int lenght)
        {
            byte[] buffer = this[offset, lenght];
            File.WriteAllBytes(filename,buffer);
        }
    }
}
