using System;
using System.Collections.Generic;

namespace SLG
{
    public class EventSet<T>
	{
		public EventSet()
		{
			m_muted = false;
		}

        public Event<T> getEvent(T name, bool bCreateIfMissing)
		{
            Event<T> ev = null;
			if (m_events.TryGetValue(name, out ev) == true)
				return ev; 

			if (bCreateIfMissing == false)
				return null;

            ev = new Event<T>(name);
			m_events.Add(name, ev);
			return ev;
		}

		public void removeEvent(T name)
		{
			m_events.Remove(name);
		}
		
		public void removeAllEvents()
		{
			m_events.Clear();
		}

		public bool isEventPresent(T name)
		{
            Event<T> ev = null;
			return m_events.TryGetValue(name, out ev);
		}

		virtual public Connection<T> subscribeEvent(T name, Subscriber subscriber)
		{
			return subscribeEvent(name, -1, subscriber);
		}

		virtual public Connection<T> subscribeEvent(T name, int group, Subscriber subscriber)
		{
            Event<T> ev = getEvent(name, true);
			return ev.subscribe(group, subscriber);
		}

		public virtual void fireEvent(T name, EventArgs args)
		{
			fireEvent_impl(name, args);
		}

		public virtual bool fireEvent_impl(T name, EventArgs args)
		{
			if (m_muted == true)
				return false;

            Event<T> ev = null;
			if (m_events.TryGetValue(name, out ev) == false)
				return false;

			return ev.fireEvent(args);
		}

		public bool isMuted
		{
			get
			{
				return m_muted;
			}

			set
			{
				m_muted = value;
			}
		}

        protected Dictionary<T, Event<T>> m_events = new Dictionary<T, Event<T>>();

		protected bool m_muted;
	};
}
