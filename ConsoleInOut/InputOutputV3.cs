namespace ConsoleInOut
{
    #region Input Output Helper

    public static class InputOutputV3
    {
        private const byte _minus = (byte)'-';
        private const byte _zero = (byte)'0';
        private static System.IO.BufferedStream _readStream, _writeStream;
        private const int BUFF_Size = 1 << 22;
        private static int _inBuffSize = BUFF_Size, _outBuffSize = BUFF_Size;
        public static bool ThrowErrorOnEof { get; set; } = false;

        static InputOutputV3()
        {
            _readStream = new System.IO.BufferedStream(System.Console.OpenStandardInput(), _inBuffSize);
            _writeStream = new System.IO.BufferedStream(System.Console.OpenStandardOutput(), _outBuffSize);
        }

        public static void SetBuffSize(int n)
        {
            _inBuffSize = _outBuffSize = n;
        }

        public static void SetFilePath(string strPath)
        {
            strPath = System.IO.Path.GetFullPath(strPath);
            _readStream = new System.IO.BufferedStream(new System.IO.FileStream(strPath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite, _inBuffSize));
        }

        public static T ReadNumber<T>()
        {
            byte rb;
            while ((rb = GetByte()) < _minus) ;

            var neg = false;
            if (rb == _minus)
            {
                neg = true;
                rb = GetByte();
            }
            dynamic m = (T)System.Convert.ChangeType(rb - _zero, typeof(T));
            while (true)
            {
                rb = GetByte();
                if (rb < _zero) break;
                m = m * 10 + (rb - _zero);
            }
            return neg ? -m : m;
        }

        public static int ReadInt()
        {
            byte readByte;
            while ((readByte = GetByte()) < _minus) ;

            var neg = false;
            if (readByte == _minus)
            {
                neg = true;
                readByte = GetByte();
            }
            var m = readByte - _zero;
            while (true)
            {
                readByte = GetByte();
                if (readByte < _zero) break;
                m = m * 10 + (readByte - _zero);
            }
            return neg ? -m : m;
        }

        public static string ReadString()
        {
            return ReadString(' ');
        }

        public static string ReadString(string delimiter)
        {
            return ReadString(delimiter[0]);
        }

        public static string ReadString(char delimiter)
        {
            byte readByte;
            while ((readByte = GetByte()) <= delimiter) ;

            var sb = new System.Text.StringBuilder();
            do
            {
                sb.Append((char)readByte);
            } while ((readByte = GetByte()) > delimiter);

            return sb.ToString();
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static byte GetByte()
        {
            var b = _readStream.ReadByte();
            if (b == -1)
            {
                if (ThrowErrorOnEof) throw new System.Exception("End Of Input");
                else return 0;
            }
            return (byte)b;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void WriteByte(byte b)
        {
            _writeStream.WriteByte(b);
        }

        public static void WriteToBuffer(string s, bool newLine = false)
        {
            foreach (var b in System.Text.Encoding.ASCII.GetBytes(s)) WriteByte(b);
            if (newLine) WriteByte(10);
        }

        public static void WriteToBuffer(int c, bool newLine = false)
        {
            int tempidx = 0;
            byte[] temp = new byte[10];
            if (c < 0)
            {
                WriteByte(_minus);
                c = -c;
            }
            do
            {
                temp[tempidx++] = (byte)((c % 10) + _zero);
                c /= 10;
            } while (c > 0);
            for (int i = tempidx - 1; i >= 0; i--) WriteByte(temp[i]);
            if (newLine) WriteByte(10);
        }

        public static void WriteLineToBuffer(string s) => WriteToBuffer(s, true);

        public static void WriteLineToBuffer(int c) => WriteToBuffer(c, true);

        public static void Flush()
        {
            _writeStream.Flush();
            ThrowErrorOnEof = false;
        }
    }

    #endregion Input Output Helper
}