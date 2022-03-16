using System;
using UnityEngine;

namespace Lunity
{
	/// A simplified generic List for keeping track of items without the memory allocation of a normal List
	public class ListNonAlloc<T>
	{
		/// The number of items currently in the list
		public int Count;

		private int _capacity;

		/// The maximum number of items in the list - attempting to add further items will call an ArgumentException
		public int Capacity => _capacity;

		private T[] _innerArray;
		
		/// Initialize a new non-allocating list of a specified capacity
		public ListNonAlloc(int capacity)
		{
			_capacity = capacity;
			_innerArray = new T[capacity];
			Count = 0;
		}

		/// Reset the counter back to zero - note that this doesn't actually clear the internal array
		public void Clear()
		{
			Count = 0;
		}

		/// Add a new item to the current index pointer element
		public void Add(T item)
		{
			if (Count >= Capacity) throw new ArgumentException();
			_innerArray[Count] = item;
			Count++;
		}

		public T this[int i]
		{
			get
			{
				if (i >= Count || i >= Capacity) throw new IndexOutOfRangeException();
				return _innerArray[i];
			}
			set
			{
				if (i >= Count || i >= Capacity) throw new IndexOutOfRangeException();
				_innerArray[i] = value;
			}
		}
	}
}