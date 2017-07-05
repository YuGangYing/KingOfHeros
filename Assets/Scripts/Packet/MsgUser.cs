using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Packet
{

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_USER_INFO_EVENT    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_USER_INFO_EVENT
    {
        public ushort wSize;
        public ushort wType;
        public byte   vip;
        public uint   coin;
        public uint   stone;
        public uint   rmb;
        public uint   unReserveSoldier;              // 后备兵力数量
        public uint   unRecruitSoldierNum;           // 正在招募中的后备兵力数量
        public uint   unRecruitSoldierFinshTime;     // 招募到期时间(时间戳)
        public byte   u8DrawCnt;                     // 每日免费抽取英雄次数
        public uint   unDrawTime;                    // 上次免费抽取英雄时间
        public uint   unDrawHeroCntByStone;          // 使用魔石抽取英雄次数
        public uint   unDrawHeroTimeByStone;         // 上次使用魔石抽取英雄时间
        public ushort usArmyInfantry;                // 步兵的当前数量
        public ushort usArmyShield;                  // 盾兵的当前数量
        public ushort usArmyCavalry;                 // 骑兵的当前数量
        public ushort usArmyArcher;                  // 弓兵的当前数量
        public ushort usArmySpell;                   // 法兵的当前数量
        public uint   unExp;                         // 经验
        public uint   idBttle1;                      // 开启普通战役ID
        public uint   idField1;                      // 开启普通关卡ID
        public uint   idBttle2;                      // 开启活动战役ID
        public uint   idField2;                      // 开启活动关卡ID
        public uint   idBttle3;                      // 开启精英战役ID
        public uint   idField3;                      // 开启精英关卡ID
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=67)]
        public string szHeroLst;// 图鉴 十六进制字符串
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=16)]
        public string szName;// 玩家名字

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            vip = br.ReadByte();
            coin = br.ReadUInt32();
            stone = br.ReadUInt32();
            rmb = br.ReadUInt32();
            unReserveSoldier = br.ReadUInt32();
            unRecruitSoldierNum = br.ReadUInt32();
            unRecruitSoldierFinshTime = br.ReadUInt32();
            u8DrawCnt = br.ReadByte();
            unDrawTime = br.ReadUInt32();
            unDrawHeroCntByStone = br.ReadUInt32();
            unDrawHeroTimeByStone = br.ReadUInt32();
            usArmyInfantry = br.ReadUInt16();
            usArmyShield = br.ReadUInt16();
            usArmyCavalry = br.ReadUInt16();
            usArmyArcher = br.ReadUInt16();
            usArmySpell = br.ReadUInt16();
            unExp = br.ReadUInt32();
            idBttle1 = br.ReadUInt32();
            idField1 = br.ReadUInt32();
            idBttle2 = br.ReadUInt32();
            idField2 = br.ReadUInt32();
            idBttle3 = br.ReadUInt32();
            idField3 = br.ReadUInt32();
            szHeroLst = MSG.Sgt.HexStringToString(BitConverter.ToString(br.ReadBytes(br.ReadUInt16())));
            szName = MSG.Sgt.HexStringToString(BitConverter.ToString(br.ReadBytes(br.ReadUInt16())));
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_USER_ATTR_EVENT    玩家属性信息

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_USER_ATTR_INFO
    {
        public ushort    unUserAttrIndex;
        public uint      unUserAttrData;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_USER_ATTR_EVENT
    {
        public ushort wSize;
        public ushort wType;

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
        public MSG_CLIENT_USER_ATTR_INFO[] lst;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();

            usCnt = br.ReadUInt16();
            lst = new MSG_CLIENT_USER_ATTR_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].unUserAttrIndex = br.ReadUInt16();
                lst[i].unUserAttrData = br.ReadUInt32();
            }
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_QUERY_USER_INFO_AFTER_LOGIN_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_QUERY_USER_INFO_AFTER_LOGIN_REQUEST
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
}

