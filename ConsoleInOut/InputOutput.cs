namespace ConsoleInOut
{
    #region Input Output Helper 
    public class InputOutput : System.IDisposable
    {
        private System.IO.Stream STRRead, STRWrite;
        private int ReadIdx, InBuffSize, BytesRead, OutBuffSize, WriteIdx;
        private byte[] INBuff, OUTBuff;
        private bool bThrowErrorOnEOF = false;

        public InputOutput(bool throwEndOfInputsError = false)
        {
            STRRead = System.Console.OpenStandardInput(); STRWrite = System.Console.OpenStandardOutput(); ReadIdx = BytesRead = WriteIdx = 0; InBuffSize = OutBuffSize = 1 << 22; INBuff = new byte[InBuffSize]; OUTBuff = new byte[OutBuffSize];
            bThrowErrorOnEOF = throwEndOfInputsError;
        }

        public int ReadInt()
        {
            byte _ReadByte;
            while (true)
            {
                _ReadByte = GetByte(); if (_ReadByte < '-') continue; else break;
            }
            bool neg = false;
            if (_ReadByte == '-')
            {
                neg = true;
                _ReadByte = GetByte();
            }
            int m = _ReadByte - '0';
            while (true)
            {
                _ReadByte = GetByte();
                if (_ReadByte < '0') break;
                m = m * 10 + (_ReadByte - '0');
            }
            return neg ? -m : m;
        }

        public string ReadString()
        {
            return ReadString(' ');
        }

        public string ReadString(char delimiter)
        {
            byte _ReadByte;
            while (true)
            {
                _ReadByte = GetByte();
                if (_ReadByte >= delimiter) break;
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append((char)_ReadByte);
            while (true)
            {
                _ReadByte = GetByte();
                if (_ReadByte <= delimiter) break;
                sb.Append((char)_ReadByte);
            }
            return sb.ToString();
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private byte GetByte()
        {
            if (ReadIdx == BytesRead)
            {
                ReadIdx = 0;
                BytesRead = STRRead.Read(INBuff, 0, InBuffSize);
                if (BytesRead < 1)
                {
                    if (bThrowErrorOnEOF) throw new System.Exception("EOF");
                    INBuff[BytesRead++] = 32;
                }
            }
            return INBuff[ReadIdx++];
        }

        public void Dispose()
        {
            Flush();
            STRWrite.Close();
            STRRead.Close();
        }

        public void WriteToBuffer(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (WriteIdx == OutBuffSize) Flush();
                OUTBuff[WriteIdx++] = (byte)s[i];
            }
        }

        public void WriteLineToBuffer(string s)
        {
            WriteToBuffer(s);
            if (WriteIdx == OutBuffSize) Flush();
            OUTBuff[WriteIdx++] = (byte)10;
        }
        public void WriteToBuffer(int c)
        {
            byte[] temp = new byte[10];
            int Tempidx = 0;
            if (c < 0)
            {
                if (WriteIdx == OutBuffSize) Flush(); OUTBuff[WriteIdx++] = (byte)'-'; c = -c;
            }
            do
            {
                temp[Tempidx++] = (byte)((c % 10) + '0');
                c /= 10;
            } while (c > 0);
            for (int i = Tempidx - 1; i >= 0; i--)
            {
                if (WriteIdx == OutBuffSize) Flush();
                OUTBuff[WriteIdx++] = temp[i];
            }
        }

        public void WriteLineToBuffer(int c)
        {
            WriteToBuffer(c);
            if (WriteIdx == OutBuffSize) Flush();
            OUTBuff[WriteIdx++] = 10;
        }

        private void Flush()
        {
            STRWrite.Write(OUTBuff, 0, WriteIdx);
            STRWrite.Flush();
            WriteIdx = 0;
        }
    }
    #endregion Input Output Helper 

}
