using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
namespace UI
{
    public class PanelInit : MonoBehaviour
    {
        [System.Serializable]
        public class PanelData
        {
            public PanelID panelid;

            public List<GameObject> prefabs; // 面板资源
        }

        Dictionary<PanelID, List<string>> m_panelResource = new Dictionary<PanelID, List<string>>();

        void Start()
        {
            this.InitFactory();
            this.InitResource();

            PanelManage.me.GetPanel<MainPanel>(PanelID.MainPanel);
            PanelManage.me.GetPanel<MessageBoxPanel>(PanelID.MessageBoxPannel).SetVisible(false);
            if (MainController.me.loadCityTimes > 1)
                PanelManage.me.GetPanel<BattlefieldPanel>(PanelID.BattlefieldPanel).SetVisible(true);
            //             PanelManage.me.GetPanel<CheckinPanel>(PanelID.CheckInPanel);
            PanelManage.me.GetPanel<BuildInfoPanel>(PanelID.BuildInfoPanel);
            PanelManage.me.GetPanel<SettingPanel>(PanelID.SettingPanel);
            AudioCenter.me.play(AudioMgr.AudioName.MAINCITY_BMG);
        }

        void OnDestroy()
        {
            PanelManage.Release();
        }

        void Update()
        {
            PanelManage.me.update();
        }

        void InitFactory()
        {
            PanelManage.me.AddPanelFactory(PanelID.BuildInfoPanel, new PanelFactory<BuildInfoPanel>());
            PanelManage.me.AddPanelFactory(PanelID.MainPanel, new PanelFactory<MainPanel>());
            PanelManage.me.AddPanelFactory(PanelID.ShopPanel, new PanelFactory<ShopPanel>());
            PanelManage.me.AddPanelFactory(PanelID.BuildMallPanel, new PanelFactory<BuildMallPanel>());
            PanelManage.me.AddPanelFactory(PanelID.SoldierRecruitPanel, new PanelFactory<UISoldierPanel>());
            PanelManage.me.AddPanelFactory(PanelID.CardHeroListPanel, new PanelFactory<CardHeroListPanel>());
            PanelManage.me.AddPanelFactory(PanelID.CardHeroDetailPanel, new PanelFactory<CardHeroDetailPanel>());
            PanelManage.me.AddPanelFactory(PanelID.CardIllustratedListPanel, new PanelFactory<CardIllustratedListPanel>());
            PanelManage.me.AddPanelFactory(PanelID.CardIllustratedDetailPanel, new PanelFactory<CardIllustratedDetailPanel>());
            PanelManage.me.AddPanelFactory(PanelID.CardQualityUpdatePanel, new PanelFactory<CardQualityUpdatePanel>());
            PanelManage.me.AddPanelFactory(PanelID.CardStarUpdatePanel, new PanelFactory<CardStarUpdatePanel>());
            PanelManage.me.AddPanelFactory(PanelID.HeroUpgradePanel, new PanelFactory<HeroUpgradePanel>());
            PanelManage.me.AddPanelFactory(PanelID.RewardPanel, new PanelFactory<RewardPanel>());
            PanelManage.me.AddPanelFactory(PanelID.TechPanel, new PanelFactory<UITechPanel>());
            PanelManage.me.AddPanelFactory(PanelID.RewardCardPanel, new PanelFactory<RewardCardPanel>());
            PanelManage.me.AddPanelFactory(PanelID.WallUpgradePanel, new PanelFactory<WallUpgradePanel>());
            PanelManage.me.AddPanelFactory(PanelID.GoldUpgradePanel, new PanelFactory<GoldUpgradePanel>());
            PanelManage.me.AddPanelFactory(PanelID.MagicStoneUpgradePanel, new PanelFactory<MagicStoneUpgradePanel>());
            PanelManage.me.AddPanelFactory(PanelID.mainCityUpgradePanel, new PanelFactory<mainCityUpgradePanel>());
            PanelManage.me.AddPanelFactory(PanelID.StoreUpgradePanel, new PanelFactory<StoreUpgradePanel>());
            PanelManage.me.AddPanelFactory(PanelID.TrainingUpgradePanel, new PanelFactory<TrainingUpgradePanel>());
            PanelManage.me.AddPanelFactory(PanelID.HouseUpgradePanel, new PanelFactory<HouseUpgradePanel>());
            PanelManage.me.AddPanelFactory(PanelID.CollegeUpgradePanel, new PanelFactory<CollegeUpgradePanel>());
            PanelManage.me.AddPanelFactory(PanelID.MessageBoxPannel, new PanelFactory<MessageBoxPanel>());
            PanelManage.me.AddPanelFactory(PanelID.HeroSkillUpdatePanel, new PanelFactory<HeroSkillUpdatePanel>());
            PanelManage.me.AddPanelFactory(PanelID.AttendantsPanel, new PanelFactory<AttendantsPanel>());
            PanelManage.me.AddPanelFactory(PanelID.MailPanel, new PanelFactory<MailPanel>());
            PanelManage.me.AddPanelFactory(PanelID.ChatPanel, new PanelFactory<ChatPanel>());
            PanelManage.me.AddPanelFactory(PanelID.ManeuverPanel, new PanelFactory<ManeuverPanel>());
            PanelManage.me.AddPanelFactory(PanelID.BattlefieldPanel, new PanelFactory<BattlefieldPanel>());
            PanelManage.me.AddPanelFactory(PanelID.AchievenmentPanel, new PanelFactory<AchievenmentPanel>());
            PanelManage.me.AddPanelFactory(PanelID.AuneauUpgradePanel, new PanelFactory<AuneauUpgradePanel>());
            PanelManage.me.AddPanelFactory(PanelID.CellarUpgradePanel, new PanelFactory<CellarUpgradePanel>());
            PanelManage.me.AddPanelFactory(PanelID.TownHallUpgradePanel, new PanelFactory<TownHallUpgradePanel>());
            PanelManage.me.AddPanelFactory(PanelID.PantheonUpgradePanel, new PanelFactory<PantheonUpgradePanel>());
            PanelManage.me.AddPanelFactory(PanelID.PubUpgradePanel, new PanelFactory<PubUpgradePanel>());
            PanelManage.me.AddPanelFactory(PanelID.SmithyUpgradePanel, new PanelFactory<SmithyUpgradePanel>());
            PanelManage.me.AddPanelFactory(PanelID.CheckInPanel, new PanelFactory<CheckinPanel>());
            PanelManage.me.AddPanelFactory(PanelID.SettingPanel, new PanelFactory<SettingPanel>());
        }

        void InitResource()
        {
            PanelManage.me.addResource(PanelID.ShopPanel, "MallUI");
            PanelManage.me.addResource(PanelID.MainPanel, "MainUI");
            PanelManage.me.addResource(PanelID.SoldierRecruitPanel, "Recruit");
            PanelManage.me.addResource(PanelID.BuildInfoPanel, "buildInfoUI");
            PanelManage.me.addResource(PanelID.BuildInfoPanel, "buildInfoItem");
            PanelManage.me.addResource(PanelID.CardHeroListPanel, "HeroListWin");
            PanelManage.me.addResource(PanelID.CardHeroListPanel, "ItemHero");
            PanelManage.me.addResource(PanelID.CardHeroListPanel, "viewListCameraArea");
            PanelManage.me.addResource(PanelID.CardHeroListPanel, "ItemHeroNull");
            PanelManage.me.addResource(PanelID.CardHeroDetailPanel, "HeroDetailWin");
            PanelManage.me.addResource(PanelID.CardIllustratedListPanel, "HeroCardListWin");
            PanelManage.me.addResource(PanelID.CardIllustratedListPanel, "ItemCard");
            PanelManage.me.addResource(PanelID.CardIllustratedDetailPanel, "HeroCardDetailWin");
            PanelManage.me.addResource(PanelID.CardIllustratedDetailPanel, "skillRoot");
            PanelManage.me.addResource(PanelID.CardIllustratedDetailPanel, "skillRootLabelDescribute");
            PanelManage.me.addResource(PanelID.CardQualityUpdatePanel, "HeroQualityUpdateWin");
            PanelManage.me.addResource(PanelID.CardQualityUpdatePanel, "HeroItem");
            PanelManage.me.addResource(PanelID.CardQualityUpdatePanel, "ItemQuality");
            PanelManage.me.addResource(PanelID.CardQualityUpdatePanel, "iconbtn");
            PanelManage.me.addResource(PanelID.CardStarUpdatePanel, "StarUpdateWin");
            PanelManage.me.addResource(PanelID.BuildMallPanel, "BuildMallUI");
            PanelManage.me.addResource(PanelID.BuildMallPanel, "BuildMallItemUI");
            PanelManage.me.addResource(PanelID.HeroUpgradePanel, "heroUpgrade");
            PanelManage.me.addResource(PanelID.HeroUpgradePanel, "HeroItem");
            PanelManage.me.addResource(PanelID.RewardPanel, "HeroRewardWin");
            PanelManage.me.addResource(PanelID.TechPanel, "Tech");
            PanelManage.me.addResource(PanelID.RewardCardPanel, "HeroRewardAcardWin");
            PanelManage.me.addResource(PanelID.WallUpgradePanel, "wallUpgradeUI");
            PanelManage.me.addResource(PanelID.GoldUpgradePanel, "GoldUpgradeUI");
            PanelManage.me.addResource(PanelID.MagicStoneUpgradePanel, "MagicStoneUpgradeUI");
            PanelManage.me.addResource(PanelID.mainCityUpgradePanel, "mainCityUI");
            PanelManage.me.addResource(PanelID.StoreUpgradePanel, "StoreUpgrade");
            PanelManage.me.addResource(PanelID.TrainingUpgradePanel, "trainingUpgradeUI");
            PanelManage.me.addResource(PanelID.HouseUpgradePanel, "HouseUpgradeUI");
            PanelManage.me.addResource(PanelID.CollegeUpgradePanel, "CollegeUpgradeUI");
            PanelManage.me.addResource(PanelID.MessageBoxPannel, "MessageBoxUI");
            PanelManage.me.addResource(PanelID.HeroSkillUpdatePanel, "heroSkillUpdate");
            PanelManage.me.addResource(PanelID.AttendantsPanel, "HeroAttendantsPanel");
            PanelManage.me.addResource(PanelID.AttendantsPanel, "HeroItem");
            PanelManage.me.addResource(PanelID.MailPanel, "MailUI");
            PanelManage.me.addResource(PanelID.ChatPanel, "ChatUI");
            PanelManage.me.addResource(PanelID.ChatPanel, "FriendUI");
            PanelManage.me.addResource(PanelID.ManeuverPanel, "ALineup");
            PanelManage.me.addResource(PanelID.BattlefieldPanel, "BattlefieldUI");
            PanelManage.me.addResource(PanelID.AchievenmentPanel, "AchievenmentUI");
            PanelManage.me.addResource(PanelID.AchievenmentPanel, "AchievenmentItemUI");
            PanelManage.me.addResource(PanelID.AuneauUpgradePanel, "AuneauUpUI");
            PanelManage.me.addResource(PanelID.CellarUpgradePanel, "CellarUpUI");
            PanelManage.me.addResource(PanelID.TownHallUpgradePanel, "TownHallUpgrade");
            PanelManage.me.addResource(PanelID.PantheonUpgradePanel, "PantheonUpgradeUI");
            PanelManage.me.addResource(PanelID.PubUpgradePanel, "PubUpgradeUI");
            PanelManage.me.addResource(PanelID.SmithyUpgradePanel, "SmithyUpgradeUI");
            PanelManage.me.addResource(PanelID.CheckInPanel, "CheckInPanel");
            PanelManage.me.addResource(PanelID.SettingPanel, "SettingPanel");
        }
    }
}