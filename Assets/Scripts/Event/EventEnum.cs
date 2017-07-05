namespace SLG
{
    // �¼�����,�¼�ö����ֵ��Χ��1~65535֮��
    public enum eEventType : int
    {
        // �¼�ö��
        PanelShow = 1, // �����ʾ
        PanelHide,     // �������
        ClickBuild,    // �������
		RefreshMainUI,
        ShowError,
        AddIllustratedUI,  // ˢ��ͼ������
        AddIllustratedItem,
        UpdateQueueTime,

        // ȷ�����������¼�
        HeroUpgradeEvent,
        NodifyAddCardItem,
        NodifyRefreshQualityUpgrade,
        NodifyUpdateAttribute,
        NodifyUpdateWaiter,

        FreshRewardMoenyUI,    // ˢ�³齱������Ϣ
        NodifyOpenRewardCard,  // 
        NodifyOpenRewardCardBg,
        NodifyReceivRewardCard,
        NodifyRefreshMoney,
        NodifyReceivRewardIllustrated,
        NodifyUpdateFreeRewardCount,
        NodifyAutoOpenRewardItem,

        StartBattle, // ��ʼս���¼�

        OpenedPanel,
        ShowMessageBox,         // ��ʾ��

        NodifyChangeFightState,

        //�л�����
        ChangeScene,
        //
        NetConntSucc, 
        NetConntFailed,
        NetDiscon,
        NodifyTechLearn,
        NodifyTechUpdate,
        NodifyTechAcce,
    }
}