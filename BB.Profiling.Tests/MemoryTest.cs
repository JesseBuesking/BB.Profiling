using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using NUnit.Framework;

namespace BB.Profiling.Tests
{
	[TestFixture]
	public class MemoryTest
	{
		[Test]
		[Ignore("Performance test that doesn't need to be run every time.")]
		public void TestPrimitiveLookupSpeed()
		{
			const long iterations = 100000000;

			System.Type t = typeof(System.UInt64);

			var types = new Dictionary<System.Type, long>
				{
					{BB.Profiling.Type.Boolean, BB.Profiling.Size.Boolean},
					{BB.Profiling.Type.Byte, BB.Profiling.Size.Byte},
					{BB.Profiling.Type.Char, BB.Profiling.Size.Char},
					{BB.Profiling.Type.Decimal, BB.Profiling.Size.Decimal},
					{BB.Profiling.Type.Double, BB.Profiling.Size.Double},
					{BB.Profiling.Type.Int16, BB.Profiling.Size.Int16},
					{BB.Profiling.Type.Int32, BB.Profiling.Size.Int32},
					{BB.Profiling.Type.Int64, BB.Profiling.Size.Int64},
					{BB.Profiling.Type.SByte, BB.Profiling.Size.SByte},
					{BB.Profiling.Type.Single, BB.Profiling.Size.Single},
					{BB.Profiling.Type.UInt16, BB.Profiling.Size.UInt16},
					{BB.Profiling.Type.UInt32, BB.Profiling.Size.UInt32},
					{BB.Profiling.Type.UInt64, BB.Profiling.Size.UInt64}
				};

			long dummy = 0;

			var sw = new Stopwatch();

			sw.Start();
			for (int i = 0; i < iterations; i++)
			{
				if (BB.Profiling.Type.Boolean == t)
					dummy = BB.Profiling.Size.Boolean;
				else if (BB.Profiling.Type.Byte == t)
					dummy = BB.Profiling.Size.Byte;
				else if (BB.Profiling.Type.Char == t)
					dummy = BB.Profiling.Size.Char;
				else if (BB.Profiling.Type.Decimal == t)
					dummy = BB.Profiling.Size.Decimal;
				else if (BB.Profiling.Type.Double == t)
					dummy = BB.Profiling.Size.Double;
				else if (BB.Profiling.Type.Int16 == t)
					dummy = BB.Profiling.Size.Int16;
				else if (BB.Profiling.Type.Int32 == t)
					dummy = BB.Profiling.Size.Int32;
				else if (BB.Profiling.Type.Int64 == t)
					dummy = BB.Profiling.Size.Int64;
				else if (BB.Profiling.Type.SByte == t)
					dummy = BB.Profiling.Size.SByte;
				else if (BB.Profiling.Type.Single == t)
					dummy = BB.Profiling.Size.Single;
				else if (BB.Profiling.Type.UInt16 == t)
					dummy = BB.Profiling.Size.UInt64;
				else if (BB.Profiling.Type.UInt32 == t)
					dummy = BB.Profiling.Size.UInt32;
				else if (BB.Profiling.Type.UInt64 == t)
					dummy = BB.Profiling.Size.UInt64;
			}
			sw.Stop();

			long ifStatementDuration = sw.ElapsedMilliseconds;

			sw.Reset();
			sw.Start();
			for (int i = 0; i < iterations; i++)
			{
				types.TryGetValue(t, out dummy);
			}
			sw.Stop();

			long dictionaryLookupDuration = sw.ElapsedMilliseconds;

			Assert.Less(dictionaryLookupDuration, ifStatementDuration);
			Assert.AreEqual(sizeof(System.UInt64), dummy);
		}

		private class Bar
		{
			public int Test;
		}

		private class Foo
		{
			public int Test;

			private List<int> _blahInt = new List<int>
				{
					1,2,3,4,5
				};

			private List<Bar> _blahBar = new List<Bar>
				{
					new Bar(),
					new Bar(),
					new Bar(),
					new Bar()
				};
		}

		[Test]
		public void TestFooClass()
		{
			long fooSize = Memory.GetSize(new Foo());
			Assert.AreEqual(40, fooSize);
		}

		[Test]
		public void TestFooClassArray()
		{
			long fooArraySize = Memory.GetSize(new[] { new Foo(), new Foo(), new Foo() });
			Assert.AreEqual(120, fooArraySize);
		}

		[Test]
		public void TestNull()
		{
			long nullsize = Memory.GetSize(null);
			Assert.AreEqual(0, nullsize);
		}

		[Test]
		public void TestBool()
		{
			long boolsize = Memory.GetSize(new bool());
			Assert.AreEqual(1, boolsize);
		}

		[Test]
		public void TestBooleanArray()
		{
			long boolsize = Memory.GetSize(new[] { true, false, true });
			Assert.AreEqual(3, boolsize);
		}

		[Test]
		public void TestByte()
		{
			long bytesize = Memory.GetSize(new byte());
			Assert.AreEqual(1, bytesize);
		}

		[Test]
		public void TestByteArray()
		{
			long bytesize = Memory.GetSize(new[] { new byte(), new byte(), new byte() });
			Assert.AreEqual(3, bytesize);
		}

		[Test]
		public void TestChar()
		{
			long charsize = Memory.GetSize(new char());
			Assert.AreEqual(2, charsize);
		}

		[Test]
		public void TestCharArray()
		{
			long charsize = Memory.GetSize(new[] { new char(), new char(), new char() });
			Assert.AreEqual(6, charsize);
		}

		[Test]
		public void TestDecimal()
		{
			long decimalsize = Memory.GetSize(new decimal());
			Assert.AreEqual(16, decimalsize);
		}

		[Test]
		public void TestDecimalArray()
		{
			long decimalsize = Memory.GetSize(new[] { new decimal(), new decimal(), new decimal() });
			Assert.AreEqual(48, decimalsize);
		}

		[Test]
		public void TestDouble()
		{
			long doublesize = Memory.GetSize(new double());
			Assert.AreEqual(8, doublesize);
		}

		[Test]
		public void TestDoubleArray()
		{
			long doublesize = Memory.GetSize(new[] { new double(), new double(), new double() });
			Assert.AreEqual(24, doublesize);
		}

		[Test]
		public void TestInt16()
		{
			long int16Size = Memory.GetSize(new Int16());
			Assert.AreEqual(2, int16Size);
		}

		[Test]
		public void TestInt16Array()
		{
			long int16Size = Memory.GetSize(new[] { new Int16(), new Int16(), new Int16() });
			Assert.AreEqual(6, int16Size);
		}

		[Test]
		public void TestInt32()
		{
			long int32Size = Memory.GetSize(new Int32());
			Assert.AreEqual(4, int32Size);
		}

		[Test]
		public void TestInt32Array()
		{
			long int32Size = Memory.GetSize(new[] { new Int32(), new Int32(), new Int32() });
			Assert.AreEqual(12, int32Size);
		}

		[Test]
		public void TestInt64()
		{
			long int64Size = Memory.GetSize(new Int64());
			Assert.AreEqual(8, int64Size);
		}

		[Test]
		public void TestInt64Array()
		{
			long int64Size = Memory.GetSize(new[] { new Int64(), new Int64(), new Int64() });
			Assert.AreEqual(24, int64Size);
		}

		[Test]
		public void TestSByte()
		{
			long sbytesize = Memory.GetSize(new sbyte());
			Assert.AreEqual(1, sbytesize);
		}

		[Test]
		public void TestSByteArray()
		{
			long sbytesize = Memory.GetSize(new[] { new sbyte(), new sbyte(), new sbyte() });
			Assert.AreEqual(3, sbytesize);
		}

		[Test]
		public void TestSingle()
		{
			long singlesize = Memory.GetSize(new Single());
			Assert.AreEqual(4, singlesize);
		}

		[Test]
		public void TestSingleArray()
		{
			long singlesize = Memory.GetSize(new[] { new Single(), new Single(), new Single() });
			Assert.AreEqual(12, singlesize);
		}

		[Test]
		public void TestUInt16()
		{
			long uint16Size = Memory.GetSize(new UInt16());
			Assert.AreEqual(2, uint16Size);
		}

		[Test]
		public void TestUInt16Array()
		{
			long uint16Size = Memory.GetSize(new[] { new UInt16(), new UInt16(), new UInt16() });
			Assert.AreEqual(6, uint16Size);
		}

		[Test]
		public void TestUInt32()
		{
			long uint32Size = Memory.GetSize(new UInt32());
			Assert.AreEqual(4, uint32Size);
		}

		[Test]
		public void TestUInt32Array()
		{
			long uint32Size = Memory.GetSize(new[] { new UInt32(), new UInt32(), new UInt32() });
			Assert.AreEqual(12, uint32Size);
		}

		[Test]
		public void TestUInt64()
		{
			long uint64Size = Memory.GetSize(new UInt64());
			Assert.AreEqual(8, uint64Size);
		}

		[Test]
		public void TestUInt64Array()
		{
			long uint64Size = Memory.GetSize(new[] { new UInt64(), new UInt64(), new UInt64() });
			Assert.AreEqual(24, uint64Size);
		}

		[Test]
		public void TestPrimitiveArrayList()
		{
			long arrayListIntSize = Memory.GetSize(new ArrayList { 1, 2, 3, 4 });
			Assert.AreEqual(16, arrayListIntSize);
		}

		[Test]
		public void TestNullArrayList()
		{
			long nullArrayListSize = Memory.GetSize((ArrayList)null);
			Assert.AreEqual(0, nullArrayListSize);
		}

		[Test]
		public void TestPrimitiveList()
		{
			long listIntSize = Memory.GetSize(new List<int> { 1, 2, 3, 4 });
			Assert.AreEqual(16, listIntSize);
		}

		[Test]
		public void TestFooList()
		{
			long listFooSize = Memory.GetSize(new List<Foo> { new Foo(), new Foo(), new Foo(), new Foo() });
			Assert.AreEqual(160, listFooSize);
		}

		[Test]
		public void TestNullList()
		{
			long nullListSize = Memory.GetSize((IList)null);
			Assert.AreEqual(0, nullListSize);
		}

		[Test]
		[Ignore("Not implemented")]
		public void TestPrimitiveHashtable()
		{
			long hashtableSize = Memory.GetSize(new Hashtable { { 1, 1 }, { 2, 2 }, { 3, 3 }, { 4, 4 } });
			Assert.AreEqual(32, hashtableSize);
		}

		[Test]
		[Ignore("Not implemented")]
		public void TestNullHashtable()
		{
			long nullHashtableSize = Memory.GetSize((Hashtable)null);
			Assert.AreEqual(0, nullHashtableSize);
		}

		[Test]
		public void TestPrimitiveDictionary()
		{
			long dictionaryIntSize = Memory.GetSize(new Dictionary<int, int> { { 1, 1 }, { 2, 2 }, { 3, 3 }, { 4, 4 } });
			Assert.AreEqual(32, dictionaryIntSize);
		}

		[Test]
		public void TestFooDictionary()
		{
			long dictionaryFooSize = Memory.GetSize(new Dictionary<int, Foo> { { 1, new Foo() }, { 2, new Foo() }, 
				{ 3, new Foo() }, { 4, new Foo() } });
			Assert.AreEqual(176, dictionaryFooSize);
		}

		[Test]
		public void TestFooFooDictionary()
		{
			long dictionaryFooSize = Memory.GetSize(new Dictionary<Foo, Foo> { { new Foo(), new Foo() }, 
				{ new Foo(), new Foo() }, { new Foo(), new Foo() }, { new Foo(), new Foo() } });
			Assert.AreEqual(320, dictionaryFooSize);
		}

		[Test]
		public void TestNullDictionary()
		{
			long nullDictionarySize = Memory.GetSize((IDictionary)null);
			Assert.AreEqual(0, nullDictionarySize);
		}

		[Test]
		[Ignore("Not implemented")]
		public void TestPrimitiveLinkedList()
		{
			var myLinkedList = new LinkedList<int>();
			myLinkedList.AddFirst(1);
			myLinkedList.AddFirst(2);
			myLinkedList.AddFirst(3);
			myLinkedList.AddFirst(4);
			long linkedListIntSize = Memory.GetSize(myLinkedList);
			Assert.AreEqual(16, linkedListIntSize);
		}

		[Test]
		[Ignore("Not implemented")]
		public void TestNullLinkedList()
		{
			long nullLinkedListSize = Memory.GetSize((LinkedList<int>)null);
			Assert.AreEqual(0, nullLinkedListSize);
		}

		[Test]
		public void TestPrimitiveSortedDictionary()
		{
			long sortedDictionaryIntSize = Memory.GetSize(
				new SortedDictionary<int, int> { { 1, 1 }, { 2, 2 }, { 3, 3 }, { 4, 4 } });
			Assert.AreEqual(32, sortedDictionaryIntSize);
		}

		[Test]
		public void TestNullSortedDictionary()
		{
			long nullSortedDictionarySize = Memory.GetSize((SortedDictionary<int, int>)null);
			Assert.AreEqual(0, nullSortedDictionarySize);
		}

		private class Keyed : KeyedCollection<int, int>
		{
			protected override int GetKeyForItem(int item)
			{
				return item;
			}
		}

		[Test]
		[Ignore("Not implemented")]
		public void TestPrimitiveKeyedCollection()
		{
			long keyedCollectionIntSize = Memory.GetSize(new Keyed { 1, 2, 3, 4 });
			Assert.AreEqual(16, keyedCollectionIntSize);
		}

		[Test]
		[Ignore("Not implemented")]
		public void TestNullKeyedCollection()
		{
			long nullKeyedCollectionSize = Memory.GetSize((Keyed)null);
			Assert.AreEqual(0, nullKeyedCollectionSize);
		}
	}
}
