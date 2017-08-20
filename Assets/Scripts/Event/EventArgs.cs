
namespace SLG
{
	public class EventArgs
	{
        public EventArgs()
        {

        }

        public EventArgs(object obj)
		{
            m_obj = obj;
		}

        public object m_obj;
	};
}