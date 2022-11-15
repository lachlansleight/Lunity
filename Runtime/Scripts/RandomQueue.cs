using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
	/// A generic class useful whenever you need to select random items from an array or list in sequence without repetition
	/// For example, playing random audio files, spawning random prefabs, selecting random strings, etc.
	public class RandomQueue<T>
	{
		/// Whether the order of items in the queue is randomized each time the queue is populated
		public bool Randomize;
		
		/// How many items are in the internal list of items - this value does not change
		public int FullCount => _allItems.Length;
		
		/// How many items are left in the queue - reduces each time GetNext is called
		public int RemainingCount => _queue.Count;
		
		private T[] _allItems;
		private List<T> _queue;
		
		public RandomQueue(T[] items, bool randomize = true)
		{
			if (items.Length == 0) throw new UnityException("RandomQueue requires at least one item");
			
			Randomize = randomize;
			
			_allItems = items;
			_queue = new List<T>();
			RebuildQueue();
		}
		
		public RandomQueue(List<T> items, bool randomize = true)
		{
			if (items.Count == 0) throw new UnityException("RandomQueue requires at least one item");
			
			Randomize = randomize;
			
			_allItems = items.ToArray();
			_queue = new List<T>();
			RebuildQueue();
		}

		/// Repopulates the queue using the internal list of items
		public void RebuildQueue()
		{
			_queue.Clear();
			var indices = new List<int>();
			for (var i = 0; i < _allItems.Length; i++) {
				indices.Add(i);
			}

			while (indices.Count > 0) {
				var index = Randomize ? indices[Random.Range(0, indices.Count)] : indices[0];
				indices.Remove(index);
				_queue.Add(_allItems[index]);
			}
		}

		/// Gets the next item in the queue and removes it - will repopulate the queue first if it is empty
		public T GetNext()
		{
			if (_queue.Count == 0) RebuildQueue();
			var item = _queue[0];
			_queue.RemoveAt(0);
			return item;
		}
	}
}