using System;
using UnityEngine;

namespace Lunity
{
	/// A serializable field with a property which has an OnChanged notifier - handy!
	[Serializable]
	public class LProp<T>
	{
		[SerializeField] private T _value;
		public T Value
		{
			get => _value;
			set
			{
				var changed = !_value.Equals(value);
				_value = value;
				if (changed) OnChange?.Invoke(value);
			}
		}
		public Action<T> OnChange;

		public void SetWithoutNotify(T value) => _value = value;
		
		public LProp()
		{
			Value = default;
		}

		public LProp(T value)
		{
			_value = value;
		}
	}
}