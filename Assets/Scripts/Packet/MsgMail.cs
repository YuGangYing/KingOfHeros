using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Packet
{

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_MAIL_INFO    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CLIENT_USER_MAIL_INFO
    {
        public uint      idMail;            // 邮件ID
        public uint      idSender;            // 发送玩家ID
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=16)]
        public string szSenderName;            // 发送玩家名字
        public byte      nMailType;            // 邮件类型
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=32)]
        public string szMailTitle;            // 邮件主题
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=128)]
        public string szMailContent;            // 邮件内容
        public uint      nMoney;            // 金币
        public uint      nStone;            // 魔石
        public uint      nDiamond;            // 钻石
        public uint      nCreateTime;            // 创建时间
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_MAIL_INFO
    {
        public ushort wSize;
        public ushort wType;

        public ushort usCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 45)]
        public CLIENT_USER_MAIL_INFO[] lst;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();

            usCnt = br.ReadUInt16();
            lst = new CLIENT_USER_MAIL_INFO[usCnt];
            for (int i = 0; i < usCnt; ++i)
            {
                lst[i].idMail = br.ReadUInt32();
                lst[i].idSender = br.ReadUInt32();
                lst[i].szSenderName = MSG.Sgt.HexStringToString(BitConverter.ToString(br.ReadBytes(br.ReadUInt16())));
                lst[i].nMailType = br.ReadByte();
                lst[i].szMailTitle = MSG.Sgt.HexStringToString(BitConverter.ToString(br.ReadBytes(br.ReadUInt16())));
                lst[i].szMailContent = MSG.Sgt.HexStringToString(BitConverter.ToString(br.ReadBytes(br.ReadUInt16())));
                lst[i].nMoney = br.ReadUInt32();
                lst[i].nStone = br.ReadUInt32();
                lst[i].nDiamond = br.ReadUInt32();
                lst[i].nCreateTime = br.ReadUInt32();
            }
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_MAIL_INFO_DEL    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_MAIL_INFO_DEL
    {
        public ushort wSize;
        public ushort wType;
        public uint   idMail;                        // 邮件ID

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            idMail = br.ReadUInt32();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_SENDMAIL_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_SENDMAIL_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idAccepter;                    // 接收玩家ID
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=32)]
        public string szMailTitle;// 邮件主题
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=128)]
        public string szMailContent;// 邮件内容

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idAccepter);
            bw.Write((ushort)System.Text.Encoding.UTF8.GetBytes(szMailTitle).Length);
            bw.Write(System.Text.Encoding.UTF8.GetBytes(szMailTitle));
            bw.Write((ushort)System.Text.Encoding.UTF8.GetBytes(szMailContent).Length);
            bw.Write(System.Text.Encoding.UTF8.GetBytes(szMailContent));

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_SENDMAIL_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_SENDMAIL_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public byte   nRet;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            nRet = br.ReadByte();
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_DELMAIL_REQUEST    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_DELMAIL_REQUEST
    {
        public ushort wSize;
        public ushort wType;
        public uint   idMail;                        // 邮件ID

        public object pack(ref byte[] bt)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            wType = MSG.Sgt.GetTypeCode(this.GetType().FullName);

            bw.Write(wSize);
            bw.Write(wType);
            bw.Write(idMail);

            wSize = (ushort)ms.Length;
            ms.Position = 0;
            bw.Write(wSize);
            bt = MSG.Sgt.Truncate(ms.GetBuffer());
            return this;
        }
    }  // end struct

//////////////////////////////////////////////////////////////////////////
// MSG_CLIENT_DELMAIL_RESPONSE    

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_CLIENT_DELMAIL_RESPONSE
    {
        public ushort wSize;
        public ushort wType;
        public byte   nRet;

        public object unpack(ref byte[] msg)
        {
            msg = MSG.Sgt.Truncate(msg);
            MemoryStream ms = new MemoryStream(msg);
            BinaryReader br = new BinaryReader(ms);
            wSize = br.ReadUInt16();
            wType = br.ReadUInt16();
            nRet = br.ReadByte();
            return this;
        }
    }  // end struct
}

