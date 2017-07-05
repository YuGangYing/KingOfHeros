using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataMgr;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
public class UICardMgr
{
	#region inline class
	public enum enItemEventType
	{
		enIET_Invalid = -1,
		enIET_Level = 0,
		enIET_Quality,
		enIET_Amount,
	}

	public class CItemData
	{
		public enItemEventType enEt;
		public int nId;
        public int nTypeId;
		public int nIndex;
		public GameObject goIcon;

		public CItemData()
		{
			enEt = enItemEventType.enIET_Invalid;
			nId = CConstance.INVALID_ID;
            nTypeId = CConstance.INVALID_ID;
			nIndex = CConstance.INVALID_ID;
			goIcon = null;
		}
	}

	#endregion

	#region singleton variable
	private static UICardMgr msInstance = null;
	public static UICardMgr singleton
	{
		get
		{
			if(msInstance == null)
				msInstance = new UICardMgr();
			return msInstance;
		}
	}

	#endregion

	#region attribute variable define

	// [物品详情界面]   [等级升级界面]   [品质升级界面]   [背景]  [拥有卡牌物品数据清单界面]
	[SerializeField]
	List<GameObject> mlistUIRoot = new List<GameObject>();
//	[SerializeField]
//	string mstrHaveViewListDataWindowName = "AnchorViewList";

    // 卡牌物品清单数据
    List<CItemData> mlistItem = new List<CItemData>();  // 英雄列表
    public int itemAmount { get { return mlistItem.Count; } }

    List<CItemData> mlistIllustrated = new List<CItemData>();  // 图鉴列表(曾经拥有过的英雄)
    public int illustratedItemAmount { get { return mlistIllustrated.Count;  } }

    private bool mbInit = false;

    public const int mNullId = 0x7000;
    public const int mDefauldTypeId = 10101;

	#endregion


	private UICardMgr()
	{
	}

    public void initial()
    {
        if (mbInit)
            return;

        loadFile();

        //testNetwork();
        //testAddItem();

        mbInit = true;
    }

	void loadFile()
	{

	}
	
    public bool addItem(CardItem ci, GameObject goHangParent, GameObject goChildPrefab,
        UICardLogic.enItemSource en = UICardLogic.enItemSource.enIS_HeroList)
    {
        if (goHangParent == null)
        {
            Logger.LogDebug("UICardMgr::addItem  goHangParent is null");
            return false;
        }
        else if (null == goChildPrefab)
        {
            Logger.LogDebug("UICardMgr::addItem  goChildPrefab is null");
            return false;
        }

        bool bRet = false;
        if (en == UICardLogic.enItemSource.enIS_HeroList)
        {
            if (ci.mnId < mNullId)
            {
                bRet = isHaveIllustratedById(ci.mnId);
                if (bRet)
                    return false;
            }
            else
                bRet = true;
//             bRet = HeroItemMgr.singleton.addCardItem(ci);
//             if (!bRet)
//                 return false;
        }
        else
        {
//             bRet = HeroItemMgr.singleton.addCardItem(ci);
//             if (!bRet)
//                 return false;
//             bRet = isHaveIllustratedById(ci.mnId);
//             if (bRet)
//                 return false;
        }

        CItemData clsid = new CItemData();
        clsid.nIndex = ci.mnIndex;
        clsid.nId = ci.mnId;
        clsid.nTypeId = ci.mBaseData.typeId;

        try
        {
            ConfigRow cr;
            CHerroTalbeAttribute.getHeroBaseDetail(clsid.nTypeId, out cr);
            if (cr == null)
            {
                Logger.LogDebug("UICardMgr::AddItem no find cvs field typid:" + clsid.nTypeId.ToString());
            }

            string strPrefabName = cr.getStringValue(enCVS_HERO_BASE_ATTRIBUTE.ICON_FILE);// cd.getAttributeStringValue(CardData.enAttributeName.enAN_IconFile);
            GameObject goPrefab = goChildPrefab;

            clsid.goIcon = NGUITools.AddChild(goHangParent, goPrefab);
            clsid.goIcon.transform.parent = goHangParent.transform;

            clsid.goIcon.AddComponent<UICardLogic>();

            if (en == UICardLogic.enItemSource.enIS_HeroIllustrated)
            {
//                 float fScale = 1.0f;// 0.65f;
//                 clsid.goIcon.transform.localScale = new Vector3(fScale, fScale, fScale);
            }

            UICardLogic uicl = clsid.goIcon.GetComponent<UICardLogic>();
            if (uicl != null)
            {
                uicl.menType = en;
                uicl.mnId = ci.mnId;
                uicl.mnIndex = ci.mnIndex;
                uicl.mnTypeId = ci.typeId;
            }

            if (en == UICardLogic.enItemSource.enIS_HeroList)
            {
                mlistItem.Add(clsid);
            }
            else if (en == UICardLogic.enItemSource.enIS_HeroIllustrated)
            {
                mlistIllustrated.Add(clsid);
            }
            
        }
        catch (System.Exception ex)
        {

        }

        return true;
    }

    IEnumerator AddLogicFile(float fWaitTime)
    {
        yield return new WaitForSeconds(0.5f);
    }

	public bool DelItem(int nIndexId)
	{
        bool bRet = false;
        Packet.HERO_INFO hi = new Packet.HERO_INFO();
        bRet = HeroData.getHeroData(nIndexId, ref hi);
        if (!bRet)
            return false;

        bRet = RemoveItemByIndex(nIndexId);

		return bRet;
	}

	bool RemoveItemByIndex(int nIndexId)
	{
		foreach (CItemData item in mlistItem)
		{
			if(item.nIndex == nIndexId)
			{
				GameObject.Destroy(item.goIcon);
				item.goIcon = null;

				mlistItem.Remove(item);
				return true;
			}
		}

		return false;
	}

    public bool RemoveItemById(int nId)
    {
        foreach (CItemData item in mlistItem)
        {
            if (item.nId == nId)
            {
                GameObject.Destroy(item.goIcon);
                item.goIcon = null;

                mlistItem.Remove(item);
                return true;
            }
        }

        return false;
    }

    public CItemData getItemByIndex(int nIndex)
    {
        if (nIndex < 0 || nIndex >= mlistItem.Count)
            return null;

        return mlistItem[nIndex];
    }

    public CItemData getItemById(int nId)
    {
        foreach (CItemData cid in mlistItem)
        {
            if (cid.nId == nId)
                return cid;
        }

        return null;
    }

    public bool getItemByTypeId(int nId, out CItemData cid)
    {
        cid = null;
        foreach (CItemData item in mlistItem)
        {
            if (item.nTypeId == nId)
            {
                cid = item;
                return true;
            }
        }

        return false;
    }

    public bool isHaveItemById(int nId)
    {
        foreach (CItemData cid in mlistItem)
        {
            if (cid.nId == nId)
                return true;
        }

        return false;
    }

    public bool isHaveIllustratedById(int nId)
    {
        foreach (CItemData cid in mlistIllustrated)
        {
            if (cid.nId == nId)
                return true;
        }

        return false;
    }

    public CItemData getIllustratedItemById(int nId)
    {
        foreach (CItemData cid in mlistIllustrated)
        {
            if (cid.nId == nId)
                return cid;
        }

        return null;
    }

    public CItemData getIllustratedItemByIndex(int nIndex)
    {
        if (nIndex < 0 || nIndex >= mlistIllustrated.Count)
            return null;

        return mlistIllustrated[nIndex];
    }

    public bool DelIllustratedItem(int nIndexId)
    {
        bool bRet = false;
        Packet.HERO_INFO hi = new Packet.HERO_INFO();
        bRet = HeroData.getHeroData(nIndexId, ref hi);
        if (!bRet)
            return false;

        bRet = RemoveIllustratedItemByIndex(nIndexId);

        return bRet;
    }

    bool RemoveIllustratedItemByIndex(int nIndexId)
    {
        foreach (CItemData item in mlistIllustrated)
        {
            if (item.nIndex == nIndexId)
            {
                GameObject.Destroy(item.goIcon);
                item.goIcon = null;

                mlistIllustrated.Remove(item);
                return true;
            }
        }

        return false;
    }

    public bool RemoveIllustratedItemById(int nId)
    {
        foreach (CItemData item in mlistIllustrated)
        {
            if (item.nId == nId)
            {
                GameObject.Destroy(item.goIcon);
                item.goIcon = null;

                mlistIllustrated.Remove(item);
                return true;
            }
        }

        return false;
    }

    public void cleanHeroItem()
    {
        do
        {
            if (mlistItem.Count == 0)
                break;

            CItemData cid = mlistItem[0];
            GameObject.Destroy(cid.goIcon);
            cid.goIcon = null;

            mlistItem.RemoveAt(0);
        } while (true);
    }

    public void cleanIlIllustratedItem()
    {
        do
        {
            if (mlistIllustrated.Count == 0)
                break;

            CItemData cid = mlistIllustrated[0];
            GameObject.Destroy(cid.goIcon);
            cid.goIcon = null;

            mlistIllustrated.RemoveAt(0);
        } while (true);
    }

    public void cleanItem()
    {
        cleanHeroItem();
        cleanIlIllustratedItem();
    }

    //---- 多个分支查找  分割符","
    public static Transform findChild(Transform tfParent, string strName)
    {
        if (strName == "")
            return null;

        if (tfParent == null)
        {
            //throw new System.Exception("UICardMgr::findChild  tfParent is null");
            //tfParent = Root.transform;
            return null;
        }

        string strTag = "";
        int nIndex = strName.IndexOf(",");
        if (nIndex != CConstance.INVALID_ID)
        {
            strTag = strName.Substring(0, nIndex);
            strName = strName.Substring(nIndex + 1);
        }
        else
        {
            strTag = strName;
        }

        tfParent = tfParent.FindChild(strTag);
        if (strTag == strName && nIndex == CConstance.INVALID_ID)
            return tfParent;
        else
        {
            return findChild(tfParent, strName);
        }
    }

    //---- 多个分支查找  分割符","
    public static T FindChild<T>(Transform tfParent, string strName) where T : Component
    {
        Transform tf = findChild(tfParent, strName);
        if (tf != null)
        {
            T temp = tf.GetComponent<T>();

            return temp;
        }

        return null;
    }

    public static GameObject findChild(GameObject goParent, string strTreeName)
    {
        if(goParent == null)
            return null;

        Transform tf = findChild(goParent.transform, strTreeName);
        if (tf == null)
            return null;

        return tf.gameObject;
    }

    public static T FindChild<T>(GameObject goParent, string strTreeName) where T : Component
    {
        if (goParent == null)
            return null;

        T t = FindChild<T>(goParent.transform, strTreeName);

        return t;
    }

    public static GameObject getCamera()
    {
        //return UICamera.FindCameraForLayer(Root.layer).gameObject;
        return GameObject.Find("UICamera");
    }

    public static void setLabelText(GameObject goParent, string strwidgetName, int nId)
    {
        if (goParent == null)
            return;

        UILabel ul = FindChild<UILabel>(goParent.transform, strwidgetName);
        if (null == ul)
            return;

        ul.text = DataMgr.DataManager.getLanguageMgr().getString(nId);
    }
}
