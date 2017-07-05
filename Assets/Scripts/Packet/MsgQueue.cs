using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Packet
{

//////////////////////////////////////////////////////////////////////////
// MSG_QUEUE_LIST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct QUEUE_INFO
    {
        public uint      idProgress;
        public uint      u32DueTime;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_QUEUE_LIST
    {
        public ushort wSize;
        public ushort wType;
        public byte   cbProgressType;

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
        public QUEUE_INFO[] lst;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            cbProgressType = br.ReadByte();

            usCnt = br.ReadUInt16();
            lst = new QUEUE_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idProgress = br.ReadUInt32();
                lst[i].u32DueTime = br.ReadUInt32();
            }
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_QUEUE_UPDATE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_QUEUE_UPDATE
    {
        public ushort wSize;
        public ushort wType;
        public byte   cbProgressType;
        public uint   idProgress;
        public uint   u32DueTime;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            cbProgressType = br.ReadByte();
            idProgress = br.ReadUInt32();
            u32DueTime = br.ReadUInt32();
            return this;
        }
    }  // end struct
}

