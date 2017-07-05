using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Packet
{

//////////////////////////////////////////////////////////////////////////
// MSG_DAY_CHANGE_EVENT    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_DAY_CHANGE_EVENT
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

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_DAY_LOGIN_AWARD    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_DAY_LOGIN_AWARD
    {
        public ushort wSize;
        public ushort wType;
        public uint   unDayLogined;                  // 一星期内登陆的星期天数，1-星期一，2-星期二，4-星期三，8-星期四，16-星期五，32-星期六，64-星期天
        public uint   unToday;                       // 当天星期天数，0-星期一，1-星期二，2-星期三，3-星期四，4-星期五，5-星期六，6-星期天

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            unDayLogined = br.ReadUInt32();
            unToday = br.ReadUInt32();
            return this;
        }
    }  // end struct
}

