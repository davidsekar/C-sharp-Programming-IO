using System.Collections.Generic;

namespace ConsoleInOut
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var s = new List<string>();
            //for (int i = 0; i < 26; i++)
            //{
            //    var ss = string.Empty;
            //    for (int j = 0; j <= i; j++)
            //        ss += (char)('a' + j);

            //    s.Add(ss);
            //}

            //for (int i = 1; i < 26; i++)
            //{
            //    InputOutputV2.SetBuffSize(i);
            //    InputOutputV2.WriteLineToBuffer($"Buffer Size: {i}");
            //    for (int j = 0; j < s.Count; j++)
            //        InputOutputV2.WriteLineToBuffer(s[j]);
            //    InputOutputV2.Flush();
            //}

            var n = new List<int>();
            for (int i = 0; i < 26; i++)
            {
                var ss = string.Empty;
                for (int j = 0; j <= i % 10; j++)
                    ss += (char)('0' + j);

                n.Add(System.Convert.ToInt32(ss));
            }

            for (int i = 1; i < 26; i++)
            {
                InputOutputV2.SetBuffSize(i);
                InputOutputV2.WriteLineToBuffer($"Buffer Size: {i}");
                for (int j = 0; j < n.Count; j++)
                    InputOutputV2.WriteLineToBuffer(n[j]);
                InputOutputV2.Flush();
            }
        }
    }
}