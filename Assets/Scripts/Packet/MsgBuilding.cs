using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Packet
{

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_BUILD_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_BUILDING_BUILD_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idBuildingType;
        public float  fPosX;
        public float  fPosY;

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idBuildingType);
            bw.Write(fPosX);
            bw.Write(fPosY);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_BUILD_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_BUILDING_BUILD_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public uint   idBuildingType;
        public uint   wErrCode;
        public uint   idBuilding;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idBuildingType = br.ReadUInt32();
            wErrCode = br.ReadUInt32();
            idBuilding = br.ReadUInt32();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_UPLEV_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_BUILDING_UPLEV_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idBuilding;

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idBuilding);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_UPLEV_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_BUILDING_UPLEV_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public uint   idBuilding;
        public uint   wErrCode;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idBuilding = br.ReadUInt32();
            wErrCode = br.ReadUInt32();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_ACCELERATE_UPLEV_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_BUILDING_ACCELERATE_UPLEV_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idBuilding;
        public uint   u32Secs;

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idBuilding);
            bw.Write(u32Secs);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_ACCELERATE_UPLEV_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_BUILDING_ACCELERATE_UPLEV_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public uint   idBuilding;
        public uint   wErrCode;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idBuilding = br.ReadUInt32();
            wErrCode = br.ReadUInt32();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_MOVE_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_BUILDING_MOVE_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idBuilding;
        public float  fPosX;
        public float  fPosY;

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idBuilding);
            bw.Write(fPosX);
            bw.Write(fPosY);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_MOVE_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_BUILDING_MOVE_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public uint   idBuilding;
        public uint   wErrCode;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idBuilding = br.ReadUInt32();
            wErrCode = br.ReadUInt32();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_LIST_EVENT    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BUILDING_INFO
    {
        public uint      idBuilding;
        public uint      idBuildingType;
        public uint      u32LevyTime;
        public byte      cbLev;
        public byte      cbAreaWidth;
        public byte      cbAreaHigh;
        public byte      cbState;
        public float     fPosX;
        public float     fPosY;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_BUILDING_LIST_EVENT
    {
        public ushort wSize;
        public ushort wType;

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 45)]
        public BUILDING_INFO[] lst;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();

            usCnt = br.ReadUInt16();
            lst = new BUILDING_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idBuilding = br.ReadUInt32();
                lst[i].idBuildingType = br.ReadUInt32();
                lst[i].u32LevyTime = br.ReadUInt32();
                lst[i].cbLev = br.ReadByte();
                lst[i].cbAreaWidth = br.ReadByte();
                lst[i].cbAreaHigh = br.ReadByte();
                lst[i].cbState = br.ReadByte();
                lst[i].fPosX = br.ReadSingle();
                lst[i].fPosY = br.ReadSingle();
            }
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_OTHER_LIST_EVENT    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BUILDING_INFO_OTHER
    {
        public uint      idUser;
        public uint      idBuilding;
        public uint      idBuildingType;
        public uint      u32LevyTime;
        public byte      cbLev;
        public byte      cbAreaWidth;
        public byte      cbAreaHigh;
        public byte      cbState;
        public float     fPosX;
        public float     fPosY;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_BUILDING_OTHER_LIST_EVENT
    {
        public ushort wSize;
        public ushort wType;

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 45)]
        public BUILDING_INFO_OTHER[] lst;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();

            usCnt = br.ReadUInt16();
            lst = new BUILDING_INFO_OTHER[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idUser = br.ReadUInt32();
                lst[i].idBuilding = br.ReadUInt32();
                lst[i].idBuildingType = br.ReadUInt32();
                lst[i].u32LevyTime = br.ReadUInt32();
                lst[i].cbLev = br.ReadByte();
                lst[i].cbAreaWidth = br.ReadByte();
                lst[i].cbAreaHigh = br.ReadByte();
                lst[i].cbState = br.ReadByte();
                lst[i].fPosX = br.ReadSingle();
                lst[i].fPosY = br.ReadSingle();
            }
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_UPDATE_EVENT    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_BUILDING_UPDATE_EVENT
    {
        public ushort wSize;
        public ushort wType;
        public uint   idBuilding;
        public uint   idBuildingType;
        public byte   cbLev;
        public byte   cbState;
        public uint   u32LevyTime;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idBuilding = br.ReadUInt32();
            idBuildingType = br.ReadUInt32();
            cbLev = br.ReadByte();
            cbState = br.ReadByte();
            u32LevyTime = br.ReadUInt32();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_UPLEV_CANCEL_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_BUILDING_UPLEV_CANCEL_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idBuilding;

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idBuilding);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_UPLEV_CANCEL_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_BUILDING_UPLEV_CANCEL_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public uint   idBuilding;
        public uint   wErrCode;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idBuilding = br.ReadUInt32();
            wErrCode = br.ReadUInt32();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_LEVY_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_BUILDING_LEVY_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idBuilding;

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idBuilding);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_LEVY_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_BUILDING_LEVY_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public uint   idBuilding;
        public uint   wErrCode;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idBuilding = br.ReadUInt32();
            wErrCode = br.ReadUInt32();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_BUILDING_OTHER_ENTER    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_BUILDING_OTHER_ENTER
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
}

