//	LumenWorks.Framework.Tests.Unit.IO.CSV.CsvRecordReaderTest
//	Copyright (c) 2005 Sébastien Lorion
//
//	MIT license (http://en.wikipedia.org/wiki/MIT_License)
//
//	Permission is hereby granted, free of charge, to any person obtaining a copy
//	of this software and associated documentation files (the "Software"), to deal
//	in the Software without restriction, including without limitation the rights 
//	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//	of the Software, and to permit persons to whom the Software is furnished to do so, 
//	subject to the following conditions:
//
//	The above copyright notice and this permission notice shall be included in all 
//	copies or substantial portions of the Software.
//
//	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//	INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
//	PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
//	FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//	ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


// A special thanks goes to "shriop" at CodeProject for providing many of the standard and Unicode parsing tests.


namespace Sjoerder.CsvWrapper.Tests.Unit
{
    using System;
    using System.IO;
    using System.Text;
    using LumenWorks.Framework.IO.Csv;
    using NUnit.Framework;

    [TestFixture()]
    public class CsvRecordReaderTest
    {
        CsvRecord record;

        #region Argument validation tests

        #region Constructors

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentTestCtor1()
        {
            using (CsvRecordReader csv = new CsvRecordReader(null, false))
            {
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ArgumentTestCtor2()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), false, 0))
            {
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ArgumentTestCtor3()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), false, -1))
            {
            }
        }

        #endregion

        #region Indexers

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ArgumentTestIndexer1()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), false))
            {
                record = csv.Read();
                string s = record[-1];
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ArgumentTestIndexer2()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), false))
            {
                string s = csv.Read()[CsvReaderSampleData.SampleData1RecordCount];
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ArgumentTestIndexer3()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), false))
            {
                string s = csv.Read()["asdf"];
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ArgumentTestIndexer4()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), false))
            {
                string s = csv.Read()[CsvReaderSampleData.SampleData1Header0];
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentTestIndexer5()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), false))
            {
                string s = csv.Read()[null];
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentTestIndexer6()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), false))
            {
                string s = csv.Read()[string.Empty];
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentTestIndexer7()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), true))
            {
                var record = csv.Read();
                string s = record[null];
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentTestIndexer8()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), true))
            {
                var record = csv.Read();
                string s = record[string.Empty];
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ArgumentTestIndexer9()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), true))
            {
                var record = csv.Read();
                string s = record["asdf"];
            }
        }
        #endregion

        #endregion

        #region Parsing tests

        [Test]
        public void ParsingTest1()
        {
            const string data = "1\r\n\r\n1";

            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(data), false))
            {
                CsvRecord record;

                record = csv.Read();
                Assert.NotNull(record);
                Assert.AreEqual("1", record[0]);

                record = csv.Read();
                Assert.NotNull(record);
                Assert.AreEqual("1", record[0]);

                record = csv.Read();
                Assert.IsNull(record);
            }
        }

        [Test]
        public void ParsingTest2()
        {
            // ["Bob said, ""Hey!""",2, 3 ]
            const string data = "\"Bob said, \"\"Hey!\"\"\",2, 3 ";

            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(data), false))
            {
                CsvRecord record;

                record = csv.Read();
                Assert.NotNull(record);
                Assert.AreEqual(@"Bob said, ""Hey!""", record[0]);
                Assert.AreEqual("2", record[1]);
                Assert.AreEqual("3", record[2]);

                Assert.IsNull(csv.Read());
            }

            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(data), false, ',', '"', '"', '#', ValueTrimmingOptions.None))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual(@"Bob said, ""Hey!""", record[0]);
                Assert.AreEqual("2", record[1]);
                Assert.AreEqual(" 3 ", record[2]);

                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest3()
        {
            const string data = "1\r2\n";

            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(data), false))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("1", record[0]);

                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("2", record[0]);

                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest4()
        {
            const string data = "\"\n\r\n\n\r\r\",,\t,\n";

            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(data), false))
            {
                record = csv.Read(); Assert.NotNull(record);

                Assert.AreEqual(4, csv.FieldCount);

                Assert.AreEqual("\n\r\n\n\r\r", record[0]);
                Assert.AreEqual("", record[1]);
                Assert.AreEqual("", record[2]);
                Assert.AreEqual("", record[3]);

                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest5()
        {
            Checkdata5(1024);

            // some tricky ones ...

            Checkdata5(1);
            Checkdata5(9);
            Checkdata5(14);
            Checkdata5(39);
            Checkdata5(166);
            Checkdata5(194);
        }

        [Test]
        public void ParsingTest5_RandomBufferSizes()
        {
            Random random = new Random();

            for (int i = 0; i < 1000; i++)
                Checkdata5(random.Next(1, 512));
        }

        public void Checkdata5(int bufferSize)
        {
            const string data = CsvReaderSampleData.SampleData1;

            try
            {
                using (CsvRecordReader csv = new CsvRecordReader(new StringReader(data), true, bufferSize))
                {
                    CsvReaderSampleData.CheckSampleData1(csv);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("BufferSize={0}", bufferSize), ex);
            }
        }

        [Test]
        public void ParsingTest6()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader("1,2"), false))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("1", record[0]);
                Assert.AreEqual("2", record[1]);
                Assert.AreEqual(',', csv.Delimiter);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(2, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest7()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader("\r\n1\r\n"), false))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual(',', csv.Delimiter);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                Assert.AreEqual("1", record[0]);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest8()
        {
            const string data = "\"bob said, \"\"Hey!\"\"\",2, 3 ";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false, ',', '\"', '\"', '#', ValueTrimmingOptions.UnquotedOnly))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("bob said, \"Hey!\"", record[0]);
                Assert.AreEqual("2", record[1]);
                Assert.AreEqual("3", record[2]);
                Assert.AreEqual(',', csv.Delimiter);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(3, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest9()
        {
            const string data = ",";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual(String.Empty, record[0]);
                Assert.AreEqual(String.Empty, record[1]);
                Assert.AreEqual(',', csv.Delimiter);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(2, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest10()
        {
            const string data = "1\r2";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("1", record[0]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("2", record[0]);
                Assert.AreEqual(1, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest11()
        {
            const string data = "1\n2";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("1", record[0]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("2", record[0]);
                Assert.AreEqual(1, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest12()
        {
            const string data = "1\r\n2";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("1", record[0]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("2", record[0]);
                Assert.AreEqual(1, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest13()
        {
            const string data = "1\r";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("1", record[0]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest14()
        {
            const string data = "1\n";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("1", record[0]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest15()
        {
            const string data = "1\r\n";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("1", record[0]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest16()
        {
            const string data = "1\r2\n";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false, '\r', '"', '\"', '#', ValueTrimmingOptions.UnquotedOnly))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("1", record[0]);
                Assert.AreEqual("2", record[1]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(2, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest17()
        {
            const string data = "\"July 4th, 2005\"";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("July 4th, 2005", record[0]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest18()
        {
            const string data = " 1";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false, ',', '\"', '\"', '#', ValueTrimmingOptions.None))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual(" 1", record[0]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest19()
        {
            string data = String.Empty;

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false))
            {
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest20()
        {
            const string data = "user_id,name\r\n1,Bruce";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), true))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("1", record[0]);
                Assert.AreEqual("Bruce", record[1]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(2, csv.FieldCount);
                Assert.AreEqual("1", record["user_id"]);
                Assert.AreEqual("Bruce", record["name"]);
                Assert.IsNull(csv.Read());
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest21()
        {
            const string data = "\"data \r\n here\"";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false, ',', '\"', '\"', '#', ValueTrimmingOptions.None))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("data \r\n here", record[0]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest22()
        {
            const string data = "\r\r\n1\r";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false, '\r', '\"', '\"', '#', ValueTrimmingOptions.None))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual(3, csv.FieldCount);

                Assert.AreEqual(String.Empty, record[0]);
                Assert.AreEqual(String.Empty, record[1]);
                Assert.AreEqual(String.Empty, record[2]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);

                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("1", record[0]);
                Assert.AreEqual(String.Empty, record[1]);
                Assert.AreEqual(1, csv.CurrentRecordIndex);

                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest23()
        {
            const string data = "\"double\"\"\"\"double quotes\"";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false, ',', '\"', '\"', '#', ValueTrimmingOptions.None))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("double\"\"double quotes", record[0]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest24()
        {
            const string data = "1\r";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("1", record[0]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest25()
        {
            const string data = "1\r\n";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("1", record[0]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest26()
        {
            const string data = "1\n";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("1", record[0]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest27()
        {
            const string data = "'bob said, ''Hey!''',2, 3 ";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false, ',', '\'', '\'', '#', ValueTrimmingOptions.UnquotedOnly))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("bob said, 'Hey!'", record[0]);
                Assert.AreEqual("2", record[1]);
                Assert.AreEqual("3", record[2]);
                Assert.AreEqual(',', csv.Delimiter);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(3, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest28()
        {
            const string data = "\"data \"\" here\"";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false, ',', '\0', '\\', '#', ValueTrimmingOptions.None))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("\"data \"\" here\"", record[0]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest29()
        {
            string data = new String('a', 75) + "," + new String('b', 75);

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual(new String('a', 75), record[0]);
                Assert.AreEqual(new String('b', 75), record[1]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(2, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest30()
        {
            const string data = "1\r\n\r\n1";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("1", record[0]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("1", record[0]);
                Assert.AreEqual(1, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest31()
        {
            const string data = "1\r\n# bunch of crazy stuff here\r\n1";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false, ',', '\"', '\"', '#', ValueTrimmingOptions.None))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("1", record[0]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("1", record[0]);
                Assert.AreEqual(1, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest32()
        {
            const string data = "\"1\",Bruce\r\n\"2\n\",Toni\r\n\"3\",Brian\r\n";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false, ',', '\"', '\"', '#', ValueTrimmingOptions.None))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("1", record[0]);
                Assert.AreEqual("Bruce", record[1]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(2, csv.FieldCount);
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("2\n", record[0]);
                Assert.AreEqual("Toni", record[1]);
                Assert.AreEqual(1, csv.CurrentRecordIndex);
                Assert.AreEqual(2, csv.FieldCount);
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("3", record[0]);
                Assert.AreEqual("Brian", record[1]);
                Assert.AreEqual(2, csv.CurrentRecordIndex);
                Assert.AreEqual(2, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest33()
        {
            const string data = "\"double\\\\\\\\double backslash\"";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false, ',', '\"', '\\', '#', ValueTrimmingOptions.None))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("double\\\\double backslash", record[0]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(1, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest34()
        {
            const string data = "\"Chicane\", \"Love on the Run\", \"Knight Rider\", \"This field contains a comma, but it doesn't matter as the field is quoted\"\r\n" +
                      "\"Samuel Barber\", \"Adagio for Strings\", \"Classical\", \"This field contains a double quote character, \"\", but it doesn't matter as it is escaped\"";

            using (CsvRecordReader csv = new CsvRecordReader(new System.IO.StringReader(data), false, ',', '\"', '\"', '#', ValueTrimmingOptions.UnquotedOnly))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("Chicane", record[0]);
                Assert.AreEqual("Love on the Run", record[1]);
                Assert.AreEqual("Knight Rider", record[2]);
                Assert.AreEqual("This field contains a comma, but it doesn't matter as the field is quoted", record[3]);
                Assert.AreEqual(0, csv.CurrentRecordIndex);
                Assert.AreEqual(4, csv.FieldCount);
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("Samuel Barber", record[0]);
                Assert.AreEqual("Adagio for Strings", record[1]);
                Assert.AreEqual("Classical", record[2]);
                Assert.AreEqual("This field contains a double quote character, \", but it doesn't matter as it is escaped", record[3]);
                Assert.AreEqual(1, csv.CurrentRecordIndex);
                Assert.AreEqual(4, csv.FieldCount);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest35()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader("\t"), false, '\t'))
            {
                Assert.AreEqual(2, csv.FieldCount);

                record = csv.Read(); Assert.NotNull(record);

                Assert.AreEqual(string.Empty, record[0]);
                Assert.AreEqual(string.Empty, record[1]);

                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest36()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), true))
            {
                csv.SupportsMultiline = false;
                CsvReaderSampleData.CheckSampleData1(csv);
            }
        }

        [Test]
        public void ParsingTest37()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), false))
            {
                csv.SupportsMultiline = false;
                CsvReaderSampleData.CheckSampleData1(csv);
            }
        }

        [Test]
        public void ParsingTest38()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader("abc,def,ghi\n"), false))
            {
                int fieldCount = csv.FieldCount;

                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("abc", record[0]);
                Assert.AreEqual("def", record[1]);
                Assert.AreEqual("ghi", record[2]);

                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest39()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader("00,01,   \n10,11,   "), false, CsvReader.DefaultDelimiter, CsvReader.DefaultQuote, CsvReader.DefaultEscape, CsvReader.DefaultComment, ValueTrimmingOptions.UnquotedOnly, 1))
            {
                int fieldCount = csv.FieldCount;

                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("00", record[0]);
                Assert.AreEqual("01", record[1]);
                Assert.AreEqual("", record[2]);

                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("10", record[0]);
                Assert.AreEqual("11", record[1]);
                Assert.AreEqual("", record[2]);

                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest40()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader("\"00\",\n\"10\","), false))
            {
                Assert.AreEqual(2, csv.FieldCount);

                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("00", record[0]);
                Assert.AreEqual(string.Empty, record[1]);

                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("10", record[0]);
                Assert.AreEqual(string.Empty, record[1]);

                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest41()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader("First record          ,Second record"), false, CsvReader.DefaultDelimiter, CsvReader.DefaultQuote, CsvReader.DefaultEscape, CsvReader.DefaultComment, ValueTrimmingOptions.UnquotedOnly, 16))
            {
                Assert.AreEqual(2, csv.FieldCount);

                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("First record", record[0]);
                Assert.AreEqual("Second record", record[1]);

                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void ParsingTest42()
        {
            using (var csv = new CsvRecordReader(new StringReader(" "), false))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual(1, csv.FieldCount);
                Assert.AreEqual(string.Empty, record[0]);
                Assert.IsNull(csv.Read());
            }
        }

        #endregion

        #region UnicodeParsing tests

        [Test]
        public void UnicodeParsingTest1()
        {
            // control characters and comma are skipped

            char[] raw = new char[65536 - 13];

            for (int i = 0; i < raw.Length; i++)
                raw[i] = (char)(i + 14);

            raw[44 - 14] = ' '; // skip comma

            string data = new string(raw);

            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(data), false))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual(data, record[0]);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void UnicodeParsingTest2()
        {
            byte[] buffer;

            string test = "München";

            using (MemoryStream stream = new MemoryStream())
            {
                using (TextWriter writer = new StreamWriter(stream, Encoding.Unicode))
                {
                    writer.WriteLine(test);
                }

                buffer = stream.ToArray();
            }

            using (CsvRecordReader csv = new CsvRecordReader(new StreamReader(new MemoryStream(buffer), Encoding.Unicode, false), false))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual(test, record[0]);
                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void UnicodeParsingTest3()
        {
            byte[] buffer;

            string test = "München";

            using (MemoryStream stream = new MemoryStream())
            {
                using (TextWriter writer = new StreamWriter(stream, Encoding.Unicode))
                {
                    writer.Write(test);
                }

                buffer = stream.ToArray();
            }

            using (CsvRecordReader csv = new CsvRecordReader(new StreamReader(new MemoryStream(buffer), Encoding.Unicode, false), false))
            {
                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual(test, record[0]);
                Assert.IsNull(csv.Read());
            }
        }

        #endregion

        #region FieldCount

        [Test]
        public void FieldCountTest1()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), false))
            {
                CsvReaderSampleData.CheckSampleData1(csv);
            }
        }

        #endregion

        #region GetFieldHeaders

        [Test]
        public void GetFieldHeadersTest1()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), false))
            {
                var headers = csv.GetFieldHeaders();

                Assert.IsNull(headers);
                Assert.IsFalse(csv.HasHeaders);
            }
        }

        [Test]
        public void GetFieldHeadersTest2()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), true))
            {
                var headers = csv.GetFieldHeaders();

                Assert.IsNotNull(headers);
                Assert.AreEqual(CsvReaderSampleData.SampleData1RecordCount, headers.Length);

                Assert.AreEqual(CsvReaderSampleData.SampleData1Header0, headers[0]);
                Assert.AreEqual(CsvReaderSampleData.SampleData1Header1, headers[1]);
                Assert.AreEqual(CsvReaderSampleData.SampleData1Header2, headers[2]);
                Assert.AreEqual(CsvReaderSampleData.SampleData1Header3, headers[3]);
                Assert.AreEqual(CsvReaderSampleData.SampleData1Header4, headers[4]);
                Assert.AreEqual(CsvReaderSampleData.SampleData1Header5, headers[5]);

                Assert.AreEqual(0, headers.GetFieldIndex(CsvReaderSampleData.SampleData1Header0));
                Assert.AreEqual(1, headers.GetFieldIndex(CsvReaderSampleData.SampleData1Header1));
                Assert.AreEqual(2, headers.GetFieldIndex(CsvReaderSampleData.SampleData1Header2));
                Assert.AreEqual(3, headers.GetFieldIndex(CsvReaderSampleData.SampleData1Header3));
                Assert.AreEqual(4, headers.GetFieldIndex(CsvReaderSampleData.SampleData1Header4));
                Assert.AreEqual(5, headers.GetFieldIndex(CsvReaderSampleData.SampleData1Header5));
            }
        }

        [Test]
        public void GetFieldHeadersTest_EmptyCsv()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader("#asdf\n\n#asdf,asdf"), true))
            {
                var headers = csv.GetFieldHeaders();

                Assert.IsNotNull(headers);
                Assert.AreEqual(0, headers.Length);
            }
        }

        [TestCase((string)null)]
        [TestCase("")]
        [TestCase("AnotherName")]
        public void GetFieldHeaders_WithEmptyHeaderNames(string defaultHeaderName)
        {
            if (defaultHeaderName == null)
                defaultHeaderName = "Column";

            using (var csv = new CsvRecordReader(new StringReader(",  ,,aaa,\"   \",,,"), true))
            {
                csv.DefaultHeaderName = defaultHeaderName;

                Assert.IsNull(csv.Read());
                Assert.AreEqual(8, csv.FieldCount);

                var headers = csv.GetFieldHeaders();
                Assert.AreEqual(csv.FieldCount, headers.Length);

                Assert.AreEqual("aaa", headers[3]);
                foreach (var index in new int[] { 0, 1, 2, 4, 5, 6, 7 })
                    Assert.AreEqual(defaultHeaderName + index.ToString(), headers[index]);
            }
        }

        #endregion

        #region Iteration tests

        [Test]
        public void IterationTest1()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), true))
            {
                int index = 0;

                foreach (CsvRecord record in csv)
                {
                    CsvReaderSampleData.CheckSampleData1(csv.HasHeaders, index, record.ToArray());
                    index++;
                }
            }
        }

        [Test]
        public void IterationTest2()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), true))
            {
                CsvRecord previous = null;

                foreach (CsvRecord record in csv)
                {
                    Assert.IsFalse(object.ReferenceEquals(previous, record));

                    previous = record;
                }
            }
        }

        #endregion

        #region SkipEmptyLines

        [Test]
        public void SkipEmptyLinesTest1()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader("00\n\n10"), false))
            {
                csv.SkipEmptyLines = false;

                Assert.AreEqual(1, csv.FieldCount);

                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("00", record[0]);

                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual(string.Empty, record[0]);

                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("10", record[0]);

                Assert.IsNull(csv.Read());
            }
        }

        [Test]
        public void SkipEmptyLinesTest2()
        {
            using (CsvRecordReader csv = new CsvRecordReader(new StringReader("00\n\n10"), false))
            {
                csv.SkipEmptyLines = true;

                Assert.AreEqual(1, csv.FieldCount);

                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("00", record[0]);

                record = csv.Read(); Assert.NotNull(record);
                Assert.AreEqual("10", record[0]);

                Assert.IsNull(csv.Read());
            }
        }

        #endregion

        #region Trimming tests

        [TestCase("", ValueTrimmingOptions.None, new string[] { })]
        [TestCase("", ValueTrimmingOptions.QuotedOnly, new string[] { })]
        [TestCase("", ValueTrimmingOptions.UnquotedOnly, new string[] { })]
        [TestCase(" aaa , bbb , ccc ", ValueTrimmingOptions.None, new string[] { " aaa ", " bbb ", " ccc " })]
        [TestCase(" aaa , bbb , ccc ", ValueTrimmingOptions.QuotedOnly, new string[] { " aaa ", " bbb ", " ccc " })]
        [TestCase(" aaa , bbb , ccc ", ValueTrimmingOptions.UnquotedOnly, new string[] { "aaa", "bbb", "ccc" })]
        [TestCase("\" aaa \",\" bbb \",\" ccc \"", ValueTrimmingOptions.None, new string[] { " aaa ", " bbb ", " ccc " })]
        [TestCase("\" aaa \",\" bbb \",\" ccc \"", ValueTrimmingOptions.QuotedOnly, new string[] { "aaa", "bbb", "ccc" })]
        [TestCase("\" aaa \",\" bbb \",\" ccc \"", ValueTrimmingOptions.UnquotedOnly, new string[] { " aaa ", " bbb ", " ccc " })]
        [TestCase(" aaa , bbb ,\" ccc \"", ValueTrimmingOptions.None, new string[] { " aaa ", " bbb ", " ccc " })]
        [TestCase(" aaa , bbb ,\" ccc \"", ValueTrimmingOptions.QuotedOnly, new string[] { " aaa ", " bbb ", "ccc" })]
        [TestCase(" aaa , bbb ,\" ccc \"", ValueTrimmingOptions.UnquotedOnly, new string[] { "aaa", "bbb", " ccc " })]
        public void TrimFieldValuesTest(string data, ValueTrimmingOptions trimmingOptions, params string[] expected)
        {
            using (var csv = new CsvRecordReader(new StringReader(data), false, CsvReader.DefaultDelimiter, CsvReader.DefaultQuote, CsvReader.DefaultEscape, CsvReader.DefaultComment, trimmingOptions))
            {
                foreach (CsvRecord record in csv)
                {
                    var actual = record.ToArray();
                    CollectionAssert.AreEqual(expected, actual);
                }
            }
        }

        #endregion
    }
}