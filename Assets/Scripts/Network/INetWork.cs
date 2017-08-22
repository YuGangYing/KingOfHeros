using System;
using Packet;
using System.Reflection;
using System.Runtime.InteropServices;
//Line End
namespace Network
{
    public delegate void MessageFunc(ushort id, object ar);
    public class Proxy
    {
        private Type _Type;
        private MethodInfo[] _Info;

        public Proxy(Type type)
        {
            _Type = type;
            _Info = new MethodInfo[]
            {
                _Type.GetMethod("pack", BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(byte[]).MakeByRefType() }, null),
                _Type.GetMethod("unpack", BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(byte[]).MakeByRefType() }, null),
            };
        }

        public UInt16 Key
        {
            get { return MSG.Sgt.GetTypeCode(_Type); }
        }

        public bool CanM2B
        {
            get { return _Info[0] != null;  }
        }

        public bool CanB2M
        {
            get { return _Info[1] != null; }
        }

        public object CreateObject()
        {
            return Activator.CreateInstance(_Type);
        }

        public bool M2B(ref object msg, ref byte[] buf)
        {
            if (msg == null || _Info[0] == null)
            {
                return false;
            }

            object[] arg = new object[]{ buf };
            msg = _Info[0].Invoke(msg, arg);
            buf = (byte[])arg[0];
            return true;
        }

        public bool B2M(ref object msg, ref byte[] buf)
        {
            if (msg == null || _Info[1] == null)
            {
                return false;
            }

            object[] arg = new object[] { buf };
            msg = _Info[1].Invoke(msg, arg);
            buf = (byte[])arg[0];

            return true;
        }
    };

	public class NetPacket
    {
        public int length;
		public int msgId;
		public byte[] data;

		public NetPacket()
		{
		}

		public NetPacket(int nLength,int nMsgId,byte[] bData)
        {
            length = nLength;
			msgId = nMsgId;
			data = bData;
		}
	}

    public abstract class MsgFactoryBase
    {
        public abstract void Convert(ref object msg, ref byte[] buf);
        public abstract void Convert(ref byte[] buf, ref object msg);
        
    }

    public class MsgPacketFactory<T> : MsgFactoryBase where T : new()
    {
        public object Create()
        {
            return new T();
        }

        public override void Convert(ref object msg, ref byte[] buf)
        {
        }

        public override void Convert(ref byte[] buf, ref object msg)
        {
        }
    }

    public class NetMsgBase
    {
        public UInt16 m_msg;
        public Object m_obj;

        public NetMsgBase()
        {

        }

        public NetMsgBase(UInt16 msg, Object obj)
        {
            m_msg = msg;
            m_obj = obj;
        }

        /// <summary>
        /// 对消息接收后的处理
        /// </summary>
        public static void Process(UInt16 msg, Object obj) { }
    }

}