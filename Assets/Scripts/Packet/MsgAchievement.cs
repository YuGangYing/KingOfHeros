using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Packet
{

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_ACHIEVEMENT_INFO_QUERY_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_ACHIEVEMENT_INFO_QUERY_REQUEST
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
// MSG_CLIENT_ACHIEVEMENT_INFO    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CLIENT_USER_ACHIEVEMENT_INFO
    {
        public uint      idAchievementType;
        public byte      cbStep;
        public uint      AccumulateValue;
        public byte      cbStatus;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_ACHIEVEMENT_INFO
    {
        public ushort wSize;
        public ushort wType;

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 90)]
        public CLIENT_USER_ACHIEVEMENT_INFO[] lst;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();

            usCnt = br.ReadUInt16();
            lst = new CLIENT_USER_ACHIEVEMENT_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idAchievementType = br.ReadUInt32();
                lst[i].cbStep = br.ReadByte();
                lst[i].AccumulateValue = br.ReadUInt32();
                lst[i].cbStatus = br.ReadByte();
            }
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_ACHIEVEMENT_GET_REWARD_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_ACHIEVEMENT_GET_REWARD_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idAchievementType;

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idAchievementType);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_ACHIEVEMENT_GET_REWARD_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_ACHIEVEMENT_GET_REWARD_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public uint   idAchievementType;
        public byte   cbRet;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idAchievementType = br.ReadUInt32();
            cbRet = br.ReadByte();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_ACHIEVEMENT_INFO_UPDATE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_ACHIEVEMENT_INFO_UPDATE
    {
        public ushort wSize;
        public ushort wType;
        public uint   idAchievementType;
        public byte   cbStep;
        public uint   AccumulateValue;
        public byte   cbStatus;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idAchievementType = br.ReadUInt32();
            cbStep = br.ReadByte();
            AccumulateValue = br.ReadUInt32();
            cbStatus = br.ReadByte();
            return this;
        }
    }  // end struct
}

