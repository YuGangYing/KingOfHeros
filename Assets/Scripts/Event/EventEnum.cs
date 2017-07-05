namespace SLG
{
    // 事件类型,事件枚举数值范围在1~65535之间
    public enum eEventType : int
    {
        // 事件枚举
        PanelShow = 1, // 面板显示
        PanelHide,     // 面板隐藏
        ClickBuild,    // 点击建筑
		RefreshMainUI,
        ShowError,
        AddIllustratedUI,  // 刷新图鉴界面
        AddIllustratedItem,
        UpdateQueueTime,

        // 确定卡牌升级事件
        HeroUpgradeEvent,
        NodifyAddCardItem,
        NodifyRefreshQualityUpgrade,
        NodifyUpdateAttribute,
        NodifyUpdateWaiter,

        FreshRewardMoenyUI,    // 刷新抽奖界面信息
        NodifyOpenRewardCard,  // 
        NodifyOpenRewardCardBg,
        NodifyReceivRewardCard,
        NodifyRefreshMoney,
        NodifyReceivRewardIllustrated,
        NodifyUpdateFreeRewardCount,
        NodifyAutoOpenRewardItem,

        StartBattle, // 开始战斗事件

        OpenedPanel,
        ShowMessageBox,         // 提示框

        NodifyChangeFightState,

        //切换场景
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