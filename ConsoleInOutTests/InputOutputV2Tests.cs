using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace ConsoleInOut.Tests
{
    [TestClass]
    [DoNotParallelize]
    public class InputOutputV2Tests
    {
        private void TestInput1()
        {
            Assert.AreEqual(InputOutputV2.ReadInt(), 1);
            Assert.AreEqual(InputOutputV2.ReadInt(), 4);
            Assert.AreEqual(InputOutputV2.ReadString(), "gdansk");

            Assert.AreEqual(InputOutputV2.ReadInt(), 2);
            Assert.AreEqual(InputOutputV2.ReadInt(), 2);
            Assert.AreEqual(InputOutputV2.ReadInt(), 1);
            Assert.AreEqual(InputOutputV2.ReadInt(), 3);
            Assert.AreEqual(InputOutputV2.ReadInt(), 3);
            Assert.AreEqual(InputOutputV2.ReadString(), "bydgoszcz");

            Assert.AreEqual(InputOutputV2.ReadInt(), 3);
            Assert.AreEqual(InputOutputV2.ReadInt(), 1);
            Assert.AreEqual(InputOutputV2.ReadInt(), 1);
            Assert.AreEqual(InputOutputV2.ReadInt(), 3);
            Assert.AreEqual(InputOutputV2.ReadInt(), 1);
            Assert.AreEqual(InputOutputV2.ReadInt(), 4);
            Assert.AreEqual(InputOutputV2.ReadInt(), 4);
            Assert.AreEqual(InputOutputV2.ReadString(), "torun");

            Assert.AreEqual(InputOutputV2.ReadInt(), 3);
            Assert.AreEqual(InputOutputV2.ReadInt(), 1);
            Assert.AreEqual(InputOutputV2.ReadInt(), 3);
            Assert.AreEqual(InputOutputV2.ReadInt(), 2);
            Assert.AreEqual(InputOutputV2.ReadInt(), 1);
            Assert.AreEqual(InputOutputV2.ReadInt(), 4);
            Assert.AreEqual(InputOutputV2.ReadInt(), 1);
            Assert.AreEqual(InputOutputV2.ReadString(), "warszawa");

            Assert.AreEqual(InputOutputV2.ReadInt(), 2);
            Assert.AreEqual(InputOutputV2.ReadInt(), 2);
            Assert.AreEqual(InputOutputV2.ReadInt(), 4);
            Assert.AreEqual(InputOutputV2.ReadInt(), 3);
            Assert.AreEqual(InputOutputV2.ReadInt(), 1);
            Assert.AreEqual(InputOutputV2.ReadInt(), 2);
            Assert.AreEqual(InputOutputV2.ReadString(), "gdansk");
            Assert.AreEqual(InputOutputV2.ReadString(), "warszawa");
            Assert.AreEqual(InputOutputV2.ReadString(), "bydgoszcz");
            Assert.AreEqual(InputOutputV2.ReadString(), "warszawa");
        }

        [TestMethod()]
        public void Test_Different_Input_BufferSizes()
        {
            for (int i = 1; i < 100; i++)
            {
                InputOutputV2.SetBuffSize(i);
                InputOutputV2.SetFilePath("../../testdata/in1.txt");
                TestInput1();
                InputOutputV2.Flush();
            }
        }

        [TestMethod()]
        public void Test_String_Custom_Delimiters_ReadLine()
        {
            var inputFilePath = "../../testdata/in2.txt";
            string[] expected = File.ReadAllLines(inputFilePath);
            for (int i = 1; i < 100; i++)
            {
                InputOutputV2.SetBuffSize(i);
                InputOutputV2.SetFilePath(inputFilePath);
                for (int j = 0; j < expected.Length; j++)
                {
                    string t = InputOutputV2.ReadString(Environment.NewLine);
                    Assert.AreEqual(t, expected[j]);
                }
                InputOutputV2.Flush();
            }
        }

        [TestMethod()]
        public void Test_EOF_Signal()
        {
            InputOutputV2.SetBuffSize(3);
            InputOutputV2.ThrowErrorOnEof = true;
            InputOutputV2.SetFilePath("../../testdata/in1.txt");
            TestInput1();
            Assert.ThrowsException<Exception>(() => InputOutputV2.ReadInt());
            InputOutputV2.Flush();
        }

        [TestMethod]
        public void TestGenericInt()
        {
            var inputFilePath = "../../testdata/in3.txt";
            string[] expected = File.ReadAllLines(inputFilePath);
            InputOutputV2.SetBuffSize(3);
            InputOutputV2.SetFilePath(inputFilePath);
            foreach (var ex in expected)
            {
                Assert.AreEqual(ex, InputOutputV2.ReadNumber<decimal>() + "");
            }
            InputOutputV2.Flush();
        }

        [TestMethod]
        public void Test_Generic_Number_Rd_Speed()
        {
            Stopwatch sw = new Stopwatch();
            var inputFilePath = "../../testdata/in3.txt";
            sw.Start();
            InputOutputV2.SetBuffSize(3);
            InputOutputV2.ThrowErrorOnEof = true;
            InputOutputV2.SetFilePath(inputFilePath);
            while (true)
            {
                try
                {
                    InputOutputV2.ReadInt();
                }
                catch
                {
                    break;
                }
            }
            InputOutputV2.Flush();
            sw.Stop();
            long elapsed1 = sw.ElapsedMilliseconds;

            sw.Restart();
            InputOutputV2.SetBuffSize(3);
            InputOutputV2.ThrowErrorOnEof = true;
            InputOutputV2.SetFilePath(inputFilePath);
            while (true)
            {
                try
                {
                    InputOutputV2.ReadNumber<int>();
                }
                catch
                {
                    break;
                }
            }
            InputOutputV2.Flush();
            sw.Stop();
            long elapsed2 = sw.ElapsedMilliseconds;

            Assert.IsTrue(elapsed1 <= elapsed2, "Normal: " + elapsed1 + "ms; Generic " + elapsed2 + "ms");
        }
    }
}