using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Packet
{

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_LOGIN_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_LOGIN_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public ulong  idIGG;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=64)]
        public string szCookie;

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idIGG);
            bw.Write((ushort)System.Text.Encoding.UTF8.GetBytes(szCookie).Length);
            bw.Write(System.Text.Encoding.UTF8.GetBytes(szCookie));

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_LOGIN_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_LOGIN_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public ushort res;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            res = br.ReadUInt16();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_SERVER_TIME_EVENT    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_SERVER_TIME_EVENT
    {
        public ushort wSize;
        public ushort wType;
        public uint   unServerTime;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            unServerTime = br.ReadUInt32();
            return this;
        }
    }  // end struct
}

