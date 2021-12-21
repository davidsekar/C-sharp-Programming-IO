using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace ConsoleInOut.Tests
{
    [TestClass]
    [DoNotParallelize]
    public class InputOutputV1Tests
    {
        private void TestInput1()
        {
            Assert.AreEqual(InputOutputV1.ReadInt(), 1);
            Assert.AreEqual(InputOutputV1.ReadInt(), 4);
            Assert.AreEqual(InputOutputV1.ReadString(), "gdansk");

            Assert.AreEqual(InputOutputV1.ReadInt(), 2);
            Assert.AreEqual(InputOutputV1.ReadInt(), 2);
            Assert.AreEqual(InputOutputV1.ReadInt(), 1);
            Assert.AreEqual(InputOutputV1.ReadInt(), 3);
            Assert.AreEqual(InputOutputV1.ReadInt(), 3);
            Assert.AreEqual(InputOutputV1.ReadString(), "bydgoszcz");

            Assert.AreEqual(InputOutputV1.ReadInt(), 3);
            Assert.AreEqual(InputOutputV1.ReadInt(), 1);
            Assert.AreEqual(InputOutputV1.ReadInt(), 1);
            Assert.AreEqual(InputOutputV1.ReadInt(), 3);
            Assert.AreEqual(InputOutputV1.ReadInt(), 1);
            Assert.AreEqual(InputOutputV1.ReadInt(), 4);
            Assert.AreEqual(InputOutputV1.ReadInt(), 4);
            Assert.AreEqual(InputOutputV1.ReadString(), "torun");

            Assert.AreEqual(InputOutputV1.ReadInt(), 3);
            Assert.AreEqual(InputOutputV1.ReadInt(), 1);
            Assert.AreEqual(InputOutputV1.ReadInt(), 3);
            Assert.AreEqual(InputOutputV1.ReadInt(), 2);
            Assert.AreEqual(InputOutputV1.ReadInt(), 1);
            Assert.AreEqual(InputOutputV1.ReadInt(), 4);
            Assert.AreEqual(InputOutputV1.ReadInt(), 1);
            Assert.AreEqual(InputOutputV1.ReadString(), "warszawa");

            Assert.AreEqual(InputOutputV1.ReadInt(), 2);
            Assert.AreEqual(InputOutputV1.ReadInt(), 2);
            Assert.AreEqual(InputOutputV1.ReadInt(), 4);
            Assert.AreEqual(InputOutputV1.ReadInt(), 3);
            Assert.AreEqual(InputOutputV1.ReadInt(), 1);
            Assert.AreEqual(InputOutputV1.ReadInt(), 2);
            Assert.AreEqual(InputOutputV1.ReadString(), "gdansk");
            Assert.AreEqual(InputOutputV1.ReadString(), "warszawa");
            Assert.AreEqual(InputOutputV1.ReadString(), "bydgoszcz");
            Assert.AreEqual(InputOutputV1.ReadString(), "warszawa");
        }

        [TestMethod()]
        public void Test_Different_Input_BufferSizes()
        {
            for (int i = 1; i < 100; i++)
            {
                InputOutputV1.SetBuffSize(i);
                InputOutputV1.SetFilePath("../../testdata/in1.txt");
                TestInput1();
                InputOutputV1.Flush();
            }
        }

        [TestMethod()]
        public void Test_String_Custom_Delimiters_ReadLine()
        {
            var inputFilePath = "../../testdata/in2.txt";
            string[] expected = File.ReadAllLines(inputFilePath);
            for (int i = 1; i < 100; i++)
            {
                InputOutputV1.SetBuffSize(i);
                InputOutputV1.SetFilePath(inputFilePath);
                for (int j = 0; j < expected.Length; j++)
                {
                    string t = InputOutputV1.ReadString(Environment.NewLine);
                    Assert.AreEqual(t, expected[j]);
                }
                InputOutputV1.Flush();
            }
        }

        [TestMethod()]
        public void Test_EOF_Signal()
        {
            InputOutputV1.SetBuffSize(3);
            InputOutputV1.ThrowErrorOnEof = true;
            InputOutputV1.SetFilePath("../../testdata/in1.txt");
            TestInput1();
            Assert.ThrowsException<Exception>(() => InputOutputV1.ReadInt());
            InputOutputV1.Flush();
        }

        [TestMethod]
        public void TestGenericInt()
        {
            var inputFilePath = "../../testdata/in3.txt";
            string[] expected = File.ReadAllLines(inputFilePath);
            InputOutputV1.SetBuffSize(3);
            InputOutputV1.SetFilePath(inputFilePath);
            foreach (var ex in expected)
            {
                Assert.AreEqual(ex, InputOutputV1.ReadNumber<decimal>() + "");
            }
            InputOutputV1.Flush();
        }

        [TestMethod]
        public void Test_Generic_Number_Rd_Speed()
        {
            Stopwatch sw = new Stopwatch();
            var inputFilePath = "../../testdata/in3.txt";
            sw.Start();
            InputOutputV1.SetBuffSize(3);
            InputOutputV1.ThrowErrorOnEof = true;
            InputOutputV1.SetFilePath(inputFilePath);
            while (true)
            {
                try
                {
                    InputOutputV1.ReadInt();
                }
                catch
                {
                    break;
                }
            }
            InputOutputV1.Flush();
            sw.Stop();
            long elapsed1 = sw.ElapsedMilliseconds;

            sw.Restart();
            InputOutputV1.SetBuffSize(3);
            InputOutputV1.ThrowErrorOnEof = true;
            InputOutputV1.SetFilePath(inputFilePath);
            while (true)
            {
                try
                {
                    InputOutputV1.ReadNumber<int>();
                }
                catch
                {
                    break;
                }
            }
            InputOutputV1.Flush();
            sw.Stop();
            long elapsed2 = sw.ElapsedMilliseconds;

            Assert.IsTrue(elapsed1 <= elapsed2, "Normal: " + elapsed1 + "ms; Generic " + elapsed2 + "ms");
        }
    }
}