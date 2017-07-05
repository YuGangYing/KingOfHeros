using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Packet
{

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_BATTLE_PVE_ENTERTIMES_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_BATTLE_PVE_ENTERTIMES_REQUEST
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
// MSG_CLIENT_BATTLE_PVE_ENTERTIMES_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BATTLE_PVE_ENTERTIMES_INFO
    {
        public uint      idBattle;
        public uint      idField;
        public ushort    u16EnterTimes;
        public byte      cbMaxStar;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_BATTLE_PVE_ENTERTIMES_RESPONSE
    {
        public ushort wSize;
        public ushort wType;

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
        public BATTLE_PVE_ENTERTIMES_INFO[] lst;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();

            usCnt = br.ReadUInt16();
            lst = new BATTLE_PVE_ENTERTIMES_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idBattle = br.ReadUInt32();
                lst[i].idField = br.ReadUInt32();
                lst[i].u16EnterTimes = br.ReadUInt16();
                lst[i].cbMaxStar = br.ReadByte();
            }
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_BATTLE_PVE_START_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_BATTLE_PVE_START_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idBattle;
        public uint   idField;
        public uint   u32TotalHP;
        public byte   cbBuy;

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idBattle);
            bw.Write(idField);
            bw.Write(u32TotalHP);
            bw.Write(cbBuy);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_BATTLE_PVE_START_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_BATTLE_PVE_START_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public uint   idBattle;
        public uint   idField;
        public uint   wErrCode;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idBattle = br.ReadUInt32();
            idField = br.ReadUInt32();
            wErrCode = br.ReadUInt32();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_BATTLE_PVE_END_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HERO_BATTLE_END_INFO
    {
        public uint      idHero;
        public uint      u32HeroHP;
        public byte      cbArmyDieAmount;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_BATTLE_PVE_END_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idBattle;
        public uint   idField;
        public uint   u32TotalHPRemain;
        public byte   cbResult;

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public HERO_BATTLE_END_INFO[] lst;

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idBattle);
            bw.Write(idField);
            bw.Write(u32TotalHPRemain);
            bw.Write(cbResult);

            usCnt = (ushort)lst.Length;
            bw.Write(usCnt);
            for (int i = 0; i < usCnt; ++i)
            {
                bw.Write(lst[i].idHero);
                bw.Write(lst[i].u32HeroHP);
                bw.Write(lst[i].cbArmyDieAmount);
            }

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_BATTLE_PVE_REWARD_EVENT    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HERO_EXP_ADD
    {
        public uint      idHero;
        public uint      u32AddExp;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_BATTLE_PVE_REWARD_EVENT
    {
        public ushort wSize;
        public ushort wType;
        public uint   idBattle;
        public uint   idField;
        public byte   cbResult;
        public byte   cbMaxStar;
        public uint   u32Coin;
        public uint   u32Stone;
        public uint   u32Diamond;
        public uint   idItemType1;
        public uint   u32Amount1;
        public uint   idItemType2;
        public uint   u32Amount2;

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public HERO_EXP_ADD[] lst;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idBattle = br.ReadUInt32();
            idField = br.ReadUInt32();
            cbResult = br.ReadByte();
            cbMaxStar = br.ReadByte();
            u32Coin = br.ReadUInt32();
            u32Stone = br.ReadUInt32();
            u32Diamond = br.ReadUInt32();
            idItemType1 = br.ReadUInt32();
            u32Amount1 = br.ReadUInt32();
            idItemType2 = br.ReadUInt32();
            u32Amount2 = br.ReadUInt32();

            usCnt = br.ReadUInt16();
            lst = new HERO_EXP_ADD[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idHero = br.ReadUInt32();
                lst[i].u32AddExp = br.ReadUInt32();
            }
            return this;
        }
    }  // end struct
}

