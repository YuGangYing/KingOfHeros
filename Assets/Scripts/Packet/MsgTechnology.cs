using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Packet
{

//////////////////////////////////////////////////////////////////////////
// MSG_TECHNOLOGY_LEARND_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_TECHNOLOGY_LEARND_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idTechnologyType;

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idTechnologyType);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_TECHNOLOGY_LEARND_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_TECHNOLOGY_LEARND_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public uint   idTechnologyType;
        public uint   idTechnology;
        public uint   unRet;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idTechnologyType = br.ReadUInt32();
            idTechnology = br.ReadUInt32();
            unRet = br.ReadUInt32();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_TECHNOLOGY_UPLEV_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_TECHNOLOGY_UPLEV_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idTechnology;

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idTechnology);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_TECHNOLOGY_UPLEV_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_TECHNOLOGY_UPLEV_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public uint   idTechnology;
        public uint   unRet;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idTechnology = br.ReadUInt32();
            unRet = br.ReadUInt32();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_TECHNOLOGY_ACCELERATE_UPLEV_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_TECHNOLOGY_ACCELERATE_UPLEV_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idTechnology;
        public uint   u32Secs;

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idTechnology);
            bw.Write(u32Secs);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_TECHNOLOGY_ACCELERATE_UPLEV_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_TECHNOLOGY_ACCELERATE_UPLEV_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public uint   idTechnology;
        public uint   unRet;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idTechnology = br.ReadUInt32();
            unRet = br.ReadUInt32();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_TECHNOLOGY_LIST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TECHNOLOGY_INFO
    {
        public uint      idTechnology;
        public uint      idTechnologyType;
        public byte      cbLev;
        public byte      cbState;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_TECHNOLOGY_LIST
    {
        public ushort wSize;
        public ushort wType;

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
        public TECHNOLOGY_INFO[] lst;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();

            usCnt = br.ReadUInt16();
            lst = new TECHNOLOGY_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idTechnology = br.ReadUInt32();
                lst[i].idTechnologyType = br.ReadUInt32();
                lst[i].cbLev = br.ReadByte();
                lst[i].cbState = br.ReadByte();
            }
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_TECHNOLOGY_UPDATE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_TECHNOLOGY_UPDATE
    {
        public ushort wSize;
        public ushort wType;
        public uint   idTechnology;
        public uint   idTechnologyType;
        public byte   cbLev;
        public byte   cbState;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idTechnology = br.ReadUInt32();
            idTechnologyType = br.ReadUInt32();
            cbLev = br.ReadByte();
            cbState = br.ReadByte();
            return this;
        }
    }  // end struct
}

