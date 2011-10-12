//	LumenWorks.Framework.Tests.Unit.IO.CSV.CsvRecordReaderIDataReaderTest
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

namespace Sjoerder.CsvWrapper.Tests.Unit
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using NUnit.Framework;

    [TestFixture()]
	public class CsvRecordReaderIDataReaderTest
	{
		#region IDataRecord interface

		[Test()]
		public void GetBooleanTest()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleTypedData1), true))
			{
				

				Boolean value = true;
				foreach (IDataRecord reader in csv)
				{
					Assert.AreEqual(value, reader.GetBoolean(reader.GetOrdinal(typeof(Boolean).FullName)));
				}
			}
		}

		[Test()]
		public void GetByteTest()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleTypedData1), true))
			{
				

				Byte value = 1;
				foreach (IDataRecord reader in csv)
				{
					Assert.AreEqual(value, reader.GetByte(reader.GetOrdinal(typeof(Byte).FullName)));
				}
			}
		}

		[Test()]
		public void GetBytesTest()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleTypedData1), true))
			{
				

				Char[] temp = "abc".ToCharArray();
				Byte[] value = new Byte[temp.Length];

				for (int i = 0; i < temp.Length; i++)
					value[i] = Convert.ToByte(temp[i]);

				foreach (IDataRecord reader in csv)
				{
					Byte[] csvValue = new Byte[value.Length];

					long count = reader.GetBytes(reader.GetOrdinal(typeof(String).FullName), 0, csvValue, 0, value.Length);

					Assert.AreEqual(value.Length, count);
					Assert.AreEqual(value.Length, csvValue.Length);

					for (int i = 0; i < value.Length; i++)
						Assert.AreEqual(value[i], csvValue[i]);
				}
			}
		}

		[Test()]
		public void GetCharTest()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleTypedData1), true))
			{
				

				Char value = 'a';
				foreach (IDataRecord reader in csv)
				{
					Assert.AreEqual(value, reader.GetChar(reader.GetOrdinal(typeof(Char).FullName)));
				}
			}
		}

		[Test()]
		public void GetCharsTest()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleTypedData1), true))
			{
				

				Char[] value = "abc".ToCharArray();
				foreach (IDataRecord reader in csv)
				{
					Char[] csvValue = new Char[value.Length];

					long count = reader.GetChars(reader.GetOrdinal(typeof(String).FullName), 0, csvValue, 0, value.Length);

					Assert.AreEqual(value.Length, count);
					Assert.AreEqual(value.Length, csvValue.Length);

					for (int i = 0; i < value.Length; i++)
						Assert.AreEqual(value[i], csvValue[i]);
				}
			}
		}

		[Test()]
		public void GetDataTest()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleTypedData1), true))
			{
				

				foreach (IDataRecord reader in csv)
				{
					for (int i = 0; i < reader.FieldCount; i++)
						Assert.IsNull(reader.GetData(i));
				}
			}
		}

		[Test()]
		public void GetDataTypeNameTest()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleTypedData1), true))
			{
				

				foreach (IDataRecord reader in csv)
				{
					for (int i = 0; i < reader.FieldCount; i++)
						Assert.AreEqual(typeof(string).FullName, reader.GetDataTypeName(i));
				}
			}
		}

		[Test()]
		public void GetDateTimeTest()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleTypedData1), true))
			{
				

				DateTime value = new DateTime(2001, 1, 1);
				foreach (IDataRecord reader in csv)
				{
					Assert.AreEqual(value, reader.GetDateTime(reader.GetOrdinal(typeof(DateTime).FullName)));
				}
			}
		}

		[Test()]
		public void GetDecimalTest()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleTypedData1), true))
			{
				

				Decimal value = 1;
				foreach (IDataRecord reader in csv)
				{
					Assert.AreEqual(value, reader.GetDecimal(reader.GetOrdinal(typeof(Decimal).FullName)));
				}
			}
		}

		[Test()]
		public void GetDoubleTest()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleTypedData1), true))
			{
				

				Double value = 1;
				foreach (IDataRecord reader in csv)
				{
					Assert.AreEqual(value, reader.GetDouble(reader.GetOrdinal(typeof(Double).FullName)));
				}
			}
		}

		[Test()]
		public void GetFieldTypeTest()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleTypedData1), true))
			{
				

				foreach (IDataRecord reader in csv)
				{
					for (int i = 0; i < reader.FieldCount; i++)
						Assert.AreEqual(typeof(string), reader.GetFieldType(i));
				}
			}
		}

		[Test()]
		public void GetFloatTest()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleTypedData1), true))
			{
				

				Single value = 1;
				foreach (IDataRecord reader in csv)
				{
					Assert.AreEqual(value, reader.GetFloat(reader.GetOrdinal(typeof(Single).FullName)));
				}
			}
		}

		[Test()]
		public void GetGuidTest()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleTypedData1), true))
			{
				

				Guid value = new Guid("{11111111-1111-1111-1111-111111111111}");
				foreach (IDataRecord reader in csv)
				{
					Assert.AreEqual(value, reader.GetGuid(reader.GetOrdinal(typeof(Guid).FullName)));
				}
			}
		}

		[Test()]
		public void GetInt16Test()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleTypedData1), true))
			{
				

				Int16 value = 1;
				foreach (IDataRecord reader in csv)
				{
					Assert.AreEqual(value, reader.GetInt16(reader.GetOrdinal(typeof(Int16).FullName)));
				}
			}
		}

		[Test()]
		public void GetInt32Test()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleTypedData1), true))
			{
				

				Int32 value = 1;
				foreach (IDataRecord reader in csv)
				{
					Assert.AreEqual(value, reader.GetInt32(reader.GetOrdinal(typeof(Int32).FullName)));
				}
			}
		}

		[Test()]
		public void GetInt64Test()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleTypedData1), true))
			{
				

				Int64 value = 1;
				foreach (IDataRecord reader in csv)
				{
					Assert.AreEqual(value, reader.GetInt64(reader.GetOrdinal(typeof(Int64).FullName)));
				}
			}
		}

		[Test()]
		public void GetNameTest()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), true))
			{
				

				foreach (IDataRecord reader in csv)
				{
					Assert.AreEqual(CsvReaderSampleData.SampleData1Header0, reader.GetName(0));
					Assert.AreEqual(CsvReaderSampleData.SampleData1Header1, reader.GetName(1));
					Assert.AreEqual(CsvReaderSampleData.SampleData1Header2, reader.GetName(2));
					Assert.AreEqual(CsvReaderSampleData.SampleData1Header3, reader.GetName(3));
					Assert.AreEqual(CsvReaderSampleData.SampleData1Header4, reader.GetName(4));
					Assert.AreEqual(CsvReaderSampleData.SampleData1Header5, reader.GetName(5));
				}
			}
		}

		[Test()]
		public void GetOrdinalTest()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), true))
			{
				

				foreach (IDataRecord reader in csv)
				{
					Assert.AreEqual(0, reader.GetOrdinal(CsvReaderSampleData.SampleData1Header0));
					Assert.AreEqual(1, reader.GetOrdinal(CsvReaderSampleData.SampleData1Header1));
					Assert.AreEqual(2, reader.GetOrdinal(CsvReaderSampleData.SampleData1Header2));
					Assert.AreEqual(3, reader.GetOrdinal(CsvReaderSampleData.SampleData1Header3));
					Assert.AreEqual(4, reader.GetOrdinal(CsvReaderSampleData.SampleData1Header4));
					Assert.AreEqual(5, reader.GetOrdinal(CsvReaderSampleData.SampleData1Header5));
				}
			}
		}

		[Test()]
		public void GetStringTest()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleTypedData1), true))
			{
				

				String value = "abc";
				foreach (IDataRecord reader in csv)
				{
					Assert.AreEqual(value, reader.GetString(reader.GetOrdinal(typeof(String).FullName)));
				}
			}
		}

		[Test()]
		public void GetValueTest()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), true))
			{
				

				string[] values = new string[CsvReaderSampleData.SampleData1RecordCount];

				foreach (IDataRecord reader in csv)
				{
					for (int i = 0; i < reader.FieldCount; i++)
					{
						object value = reader.GetValue(i);

                        var csvRecord = (CsvRecord)reader;
                        if (string.IsNullOrEmpty(csvRecord[i]))
							Assert.AreEqual(DBNull.Value, value);

						values[i] = value.ToString();
					}

					CsvReaderSampleData.CheckSampleData1(csv.HasHeaders, csv.CurrentRecordIndex, values);
				}
			}
		}

		[Test()]
		public void GetValuesTest()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), true))
			{
				

				object[] objValues = new object[CsvReaderSampleData.SampleData1RecordCount];
				string[] values = new string[CsvReaderSampleData.SampleData1RecordCount];

				foreach (IDataRecord reader in csv)
				{
					Assert.AreEqual(CsvReaderSampleData.SampleData1RecordCount, reader.GetValues(objValues));

					for (int i = 0; i < reader.FieldCount; i++)
					{
                        var csvRecord = (CsvRecord)reader;
                        if (string.IsNullOrEmpty(csvRecord[i]))
							Assert.AreEqual(DBNull.Value, objValues[i]);

						values[i] = objValues[i].ToString();
					}

					CsvReaderSampleData.CheckSampleData1(csv.HasHeaders, csv.CurrentRecordIndex, values);
				}
			}
		}

		[Test()]
		public void IsDBNullTest()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleTypedData1), true))
			{
				

				foreach (IDataRecord reader in csv)
				{
					Assert.IsTrue(reader.IsDBNull(reader.GetOrdinal(typeof(DBNull).FullName)));
				}
			}
		}

		[Test()]
		public void FieldCountTest()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), true))
			{
                Assert.AreEqual(CsvReaderSampleData.SampleData1RecordCount, csv.FieldCount);
			}
		}

		[Test()]
		public void IndexerByFieldNameTest()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), true))
			{
				

				string[] values = new string[CsvReaderSampleData.SampleData1RecordCount];

				foreach (IDataRecord reader in csv)
				{
					values[0] = (string) reader[CsvReaderSampleData.SampleData1Header0];
					values[1] = (string) reader[CsvReaderSampleData.SampleData1Header1];
					values[2] = (string) reader[CsvReaderSampleData.SampleData1Header2];
					values[3] = (string) reader[CsvReaderSampleData.SampleData1Header3];
					values[4] = (string) reader[CsvReaderSampleData.SampleData1Header4];
					values[5] = (string) reader[CsvReaderSampleData.SampleData1Header5];

					CsvReaderSampleData.CheckSampleData1(csv.HasHeaders, csv.CurrentRecordIndex, values);
				}
			}
		}

		[Test()]
		public void IndexerByFieldIndexTest()
		{
			using (CsvRecordReader csv = new CsvRecordReader(new StringReader(CsvReaderSampleData.SampleData1), true))
			{
				

				string[] values = new string[CsvReaderSampleData.SampleData1RecordCount];

				foreach (IDataRecord reader in csv)
				{
					for (int i = 0; i < reader.FieldCount; i++)
						values[i] = (string) reader[i];

					CsvReaderSampleData.CheckSampleData1(csv.HasHeaders, csv.CurrentRecordIndex, values);
				}
			}
		}

		#endregion
	}
}
