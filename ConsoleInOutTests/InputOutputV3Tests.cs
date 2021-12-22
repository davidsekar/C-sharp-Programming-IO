using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace ConsoleInOut.Tests
{
    [TestClass]
    [DoNotParallelize]
    public class InputOutputV3Tests
    {
        private void TestInput1()
        {
            Assert.AreEqual(InputOutputV3.ReadInt(), 1);
            Assert.AreEqual(InputOutputV3.ReadInt(), 4);
            Assert.AreEqual(InputOutputV3.ReadString(), "gdansk");

            Assert.AreEqual(InputOutputV3.ReadInt(), 2);
            Assert.AreEqual(InputOutputV3.ReadInt(), 2);
            Assert.AreEqual(InputOutputV3.ReadInt(), 1);
            Assert.AreEqual(InputOutputV3.ReadInt(), 3);
            Assert.AreEqual(InputOutputV3.ReadInt(), 3);
            Assert.AreEqual(InputOutputV3.ReadString(), "bydgoszcz");

            Assert.AreEqual(InputOutputV3.ReadInt(), 3);
            Assert.AreEqual(InputOutputV3.ReadInt(), 1);
            Assert.AreEqual(InputOutputV3.ReadInt(), 1);
            Assert.AreEqual(InputOutputV3.ReadInt(), 3);
            Assert.AreEqual(InputOutputV3.ReadInt(), 1);
            Assert.AreEqual(InputOutputV3.ReadInt(), 4);
            Assert.AreEqual(InputOutputV3.ReadInt(), 4);
            Assert.AreEqual(InputOutputV3.ReadString(), "torun");

            Assert.AreEqual(InputOutputV3.ReadInt(), 3);
            Assert.AreEqual(InputOutputV3.ReadInt(), 1);
            Assert.AreEqual(InputOutputV3.ReadInt(), 3);
            Assert.AreEqual(InputOutputV3.ReadInt(), 2);
            Assert.AreEqual(InputOutputV3.ReadInt(), 1);
            Assert.AreEqual(InputOutputV3.ReadInt(), 4);
            Assert.AreEqual(InputOutputV3.ReadInt(), 1);
            Assert.AreEqual(InputOutputV3.ReadString(), "warszawa");

            Assert.AreEqual(InputOutputV3.ReadInt(), 2);
            Assert.AreEqual(InputOutputV3.ReadInt(), 2);
            Assert.AreEqual(InputOutputV3.ReadInt(), 4);
            Assert.AreEqual(InputOutputV3.ReadInt(), 3);
            Assert.AreEqual(InputOutputV3.ReadInt(), 1);
            Assert.AreEqual(InputOutputV3.ReadInt(), 2);
            Assert.AreEqual(InputOutputV3.ReadString(), "gdansk");
            Assert.AreEqual(InputOutputV3.ReadString(), "warszawa");
            Assert.AreEqual(InputOutputV3.ReadString(), "bydgoszcz");
            Assert.AreEqual(InputOutputV3.ReadString(), "warszawa");
        }

        [TestMethod()]
        public void Test_Different_Input_BufferSizes()
        {
            for (int i = 1; i < 100; i++)
            {
                InputOutputV3.SetBuffSize(i);
                InputOutputV3.SetFilePath("../../testdata/in1.txt");
                TestInput1();
                InputOutputV3.Flush();
            }
        }

        [TestMethod()]
        public void Test_String_Custom_Delimiters_ReadLine()
        {
            var inputFilePath = "../../testdata/in2.txt";
            string[] expected = File.ReadAllLines(inputFilePath);
            for (int i = 1; i < 100; i++)
            {
                InputOutputV3.SetBuffSize(i);
                InputOutputV3.SetFilePath(inputFilePath);
                for (int j = 0; j < expected.Length; j++)
                {
                    string t = InputOutputV3.ReadString(Environment.NewLine);
                    Assert.AreEqual(t, expected[j]);
                }
                InputOutputV3.Flush();
            }
        }

        [TestMethod()]
        public void Test_EOF_Signal()
        {
            InputOutputV3.SetBuffSize(3);
            InputOutputV3.ThrowErrorOnEof = true;
            InputOutputV3.SetFilePath("../../testdata/in1.txt");
            TestInput1();
            Assert.ThrowsException<Exception>(() => InputOutputV3.ReadInt());
            InputOutputV3.Flush();
        }

        [TestMethod]
        public void TestGenericInt()
        {
            var inputFilePath = "../../testdata/in3.txt";
            string[] expected = File.ReadAllLines(inputFilePath);
            InputOutputV3.SetBuffSize(3);
            InputOutputV3.SetFilePath(inputFilePath);
            foreach (var ex in expected)
            {
                Assert.AreEqual(ex, InputOutputV3.ReadNumber<decimal>() + "");
            }
            InputOutputV3.Flush();
        }

        [TestMethod]
        public void Test_Generic_Number_Rd_Speed()
        {
            Stopwatch sw = new Stopwatch();
            var inputFilePath = "../../testdata/in3.txt";
            sw.Start();
            InputOutputV3.SetBuffSize(3);
            InputOutputV3.ThrowErrorOnEof = true;
            InputOutputV3.SetFilePath(inputFilePath);
            while (true)
            {
                try
                {
                    InputOutputV3.ReadInt();
                }
                catch
                {
                    break;
                }
            }
            InputOutputV3.Flush();
            sw.Stop();
            long elapsed1 = sw.ElapsedMilliseconds;

            sw.Restart();
            InputOutputV3.SetBuffSize(3);
            InputOutputV3.ThrowErrorOnEof = true;
            InputOutputV3.SetFilePath(inputFilePath);
            while (true)
            {
                try
                {
                    InputOutputV3.ReadNumber<int>();
                }
                catch
                {
                    break;
                }
            }
            InputOutputV3.Flush();
            sw.Stop();
            long elapsed2 = sw.ElapsedMilliseconds;

            Assert.IsTrue(elapsed1 <= elapsed2, "Normal: " + elapsed1 + "ms; Generic " + elapsed2 + "ms");
        }
    }
}