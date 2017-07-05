using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Packet
{

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_AUTH_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_AUTH_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public ulong  idIGG;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=64)]
        public string token;

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idIGG);
            bw.Write((ushort)System.Text.Encoding.UTF8.GetBytes(token).Length);
            bw.Write(System.Text.Encoding.UTF8.GetBytes(token));

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_AUTH_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_AUTH_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public ulong  idIGG;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=64)]
        public string cookie;
        public ushort port;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=32)]
        public string ip;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idIGG = br.ReadUInt64();
            cookie = MSG.Sgt.HexStringToString(BitConverter.ToString(br.ReadBytes(br.ReadUInt16())));
            port = br.ReadUInt16();
            ip = MSG.Sgt.HexStringToString(BitConverter.ToString(br.ReadBytes(br.ReadUInt16())));
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_DISCONNECT_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_DISCONNECT_REQUEST
    {
        public ushort wSize;
        public ushort wType;

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_VERSION_CHK_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_VERSION_CHK_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   u32CurVer;

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(u32CurVer);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_VERSION_CHK_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_VERSION_CHK_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public uint   u32CurVer;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            u32CurVer = br.ReadUInt32();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_KICKOUT_EVENT    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_KICKOUT_EVENT
    {
        public ushort wSize;
        public ushort wType;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            return this;
        }
    }  // end struct
}

