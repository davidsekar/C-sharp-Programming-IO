using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleInOut;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleInOut.Tests
{
    [TestClass()]
    public class InputOutputTests
    {
        private void TestInput1(InputOutput io)
        {
            Assert.AreEqual(io.ReadInt(), 1);
            Assert.AreEqual(io.ReadInt(), 4);
            Assert.AreEqual(io.ReadString(), "gdansk");

            Assert.AreEqual(io.ReadInt(), 2);
            Assert.AreEqual(io.ReadInt(), 2);
            Assert.AreEqual(io.ReadInt(), 1);
            Assert.AreEqual(io.ReadInt(), 3);
            Assert.AreEqual(io.ReadInt(), 3);
            Assert.AreEqual(io.ReadString(), "bydgoszcz");

            Assert.AreEqual(io.ReadInt(), 3);
            Assert.AreEqual(io.ReadInt(), 1);
            Assert.AreEqual(io.ReadInt(), 1);
            Assert.AreEqual(io.ReadInt(), 3);
            Assert.AreEqual(io.ReadInt(), 1);
            Assert.AreEqual(io.ReadInt(), 4);
            Assert.AreEqual(io.ReadInt(), 4);
            Assert.AreEqual(io.ReadString(), "torun");

            Assert.AreEqual(io.ReadInt(), 3);
            Assert.AreEqual(io.ReadInt(), 1);
            Assert.AreEqual(io.ReadInt(), 3);
            Assert.AreEqual(io.ReadInt(), 2);
            Assert.AreEqual(io.ReadInt(), 1);
            Assert.AreEqual(io.ReadInt(), 4);
            Assert.AreEqual(io.ReadInt(), 1);
            Assert.AreEqual(io.ReadString(), "warszawa");

            Assert.AreEqual(io.ReadInt(), 2);
            Assert.AreEqual(io.ReadInt(), 2);
            Assert.AreEqual(io.ReadInt(), 4);
            Assert.AreEqual(io.ReadInt(), 3);
            Assert.AreEqual(io.ReadInt(), 1);
            Assert.AreEqual(io.ReadInt(), 2);
            Assert.AreEqual(io.ReadString(), "gdansk");
            Assert.AreEqual(io.ReadString(), "warszawa");
            Assert.AreEqual(io.ReadString(), "bydgoszcz");
            Assert.AreEqual(io.ReadString(), "warszawa");
        }

        [TestMethod()]
        public void Test_Different_Input_BufferSizes()
        {
            for (int i = 1; i < 100; i++)
            {
                using (var io = new InputOutput())
                {
                    io.SetBuffSize(i);
                    io.SetFilePath("../../testdata/in1.txt");
                    TestInput1(io);
                }
            }
        }

        [TestMethod()]
        public void Test_String_Custom_Delimiters_ReadLine()
        {
            var inputFilePath = "../../testdata/in2.txt";
            string[] expected = File.ReadAllLines(inputFilePath);
            for (int i = 1; i < 100; i++)
            {
                using (var io = new InputOutput())
                {
                    io.SetBuffSize(i);
                    io.SetFilePath(inputFilePath);
                    for (int j = 0; j < expected.Length; j++)
                    {
                        string t = io.ReadString(Environment.NewLine);
                        Assert.AreEqual(t, expected[j]);
                    }
                }
            }
        }

        [TestMethod()]
        public void Test_EOF_Signal()
        {
            using (var io = new InputOutput(true))
            {
                io.SetBuffSize(3);
                io.SetFilePath("../../testdata/in1.txt");
                TestInput1(io);

                Assert.ThrowsException<Exception>(() => io.ReadInt());
            }
        }

        [TestMethod]
        public void TestGenericInt()
        {
            var inputFilePath = "../../testdata/in3.txt";
            string[] expected = File.ReadAllLines(inputFilePath);
            using (var io = new InputOutput())
            {
                io.SetBuffSize(3);
                io.SetFilePath(inputFilePath);
                foreach (var ex in expected)
                {
                    Assert.AreEqual(ex, io.ReadNumber<decimal>() + "");
                }
            }
        }

        [TestMethod]
        public void Test_Generic_Number_Rd_Speed()
        {
            Stopwatch sw = new Stopwatch();
            var inputFilePath = "../../testdata/in3.txt";
            sw.Start();
            using (var io = new InputOutput(true))
            {
                io.SetBuffSize(3);
                io.SetFilePath(inputFilePath);
                while (true)
                {
                    try
                    {
                        io.ReadInt();
                    }
                    catch
                    {
                        break;
                    }
                }
            }
            sw.Stop();
            long elapsed1 = sw.ElapsedMilliseconds;

            sw.Restart();
            using (var io = new InputOutput(true))
            {
                io.SetBuffSize(3);
                io.SetFilePath(inputFilePath);
                while (true)
                {
                    try
                    {
                        io.ReadNumber<int>();
                    }
                    catch
                    {
                        break;
                    }
                }
            }
            sw.Stop();
            long elapsed2 = sw.ElapsedMilliseconds;

            Assert.IsTrue(elapsed1 <= elapsed2, "Normal: " + elapsed1 + "ms; Generic " + elapsed2 + "ms");
        }
    }
}