using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
public class SkillState : MonoBehaviour {
    
    GameObject skill1;
    GameObject skill2;
    GameObject skill3;
    List<GameObject> skillList = new List<GameObject>();

    UISprite skillcover1;
    UISprite skillcover2;
    UISprite skillcover3;
    List<UISprite> coverList = new List<UISprite>();

    UISprite getsure1;
    UISprite getsure2;
    UISprite getsure3;
    List<UISprite> getsureList = new List<UISprite>();

    UILabel skillLable1;
    UILabel skillLable2;
    UILabel skillLable3;

    UISprite skillIcon1;
    UISprite skillIcon2;
    UISprite skillIcon3;
    List<UISprite> iconList = new List<UISprite>();

    bool isSkillCD = false;
    float timeSkill1 = 0;
    float timeSkill2 = 0;
    float timeSkill3 = 0;

    float timeRemain1 = 0;
    float timeRemain2 = 0;
    float timeRemain3 = 0;

    float convertTime = 1;
    float cumulativeTime = 0;
    bool isShowing = true;
    bool isStart = false;
    public bool isFlashGetsure = true;
    
    // Use this for initialization
	void Start () {
        skill1 = transform.FindChild("skill1").gameObject;
        skill2 = transform.FindChild("skill2").gameObject;
        skill3 = transform.FindChild("skill3").gameObject;

        skill1.SetActive(false);
        skill2.SetActive(false);
        skill3.SetActive(false);

        skillList.Add(skill1);
        skillList.Add(skill2);
        skillList.Add(skill3);

        skillcover1 = PanelTools.Find<UISprite>(skill1, "cover");
        skillcover2 = PanelTools.Find<UISprite>(skill2, "cover");
        skillcover3 = PanelTools.Find<UISprite>(skill3, "cover");

        coverList.Add(skillcover1);
        coverList.Add(skillcover2);
        coverList.Add(skillcover3);

        skillLable1 = PanelTools.Find<UILabel>(skill1, "Label");
        skillLable2 = PanelTools.Find<UILabel>(skill2, "Label");
        skillLable3 = PanelTools.Find<UILabel>(skill3, "Label");

        getsure1 = PanelTools.Find<UISprite>(skill1, "getsure");
        getsure2 = PanelTools.Find<UISprite>(skill2, "getsure");
        getsure3 = PanelTools.Find<UISprite>(skill3, "getsure");

        getsureList.Add(getsure1);
        getsureList.Add(getsure2);
        getsureList.Add(getsure3);

        skillIcon1 = PanelTools.Find<UISprite>(skill1, "icon");
        skillIcon2 = PanelTools.Find<UISprite>(skill2, "icon");
        skillIcon3 = PanelTools.Find<UISprite>(skill3, "icon");

        iconList.Add(skillIcon1);
        iconList.Add(skillIcon2);
        iconList.Add(skillIcon3);

        isStart = true;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (isFlashGetsure)
        {
            FlashGetsure();
        }
    }

    protected void FlashGetsure()
    {

        if (isShowing)
        {
            cumulativeTime += Time.deltaTime;
        }
        else
        {
            cumulativeTime -= Time.deltaTime;
        }

        if (cumulativeTime >= 1.5)
        {
            isShowing = false;
        }

        if (cumulativeTime <= 0)
        {
            isShowing = true;
        }
        
        for (int i = 0; i < 3; ++i)
        {
            if (coverList[i].fillAmount == 0)
            {
                getsureList[i].alpha = cumulativeTime / convertTime;
            }
            else
            {
                getsureList[i].alpha = 0;
            }
        }
    }


    public void ClearCDTime()
    {
        if (!isStart)
        {
            Start();
        }
        
        skillcover1.fillAmount = 0;
        skillcover2.fillAmount = 0;
        skillcover3.fillAmount = 0;
    }

    public void InitCDTime(int nIndex)
    {
        if (!isStart)
        {
            Start();
        }

        switch (nIndex)
        {
            case 0:
                skillcover1.fillAmount = 1;
                break;
            case 1:
                skillcover1.fillAmount = 1;
                break;
            case 2:
                skillcover1.fillAmount = 1;
                break;

        }
    }

    public void UpdateSkillCD(int index, float cdTime, float remainTime)
    {

        if (!isStart)
        {
            Start();
        }
        
        
        if (index < 0 || cdTime < 0)
        {
            return;
        }
        
        switch (index)
        {
            case 0:
                //skillcover1.fillAmount = 1;
                timeSkill1 = cdTime;
                timeRemain1 = remainTime;
                //skillLable1.text = timeRemain1.ToString() + "/" + timeSkill1.ToString();
                skillcover1.fillAmount = timeRemain1 / timeSkill1;
                break;
            case 1:
                //skillcover2.fillAmount = 1;
                timeSkill2 = cdTime;
                timeRemain2 = remainTime;
                //skillLable2.text = timeRemain2.ToString() + "/" + timeSkill2.ToString();
                skillcover2.fillAmount = timeRemain2 / timeSkill2;
                break;
            case 2:
                //skillcover3.fillAmount = 1;
                timeSkill3 = cdTime;
                timeRemain3 = remainTime;
                //skillLable3.text = timeRemain3.ToString() + "/" + timeSkill3.ToString();
                skillcover3.fillAmount = timeRemain3 / timeSkill3;
                break;
        }

        if (cdTime > 0)
        {
            isSkillCD = true;
        }
    }

//     public void UpdateSkillIcon(int index, Fight.SkillBase skill)
//     {
//         GameObject skillItem = null;
// 
//         switch (index)
//         {
//             case 0:
//                 skillItem = skill1;
//                 break;
//             case 1:
//                 skillItem = skill2;
//                 break;
//             case 2:
//                 skillItem = skill3;
//                 break;
//         }
// 
//         if (skillItem != null)
//         {
//             skillItem.SetActive(true);
//         }
//     }

    public void ClearSkillItems()
    {
        foreach(GameObject skill in skillList)
        {
            skill.SetActive(false);
        }
    }

    public void GrayIcon()
    {
        foreach (GameObject skill in skillList)
        {
            UISprite icon = PanelTools.Find<UISprite>(skill, "icon");
            icon.color = Color.black;
        }

        isFlashGetsure = false;

        for (int i = 0; i < 3; ++i)
        {
            getsureList[i].alpha = 0;
        }
    }

    public void GrayIconByIndex(int nIndex)
    {
        if (nIndex < 0 || nIndex >2)
        {
            return;
        }

        UISprite icon = PanelTools.Find<UISprite>(skillList[nIndex], "icon");
        icon.color = Color.black;

        for (int i = 0; i < 3; ++i)
        {
            getsureList[i].alpha = 0;
        }
    }

    public void RestoreIcon()
    {
        foreach (GameObject skill in skillList)
        {
            UISprite icon = PanelTools.Find<UISprite>(skill, "icon");
            icon.color = Color.white;
        }

        isFlashGetsure = true;
    }

    public void SetSkillIcon(int nIndex, string strIcon)
    {
        if (!isStart)
        {
            Start();
        }

        if (nIndex < 3 && nIndex >= 0)
        {
            skillList[nIndex].SetActive(true);
            iconList[nIndex].spriteName = strIcon;
        }


    }

    public void SetGetsure(int nIndex, string strIcon)
    {
        if (!isStart)
        {
            Start();
        }

        if (nIndex < 3 && nIndex >= 0)
        {
            getsureList[nIndex].spriteName = strIcon;
        }
    }

    //测试
    void OnClick()
    {
    }
}
