using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CardItem
{
	#region defined variable
	public BaseCardItem mBaseData = new BaseCardItem();

	public int mnId = CConstance.INVALID_ID;
	public int mnLevel = CConstance.LEVEL_ID;
    public int mnStar = CConstance.DEFAULT_ID;
    public uint mnExp = CConstance.DEFAULT_ID;

    public int mnStatus = CConstance.INVALID_ID;
    public int mnWeapon = CConstance.INVALID_ID;

    public List<int> mlistSkillLv = new List<int>();
    public List<uint> mlistSkillExp = new List<uint>();

	[HideInInspector]
	public int mnIndex = CConstance.INVALID_ID;       // 对象索引

	//private CardData mClsDetail = null;
	#endregion

	#region defined variale function

	public int typeId
	{
		get
		{
           return mBaseData.typeId;
		}
	}

	#endregion


	public CardItem()
	{
		init();
	}

	public void init()
	{
		mBaseData.typeId = CConstance.INVALID_ID;
        mBaseData.quality = DataMgr.enQualityType.enQT_Copper;

		mnId = CConstance.INVALID_ID;
 		mnLevel = CConstance.LEVEL_ID;
        mnIndex = CConstance.INVALID_ID;

		//mClsDetail = null;
	}

	void updatDetail()
	{
		//mClsDetail = CsvConfigMgr.me.getDetailByTypeId(mBaseData.typeId);
	}
}
