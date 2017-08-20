using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI;
using Packet;
using Network;
using DataMgr;


namespace DataMgr
{
    enum ACHIEVEMENT_STATUS : int
    {
        ACHIEVEMENT_STATUS_NONE = 0,
        ACHIEVEMENT_STATUS_FINISH = 1, //当前步骤的成就已完成未领取
        ACHIEVEMENT_STATUS_GET_ALL_REWARD = 2, //成就已全部领取
    };

    public class Achievenment
    {
        public uint idAchevenmentType; // 成就类型ID
        public int nStep; // 当前成就阶段
        public uint nAccumulateValue; // 成就累积值
        public int nStatus; // 成就状态

        public bool Init(uint idAchevenmentType, int nStep, uint nAccumulateValue, int nStatus)
        {
            this.idAchevenmentType = idAchevenmentType;
            this.nStep = nStep;
            this.nAccumulateValue = nAccumulateValue;
            this.nStatus = nStatus;
            return true;
        }

        public bool CanAward()
        {
            return nStatus == (int)ACHIEVEMENT_STATUS.ACHIEVEMENT_STATUS_FINISH;
        }

        public bool IsAllReward()
        {
            return nStatus == (int)ACHIEVEMENT_STATUS.ACHIEVEMENT_STATUS_GET_ALL_REWARD;
        }

        public int CompareTo(Achievenment other)
        {
            if (null == other)
            {
                return 1;
            }

            if (this == other)
            {
                return 0;
            }

            if (this.CanAward())
            {
                if (other.CanAward())
                {
                    return this.idAchevenmentType.CompareTo(other.idAchevenmentType);
                }

                return 1;
            }
            else
            {
                if (other.CanAward())
                {
                    return -1;
                }

                if (!this.IsAllReward())
                {
                    if (!other.IsAllReward())
                    {
                        return this.idAchevenmentType.CompareTo(other.idAchevenmentType);
                    }

                    return 1;
                }
                else
                {
                    if (other.IsAllReward())
                    {
                        return this.idAchevenmentType.CompareTo(other.idAchevenmentType);
                    }

                    return -1;
                }
            }
        }
    }


    public class AchievementData
    {

        Dictionary<uint, Achievenment> m_dicAchievenment;

        public AchievementData()
        {
        }

        public bool init()
        {
            // 注册消息处理函数
            NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_ACHIEVEMENT_INFO>(OnRecClientAchievenmentInfo);
            NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_ACHIEVEMENT_GET_REWARD_RESPONSE>(OnRecClientAchievenmentGetReward);
            NetworkMgr.me.getClient().RegMsgFunc<MSG_CLIENT_ACHIEVEMENT_INFO_UPDATE>(OnRecClientAchievenmentInfoUpdate);

            // 初始化数据
            m_dicAchievenment = new Dictionary<uint, Achievenment>();
            return true;
        }

        public void release()
        {
            m_dicAchievenment.Clear();
        }

        public void reload()
        {
        }

        public void ReleaseAchievement()
        {
            m_dicAchievenment.Clear();
        }

        public void SortAchievenmentToShow()
        {
            AchievenmentPanel achievenmentPannel = PanelManage.me.GetPanel<AchievenmentPanel>(PanelID.AchievenmentPanel);

            if (achievenmentPannel == null)
            {
                return;
            }

            achievenmentPannel.ReleaseItem();
            List<Achievenment> listAchievenment = new List<Achievenment>(m_dicAchievenment.Values);
            listAchievenment.Sort(delegate(Achievenment achievenmentFront, Achievenment achievenmentNext)
                {
                    if (null == achievenmentFront && null == achievenmentNext)
                    {
                        return 0;
                    }

                    if (null == achievenmentFront)
                    {
                        return -1;
                    }

                    if (null == achievenmentNext)
                    {
                        return 1;
                    }

                    return achievenmentFront.CompareTo(achievenmentNext);
                });

            for (int i = listAchievenment.Count - 1; i >= 0; --i)
            {
                Achievenment achievenment = listAchievenment[i];
                achievenmentPannel.AddItem(achievenment.idAchevenmentType);
            }
        }

        public Achievenment QueryAchievenment(uint idAchevenmentType)
        {
            Achievenment achievenment = null;
            this.m_dicAchievenment.TryGetValue(idAchevenmentType, out achievenment);
            return achievenment;
        }

        public void OnRecClientAchievenmentInfo(ushort id, object ar)
        {
            MSG.Sgt.CheckMessageId<MSG_CLIENT_ACHIEVEMENT_INFO>(id);
            MSG_CLIENT_ACHIEVEMENT_INFO msg_struct = (MSG_CLIENT_ACHIEVEMENT_INFO)ar;

            for (int i = 0; i < msg_struct.usCnt; i++)
            {
                Achievenment achievenment = new Achievenment();
                achievenment.Init(msg_struct.lst[i].idAchievementType, msg_struct.lst[i].cbStep, msg_struct.lst[i].AccumulateValue, msg_struct.lst[i].cbStatus);
                m_dicAchievenment.Add(achievenment.idAchevenmentType, achievenment);
            }

            SortAchievenmentToShow();
        }

        public void OnRecClientAchievenmentInfoUpdate(ushort id, object ar)
        {
            MSG.Sgt.CheckMessageId<MSG_CLIENT_ACHIEVEMENT_INFO_UPDATE>(id);
            MSG_CLIENT_ACHIEVEMENT_INFO_UPDATE msg_struct = (MSG_CLIENT_ACHIEVEMENT_INFO_UPDATE)ar;

            Achievenment achievenment = QueryAchievenment(msg_struct.idAchievementType);

            if (null == achievenment)
            {
                return;
            }

            achievenment.Init(msg_struct.idAchievementType, msg_struct.cbStep, msg_struct.AccumulateValue, msg_struct.cbStatus);
        }

        public void SendQueryAchievenment()
        {
            ReleaseAchievement();
            MSG_CLIENT_ACHIEVEMENT_INFO_QUERY_REQUEST msg_struct = new MSG_CLIENT_ACHIEVEMENT_INFO_QUERY_REQUEST();
            NetworkMgr.me.getClient().Send(ref msg_struct);
        }

        public void AwardAchievenment(uint idAchievenmentType)
        {
            Achievenment achievenment = QueryAchievenment(idAchievenmentType);

            if (null == achievenment)
            {
                return;
            }

            if (!achievenment.CanAward())
            {
                return;
            }

            MSG_CLIENT_ACHIEVEMENT_GET_REWARD_REQUEST msg_struct = new MSG_CLIENT_ACHIEVEMENT_GET_REWARD_REQUEST();
            msg_struct.idAchievementType = idAchievenmentType;
            NetworkMgr.me.getClient().Send(ref msg_struct);
        }

        public void OnRecClientAchievenmentGetReward(ushort id, object ar)
        {
            MSG.Sgt.CheckMessageId<MSG_CLIENT_ACHIEVEMENT_GET_REWARD_RESPONSE>(id);
            MSG_CLIENT_ACHIEVEMENT_GET_REWARD_RESPONSE msg_struct = (MSG_CLIENT_ACHIEVEMENT_GET_REWARD_RESPONSE)ar;

            if (0 != msg_struct.cbRet)
            {
                return;
            }

            SortAchievenmentToShow();
        }
    }
}
