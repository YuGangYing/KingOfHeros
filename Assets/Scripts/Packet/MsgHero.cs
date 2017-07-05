using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Packet
{

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_HERO_LST_EVENT    玩家英雄列表

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HERO_INFO
    {
        public uint      idHero;
        public uint      idType;
        public byte      u8Star;
        public ushort    usLevel;
        public uint      unExp;
        public byte      u8Status;
        public byte      u8BattlePos;
        public uint      unSkill1;
        public ushort    usSkillLvl1;
        public uint      unSkillExp1;
        public uint      unSkill2;
        public ushort    usSkillLvl2;
        public uint      unSkillExp2;
        public uint      unSkill3;
        public ushort    usSkillLvl3;
        public uint      unSkillExp3;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_HERO_LST_EVENT
    {
        public ushort wSize;
        public ushort wType;

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public HERO_INFO[] lst;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();

            usCnt = br.ReadUInt16();
            lst = new HERO_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idHero = br.ReadUInt32();
                lst[i].idType = br.ReadUInt32();
                lst[i].u8Star = br.ReadByte();
                lst[i].usLevel = br.ReadUInt16();
                lst[i].unExp = br.ReadUInt32();
                lst[i].u8Status = br.ReadByte();
                lst[i].u8BattlePos = br.ReadByte();
                lst[i].unSkill1 = br.ReadUInt32();
                lst[i].usSkillLvl1 = br.ReadUInt16();
                lst[i].unSkillExp1 = br.ReadUInt32();
                lst[i].unSkill2 = br.ReadUInt32();
                lst[i].usSkillLvl2 = br.ReadUInt16();
                lst[i].unSkillExp2 = br.ReadUInt32();
                lst[i].unSkill3 = br.ReadUInt32();
                lst[i].usSkillLvl3 = br.ReadUInt16();
                lst[i].unSkillExp3 = br.ReadUInt32();
            }
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_ACQUIRE_HERO_REQUEST    获取英雄

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_ACQUIRE_HERO_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public byte   u8Method;

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(u8Method);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_ACQUIRE_HERO_RESPONSE    获取英雄

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_ACQUIRE_HERO_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public byte   u8Method;                      // 获得英雄方式	HERO_ACQUIRE
        public uint   unErr;                         // 操作结果
        public uint   idHeroType;                    // 获得的英雄类型id  未获得该值为0
        public uint   idHero;                        // 英雄实体id

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            u8Method = br.ReadByte();
            unErr = br.ReadUInt32();
            idHeroType = br.ReadUInt32();
            idHero = br.ReadUInt32();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_IMPROVE_STAR_REQUEST    英雄升星

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VICE_HEROS
    {
        public uint      idViceHero;            // 副卡英雄id
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_IMPROVE_STAR_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idHero;                        // 主卡英雄id

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public VICE_HEROS[] lst;

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idHero);

            usCnt = (ushort)lst.Length;
            bw.Write(usCnt);
            for (int i = 0; i < usCnt; ++i)
            {
                bw.Write(lst[i].idViceHero);
            }

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_IMPROVE_STAR_RESPONSE    英雄升星

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_IMPROVE_STAR_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public uint   idHero;                        // 主卡英雄
        public uint   unErr;                         // 操作结果

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public VICE_HEROS[] lst;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idHero = br.ReadUInt32();
            unErr = br.ReadUInt32();

            usCnt = br.ReadUInt16();
            lst = new VICE_HEROS[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idViceHero = br.ReadUInt32();
            }
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_HERO_LVLUP_EVENT    英雄升级事件

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_HERO_LVLUP_EVENT
    {
        public ushort wSize;
        public ushort wType;
        public uint   idHero;
        public ushort usLvl;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idHero = br.ReadUInt32();
            usLvl = br.ReadUInt16();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_HERO_ATTR_EVENT    英雄属性更新事件

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HERO_ATTR_INFO
    {
        public ushort    attr;            // 英雄属性索引
        public uint      unVal;            // 英雄属性值
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_HERO_ATTR_EVENT
    {
        public ushort wSize;
        public ushort wType;
        public uint   idHero;

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
        public HERO_ATTR_INFO[] lst;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idHero = br.ReadUInt32();

            usCnt = br.ReadUInt16();
            lst = new HERO_ATTR_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].attr = br.ReadUInt16();
                lst[i].unVal = br.ReadUInt32();
            }
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_HERO_FIGHT_REQUEST    英雄出战请求

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HERO_FIGHT
    {
        public uint      idHero;
        public byte      u8Pos;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_HERO_FIGHT_REQUEST
    {
        public ushort wSize;
        public ushort wType;

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public HERO_FIGHT[] lst;

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
                bw.Write(lst[i].idHero);
                bw.Write(lst[i].u8Pos);
            }

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_HERO_FIGHT_RESPONSE    英雄出战请求

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_HERO_FIGHT_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public uint   unErr;

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public HERO_FIGHT[] lst;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            unErr = br.ReadUInt32();

            usCnt = br.ReadUInt16();
            lst = new HERO_FIGHT[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idHero = br.ReadUInt32();
                lst[i].u8Pos = br.ReadByte();
            }
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_HERO_SET_BATTLE_POS_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HERO_SET_BATTLE_POS_INFO
    {
        public uint      idHero;
        public byte      unBattlePos;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_HERO_SET_BATTLE_POS_REQUEST
    {
        public ushort wSize;
        public ushort wType;

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
        public HERO_SET_BATTLE_POS_INFO[] lst;

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
                bw.Write(lst[i].idHero);
                bw.Write(lst[i].unBattlePos);
            }

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_HERO_SET_BATTLE_POS_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_HERO_SET_BATTLE_POS_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public uint   unErr;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            unErr = br.ReadUInt32();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_WAITER_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_WAITER_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idMajorHero;                   // 主卡英雄id
        public uint   idAtkWaiter;                   // 攻击侍从id
        public uint   idDefenceWaiter;               // 防御侍从id
        public uint   idHpWaiter;                    // 血量侍从id
        public uint   idLeaderWaiter;                // 领导力侍从id

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idMajorHero);
            bw.Write(idAtkWaiter);
            bw.Write(idDefenceWaiter);
            bw.Write(idHpWaiter);
            bw.Write(idLeaderWaiter);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_WAITER_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_WAITER_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public uint   unErr;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            unErr = br.ReadUInt32();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_HERO_CHANGE_EVENT    英雄增减事件

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HERO_CHANGE
    {
        public uint      idHero;
        public uint      idHeroType;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_HERO_CHANGE_EVENT
    {
        public ushort wSize;
        public ushort wType;
        public byte   tag;                           // 0:新增	1:删除

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public HERO_CHANGE[] lst;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            tag = br.ReadByte();

            usCnt = br.ReadUInt16();
            lst = new HERO_CHANGE[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idHero = br.ReadUInt32();
                lst[i].idHeroType = br.ReadUInt32();
            }
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_HERO_TRAIN_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_HERO_TRAIN_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idHero;                        // 英雄ID
        public uint   nTrainType;                    // 英雄培养类型

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idHero);
            bw.Write(nTrainType);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_HERO_TRAIN_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_HERO_TRAIN_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public uint   idHero;                        // 英雄ID
        public uint   unError;                       // 返回结果(0成功；具体数值代表失败类型)

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idHero = br.ReadUInt32();
            unError = br.ReadUInt32();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_HERO_TRAIN_CLEAR_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_HERO_TRAIN_CLEAR_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idHero;                        // 英雄ID
        public uint   nTrainType;                    // 英雄培养类型

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idHero);
            bw.Write(nTrainType);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_HERO_TRAIN_CLEAR_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_HERO_TRAIN_CLEAR_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public uint   idHero;                        // 英雄ID
        public uint   unError;                       // 返回结果(0成功；具体数值代表失败类型)

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idHero = br.ReadUInt32();
            unError = br.ReadUInt32();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_HERO_PANTHEON_EXP_SHARE_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_HERO_PANTHEON_EXP_SHARE_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idHero;                        // 英雄ID
        public byte   cbFlag;                        // 分配标志(1分配所有; 2提升一级)

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idHero);
            bw.Write(cbFlag);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_HERO_PANTHEON_EXP_SHARE_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_HERO_PANTHEON_EXP_SHARE_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public uint   idHero;                        // 英雄ID
        public uint   unErr;                         // 返回结果(0成功；具体数值代表失败类型)

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idHero = br.ReadUInt32();
            unErr = br.ReadUInt32();
            return this;
        }
    }  // end struct
}

