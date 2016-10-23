namespace ConsoleInOut
{
    class Program
    {
        static void Main(string[] args)
        {
            using (InputOutput io = new InputOutput())
            {
                int n = io.ReadInt();
                while (n-- > 0)
                {
                    string s = io.ReadString();
                    io.WriteLineToBuffer(s);
                }
            }
        }
    }
}
