using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Network;
using UnityEngine;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
#pragma warning disable 0472
#pragma warning disable 0114
#pragma warning disable 3021
namespace Packet
{
    public class MSG
    {

        public struct _msg
        {
            [XmlAttribute]
            public UInt16 _code;
            [XmlAttribute]
            public String _type;
        }

        private Dictionary<String, UInt16> CodeDict = new Dictionary<String, UInt16>();
        private HashSet<UInt16> CodeSet = new HashSet<UInt16>();
        private static MSG instance = null;

        public static MSG Sgt
        {
            get
            {
                if (instance == null)
                {
                    instance = new MSG();
                }
                return instance;
            }
        }

        private MSG()
        {
            TextAsset asset = (TextAsset)Resources.Load("Xml/Message", typeof(TextAsset));
            XmlSerializer serializer = new XmlSerializer(typeof(_msg[]));
            MemoryStream stream = new MemoryStream(asset.bytes);
            _msg[] s = (_msg[])serializer.Deserialize(stream);

            foreach (_msg v in s)
            {
                if (Type.GetType(v._type) == null)
                {
					Debug.LogError(string.Format("FATAL ERROR * unable to find designated type [{0}].", v._type));
                }

                try
                {

                    CodeDict.Add(v._type, v._code);
                    CodeSet.Add(v._code);
                }
                catch (ArgumentException e)
                {
					Debug.LogError(string.Format("FATAL ERROR * duplicate element {0} found in [Message.xml].", v._type));
                }
            }
        }

        public void Initialize()
        {
            foreach (var v in CodeDict)
            {
                NetworkMgr.me.getClient().RegProxy(v.Key);
            }
        }

        public UInt16 GetTypeCode(String val)
        {
            UInt16 codenum = 0;
            if (CodeDict.TryGetValue(val, out codenum))
                return codenum;
            return 0;
        }

        public UInt16 GetTypeCode(Type val)
        {
            UInt16 codenum = 0;
            if (CodeDict.TryGetValue(val.FullName, out codenum))
                return codenum;
            return 0;
        }

        public bool CheckMessageId<T>(UInt16 val) where T : new()
        {
            if (GetTypeCode(typeof(T)) == val)
            {

            }
            else
            {
                 Debug.LogError("Message : Type & Code !!!MISMATCH!!!");

                return false;
            }

            return true;
        }

        public void CheckMessageCode(UInt16 val)
        {
            if (CodeSet.Contains(val))
            {

            }
            else 
            {
                 Debug.LogError("Message : Code !!!UNKNOWN!!!");
            }
        }

        //public static uint PACK = 0;
        //public static uint UNPACK = 1;
        //public static int MAX_LEN = 1024;

        public byte[] Truncate(byte[] msg)
        {
            if (msg.Length < 2)
                return msg;

            ushort len = BitConverter.ToUInt16(msg, 0);
            byte[] message = new byte[len];

            Array.Copy(msg, message, len);
            return message;
        }

        public string HexStringToString(string hexStr)
        {
            if (0 == hexStr.Length)
                return "";

            string[] strSplit = hexStr.Split('-');
            byte[] bt = new byte[strSplit.Length];
            for (int i = 0; i < bt.Length; i++)
                bt[i] = byte.Parse(strSplit[i], System.Globalization.NumberStyles.AllowHexSpecifier);

            return System.Text.Encoding.UTF8.GetString(bt);
        }
    }
}
