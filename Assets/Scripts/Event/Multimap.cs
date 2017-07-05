using System.Collections;
using System.Collections.Generic;

namespace SLG
{
	public class Multimap<K, V>
	{
		Dictionary<K, List<V>> m_dictionary = new Dictionary<K, List<V>>();

		public Dictionary<K, List<V>> dictionary
		{
			get
			{
				return m_dictionary;
			}
		}

		public void Add(K key, V value)
		{
			List<V> list;
			if (m_dictionary.TryGetValue(key, out list) == true)
			{
				list.Add(value);
			}
			else
			{
				list = new List<V>();
				list.Add(value);
				m_dictionary[key] = list;
			}
		}

		public bool TryGetValue(K key, out List<V> list)
		{
			bool bHit = m_dictionary.TryGetValue(key, out list);
			return bHit;
		}

		public List<V> this[K key]
		{
			get
			{
				List<V> list;
				if (m_dictionary.TryGetValue(key, out list))
				{
					return list;
				}

				list = new List<V>();
				m_dictionary[key] = list;
				return list;
			}
		}
	}
}
