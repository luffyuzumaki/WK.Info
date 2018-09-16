﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WK.Info
{
	public class SafeMap<T> : IDictionary<string, T>
	{
		private readonly Dictionary<string, T> _inner = new Dictionary<string, T>();

		public T this[string key]
		{
			get
			{
				if (TryGetValue(key, out var value))
				{
					return value;
				}
				return default;
			}
			set
			{
				_inner[key] = value;
			}
		}

		public ICollection<string> Keys => _inner.Keys;

		public ICollection<T> Values => _inner.Values;

		public int Count => _inner.Count;

		public bool IsReadOnly => false;

		public void Add(string key, T value) => _inner.Add(key, value);

		public void Add(KeyValuePair<string, T> item) => ((IDictionary<string, T>)_inner).Add(item);

		public void Clear() => _inner.Clear();

		public bool Contains(KeyValuePair<string, T> item) => _inner.Contains(item);

		public bool ContainsKey(string key) => _inner.ContainsKey(key);

		public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex) => ((IDictionary<string, T>)_inner).CopyTo(array, arrayIndex);

		public IEnumerator<KeyValuePair<string, T>> GetEnumerator() => _inner.GetEnumerator();

		public bool Remove(string key) => _inner.Remove(key);

		public bool Remove(KeyValuePair<string, T> item) => ((IDictionary<string, T>)_inner).Remove(item);

		public bool TryGetValue(string key, out T value) => _inner.TryGetValue(key, out value);

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
