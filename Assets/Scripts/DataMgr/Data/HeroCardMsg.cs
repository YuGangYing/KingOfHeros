using Packet;
using Network;

#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108

namespace DataMgr
{

public class HeroCardMsg
{
    public HeroCardMsg()
	{
	}

    public bool init()
    {
        _RegMsg();
        return true;
    }

    public void release()
    {
        _UnRegMsg();
    }

    void _RegMsg()
    {
        Logger.LogDebug("MsgCardRecruit::regMsg");

        NetworkMgr.me.getClient().RegMsgFunc<MSG_ACQUIRE_HERO_RESPONSE>(responseEveryDayCard);
        NetworkMgr.me.getClient().RegMsgFunc<MSG_IMPROVE_STAR_RESPONSE>(responseCardQuality);

        NetworkMgr.me.getClient().RegMsgFunc<MSG_DAY_CHANGE_EVENT>(responseChangeFreeCount);

        NetworkMgr.me.getClient().RegMsgFunc<MSG_WAITER_RESPONSE>(responseWaiter);
    }

    void _UnRegMsg()
    {

    }

    public void questEveryDayCard(HERO_ACQUIRE en)
    {
        MSG_ACQUIRE_HERO_REQUEST msg = new MSG_ACQUIRE_HERO_REQUEST();//HERO_ACQUIRE.HERO_ACQUIRE_FREE);
        msg.u8Method = (byte)en;

        Network.NetworkMgr.me.getClient().Send<MSG_ACQUIRE_HERO_REQUEST>(ref msg);
    }

    public void responseEveryDayCard(ushort id, object ar)
    {
        MSG_ACQUIRE_HERO_RESPONSE rsponse = (MSG_ACQUIRE_HERO_RESPONSE)ar;
        if (!DataManager.checkErrcode(rsponse.unErr))
            return;

        SLG.EventArgs obj = new SLG.EventArgs((object)rsponse);
        //SLG.GlobalEventSet.FireEvent(SLG.eEventType.NodifyReceivRewardCard, obj);
        SLG.GlobalEventSet.FireEvent(SLG.eEventType.NodifyReceivRewardIllustrated, obj);
    }

    public void requestCardQuality(uint nHeroId, uint[] arr)
    {
        MSG_IMPROVE_STAR_REQUEST msg = new MSG_IMPROVE_STAR_REQUEST();
        msg.idHero = nHeroId;
        msg.lst = new Packet.VICE_HEROS[arr.Length];

        ushort nCnt = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            msg.lst[i].idViceHero = arr[i];
            nCnt++;
        }
        msg.usCnt = nCnt;

        Network.NetworkMgr.me.getClient().Send<MSG_IMPROVE_STAR_REQUEST>(ref msg);

        //        NetworkMgr.me.getClient().Send<MSG_IMPROVE_STAR_REQUEST>(MSG_TYPE._MSG_IMPROVE_STAR, ref msg);
    }

    public void responseCardQuality(ushort id, object ar)
    {
        MSG.Sgt.CheckMessageId<MSG_IMPROVE_STAR_RESPONSE>(id);
        MSG_IMPROVE_STAR_RESPONSE rsponse = (MSG_IMPROVE_STAR_RESPONSE)ar;

        if (!DataManager.checkErrcode(rsponse.unErr))
            return;

        for (ushort i = 0; i < rsponse.usCnt; i++)
        {
            uint removeid = rsponse.lst[i].idViceHero;
            UICardMgr.singleton.RemoveIllustratedItemById((int)removeid);
            //HeroItemMgr.singleton.removeItemById((int)removeid);
        }

        // refresh ui
        if (rsponse.usCnt != 0)
        {
            SLG.EventArgs obj = new SLG.EventArgs();
            obj.m_obj = rsponse.idHero;
            //SLG.GlobalEventSet.FireEvent(SLG.eEventType.NodifyRefreshListUpgrade, obj);
            //SLG.GlobalEventSet.FireEvent(SLG.eEventType.NodifyRefreshQualityUpgrade, obj);
        }
    }

    public void responseChangeFreeCount(ushort id, object ar)
    {
        MSG.Sgt.CheckMessageId<MSG_DAY_CHANGE_EVENT>(id);
        MSG_DAY_CHANGE_EVENT rsponse = (MSG_DAY_CHANGE_EVENT)ar;

        SLG.GlobalEventSet.FireEvent(SLG.eEventType.NodifyUpdateFreeRewardCount, null);
    }

    public void questWaiter(ref MSG_WAITER_REQUEST en)
    {
        MSG_WAITER_REQUEST msg = en;//HERO_ACQUIRE.HERO_ACQUIRE_FREE);

        Network.NetworkMgr.me.getClient().Send<MSG_WAITER_REQUEST>(ref msg);
    }

    public void responseWaiter(ushort id, object ar)
    {
        MSG.Sgt.CheckMessageId<MSG_WAITER_RESPONSE>(id);
        MSG_WAITER_RESPONSE rsponse = (MSG_WAITER_RESPONSE)ar;

        bool b = DataManager.checkErrcode(rsponse.unErr);
        SLG.EventArgs obj = new SLG.EventArgs();
        obj.m_obj = b;
        SLG.GlobalEventSet.FireEvent(SLG.eEventType.NodifyUpdateWaiter, obj);
    }        
}

}
