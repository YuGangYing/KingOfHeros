using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Packet
{
    public struct MSG_CLIENT_QUERY_USER_INFO_AFTER_LOGIN_RESPONSE
    {
        public ushort wSize;
        public ushort wType;

        public byte[] data;
        public uint totolsize;
        public ushort cursize;

        //解包
        public object unpack(ref byte[] msg)
        {
            MemoryStream msTmp = new MemoryStream(msg);
            BinaryReader brTmp = new BinaryReader(msTmp);
            cursize = brTmp.ReadUInt16();
            cursize -= 8;
            ushort usMsgType = brTmp.ReadUInt16();
            totolsize = brTmp.ReadUInt32();
            data = new byte[cursize];
            Array.Copy(msg, 8, data, 0, (int)cursize);
            return this;
        }
    }
}
