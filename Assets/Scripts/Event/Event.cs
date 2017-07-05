using System;
using System.Collections.Generic;

namespace SLG
{
	public delegate bool Subscriber(EventArgs go);

	public class Event<T>
	{	
		public Event(T name)
		{
			m_name = name;
		}

		public T getName()
		{
			return m_name;
		}

        public Connection<T> subscribe(Subscriber slot)
		{
			return subscribe(-1, slot);
		}

        public Connection<T> subscribe(int group, Subscriber slot)
		{
            List<Connection<T>> list = m_events[group];
            Connection<T> con = new Connection<T>(group, slot, this);
			list.Add(con);
			return con;
		}

		public bool fireEvent(EventArgs go)
		{
            List<Connection<T>> result_list = new List<Connection<T>>();
            Dictionary<int, List<Connection<T>>> dictionary = m_events.dictionary;
            foreach (KeyValuePair<int, List<Connection<T>>> item in dictionary)
			{
				for (int i = 0; i < item.Value.Count; ++i)
					result_list.Add(item.Value[i]);
			}

			bool bHit = false;
			for (int i = 0; i < result_list.Count; ++i)
			{
				if (result_list[i].connected() == false)
					continue;

                bHit = true;
                if ((result_list[i].subscriber)(go) == false)
                    break; // abort
			}

			return bHit;
		}

		public void unsubscribe(Connection<T> slot)
	   {
		   List<Connection<T>> connects = null;
			int groupid = slot.group;
			if (m_events.TryGetValue(groupid, out connects) == false)
				return ;

			connects.Remove(slot);
	   }

        protected Multimap<int, Connection<T>> m_events = new Multimap<int, Connection<T>>();

        protected T m_name;
	};
}
