using System;
using System.Collections.Generic;
using SLG;
using BattleFramework.Data;

namespace DataMgr
{
	public enum DataModel{Net,Local};
    public class DataManager : SingletonMonoBehaviourNoCreate<DataManager>
	{
        public static int OPER_SUCC = 100000; 

        public BattleFramework.Data.UserData userData;
		public List<BattleFramework.Data.HeroData> playerHeros;
		public List<BattleFramework.Data.HeroData> enemyHeros;

		public List<BattleFramework.Data.HeroData> fightPlayerHeros;
		public List<BattleFramework.Data.HeroData> fightEnemyHeros;

		public DataModel dataModel = DataModel.Local;

		#region local data
        //TODO
        public void InitLocalUserData()
        {
            userData = new BattleFramework.Data.UserData();
            userData.name = "臣妾做不到啊";
            userData.coin = 2000;
            userData.crystal = 100;
            userData.dollor = 10;
            userData.level = 1;
            userData.exp = 10;
            userData.upgradeExp = 20;
        }

		public void InitPlayerHeroDatas(){
			playerHeros = SimpleLocalData.InitLocalPlayerHeros ();
			ConfigBase configBase = _config.getCfg (CONFIG_MODULE.CFG_CVS_HERO_BASE);
			for(int i=0;i < playerHeros.Count;i ++)
			{

			}
		}

		#endregion


        LoginData _loginData = null;
        ConfigMgr _config = null;
		UserData _userData = null;
        HeroData _heroData = null;
		TechData _techData = null;
		BuildData _buildData = null;
		ChatmsgData _chatmsgData = null;
        FriendData _friendData = null;
        SoldierData _soldierData = null;
        SystemCfg _sysCfg = null;
        AchievementData _achievementData = null;
        MailData _mailData = null;
        BattleUIData _battleUIData = null;
        LanguageMgr _languageMgr = null;
        TimeServer _timeServer = null;
        QueueData _queueData = null;
        HeroCardMsg _msgHeroCard = null;
		SkillData _skillData = null;


		protected override void Init()
        {
            DontDestroyOnLoad(this.gameObject);
            base.Init();
           
            _loginData = new LoginData();
            _config = new ConfigMgr();
            _userData = new UserData();
            _heroData = new HeroData();
            _techData = new TechData();
            _buildData = new BuildData();
            _chatmsgData = new ChatmsgData();
            _friendData = new FriendData();
            _soldierData = new SoldierData();
            _sysCfg = new SystemCfg();
            _achievementData = new AchievementData();
            _mailData = new MailData();
            _battleUIData = new BattleUIData();
            _languageMgr = new LanguageMgr();
            _timeServer = new TimeServer();
            _queueData = new QueueData();
            _msgHeroCard = new HeroCardMsg();
			_skillData = new SkillData();

            _loginData.init();
            _userData.init();
			_heroData.init();
			_techData.init();
			_buildData.init();
			_chatmsgData.init();
            _soldierData.init();
			_achievementData.init();
			_mailData.init();
			_battleUIData.init();
            _languageMgr.init();
            _timeServer.init();
			_friendData.init();
			_queueData.init();
            _msgHeroCard.init();
			_skillData.Init();
            _sysCfg.init();


			InitLocalUserData();//TODO
			InitPlayerHeroDatas ();//TODO
		}

        bool ChangeScene(SLG.EventArgs obj)
        {
            if (this._techData!=null)
                this._techData.reload();
            if (this._soldierData != null)
                this._soldierData.reload();
            if (this._buildData != null)
                this._buildData.reload();
            if (this._battleUIData != null)
                this._battleUIData.reload();
            if (this._achievementData != null)
                this._achievementData.reload();
            if (this._mailData != null)
                this._mailData.reload();
            if (this._friendData != null)
    			this._friendData.reload();
			if (this._skillData != null)
				this._skillData.reload();
            return true;
        }

        public void release()
        {
            if (this._config!=null)
                this._config.release();
            if (this._userData != null)
                this._userData.release();
            if (this._heroData != null)
                this._heroData.release();
            if (this._techData != null)
                this._techData.release();
            if (this._buildData != null)
                this._buildData.release();
            if (this._chatmsgData != null)
                this._chatmsgData.release();
            if (this._soldierData != null)
                this._soldierData.release();
            if (this._achievementData != null)
                this._achievementData.release();
            if (this._mailData != null)
                this._mailData.release();
            if (this._battleUIData != null)
                this._battleUIData.release();
            if (this._friendData != null)
                this._friendData.release();
            if (this._queueData != null)
                this._queueData.release();
            if (this._msgHeroCard != null )
            {
                this._msgHeroCard.release();
            }
			if (this._skillData != null)
				this._skillData.release();
            if (this._sysCfg != null)
            {
                this._sysCfg.release();
            }
        }

        static public bool isDone()
        {
            return getLoginData().isDone;
        }

        static public LoginData getLoginData()
        {
            return DataManager.me._loginData;
        }

        static public SystemCfg getSyscfg()
        {
            return DataManager.me._sysCfg;
        }

        static public ConfigBase getConfig(CONFIG_MODULE module)
		{
			return DataManager.me._config.getCfg(module);
		}
				
		static public UserData getUserData()
		{
			return DataManager.me._userData;
		}
		
		static public  HeroData getHeroData()
		{
			return DataManager.me._heroData;
		}

		static public  TechData getTechData()
		{
			return DataManager.me._techData;
		}

		static public  BuildData getBuildData()
		{
			return DataManager.me._buildData;			
		}
		
		static public  ChatmsgData getChatmsgData()
		{
			return DataManager.me._chatmsgData;
		}
		
		static public  FriendData getFriendData()
		{
			return DataManager.me._friendData;
		}

        static public SoldierData getSoldierData()
        {
            return DataManager.me._soldierData;
        }
		static public AchievementData getAchievementData()
		{
			return DataManager.me._achievementData;
		}
		static public MailData getMailData()
		{
			return DataManager.me._mailData;
		}
		static public BattleUIData getBattleUIData()
		{
			return DataManager.me._battleUIData;
		}

        static public LanguageMgr getLanguageMgr()
        {
            return DataManager.me._languageMgr;
        }

        static public TimeServer getTimeServer()
        {
            return DataManager.me._timeServer;
        }
		
		static public QueueData getQueueData()
		{
			return DataManager.me._queueData;
		}

        static public HeroCardMsg MsgHeroCard
        {
            get{ return DataManager.me._msgHeroCard; }
        }

		static public SkillData getSkillData()
		{
			return DataManager.me._skillData;
		}

        static public bool checkErrcode(uint code)
        {
            if (code == OPER_SUCC)
                return true;
            MessageBoxMgr.ShowConfirm("Failed",DataManager.getLanguageMgr().getString((int)code));
            //MessageBoxMgr.ShowConfirm("Failed", "Errcode=" + code.ToString() + "\r\n" + DataManager.getLanguageMgr().getString((int)code));
            return false;
        }
	}
}
