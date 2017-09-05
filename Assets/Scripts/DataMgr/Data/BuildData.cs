using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI;
using Packet;
using Network;
//using Config;
using WWWNetwork;

namespace DataMgr
{
	public class BuildData 
	{
        Dictionary<uint, Build> m_dicBuilding;
		
		private Build m_createNewBuild = null;

		public BuildData()
		{
		}
		
		public bool init()
		{
			// 注册消息处理函数
			NetworkMgr.me.getClient().RegMsgFunc<MSG_BUILDING_BUILD_RESPONSE>(OnRecClientBuildingBuild);
			NetworkMgr.me.getClient().RegMsgFunc<MSG_BUILDING_UPLEV_RESPONSE>(OnRecClientBuildUplev);
			NetworkMgr.me.getClient().RegMsgFunc<MSG_BUILDING_ACCELERATE_UPLEV_RESPONSE>(OnRecClientBuildAccelerate);
			NetworkMgr.me.getClient().RegMsgFunc<MSG_BUILDING_MOVE_RESPONSE>(OnRecClientBuildMove);
			NetworkMgr.me.getClient().RegMsgFunc<MSG_BUILDING_LIST_EVENT>(OnRecClientBuildList);
			NetworkMgr.me.getClient().RegMsgFunc<MSG_BUILDING_UPDATE_EVENT>(OnRecCleintBuildUpdate);
			NetworkMgr.me.getClient().RegMsgFunc<MSG_BUILDING_UPLEV_CANCEL_RESPONSE>(OnRecClientBuildCancel);
			NetworkMgr.me.getClient().RegMsgFunc<MSG_BUILDING_LEVY_RESPONSE>(OnRecClientBuildLevy);
			
			// 初始化数据
			m_dicBuilding = new Dictionary<uint, Build>();
			
			//QueueMgr.CreateInstance();
            LoadBuildingCfg();
			return true;
		}
		
		public void release()
		{
            if(m_dicBuilding!=null)
                m_dicBuilding.Clear();
		}

		public void reload()
		{

		}
		
		// 接收到建筑成功与否消息
		public void OnRecClientBuildingBuild(ushort id, object ar)
		{
			MSG.Sgt.CheckMessageId<MSG_BUILDING_BUILD_RESPONSE>(id);
			MSG_BUILDING_BUILD_RESPONSE msg_struct = (MSG_BUILDING_BUILD_RESPONSE)ar;

            if (DataManager.checkErrcode(msg_struct.wErrCode))
			{
				if (m_createNewBuild.m_idBuildingType != msg_struct.idBuildingType)
				{
					Logger.LogDebug("OnRecClientBuildingBuild::Server rev type error idBuildingType = {0}", msg_struct.idBuildingType);
				}
				
				m_createNewBuild.m_idBuilding = msg_struct.idBuilding;
				m_createNewBuild.CleanRoundTree();
				m_dicBuilding.Add(msg_struct.idBuilding, m_createNewBuild);
				m_createNewBuild = null;
			}
			else
			{
				// 创建失败
				GameObject.Destroy(m_createNewBuild.transform.parent.gameObject);
				Logger.LogDebug("Create New Build Error Ret = {0}", msg_struct.wErrCode);
			}
			
			//ShowBuildOperaRetText((Packet.BUILDING_RET)msg_struct.cbRet);
		}
		
		// 建筑升级消息
		public void OnRecClientBuildUplev(ushort id, object ar)
		{
			MSG.Sgt.CheckMessageId<MSG_BUILDING_UPLEV_RESPONSE>(id);
			MSG_BUILDING_UPLEV_RESPONSE msg_struct = (MSG_BUILDING_UPLEV_RESPONSE)ar;

            if (DataManager.checkErrcode(msg_struct.wErrCode))
			{
				if (!m_dicBuilding.ContainsKey(msg_struct.idBuilding))
				{
					Logger.LogDebug("OnRecClientBuildUplev::Server rev id error idBuilding = {0}", msg_struct.idBuilding);
				}
				
				m_dicBuilding[msg_struct.idBuilding].ShowBuildingCDPanel();
                //m_dicBuilding[msg_struct.idBuilding].ShowBuildConstruct();
                m_dicBuilding[msg_struct.idBuilding].ShowBuildConstructShader();
			}
			else
			{
				// 升级失败
				Logger.LogDebug("Up lev Build Error Ret = {0}", msg_struct.wErrCode);
			}
			
			//ShowBuildOperaRetText((Packet.BUILDING_RET)msg_struct.cbRet);
		}
		
		// 建筑加速升级消息
		public void OnRecClientBuildAccelerate(ushort id, object ar)
		{
			MSG.Sgt.CheckMessageId<MSG_BUILDING_ACCELERATE_UPLEV_RESPONSE>(id);
			MSG_BUILDING_ACCELERATE_UPLEV_RESPONSE msg_struct = (MSG_BUILDING_ACCELERATE_UPLEV_RESPONSE)ar;

            if (DataManager.checkErrcode(msg_struct.wErrCode))
			{
				if (!m_dicBuilding.ContainsKey(msg_struct.idBuilding))
				{
					Logger.LogDebug("OnRecClientBuildAccelerate::Server rev id error idBuilding = {0}", msg_struct.idBuilding);
				}
			}
			else
			{
				// 加速升级失败
				Logger.LogDebug("Accelerate Build Error Ret = {0}", msg_struct.wErrCode);
			}
			
			//ShowBuildOperaRetText((Packet.BUILDING_RET)msg_struct.cbRet);
		}
		
		// 建筑升级取消
		public void OnRecClientBuildCancel(ushort id, object ar)
		{
			MSG.Sgt.CheckMessageId<MSG_BUILDING_UPLEV_CANCEL_RESPONSE>(id);
			MSG_BUILDING_UPLEV_CANCEL_RESPONSE msg_struct = (MSG_BUILDING_UPLEV_CANCEL_RESPONSE)ar;

            if (DataManager.checkErrcode(msg_struct.wErrCode))
			{
				if (!m_dicBuilding.ContainsKey(msg_struct.idBuilding))
				{
					Logger.LogDebug("OnRecClientBuildUplev::Server rev id error idBuilding = {0}", msg_struct.idBuilding);
				}
			}
			else
			{
				// 升级失败
				Logger.LogDebug("Up lev Build Error Ret = {0}", msg_struct.wErrCode);
			}
			
			//ShowBuildOperaRetText((Packet.BUILDING_RET)msg_struct.cbRet);
		}
		
		// 建筑征收消息
		public void OnRecClientBuildLevy(ushort id, object ar)
		{
			MSG.Sgt.CheckMessageId<MSG_BUILDING_LEVY_RESPONSE>(id);
			MSG_BUILDING_LEVY_RESPONSE msg_struct = (MSG_BUILDING_LEVY_RESPONSE)ar;

            if (DataManager.checkErrcode(msg_struct.wErrCode))
			{
				if (!m_dicBuilding.ContainsKey(msg_struct.idBuilding))
				{
					Logger.LogDebug("OnRecClientBuildUplev::Server rev id error idBuilding = {0}", msg_struct.idBuilding);
				}
			}
			else
			{
				// 升级失败
				Logger.LogDebug("Up lev Build Error Ret = {0}", msg_struct.wErrCode);
			}
			
			//ShowBuildOperaRetText((Packet.BUILDING_RET)msg_struct.cbRet);
		}
		
		// 建筑移动消息
		public void OnRecClientBuildMove(ushort id, object ar)
		{
			MSG.Sgt.CheckMessageId<MSG_BUILDING_MOVE_RESPONSE>(id);
			MSG_BUILDING_MOVE_RESPONSE msg_struct = (MSG_BUILDING_MOVE_RESPONSE)ar;

            if (DataManager.checkErrcode(msg_struct.wErrCode))
			{
				if (!m_dicBuilding.ContainsKey(msg_struct.idBuilding))
				{
					Logger.LogDebug("Move Build error id = {0}", msg_struct.idBuilding);
				}
				
				//m_dicBuilding[msg_struct.idBuilding].CleanRoundTree();
			}
			else
			{
				// 移动失败 位置返回
				if (m_dicBuilding.ContainsKey(msg_struct.idBuilding))
				{
					m_dicBuilding[msg_struct.idBuilding].RestorePos();
				}
				
				Logger.LogDebug("Move Build Error Ret = {0}", msg_struct.wErrCode);
			}
			
			//ShowBuildOperaRetText((Packet.BUILDING_RET)msg_struct.cbRet);
		}

        Dictionary<uint, BUILDING_INFO> m_dicBuildInfo = new Dictionary<uint, BUILDING_INFO>();
        
        // 建筑列表消息
		public void OnRecClientBuildList(ushort id, object ar)
		{
			MSG.Sgt.CheckMessageId<MSG_BUILDING_LIST_EVENT>(id);
			MSG_BUILDING_LIST_EVENT msg_struct = (MSG_BUILDING_LIST_EVENT)ar;

            m_dicBuildInfo.Clear();
            for (int i = 0; i < msg_struct.usCnt; i++)
			{
                BUILDING_INFO info = new BUILDING_INFO();

                info.idBuilding = msg_struct.lst[i].idBuilding;
                info.idBuildingType = msg_struct.lst[i].idBuildingType;
                info.u32LevyTime = msg_struct.lst[i].u32LevyTime;
                info.cbLev = msg_struct.lst[i].cbLev;
                info.cbAreaWidth = msg_struct.lst[i].cbAreaWidth;
                info.cbAreaHigh = msg_struct.lst[i].cbAreaHigh;
                info.cbState = msg_struct.lst[i].cbState;
                info.fPosX = msg_struct.lst[i].fPosX;
                info.fPosY = msg_struct.lst[i].fPosY;

                m_dicBuildInfo.Add(info.idBuilding, info);

			}
		}

        public void CreatBuildingByBuildList()
        {
            foreach (KeyValuePair<uint, Build> kvi in m_dicBuilding)
            {
                BUILDING_INFO info = new BUILDING_INFO();

                info.idBuilding = kvi.Value.m_idBuilding;
                info.idBuildingType = kvi.Value.m_idBuildingType;
                info.u32LevyTime = kvi.Value.m_u32LevyTime;
                info.cbLev = kvi.Value.m_cbLev;
                info.cbAreaWidth = kvi.Value.m_cbAreaWidth;
                info.cbAreaHigh = kvi.Value.m_cbAreaHigh;
                info.cbState = kvi.Value.m_cbState;
                info.fPosX = kvi.Value.m_buildFound;
                info.fPosY = 0;

                if (m_dicBuildInfo.ContainsKey(info.idBuilding))
                {
                    m_dicBuildInfo.Remove(info.idBuilding);
                }


                m_dicBuildInfo.Add(info.idBuilding, info);
            }
            
            foreach (KeyValuePair<uint, BUILDING_INFO> kvi in m_dicBuildInfo)
            {
                Build build = PutBuild.GetInstance.OnPutBuild(kvi.Value);
 				if (build != null)
 				{
                    m_dicBuilding[kvi.Value.idBuilding] = build;
 					
 					if (build.m_cbState == 2)
 					{
 						build.ShowBuildingCDPanel();
                        //build.ShowBuildConstruct();
                        build.ShowBuildConstructShader();
 					}
 				}
            }
        }
		
		// 建筑数据更新消息
		public void OnRecCleintBuildUpdate(ushort id, object ar)
		{
			MSG.Sgt.CheckMessageId<MSG_BUILDING_UPDATE_EVENT>(id);
			MSG_BUILDING_UPDATE_EVENT msg_struct = (MSG_BUILDING_UPDATE_EVENT)ar;
            
			if (!m_dicBuilding.ContainsKey(msg_struct.idBuilding))
			{
				Logger.LogDebug("update Build error id = {0}", msg_struct.idBuilding);
			}
			
			Build build = m_dicBuilding[msg_struct.idBuilding];
			build.m_idBuildingType = msg_struct.idBuildingType;
			build.m_u32LevyTime = msg_struct.u32LevyTime;
			
			if (build.m_cbState != msg_struct.cbState)
			{
                build.m_cbState = msg_struct.cbState;
                
                if (msg_struct.cbLev != 0 && build.m_cbLev != msg_struct.cbLev)
				{
					build.m_cbLev = msg_struct.cbLev;
					build = ChangeBuild(build);
				}
				
				if (msg_struct.cbState == 1)
				{
                    //播放建筑完成光效
                    DataMgr.BuildConfig config = DataManager.getBuildData().GetBuildConfig((int)build.m_idBuildingType);

                    if (config == null || !config.isMove)
                    {
                        return;
                    }

                    string strPath = "Prefabs/Buildings/BuildingFinishing_light";
                    GameObject finishEffect = DataMgr.ResourceCenter.LoadAsset<GameObject>(strPath);
                    if (finishEffect != null)
                    {
                        GameObject effect = (GameObject)Object.Instantiate(finishEffect);
                        effect.transform.parent = build.transform;
                        effect.transform.localPosition = Vector3.zero;
                    }

                    //build.HideBuildConstruct();
                    build.RestoreBuild();
				}
				else if (msg_struct.cbState == 2)
				{
					build.ShowBuildingCDPanel();
                    //build.ShowBuildConstruct();
                    build.ShowBuildConstructShader();
				}

                //build.m_cbState = msg_struct.cbState;
			}
		}
		
		public void SetShowToPanel()
		{
			BuildMallPanel mallPanel = UI.PanelManage.me.GetPanel<BuildMallPanel>(PanelID.BuildMallPanel);
			mallPanel.Release();
            foreach (KeyValuePair<int, BuildConfig> kvi in m_buildConfigList)
			{
                if (kvi.Value.id == (int)BuildType.LORD_HALL || kvi.Value.id == (int)BuildType.WALL || kvi.Value.id == (int)BuildType.MONUMENT)
				{
					continue;
				}
				
				BuildMallPanel.ItemData itemData = new BuildMallPanel.ItemData();
				itemData.id = kvi.Key;
				itemData.name = kvi.Value.name;
				itemData.icon = kvi.Value.icon;
				itemData.money = kvi.Value.levels[1].magicStone;
				itemData.time = kvi.Value.levels[1].time;
                itemData.count = GetBuildCountByType((BuildType)kvi.Value.id);
				itemData.maxCount = kvi.Value.maxCout;
                itemData.desc = kvi.Value.text;
                itemData.CastleLevel = kvi.Value.levels[1].mainLevel;

				mallPanel.AddItem(itemData);
			}
		}
		
		public Build ChangeBuild(Build build)
		{
			Build buildTemp = PutBuild.GetInstance.OnChangeBuild(build);
			if (buildTemp != null)
			{
				m_dicBuilding[buildTemp.m_idBuilding] = buildTemp;
				return buildTemp;
			}
			
			return build;
		}
		
		public bool canBuildingNewBuild()
		{
			if (m_createNewBuild != null)
			{
				return false;
			}
			
			return true;
		}
		
		public byte GetBuildLev(BuildType type)
		{
			byte cbLev = 0;
			foreach (KeyValuePair<uint, Build> kvi in m_dicBuilding)
			{
				if (kvi.Value.m_idBuildingType == (uint)type && kvi.Value.m_cbLev > cbLev)
				{
					cbLev = kvi.Value.m_cbLev;
				}
			}
			
			return cbLev;
		}
		
		public Build GetBuildByType(BuildType type)
		{
			foreach (KeyValuePair<uint, Build> kvi in m_dicBuilding)
			{
				if (kvi.Value.m_idBuildingType == (uint)type)
				{
					return kvi.Value;
				}
			}
			
			return null;
		}
		
		public int GetBuildCountByType(BuildType type)
		{
			int nCount = 0;
			foreach (KeyValuePair<uint, Build> kvi in m_dicBuilding)
			{
				if (kvi.Value.m_idBuildingType == (uint)type)
				{
					nCount++;
				}
			}
			
			return nCount;
		}
		
		public Build GetBuildByID(uint unID)
		{
			if (m_dicBuilding.ContainsKey(unID))
			{
				return m_dicBuilding[unID];
			}
			
			return null;
		}
		
		// 发送建造建筑消息
		public void SendBuildingBuild(Build build)
		{
			if (m_createNewBuild == null && build != null)
			{
				MSG_BUILDING_BUILD_REQUEST msg_struct = new MSG_BUILDING_BUILD_REQUEST();
				msg_struct.idBuildingType = build.m_idBuildingType;
				msg_struct.fPosX = (float)build.m_buildFound;
				msg_struct.fPosY = 0;
				NetworkMgr.me.getClient().Send(ref msg_struct);
				m_createNewBuild = build;
			}
		}

		// 发送移动建筑消息(socket服务器)
		public void SendMoveBuild(uint idBuilding, float x, float y)
		{
			MSG_BUILDING_MOVE_REQUEST msg_struct = new MSG_BUILDING_MOVE_REQUEST();
			msg_struct.idBuilding = idBuilding;
			msg_struct.fPosX = x;
			msg_struct.fPosY = 0;
          m_dicBuilding[msg_struct.idBuilding].m_buildFound = (uint)x;
			NetworkMgr.me.getClient().Send(ref msg_struct);
		}
		
		// 发送升级建筑消息
		public void SendBuildUplev(uint idBuilding)
		{
			MSG_BUILDING_UPLEV_REQUEST msg_struct = new MSG_BUILDING_UPLEV_REQUEST();
			msg_struct.idBuilding = idBuilding;
			NetworkMgr.me.getClient().Send(ref msg_struct);
		}
		
		// 发送加速建筑消息
		public void SendBuildAccelerate(uint idBuilding, uint u32Secs)
		{
			MSG_BUILDING_ACCELERATE_UPLEV_REQUEST msg_struct = new MSG_BUILDING_ACCELERATE_UPLEV_REQUEST();
			msg_struct.idBuilding = idBuilding;
			msg_struct.u32Secs = u32Secs;
			NetworkMgr.me.getClient().Send(ref msg_struct);
		}
		
		// 发送取消升级消息
		public void SendBuildUplevCancel(uint idBuilding)
		{
			MSG_BUILDING_UPLEV_CANCEL_REQUEST msg_struct = new MSG_BUILDING_UPLEV_CANCEL_REQUEST();
			msg_struct.idBuilding = idBuilding;
			NetworkMgr.me.getClient().Send(ref msg_struct);
		}
		
		public void SendBuildLevy(uint idBuilding)
		{
			MSG_BUILDING_LEVY_REQUEST msg_struct = new MSG_BUILDING_LEVY_REQUEST();
			msg_struct.idBuilding = idBuilding;
			NetworkMgr.me.getClient().Send(ref msg_struct);
		}
		
// 		public void ShowBuildOperaRetText(Packet.BUILDING_RET cbRet)
// 		{
// 			string strText = "";
// 			
// 			switch (cbRet)
// 			{
// 			case Packet.BUILDING_RET.BUILDING_RET_SUC:
// 			{
// 				strText = "建筑操作成功";
// 				break;
// 			}
// 			case Packet.BUILDING_RET.BUILDING_RET_UNKNOWN:
// 			{
// 				strText = "建筑操作错误";
// 				break;
// 			}
// 			case Packet.BUILDING_RET.BUILDING_RET_PARAM:
// 			{
// 				strText = "参数错误";
// 				break;
// 			}
// 			case Packet.BUILDING_RET.BUILDING_RET_NOT_EXIST:
// 			{
// 				strText = "不存在";
// 				break;
// 			}
// 			case Packet.BUILDING_RET.BUILDING_RET_MONEY:
// 			{
// 				strText = "金币不足";
// 				break;
// 			}
// 			case Packet.BUILDING_RET.BUILDING_RET_SMONEY:
// 			{
// 				strText = "魔石不足";
// 				break;
// 			}
// 			case Packet.BUILDING_RET.BUILDING_RET_RMB:
// 			{
// 				strText = "现实货币(人民币)不足";
// 				break;
// 			}
// 			case Packet.BUILDING_RET.BUILDING_RET_CB_LEV:
// 			{
// 				strText = "建筑等级无法超过城堡等级，请升级城堡";
// 				break;
// 			}
// 			case Packet.BUILDING_RET.BUILDING_RET_NOT_IDLE_QUEUE:
// 			{
// 				strText = "没有空闲的建筑队列";
// 				break;
// 			}
// 			case Packet.BUILDING_RET.BUILDING_RET_BUILDING_STATUS:
// 			{
// 				strText = "建筑中，不可操作";
// 				break;
// 			}
// 			case Packet.BUILDING_RET.BUILDING_RET_MAX_LEV:
// 			{
// 				strText = "最大等级限制";
// 				break;
// 			}
// 			case Packet.BUILDING_RET.BUILDING_RET_IN_QUEUE:
// 			{
// 				strText = "已在建筑队列中";
// 				break;
// 			}
// 			case Packet.BUILDING_RET.BUILDING_RET_NOT_IN_QUEUE:
// 			{
// 				strText = "不在建筑队列中";
// 				break;
// 			}
// 			case Packet.BUILDING_RET.BUILDING_RET_UNMOVEABLE:
// 			{
// 				strText = "不能移动";
// 				break;
// 			}
// 			case Packet.BUILDING_RET.BUILDING_RET_BUILD_MORE:
// 			{
// 				strText = "无法创建更多建筑";
// 				break;
// 			}
// 			case Packet.BUILDING_RET.BUILDING_RET_ACC_UP_LEV_FAIL:
// 			{
// 				strText = "升级加速失败";
// 				break;
// 			}
// 			case Packet.BUILDING_RET.BUILDING_RET_CANC_UPLEV_FAIL_BUILD:
// 			{
// 				strText = "建筑创建不能取消";
// 				break;
// 			}
// 			case Packet.BUILDING_RET.BUILDING_RET_LEVY_TIME_ERROR:
// 			{
// 				strText = "征收时间错误已重置为当前时间";
// 				break;
// 			}
// 			case Packet.BUILDING_RET.BUILDING_RET_LEVY_TIME_LESS_ONE_MIN:
// 			{
// 				strText = "征收时间不足一分钟";
// 				break;
// 			}
// 			case Packet.BUILDING_RET.BUILDING_RET_LEVY_FAIL_BUILD:
// 			{
// 				strText = "建筑建设中无法征收";
// 				break;
// 			}
// 			default:
// 				break;
// 			}
// 			
// 			if ("" != strText)
// 			{
// 				SLG.GlobalEventSet.FireEvent(SLG.eEventType.ShowError, PanelID.MainPanel, new SLG.EventArgs(strText));
// 			}
// 		}
        
        public Dictionary<int, BuildConfig> m_buildConfigList = new Dictionary<int, BuildConfig>();

        private bool LoadBuildingCfg()
        {
            if (m_buildConfigList.Count > 0)
            {
                return true;
            }
            
            ConfigBase buildingCfg = DataMgr.DataManager.getConfig(CONFIG_MODULE.CFG_BUILDING_BASE);
            ConfigBase levelCfg = DataMgr.DataManager.getConfig(CONFIG_MODULE.CFG_BUILDING_LEVEL);

            if (buildingCfg == null || levelCfg == null)
            {
                return false;
            }

            foreach (ConfigRow cr in buildingCfg.rows)
            {
                BuildConfig bc = new BuildConfig();

                bc.id = cr.getIntValue(CFG_BUILDING.BUILDING_TYPEID);
                bc.name = cr.getIntValue(CFG_BUILDING.NAME);
                //bc.type = cr.getEnumValue<BuildType>(CFG_BUILDING.CLASSIFY, BuildType.Null);
                bc.type = cr.getIntValue(CFG_BUILDING.CLASSIFY);
                bc.text = cr.getIntValue(CFG_BUILDING.DESCRIPTION);
                bc.maxLevel = cr.getIntValue(CFG_BUILDING.MAX_LEVEL);
                bc.maxCout = cr.getIntValue(CFG_BUILDING.MAX_COUT);
                bc.sizeX = cr.getFloatValue(CFG_BUILDING.SIZE_X);
                bc.sizeY = cr.getFloatValue(CFG_BUILDING.SIZE_Y);
                bc.icon = cr.getStringValue(CFG_BUILDING.ICON_FILE);
                bc.prefabs = cr.getStringValue(CFG_BUILDING.FBX_FILE);

                int nMove = cr.getIntValue(CFG_BUILDING.MOVABLE);

                if (nMove == 0)
                {
                    bc.isMove = false;
                }
                else
                {
                    bc.isMove = true;
                }

                Pos2D pos1 = new Pos2D(0, 0);
                pos1.posX = cr.getFloatValue(CFG_BUILDING.POSX1);
                pos1.posY = cr.getFloatValue(CFG_BUILDING.POSY1);

                Pos2D pos2 = new Pos2D(0, 0);
                pos2.posX = cr.getFloatValue(CFG_BUILDING.POSX2);
                pos2.posY = cr.getFloatValue(CFG_BUILDING.POSY2);

                Pos2D pos3 = new Pos2D(0, 0);
                pos3.posX = cr.getFloatValue(CFG_BUILDING.POSX3);
                pos3.posY = cr.getFloatValue(CFG_BUILDING.POSY3);

                Pos2D pos4 = new Pos2D(0, 0);
                pos4.posX = cr.getFloatValue(CFG_BUILDING.POSX4);
                pos4.posY = cr.getFloatValue(CFG_BUILDING.POSY4);

                Pos2D pos5 = new Pos2D(0, 0);
                pos5.posX = cr.getFloatValue(CFG_BUILDING.POSX5);
                pos5.posY = cr.getFloatValue(CFG_BUILDING.POSY5);

                bc.pos[0] = pos1;
                bc.pos[1] = pos2;
                bc.pos[2] = pos3;
                bc.pos[3] = pos4;
                bc.pos[4] = pos5;

                ConfigRow[] crs = levelCfg.getRows(CFG_BUILDING_LEVEL.BUIDING_TYPEID, bc.id);
                int count = crs.Length;
                bc.levels = new Level[count + 1];

                for (int i = 0; i < count; ++i)
                {
                    Level level = new Level();

                    level.level = crs[i].getIntValue(CFG_BUILDING_LEVEL.LEVEL);
                    level.mainLevel = crs[i].getIntValue(CFG_BUILDING_LEVEL.CASTLE_LEVEL);
                    level.money = crs[i].getIntValue(CFG_BUILDING_LEVEL.MONEY);
                    level.magicStone = crs[i].getIntValue(CFG_BUILDING_LEVEL.MAGICSTONE);
                    level.time = crs[i].getFloatValue(CFG_BUILDING_LEVEL.COST_TIME);
                    level.data = new int[3];

                    level.data[0] = crs[i].getIntValue(CFG_BUILDING_LEVEL.DATA0);
                    level.data[1] = crs[i].getIntValue(CFG_BUILDING_LEVEL.DATA1);
                    level.data[2] = crs[i].getIntValue(CFG_BUILDING_LEVEL.DATA2);

                    bc.levels[level.level] = level;
                }

                if (m_buildConfigList.ContainsKey(bc.id))
                {
                    m_buildConfigList.Remove(bc.id);
                }

                m_buildConfigList.Add(bc.id, bc);
            }

            return true;
        }

        public BuildConfig GetBuildConfig(int id)
        {
            BuildConfig config = null;
            if (m_buildConfigList.TryGetValue(id, out config) == false)
                return null;

            return config;
        }

        public int GetLordHallLevel()
        {

            foreach (KeyValuePair<uint, Build> kvp in m_dicBuilding)
            {
                if (kvp.Value.m_idBuildingType == 30000)
                {
                    return kvp.Value.m_cbLev;
                }
            }
            
            return 1;
        }
	}
}
