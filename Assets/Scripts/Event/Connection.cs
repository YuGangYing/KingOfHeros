using System;
using System.Collections.Generic;

namespace SLG
{
	public class Connection<T>
	{
		public Connection(int group, Subscriber subscriber, Event<T> ev)
		{
			m_group      = group;
			m_subscriber = subscriber;
			m_event      = ev;
		}

		~Connection()
		{
			m_subscriber = null;
		}

		public bool connected()
		{
			if (m_subscriber == null)
				return false;

			return true;
		}

		public void disconnect()
		{
			if (m_event != null)
				m_event.unsubscribe(this);

			m_subscriber = null;
			m_event = null;
		}

		public int group
		{
			get
			{
				return m_group;
			}
		}

		public Subscriber subscriber
		{
			get
			{
				return m_subscriber;
			}
		}

		public Event<T> getEvent()
		{
			return m_event;
		}

		private Subscriber m_subscriber;
		private int        m_group;
		private Event<T>   m_event;
	};
}
	