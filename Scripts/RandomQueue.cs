using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
	public class RandomQueue<T>
	{
		public bool Randomize;
		
		private T[] _allItems;
		private List<T> _queue;
		
		public RandomQueue(T[] items, bool randomize = true)
		{
			Randomize = randomize;
			
			_allItems = items;
			_queue = new List<T>();
			RebuildQueue();
		}
		
		public RandomQueue(List<T> items, bool randomize = true)
		{
			Randomize = randomize;
			
			_allItems = items.ToArray();
			_queue = new List<T>();
			RebuildQueue();
		}

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

		public T GetNext()
		{
			if (_queue.Count == 0) RebuildQueue();
			var item = _queue[0];
			_queue.RemoveAt(0);
			return item;
		}
	}
}