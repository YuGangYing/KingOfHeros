using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Packet
{

//////////////////////////////////////////////////////////////////////////
// MSG_ACQUIRE_SKILL_EXP_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_ACQUIRE_SKILL_EXP_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idMajorHero;                   // 主卡英雄id
        public uint   idSkill;                       // 技能id
        public uint   idViceHero;                    // 副卡英雄id

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idMajorHero);
            bw.Write(idSkill);
            bw.Write(idViceHero);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_ACQUIRE_SKILL_EXP_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_ACQUIRE_SKILL_EXP_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public byte   u8Err;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            u8Err = br.ReadByte();
            return this;
        }
    }  // end struct
}

