using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Packet
{

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_RECRUIT_ARMY_REQUEST    招募士兵请求

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RECRUIT_ARMY_REQ_INFO
    {
        public uint      idArmyType;            // 士兵类型ID
        public byte      u8Amount;            // 士兵数量
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_RECRUIT_ARMY_REQUEST
    {
        public ushort wSize;
        public ushort wType;

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public RECRUIT_ARMY_REQ_INFO[] lst;

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);

            usCnt = (ushort)lst.Length;
            bw.Write(usCnt);
            for (int i = 0; i < usCnt; ++i)
            {
                bw.Write(lst[i].idArmyType);
                bw.Write(lst[i].u8Amount);
            }

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_RECRUIT_ARMY_RESPONSE    招募士兵回复

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RECRUIT_ARMY_RES_INFO
    {
        public uint      idArmyType;            // 士兵类型ID
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_RECRUIT_ARMY_RESPONSE
    {
        public ushort wSize;
        public ushort wType;

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public RECRUIT_ARMY_RES_INFO[] lst;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();

            usCnt = br.ReadUInt16();
            lst = new RECRUIT_ARMY_RES_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idArmyType = br.ReadUInt32();
            }
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_DISMISS_ARMY_REQUEST    解除士兵请求

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DISMISS_ARMY_REQ_INFO
    {
        public uint      idArmyType;            // 士兵类型ID
        public byte      u8Amount;            // 士兵数量
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_DISMISS_ARMY_REQUEST
    {
        public ushort wSize;
        public ushort wType;

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public DISMISS_ARMY_REQ_INFO[] lst;

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);

            usCnt = (ushort)lst.Length;
            bw.Write(usCnt);
            for (int i = 0; i < usCnt; ++i)
            {
                bw.Write(lst[i].idArmyType);
                bw.Write(lst[i].u8Amount);
            }

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_DISMISS_ARMY_RESPONSE    解除士兵回复

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DISMISS_ARMY_RES_INFO
    {
        public uint      idArmyType;            // 士兵类型ID
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_DISMISS_ARMY_RESPONSE
    {
        public ushort wSize;
        public ushort wType;

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public DISMISS_ARMY_RES_INFO[] lst;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();

            usCnt = br.ReadUInt16();
            lst = new DISMISS_ARMY_RES_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idArmyType = br.ReadUInt32();
            }
            return this;
        }
    }  // end struct
}

