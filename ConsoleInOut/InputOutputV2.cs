namespace ConsoleInOut
{
    #region Input Output Helper

    public static unsafe class InputOutputV2
    {
        private const byte _minus = (byte)'-';
        private const byte _zero = (byte)'0';
        private static System.IO.Stream _readStream, _writeStream;
        private const int BUFF_Size = 1 << 22;
        private static int _readIdx = 0, _bytesRead = 0, _writeIdx = 0, _inBuffSize = BUFF_Size, _outBuffSize = BUFF_Size;
        private static readonly byte[] _inBuff = new byte[_inBuffSize], _outBuff = new byte[_outBuffSize];
        public static bool ThrowErrorOnEof { get; set; } = false;

        static InputOutputV2()
        {
            _readStream = System.Console.OpenStandardInput();
            _writeStream = System.Console.OpenStandardOutput();
        }

        public static void SetBuffSize(int n)
        {
            _inBuffSize = _outBuffSize = n;
        }

        public static void SetFilePath(string strPath)
        {
            strPath = System.IO.Path.GetFullPath(strPath);
            _readStream = new System.IO.FileStream(strPath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
        }

        public static T ReadNumber<T>()
        {
            fixed (byte* ptr = _inBuff)
            {
                byte rb;
                while ((rb = GetByte(ptr)) < _minus) ;

                var neg = false;
                if (rb == _minus)
                {
                    neg = true;
                    rb = GetByte(ptr);
                }
                dynamic m = (T)System.Convert.ChangeType(rb - _zero, typeof(T));
                while (true)
                {
                    rb = GetByte(ptr);
                    if (rb < _zero) break;
                    m = m * 10 + (rb - _zero);
                }
                return neg ? -m : m;
            }
        }

        public static int ReadInt()
        {
            fixed (byte* ptr = _inBuff)
            {
                byte readByte;
                while ((readByte = GetByte(ptr)) < _minus) ;

                var neg = false;
                if (readByte == _minus)
                {
                    neg = true;
                    readByte = GetByte(ptr);
                }
                var m = readByte - _zero;
                while (true)
                {
                    readByte = GetByte(ptr);
                    if (readByte < _zero) break;
                    m = m * 10 + (readByte - _zero);
                }
                return neg ? -m : m;
            }
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
            fixed (byte* ptr = _inBuff)
            {
                byte readByte;
                while ((readByte = GetByte(ptr)) <= delimiter) ;

                var sb = new System.Text.StringBuilder();
                do
                {
                    sb.Append((char)readByte);
                } while ((readByte = GetByte(ptr)) > delimiter);

                return sb.ToString();
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static byte GetByte(byte* readPtr)
        {
            if (_readIdx >= _bytesRead)
            {
                _readIdx = 0;
                _bytesRead = _readStream.Read(_inBuff, 0, _inBuffSize);
                if (_bytesRead >= 1) return *(readPtr + _readIdx++);

                if (ThrowErrorOnEof) throw new System.Exception("End Of Input");
                *(readPtr + _bytesRead++) = 0;
            }
            return *(readPtr + _readIdx++);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void WriteByte(byte* writePtr, byte b)
        {
            if (_writeIdx == _outBuffSize) FlushInternal();
            *(writePtr + _writeIdx++) = b;
        }

        public static void WriteToBuffer(string s, bool newLine = false)
        {
            fixed (byte* ptr = _outBuff)
            {
                foreach (var b in System.Text.Encoding.ASCII.GetBytes(s)) WriteByte(ptr, b);
                if (newLine) WriteByte(ptr, 10);
            }
        }

        public static void WriteToBuffer(int c, bool newLine = false)
        {
            fixed (byte* ptr = _outBuff)
            {
                int tempidx = 0;
                byte[] temp = new byte[10];
                fixed (byte* tempPtr = temp)
                {
                    if (c < 0)
                    {
                        WriteByte(ptr, _minus);
                        c = -c;
                    }
                    do
                    {
                        *(tempPtr + tempidx++) = (byte)((c % 10) + _zero);
                        c /= 10;
                    } while (c > 0);
                    for (int i = tempidx - 1; i >= 0; i--) WriteByte(ptr, *(tempPtr + i));
                }
            }
        }

        public static void WriteLineToBuffer(string s) => WriteToBuffer(s, true);

        public static void WriteLineToBuffer(int c) => WriteToBuffer(c, true);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void FlushInternal()
        {
            _writeStream.Write(_outBuff, 0, _writeIdx);
            _writeStream.Flush();
            _writeIdx = 0;
        }

        public static void Flush()
        {
            FlushInternal();
            //_readStream.Close();
            //_writeStream.Close();
            ThrowErrorOnEof = false;
        }
    }

    #endregion Input Output Helper
}