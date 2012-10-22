using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace BB.Profiling
{
	public static class Memory
	{
		#region Statics

		private static readonly Dictionary<System.Type, long> _primitiveLookup =
			new Dictionary<System.Type, long>
			{
				{ BB.Profiling.Type.Boolean, BB.Profiling.Size.Boolean },
				{ BB.Profiling.Type.Byte, BB.Profiling.Size.Byte },
				{ BB.Profiling.Type.Char, BB.Profiling.Size.Char },
				{ BB.Profiling.Type.Decimal, BB.Profiling.Size.Decimal },
				{ BB.Profiling.Type.Double, BB.Profiling.Size.Double },
				{ BB.Profiling.Type.Int16, BB.Profiling.Size.Int16 },
				{ BB.Profiling.Type.Int32, BB.Profiling.Size.Int32 },
				{ BB.Profiling.Type.Int64, BB.Profiling.Size.Int64 },
				{ BB.Profiling.Type.SByte, BB.Profiling.Size.SByte },
				{ BB.Profiling.Type.Single, BB.Profiling.Size.Single },
				{ BB.Profiling.Type.UInt16, BB.Profiling.Size.UInt16 },
				{ BB.Profiling.Type.UInt32, BB.Profiling.Size.UInt32 },
				{ BB.Profiling.Type.UInt64, BB.Profiling.Size.UInt64 }
			};

		private static readonly Dictionary<System.Type, long> _arrayLookup =
			new Dictionary<System.Type, long>
			{
				{ BB.Profiling.Type.BooleanArray, BB.Profiling.Size.Boolean },
				{ BB.Profiling.Type.ByteArray, BB.Profiling.Size.Byte },
				{ BB.Profiling.Type.CharArray, BB.Profiling.Size.Char },
				{ BB.Profiling.Type.DecimalArray, BB.Profiling.Size.Decimal },
				{ BB.Profiling.Type.DoubleArray, BB.Profiling.Size.Double },
				{ BB.Profiling.Type.Int16Array, BB.Profiling.Size.Int16 },
				{ BB.Profiling.Type.Int32Array, BB.Profiling.Size.Int32 },
				{ BB.Profiling.Type.Int64Array, BB.Profiling.Size.Int64 },
				{ BB.Profiling.Type.SByteArray, BB.Profiling.Size.SByte },
				{ BB.Profiling.Type.SingleArray, BB.Profiling.Size.Single },
				{ BB.Profiling.Type.UInt16Array, BB.Profiling.Size.UInt16 },
				{ BB.Profiling.Type.UInt32Array, BB.Profiling.Size.UInt32 },
				{ BB.Profiling.Type.UInt64Array, BB.Profiling.Size.UInt64 }
			};

		#endregion // Statics

		#region Methods

		public static long GetSize(object obj)
		{
			long size;
			if (null == obj)
				return 0;

			System.Type t = obj.GetType();

			if (_primitiveLookup.TryGetValue(t, out size))
				return size;

			if (_arrayLookup.TryGetValue(t, out size))
			{
				var array = obj as Array;
				if (null != array) return size * array.Length;
			}

			if (t.IsArray)
			{
				var objArray = (object[])obj;
				return Memory.GetSize(objArray[0]) * objArray.Length;
			}

			if (t.IsGenericType)
			{
				System.Type genericType = t.GetGenericTypeDefinition();

				if (BB.Profiling.Type.List == genericType)
					return GetListSize(t, obj);

				if (BB.Profiling.Type.Dictionary == genericType || BB.Profiling.Type.SortedDictionary == genericType)
					return GetDictionarySize(t, obj);

				//if (BB.Profiling.Type.LinkedList == genericType)
				//{
				//    var objLinkedList = obj as LinkedList<>;
				//}
			}

			if (BB.Profiling.Type.ArrayList == t)
				return GetArrayListSize(obj);

			FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
				BindingFlags.Instance | BindingFlags.Static);

			foreach (FieldInfo fieldInfo in fields)
			{
				if (null == fieldInfo.FieldType.BaseType)
					throw new NullReferenceException("fieldInfo BaseType is null.");

				if (BB.Profiling.Type.ValueType == fieldInfo.FieldType.BaseType)
					size += Memory.GetValueTypeSize(fieldInfo.FieldType);
				else if (BB.Profiling.Type.Array == fieldInfo.FieldType.BaseType)
					size += Memory.GetValueTypeSizeArray(fieldInfo, obj);
				else if (BB.Profiling.Type.String == fieldInfo.FieldType)
					size += Memory.GetStringSize(fieldInfo, obj);
				else
					size += Memory.GetSize(fieldInfo.GetValue(obj));
			}

			return size;
		}

		private static long GetStringSize(FieldInfo fieldInfo, object obj)
		{
			var subObj = fieldInfo.GetValue(obj) as String;
			if (null == subObj)
				return 0;

			return subObj.Length * BB.Profiling.Size.Char;
		}

		private static long GetArrayListSize(object obj)
		{
			var arrayList = obj as ArrayList;
			if (null == arrayList)
				return 0;

			System.Type arrayListType = arrayList[0].GetType();

			long arrayListSize;
			if (Memory._primitiveLookup.TryGetValue(arrayListType, out arrayListSize))
				return arrayListSize * arrayList.Count;

			foreach (var ele in arrayList)
				arrayListSize += Memory.GetSize(ele);

			return arrayListSize;
		}

		private static long GetListSize(System.Type t, object obj)
		{
			var objList = obj as IList;
			if (null == objList)
				return 0;

			long listSize;
			if (Memory._primitiveLookup.TryGetValue(t.GetGenericArguments()[0], out listSize))
				return listSize * objList.Count;

			foreach (var ele in objList)
				listSize += Memory.GetSize(ele);

			return listSize;
		}

		private static long GetDictionarySize(System.Type t, object obj)
		{
			var objDictionary = obj as IDictionary;
			if (null == objDictionary)
				return 0;

			long numberOfItems = objDictionary.Count;
			long keySize;
			long valueSize;
			long dictionarySize = 0;
			System.Type[] dictionaryTypes = t.GetGenericArguments();
			if (Memory._primitiveLookup.TryGetValue(dictionaryTypes[0], out keySize))
				dictionarySize += keySize * numberOfItems;
			else
			{
				foreach (var ele in objDictionary.Keys)
					dictionarySize += Memory.GetSize(ele);
			}

			if (Memory._primitiveLookup.TryGetValue(dictionaryTypes[1], out valueSize))
				dictionarySize += valueSize * numberOfItems;
			else
			{
				foreach (var ele in objDictionary.Values)
					dictionarySize += Memory.GetSize(ele);
			}

			return dictionarySize;
		}

		private static long GetValueTypeSize(System.Type type)
		{
			long value;
			if (Memory._primitiveLookup.TryGetValue(type, out value))
				return value;

			throw new Exception("Unknown type supplied: " + type);
		}

		private static long GetValueTypeSizeArray(FieldInfo fieldInfo, object obj)
		{
			var subObj = fieldInfo.GetValue(obj) as Array;
			if (null == subObj)
				return 0;

			long arrayValue;
			if (Memory._arrayLookup.TryGetValue(fieldInfo.FieldType, out arrayValue))
				return arrayValue * subObj.Length;

			throw new Exception("Unknown type supplied: " + fieldInfo.FieldType);
		}

		#endregion // Methods
	}
}
