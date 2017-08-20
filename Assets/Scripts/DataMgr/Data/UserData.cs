using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Packet;
using Network;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
#pragma warning disable 0472
namespace DataMgr
{
	public class UserData
	{
        MSG_CLIENT_USER_INFO_EVENT mstData = new MSG_CLIENT_USER_INFO_EVENT();
        public MSG_CLIENT_USER_INFO_EVENT Data { get { return mstData; } }

        // 拥有所有图鉴类型Id
        List<uint> mlistHeroPhoto = new List<uint>();
        public int Amount { get { return mlistHeroPhoto.Count; } }
        public uint this[int index]
        {
            get
            {
                if (index < 0 || index >= mlistHeroPhoto.Count)
                    return 0;
                return mlistHeroPhoto[index];
            }
        }

        int mBitCount = 0x20;

        public bool isDone
        {
            get;
            set;
        }

        public UserData()
        {
        }

        public bool init()
        {
            RegisterMsg();
            this.isDone = false;
            return true;
        }

        public void release()
        {
            this.mlistHeroPhoto.Clear();
        }

        void RegisterMsg()
        {
            NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_USER_INFO_EVENT>(this._OnMsgInfo);
            NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_USER_ATTR_EVENT>(this._OnMsgAttr);
            NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_KICKOUT_EVENT>(this._OnKickOut);
        }

        private void _OnKickOut(ushort wMsgId, object ar)
        {
            MessageBoxMgr.ShowMessageBox("Error", "You have been Kicked Out from server!!", _OnKickOutConfirm, null);
        }

        bool _OnKickOutConfirm(SLG.EventArgs obj)
        {
            MainController.me.logout();
            return true;
        }

        public void onRecv(ushort wMsgId, MSG_CLIENT_USER_INFO_EVENT obj)
        {
            this._OnMsgInfo(wMsgId,obj);
        }

        private void _OnMsgInfo(ushort wMsgId, object ar)
        {
            bool bRet = MSG.Sgt.CheckMessageId<MSG_CLIENT_USER_INFO_EVENT>(wMsgId);
            if (!bRet)
                return;

            MSG_CLIENT_USER_INFO_EVENT e = (MSG_CLIENT_USER_INFO_EVENT)ar;
            mstData = e;

            string bin = HexStr2BinStr(mstData.szHeroLst);
            bool isBit = false;

            for (int i = 0; i < bin.Length; ++i)
            {
                isBit = IsBitSet(bin, i);

                if (isBit)
                {
                    _AddPhoto(i);
                }
            }

            _FireEventRefresh();
            this.isDone = true;
        }

        private void _OnMsgAttr(ushort wMsgId, object ar)
        {
            bool bRet = MSG.Sgt.CheckMessageId<MSG_CLIENT_USER_ATTR_EVENT>(wMsgId);
            if (!bRet)
                return;
            uint dwItemValue = 0;
            bool bFire = true, bHeroPhotoItem = false;
            MSG_CLIENT_USER_ATTR_EVENT e = (MSG_CLIENT_USER_ATTR_EVENT)ar;

            for (int i = 0; i < Math.Min(e.usCnt, e.lst.Length); i++)
            {
                USER_ATTR n = (Packet.USER_ATTR)e.lst[i].unUserAttrIndex;
                switch (n)
                {
                    case USER_ATTR.USER_ATTR_VIP:
                        mstData.vip = (byte)e.lst[i].unUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_COIN:
                        mstData.coin = e.lst[i].unUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_STONE: mstData.stone = e.lst[i].unUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_RMB:
                        mstData.rmb = e.lst[i].unUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_RESERVE_SOLDIER:
                        mstData.unReserveSoldier = e.lst[i].unUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_RECRUIT_RESERVE_SOLDIER:
                        mstData.unRecruitSoldierNum = e.lst[i].unUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_RECRUIT_FINISH_TIME:
                        mstData.unRecruitSoldierFinshTime = e.lst[i].unUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_DRAW_HERO_CNT:
                        mstData.u8DrawCnt = (byte)e.lst[i].unUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_DRAW_HERO_TIME:
                        mstData.unDrawTime = (byte)e.lst[i].unUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_DRAW_HERO_CNT_STONE:
                        mstData.unDrawHeroCntByStone = (byte)e.lst[i].unUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_DRAW_HERO_TIME_STONE:
                        mstData.unDrawHeroTimeByStone = e.lst[i].unUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_ARMY_INFANTRY:
                        mstData.usArmyInfantry = (ushort)e.lst[i].unUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_ARMY_SHIELD:
                        mstData.usArmyShield = (ushort)e.lst[i].unUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_ARMY_CAVALRY:
                        mstData.usArmyCavalry = (ushort)e.lst[i].unUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_ARMY_ARCHER:
                        mstData.usArmyArcher = (ushort)e.lst[i].unUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_ARMY_SPELL:
                        mstData.usArmySpell = (ushort)e.lst[i].unUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_EXP:
                        mstData.unExp = e.lst[i].unUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_IDBATTLE1:
                        mstData.idBttle1 = e.lst[i].unUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_IDFIELD1:
                        mstData.idField1 = e.lst[i].unUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_IDBATTLE2:
                        mstData.idBttle2 = e.lst[i].unUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_IDFIELD2:
                        mstData.idField2 = e.lst[i].unUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_IDBATTLE3:
                        mstData.idBttle3 = e.lst[i].unUserAttrData;
                        break;
                    case USER_ATTR.USER_ATTR_IDFIELD3:
                        mstData.idField3 = e.lst[i].unUserAttrData;
                        break;
                }
            }

            if (bFire)
                _FireEventRefresh();

            if (bHeroPhotoItem)
            {
                _FindItem(dwItemValue);
            }
        }

        void _FireEventRefresh()
        {
            SLG.GlobalEventSet.FireEvent(SLG.eEventType.RefreshMainUI, UI.PanelID.MainPanel, new SLG.EventArgs());

            SLG.GlobalEventSet.FireEvent(SLG.eEventType.FreshRewardMoenyUI, null);

            SLG.GlobalEventSet.FireEvent(SLG.eEventType.NodifyRefreshMoney, null);
        }

        public string HexStr2BinStr(string str)
        {
            if (str.IndexOf("0x") == -1)
                return "";

            if (str.Length % 2 != 0)
                return "";

            string binaryStr = "";
            for (int i = 2; i < str.Length; ++i)
            {
                string hex = str[i].ToString() + str[++i].ToString();
                int num = Int32.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                string bin = Convert.ToString(num, 2);
                binaryStr = bin.PadLeft(8, '0') + binaryStr;
            }

            return binaryStr;
        }

        public bool IsBitSet(string bin, int idx)
        {
            idx = bin.Length - 1 - idx;
            if (idx > bin.Length - 1 || idx < 0)
                return false;

            return bin[idx] == '1';
        }

        public void SetBit(ref string bin, int idx)
        {
            idx = bin.Length - 1 - idx;
            if (idx > bin.Length - 1 || idx < 0)
                return;

            bin = bin.Remove(idx, 1);
            bin = bin.Insert(idx, "1");
        }

        void _AddPhoto(int nIndex)
        {
            int nRealId = nIndex;// +1;//(int)nTemp;

            ConfigBase cb = DataManager.getConfig(CONFIG_MODULE.CFG_CVS_HERO_BASE);

            int nHeroTypeId = CConstance.DEFAULT_ID;
            bool bRet = CHerroTalbeAttribute.getHeroDetailByIllustratedId(nRealId, ref nHeroTypeId);
            if (bRet)
            {
                if (CConstance.INVALID_ID == nHeroTypeId)
                    Logger.LogDebug("UserData::_FindItem ---- find error  typid:" + nHeroTypeId.ToString());
                else
                {
                    bool bFind = false;
                    for (int i = 0; i < mlistHeroPhoto.Count; i++)
                    {
                        uint dwTempId = mlistHeroPhoto[i];
                        if (dwTempId == (uint)nHeroTypeId)
                        {
                            break;
                        }
                    }

                    if (!bFind)
                    {
                        mlistHeroPhoto.Add((uint)nHeroTypeId);
                    }
                }
            }
        }

        void _FindItem(uint bitId)
        {
            int nOffsetBit = 0;
            Logger.LogDebug("UserData::_FindItem  value" + bitId.ToString());
            do
            {
                if (bitId == 0)
                    break;

                if (nOffsetBit >= mBitCount)
                    break;

                long nTemp = 0x01 << nOffsetBit;
                long nIndex = bitId & nTemp;
                int nTempId = (int)nIndex;
                if (nTempId != 0)
                {
                    _AddPhoto(nOffsetBit);
                }

                nOffsetBit++;
            } while (true);
        }


        #region ------ hero Hero photo static function ------

        public static int HeroPhotoAmount
        {
            get
            {
                DataMgr.UserData ud = DataManager.getUserData();
                if (ud == null)
                    return 0;

                return ud.Amount;
            }
        }

        public static uint getHeroPhotoTypeId(int nIndex)
        {
            DataMgr.UserData ud = DataManager.getUserData();
            if (ud == null)
                return 0;

            if (nIndex < 0 || nIndex >= ud.Amount)
                return 0;

            return ud[nIndex];
        }

        static public bool getHeroPhotoData(int nIndex, out uint typeId)
        {
            typeId = CConstance.DEFAULT_ID;
            DataMgr.UserData ud = DataManager.getUserData();
            if (ud == null)
                return false;

            for (int i = 0; i < ud.Amount; i++)
            {
                uint dwTypeId = ud[i];
                if (nIndex == i)
                {
                    typeId = dwTypeId;
                    return true;
                }
            }

            return false;
        }

        static public bool getHeroPhotoData(uint typeId)
        {
            DataMgr.UserData ud = DataManager.getUserData();
            if (ud == null)
                return false;

            for (int i = 0; i < ud.Amount; i++)
            {
                uint dwTypeId = ud[i];
                if (typeId == dwTypeId)
                {
                    return true;
                }
            }
           
            return false;
        }

        #endregion
    }
}