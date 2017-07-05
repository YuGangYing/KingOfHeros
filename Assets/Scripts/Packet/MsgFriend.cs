using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Packet
{

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_FRIEND_INFO    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CLIENT_USER_FRIEND_INFO
    {
        public uint      idFriendUser;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=16)]
        public string szFriendName;
        public uint      nGoldLevyTime;
        public uint      nStoneLevyTime;
        public byte      nFriendStatus;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_FRIEND_INFO
    {
        public ushort wSize;
        public ushort wType;

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 45)]
        public CLIENT_USER_FRIEND_INFO[] lst;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();

            usCnt = br.ReadUInt16();
            lst = new CLIENT_USER_FRIEND_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idFriendUser = br.ReadUInt32();
                lst[i].szFriendName = MSG.Sgt.HexStringToString(BitConverter.ToString(br.ReadBytes(br.ReadUInt16())));
                lst[i].nGoldLevyTime = br.ReadUInt32();
                lst[i].nStoneLevyTime = br.ReadUInt32();
                lst[i].nFriendStatus = br.ReadByte();
            }
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_FRIEND_INFO_UPDATE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_FRIEND_INFO_UPDATE
    {
        public ushort wSize;
        public ushort wType;
        public uint   idFriendUser;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=16)]
        public string szFriendName;
        public uint   nGoldLevyTime;
        public uint   nStoneLevyTime;
        public byte   nFriendStatus;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idFriendUser = br.ReadUInt32();
            szFriendName = MSG.Sgt.HexStringToString(BitConverter.ToString(br.ReadBytes(br.ReadUInt16())));
            nGoldLevyTime = br.ReadUInt32();
            nStoneLevyTime = br.ReadUInt32();
            nFriendStatus = br.ReadByte();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_FRIEND_APPLY_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_FRIEND_APPLY_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idTarget;                      // 被申请玩家ID

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idTarget);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_FRIEND_APPLY_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_FRIEND_APPLY_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public byte   cbRet;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            cbRet = br.ReadByte();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_FRIEND_REFUSE_APPLY_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_FRIEND_REFUSE_APPLY_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idTarget;                      // 申请玩家ID

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idTarget);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_FRIEND_REFUSE_APPLY_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_FRIEND_REFUSE_APPLY_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public byte   cbRet;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            cbRet = br.ReadByte();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_FRIEND_ACCEPT_APPLY_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_FRIEND_ACCEPT_APPLY_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idTarget;                      // 申请玩家ID

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idTarget);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_FRIEND_ACCEPT_APPLY_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_FRIEND_ACCEPT_APPLY_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public byte   cbRet;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            cbRet = br.ReadByte();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_FRIEND_DEL_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_FRIEND_DEL_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idTarget;                      // 删除玩家ID

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idTarget);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_FRIEND_DEL_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_FRIEND_DEL_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public byte   cbRet;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            cbRet = br.ReadByte();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_FRIEND_INFO_QUERY_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_FRIEND_INFO_QUERY_REQUEST
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
// MSG_CLIENT_FRIEND_ENTER_CITY_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_FRIEND_ENTER_CITY_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idTarget;                      // 进入玩家ID

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idTarget);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_FRIEND_ENTER_CITY_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_FRIEND_ENTER_CITY_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public byte   cbRet;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            cbRet = br.ReadByte();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_FRIEND_LEVY_RESOURCE_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_FRIEND_LEVY_RESOURCE_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idTarget;                      // 收集资源玩家ID
        public byte   nResourceType;                 // 资源类型0=金矿1=魔石

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idTarget);
            bw.Write(nResourceType);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_FRIEND_LEVY_RESOURCE_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_FRIEND_LEVY_RESOURCE_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public byte   cbRet;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            cbRet = br.ReadByte();
            return this;
        }
    }  // end struct
}

