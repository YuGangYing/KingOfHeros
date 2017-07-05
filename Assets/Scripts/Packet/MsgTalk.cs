using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Packet
{

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_TALK    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_TALK
    {
        public ushort wSize;
        public ushort wType;
        public const int    MAX_WORDSSIZE      = 256;  // 聊天最大字符数
        public const int    MAX_NAMESIZE      = 16;  // 聊天最大字符数

        public ushort unTxtAttribute;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=MAX_NAMESIZE)]
        public string szSender;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=MAX_NAMESIZE)]
        public string szReceiver;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=MAX_WORDSSIZE)]
        public string szWords;

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(unTxtAttribute);
            bw.Write((ushort)System.Text.Encoding.UTF8.GetBytes(szSender).Length);
            bw.Write(System.Text.Encoding.UTF8.GetBytes(szSender));
            bw.Write((ushort)System.Text.Encoding.UTF8.GetBytes(szReceiver).Length);
            bw.Write(System.Text.Encoding.UTF8.GetBytes(szReceiver));
            bw.Write((ushort)System.Text.Encoding.UTF8.GetBytes(szWords).Length);
            bw.Write(System.Text.Encoding.UTF8.GetBytes(szWords));

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            unTxtAttribute = br.ReadUInt16();
            szSender = MSG.Sgt.HexStringToString(BitConverter.ToString(br.ReadBytes(br.ReadUInt16())));
            szReceiver = MSG.Sgt.HexStringToString(BitConverter.ToString(br.ReadBytes(br.ReadUInt16())));
            szWords = MSG.Sgt.HexStringToString(BitConverter.ToString(br.ReadBytes(br.ReadUInt16())));
            return this;
        }
    }  // end struct
}

