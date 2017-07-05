using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SkillShow;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
public class CBattleFightGestureData
{
    public enSimpleGraphicsType enType;
    public List<Vector3> listVPt = new List<Vector3>();
    public float fRadius = 0f;
    public float fAngle = 0f;

    public CBattleFightGestureData(enSimpleGraphicsType en, Vector3 vOrigin, float fRad)
    {
        this.enType = en;
        this.listVPt.Add(vOrigin);
        this.fRadius = fRad;
    }
}


public class SkillShowMgr : MonoBehaviour 
{
    public MyCamera camera = null;
    public UIPopupList heroList = null;
    public UIPopupList effectList = null;
    public UIButton execBtn = null;
    public HreoObjMgr heroObjMgr = null;
    public UISlider timeSliderExec = null;
    public UILabel timeLableExec = null;
    public UISlider timeSliderCold = null;
    public UILabel timeLableCold = null;

    public UIToggle chkRound = null;
    public UIToggle chkRect = null;
    public UIToggle chkFan = null;

    // 0:simple Tap  1: double Tap   2: longTap
    public List<UIToggle> listChkTap = new List<UIToggle>();

    public UIToggle chkAttack = null;
    public UIToggle chkSkill1 = null;
    public UIToggle chkSkill2 = null;
    public UIToggle chkSkill3 = null;


    public GameObject selSkill = null;
    public GameObject selRang = null;
    public GameObject skillInfo = null;
    public GameObject roundObj = null;
    public GameObject rectObj = null;
    public GameObject fanObj = null;

    public UIButton resetBtn = null;
    public UIButton saveBtn = null;

    public UISlider circleSlider = null;
    public UILabel circleRaduisText = null;

    public UISlider rectWSlider = null;
    public UILabel rectWText = null;
    public UISlider rectHSlider = null;
    public UILabel rectHText = null;

    public UISlider fanAngleSlider = null;
    public UILabel fanAngleText = null;
    public UISlider fanRadiusSlider = null;
    public UILabel fanRadiusText = null;

    Hero mCurHero = null;
    GameObject theHero = null;
    DrawMapLine mapLine = null;

    public TextAsset SkillCfgFile = null;

    public UILabel lblTouch = null;
    public UIButton btnTouch = null;

    SkillShow.ConfigMgr mCfgMgr;

    public bool isStartGesture = false;
    private bool isLastRecordModel = false;
    public bool isMobleModel = false;
    public GameObject goUIRoot = null;
    List<Transform> listTF = new List<Transform>();

    // 实际攻击
    private Dictionary<int, VectorLine> lineMgr = new Dictionary<int, VectorLine>();
    List<Vector2> listPoints = new List<Vector2>();

    UILabel labelSkill = null;
    UILabel labelCurSkill = null;

    GameObject mgoGround = null;


    void Awake()
    {
        mCfgMgr = new SkillShow.ConfigMgr();
        mCfgMgr.init(SkillCfgFile);
        heroList.items.Clear();
        effectList.items.Clear();
    }

	void Start ()
    {
        chkSkill1.optionCanBeNone = true;
        chkSkill2.optionCanBeNone = true;
        chkSkill3.optionCanBeNone = true;
        chkAttack.optionCanBeNone = true;

        chkRound.optionCanBeNone = true;
        chkRect.optionCanBeNone = true;
        chkFan.optionCanBeNone = true;

        EventDelegate.Add(heroList.onChange, onSelHero);
        EventDelegate.Add(effectList.onChange, onSelEffect);

        EventDelegate.Add(circleSlider.onChange, onRangchg);
        EventDelegate.Add(execBtn.onClick, execSkill);

        EventDelegate.Add(chkSkill1.onChange, onSkill1);
        EventDelegate.Add(chkSkill2.onChange, onSkill2);
        EventDelegate.Add(chkSkill3.onChange, onSkill3);
        EventDelegate.Add(chkAttack.onChange, onAttack);

        EventDelegate.Add(chkRound.onChange, onRound);
        EventDelegate.Add(chkRect.onChange, onRect);
        EventDelegate.Add(chkFan.onChange, onFan);

        EventDelegate.Add(listChkTap[0].onChange, onRound);
        EventDelegate.Add(listChkTap[1].onChange, _OnSelectDouble);
        EventDelegate.Add(listChkTap[2].onChange, _OnSelectLong);

        EventDelegate.Add(rectWSlider.onChange, onRectSlide);
        EventDelegate.Add(rectHSlider.onChange, onRectSlide);

        EventDelegate.Add(fanAngleSlider.onChange, onFanSlide);
        EventDelegate.Add(fanRadiusSlider.onChange, onFanSlide);

        EventDelegate.Add(resetBtn.onClick, onReset);
        EventDelegate.Add(saveBtn.onClick, onSave);

        EventDelegate.Add(btnTouch.onClick, onTouchSwitch);

        initHeroList();
        initEffect();

        this.lblTouch.text = "启用手势";

        //SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_Line, onTouchAttack);
        //SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_Arc, onTouchSkill1);
        //SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_Circle, onTouchSkill2);

        //SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_SingleTap, onTouchAttack);
        //SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_DoubleTap, onTouchSkill1);
        //SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_LongTap, onTouchSkill2);

        SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_SingleTap, _OnTapSimpleAttack);
        SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_DoubleTap, _OnTapDoubleSkill);
        SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_LongTap, _OnTapLongSkill);

        SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_Line, _OnTapLineSkill);
        SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_Arc, _OnTapArcSkill);
        SimpleTouch.me.addAddHandle(enSimpleGraphicsType.enSGT_Circle, _OnTapCircleSkill);


        SimpleTouch.me.Enable = false;
        //this.isStartGesture = SimpleTouch.me.Enable;
        _UpdateGesture();

        //this.isMobleModel = false;
        Transform tf = UICardMgr.findChild(this.goUIRoot.transform, "resetBtn");
        if (tf != null)
        {
            listTF.Add(tf);
        }
        tf = UICardMgr.findChild(this.goUIRoot.transform, "saveBtn");
        if (tf != null)
        {
            listTF.Add(tf);
        }
        tf = UICardMgr.findChild(this.goUIRoot.transform, "selSkill");
        if (tf != null)
        {
            listTF.Add(tf);
        }
        //tf = UICardMgr.findChild(this.goUIRoot.transform, "viewBtn");
        //if (tf != null)
        //{
        //    listTF.Add(tf);
        //}

        this.labelSkill = UICardMgr.FindChild<UILabel>(this.goUIRoot.transform, "CueHaveSkill");
        this.labelCurSkill = UICardMgr.FindChild<UILabel>(this.goUIRoot.transform, "CurSkill");

        _UpdateRootVisible();

        mgoGround = GameObject.Find("TmpTerrain");

        //Vector3 vTo=Vector3.zero;
        //float a = Vector3.Angle(mCurHero.transform.position, vTo);
        //float fRotateSpeed = a / 2f;
        //float fCurRotat = fRotateSpeed * Time.deltaTime;
        //ewRotation = Quaternion.Lerp(current, newRotation, angleValue / totalAngle);
        //mCurHero.transform.rotation = 
    }

    void LateUpdate()
    {
        if (mCurHero == null)
        {
            return;
        }

        //Vector3 _offset = new Vector3(0, 0, -m_distance);
        //_offset = Quaternion.Euler(m_targetPitch, m_targetYaw, 0) * _offset;

        //Vector3 lastPosition = camera.transform.position;
        //Quaternion lastRotation = camera.transform.rotation;

        //camera.transform.position = m_bindTarget.position + _offset;
        //camera.transform.LookAt(m_bindTarget.position);

        //newPostion = camera.transform.position;
        //newRotation = camera.transform.rotation;

        // camera.transform.rotation = lastRotation;

        //Quaternion qCur = mCurHero.transform.rotation;
        //qCur.y += 1.5f;
        //qCur.x += 1.5f;
        //qCur.z += 1.5f;
        //Quaternion qNew = qCur;
        //qNew = Quaternion.Lerp(qCur, qNew, 0.5f);
        //mCurHero.transform.rotation = qNew;
    }

    void _UpdateRootVisible()
    {
        foreach (Transform item in listTF)
        {
            item.gameObject.SetActive(!this.isMobleModel);
        }

        this.labelSkill.enabled = this.isMobleModel;
        this.labelCurSkill.enabled = this.isMobleModel;

        this.isLastRecordModel = this.isMobleModel;
    }

    bool onTouchAttack(object objParam)
    {
        CBattleFightGestureData bfgd = (CBattleFightGestureData)objParam;
        this.labelCurSkill.text += bfgd.enType.ToString();
        this.labelCurSkill.text += ", SkillName:Skill02";

        this.listPoints.Clear();
        SimpleTouch.me.getTrailPoint(ref this.listPoints);

        chkSkill1.value = false;
        chkSkill2.value = false;
        chkSkill3.value = false;
        chkAttack.value = true;

        onAttack();
        this.execSkill();
        return true;
    }

    bool onTouchSkill1(object objParam)
    {
        CBattleFightGestureData bfgd = (CBattleFightGestureData)objParam;
        this.labelCurSkill.text += bfgd.enType.ToString();
        this.labelCurSkill.text += ", SkillName:Skill01";

        this.listPoints.Clear();
        SimpleTouch.me.getTrailPoint(ref this.listPoints);

        chkSkill1.value = true;
        chkSkill2.value = false;
        chkSkill3.value = false;
        chkAttack.value = false;
        onSkill1();
        this.execSkill();
        return true;
    }

    bool onTouchSkill2(object objParam)
    {
        CBattleFightGestureData bfgd = (CBattleFightGestureData)objParam;
        this.labelCurSkill.text += bfgd.enType.ToString();
        this.labelCurSkill.text += ", SkillName:Skill02";

        this.listPoints.Clear();
        SimpleTouch.me.getTrailPoint(ref this.listPoints);

        chkSkill1.value = false;
        chkSkill2.value = true;
        chkSkill3.value = false;
        chkAttack.value = false;

        onSkill2();
        this.execSkill();
        return true;
    }

    void onTouchSwitch()
    {
        if (!this.camera.enable)
        {
            this.lblTouch.text = "启用手势";
            this.camera.enable  = true;

            SimpleTouch.me.Enable = false;
        }
        else
        {
            this.lblTouch.text = "关闭手势";
            this.camera.enable = false;

            SimpleTouch.me.Enable = true;
        }

        this.isStartGesture = SimpleTouch.me.Enable;
    }

    void _UpdateGesture()
    {
        SimpleTouch.me.Enable = this.isStartGesture;
        if (this.isStartGesture)
        {
            this.lblTouch.text = "关闭手势";
            this.camera.enable = false;
        }
        else
        {
            this.lblTouch.text = "启用手势";
            this.camera.enable = true;
        }
    }

    void onReset()
    {
        mCfgMgr.init(SkillCfgFile);
        heroList.items.Clear();
        initHeroList();
    }

    void onSave()
    {
        mCfgMgr.save();
    }

    void showSelSkill(bool flag = false)
    {
        selSkill.active = flag;
        chkSkill1.value = false;
        chkSkill2.value = false;
        chkSkill3.value = false;
        chkAttack.value = false;
    }


    void initEffect()
    {
        if (effectList == null)
            return;

        foreach (HreoObjMgr.KeyValue item in heroObjMgr.effectPrepList)
        {
            effectList.items.Add(item.key);
        }
    }

    void onSelEffect()
    {
        if (mCurHero == null || mCurHero.skill == null)
            return;
        mCurHero.skill.strEffect = effectList.selection;
    }

    bool _OnCheckBtnByGesture(string strGesture)
    {
        if (strGesture == "1")
        {
            this.listChkTap[0].value = true;
        }
        else if (strGesture == "2")
        {
            this.listChkTap[1].value = true;
        }
        else if (strGesture == "3")
        {
            this.listChkTap[2].value = true;
        }
        else if (strGesture == "4")
        {
            this.chkRect.value = true;
        }
        else if (strGesture == "5")
        {
            this.chkFan.value = true;
        }
        else if (strGesture == "6")
        {
            chkRound.value = true;
        }
        else
            return false;

        return true;
    }

    void showSelRang(bool flag= false,int nType =0)
    {
        selRang.active = flag;

        chkRound.value = false;
        chkRect.value = false;
        chkFan.value = false;
        if (flag)
        {
            execBtn.isEnabled = flag;
            if (nType == SkillShow.ConfigMgr.RANG_FAN)
            {
                if (!_OnCheckBtnByGesture(mCurHero.skill.strGesture))
                    chkFan.value = true;
                showRangInfo(true, nType);
            }
            else if (nType == ConfigMgr.RANG_RECT)
            {
                if (!_OnCheckBtnByGesture(mCurHero.skill.strGesture))
                    chkRect.value = true;
                showRangInfo(true, nType);
            }
            else if (nType == ConfigMgr.RANG_ROUND)
            {
                if(!_OnCheckBtnByGesture(mCurHero.skill.strGesture))
                    chkRound.value = true;
                showRangInfo(true, nType);
            }
            else
            {
                bool bHave = true;
                if (mCurHero != null && mCurHero.skill != null)
                {
                    // double
                    if (mCurHero.skill.strGesture == "2" && this.listChkTap.Count >= 2)
                    {
                        nType = ConfigMgr.RANG_ROUND;
                        _OnCheckBtnByGesture(mCurHero.skill.strGesture);
                    }
                    // long
                    else if (mCurHero.skill.strGesture == "3" && this.listChkTap.Count >= 2)
                    {
                        nType = ConfigMgr.RANG_ROUND;
                        _OnCheckBtnByGesture(mCurHero.skill.strGesture);
                    }
                    else
                    {
                        bHave = false;
                    }
                }

                if (!bHave)
                    showRangInfo(false);
                else
                {
                    showRangInfo(true, nType);
                }
            }
        }
        else
        {
            if (mCurHero != null)
                mCurHero.skill = null;
        }
    }

    void showRangInfo(bool flag = false,int nType = 0)
    {
        if (mCurHero != null)
            mCurHero.release();

        showRound(flag &&(nType == SkillShow.ConfigMgr.RANG_ROUND));
        showRect(flag &&(nType == SkillShow.ConfigMgr.RANG_RECT));
        showFan(flag && (nType == SkillShow.ConfigMgr.RANG_FAN));
    }

    void showRound(bool flag = false)
    {
        if (mCurHero == null)
            return;

        roundObj.active = flag;
		circleSlider.enabled = flag;
		circleRaduisText.enabled = flag;

        if (!flag || mCurHero.skill == null)
            return;
        circleSlider.value = mCurHero.skill.fRang2 / SkillShow.ConfigMgr.MAX_RANG;
        circleRaduisText.text = "半径:   " + mCurHero.skill.fRang2;
        mCurHero.drawCircle(mCurHero.skill.fRang2);
    }

    void showRect(bool flag = false)
    {
        if (mCurHero == null)
            return;

        rectObj.active = flag;
		rectHSlider.enabled = flag;
		rectHText.enabled = flag;
		rectWSlider.enabled = flag;
		rectWText.enabled = flag;

        if (!flag || mCurHero.skill == null)
            return;
        rectWText.text = "长： " + mCurHero.skill.fRang1;
        rectHText.text = "宽： " + mCurHero.skill.fRang2;
        rectWSlider.value = mCurHero.skill.fRang1 / SkillShow.ConfigMgr.MAX_RANG;
        rectHSlider.value = mCurHero.skill.fRang2 / SkillShow.ConfigMgr.MAX_RANG;
        mCurHero.drawRect(mCurHero.skill.fRang1, mCurHero.skill.fRang2);
    }

    void onRectSlide()
    {
        mCurHero.skill.fRang1 = rectWSlider.value * SkillShow.ConfigMgr.MAX_RANG;
        rectWText.text = "长:   " + mCurHero.skill.fRang1;
        mCurHero.skill.fRang2 = rectHSlider.value * SkillShow.ConfigMgr.MAX_RANG;
        rectHText.text = "宽:   " + mCurHero.skill.fRang2;
        if (mCurHero != null)
            mCurHero.drawRect(mCurHero.skill.fRang1, mCurHero.skill.fRang2);
    }

    void showFan(bool flag = false)
    {
        if (mCurHero == null)
            return;

        fanObj.active = flag;
		fanAngleSlider.enabled = flag;
		fanAngleText.enabled = flag;
		fanRadiusSlider.enabled = flag;
		fanRadiusText.enabled = flag;

        if (!flag || mCurHero.skill == null)
            return;
        fanAngleText.text = "角度： " + mCurHero.skill.fRang1;
        fanRadiusText.text = "半径： " + mCurHero.skill.fRang2;
        fanAngleSlider.value = mCurHero.skill.fRang1 / 180;
        fanRadiusSlider.value = mCurHero.skill.fRang2 / SkillShow.ConfigMgr.MAX_RANG;
        mCurHero.drawFan(mCurHero.skill.fRang1, mCurHero.skill.fRang2);
    }

    void onFanSlide()
    {
        mCurHero.skill.fRang1 = fanAngleSlider.value * 180;
        fanAngleText.text = "角度:   " + mCurHero.skill.fRang1;
        mCurHero.skill.fRang2 = fanRadiusSlider.value * SkillShow.ConfigMgr.MAX_RANG;
        fanRadiusText.text = "半径:   " + mCurHero.skill.fRang2;

        if (mCurHero != null)
            mCurHero.drawFan(mCurHero.skill.fRang1, mCurHero.skill.fRang2);
    }

    void Update()
    {
        if (this.isLastRecordModel != this.isMobleModel)
        {
            _UpdateRootVisible();
        }

        if (this.isStartGesture != SimpleTouch.me.Enable)
        {
            _UpdateGesture();
        }

		execBtn.enabled = false;
        if (mCurHero == null)
            return;
        if(mCurHero.skill==null)
            return;

        timeLableCold.text = "冷却时长：" + mCurHero.skill.fColdTime + " 秒";
        timeLableExec.text = "技能时长：" + mCurHero.skill.fExecTime + " 秒";
        if (timeSliderCold != null && timeSliderExec != null)
        {
            switch (mCurHero.skill.state)
            {
            case SKillState.WAITING:
				execBtn.enabled = true;
            	break;
            case SKillState.COLDING:
                if (mCurHero.skill.fColdTime <= (Time.time - mCurHero.skill.fStateTime))
                {
                    mCurHero.skill.state = SKillState.WAITING;
                    timeSliderCold.value = 1;
                }
                else
                    timeSliderCold.value = (Time.time - mCurHero.skill.fStateTime) / mCurHero.skill.fColdTime;
                break;
            case SKillState.EXECING:
                if (mCurHero.skill.fExecTime <= (Time.time - mCurHero.skill.fStateTime))
                {
                    _ClenaPoint();

                    mCurHero.stopSkill();
                    mCurHero.skill.state = SKillState.COLDING;
                    timeSliderExec.value = 1;
                }
                else
                    timeSliderExec.value = (Time.time - mCurHero.skill.fStateTime) / mCurHero.skill.fExecTime;
                break;                        
            }
        }

        _DrawPoint();
    }

    void _DrawPoint()
    {
        if (this.listPoints.Count == 0)
            return;

        if (mCurHero == null)
            return;

        float fy = mCurHero.transform.position.y;
        Vector2[] pos_array = this.listPoints.ToArray();
        int count = pos_array.Length;
        int nIndex = lineMgr.Count - 1;
        for (int i = 0; i < count - 1; ++i)
        {
            Vector2[] line = new Vector2[2];
            line[0] = pos_array[i];
            //line[0].z = fy;
            line[1] = pos_array[i + 1];
            //line[1].z = fy;

            nIndex = lineMgr.Count - 1;
            string str = string.Format("oooxxxTempLine{0}", nIndex);
            VectorLine draw_line = new VectorLine(str, line, Color.blue, null, 1.4f);
            Vector.DrawLine(draw_line);
            lineMgr.Add(lineMgr.Count - 1, draw_line);
        }


    }

    void DrawLine(Vector2 pos1, Vector2 pos2, Color color, float width = 1.4f)
    {
        Vector2[] line = new Vector2[2];
        line[0] = pos1;
        line[1] = pos2;

        int nIndex = lineMgr.Count - 1;
        string str = string.Format("oooxxxTempLine{0}", nIndex);
        VectorLine draw_line = new VectorLine(str, line, color, null, width);
        Vector.DrawLine(draw_line);
        lineMgr.Add(lineMgr.Count - 1, draw_line);
    }

    void _ClenaPoint()
    {
        foreach (KeyValuePair<int, VectorLine> item in lineMgr)
        {
            VectorLine line = item.Value;
            Vector.DestroyLine(ref line);
        }

        this.lineMgr.Clear();

        listPoints.Clear();

        this.labelCurSkill.text = "CurSKill: ";
    }

    void initHeroList()
    {
        if (heroList == null)
            return;

        foreach (HreoObjMgr.KeyValue item in heroObjMgr.heroPrepList)
        {
            heroList.items.Add(item.key);
        }
        showSelSkill(false);
    }

    void onSelHero()
    {
        string strHeroName = heroList.selection;
        if(theHero!=null)
        {
            if (mCurHero != null)
                mCurHero.release();
            GameObject.Destroy(theHero);
        }
        if (heroObjMgr == null)
            return;
        theHero = heroObjMgr.getHreoObj(strHeroName);
        if (theHero != null)
        {
            this.camera.Target = theHero.transform;
            mapLine = theHero.AddComponent<DrawMapLine>();
            mCurHero = theHero.AddComponent<Hero>();

            this.labelSkill.text = "HaveSkills:";
            SkillShow.HeroSkill skillInfo = null;
            if (this.isMobleModel)
                skillInfo = mCfgMgr.getSkillName(strHeroName, "Skill01");
            else
                skillInfo = mCfgMgr.getSkill(strHeroName, "Skill01");  // edit model, is not finding, new skill
            if (skillInfo != null)
            {
                this.labelSkill.text += skillInfo.strSkill; // 1217152 
                this.labelSkill.text += ", ";
            }

            if (this.isMobleModel)
                skillInfo = mCfgMgr.getSkillName(strHeroName, "Skill02");
            else
                skillInfo = mCfgMgr.getSkill(strHeroName, "Skill02");
            if (skillInfo != null)
            {
                this.labelSkill.text += skillInfo.strSkill;
                this.labelSkill.text += ", ";
            }

            if (this.isMobleModel)
                skillInfo = mCfgMgr.getSkillName(strHeroName, "Skill03");
            else
                skillInfo = mCfgMgr.getSkill(strHeroName, "Skill03");
            if (skillInfo != null)
            {
                this.labelSkill.text += skillInfo.strSkill;
                this.labelSkill.text += ", ";
            }

            if (this.isMobleModel)
                skillInfo = mCfgMgr.getSkillName(strHeroName, "Attack");
            else
                skillInfo = mCfgMgr.getSkill(strHeroName, "Attack");
            if (skillInfo != null)
            {
                this.labelSkill.text += skillInfo.strSkill;
            }

            //this.labelSkill = ;
        }

        if (!this.isMobleModel)
        {
            showSelSkill(true);
            showSelRang(false);
        }
    }

    //划线
    void onRangchg()
    {
        mCurHero.skill.fRang2 = circleSlider.value * SkillShow.ConfigMgr.MAX_RANG;
        circleRaduisText.text = "半径:   " + mCurHero.skill.fRang2;
        if (mCurHero != null)
            mCurHero.drawCircle(mCurHero.skill.fRang2);
    }

    void execSkill()
    {
        if (mCurHero == null)
            return;
        if (mCurHero.skill != null)
        {
            mCurHero.showSkill();
            //特效
            GameObject effect = null;// heroObjMgr.getEffectObj(mCurHero.skill.strEffect);
            //if(effect!=null)    
            //    effect.transform.parent = theHero.transform;
            //mCurHero.effect = effect;

            effect = heroObjMgr.getEffectObj(mCurHero.skill.strEffect);
            if(effect!=null)
            {
                mCurHero.AttackPointEffect = effect;
            }
        }
    }

    void onSkill(string strSkill)
    {
        if (mCurHero == null)
            return;
        string strHero = heroList.value;
		execBtn.enabled = true;

        SkillShow.HeroSkill skillInfo = null;// mCfgMgr.getSkill(strHero, strSkill);
        if (this.isMobleModel)
            skillInfo = mCfgMgr.getSkillName(strHero, strSkill);
        else
            skillInfo = mCfgMgr.getSkill(strHero, strSkill);  // edit model, is not finding, new skill

        if (skillInfo != null)
        {
            mCurHero.skill = skillInfo;
            effectList.value = skillInfo.strEffect;
            showSelRang(true,skillInfo.rangType);
        }
        else
            showSelRang(false);
    }

    void onSelRang(int nType)
    {
        if (mCurHero != null)
            if (mCurHero.skill != null)
            {
                int n = 0;
                if(nType == ConfigMgr.RANG_ROUND)
                {
                    n = (int)enSimpleGraphicsType.enSGT_Circle;
                }
                else if(nType ==  ConfigMgr.RANG_FAN)
                {
                    n = (int)enSimpleGraphicsType.enSGT_Arc;
                }
                else if (nType == ConfigMgr.RANG_RECT)
                {
                    n = (int)enSimpleGraphicsType.enSGT_Line;
                }
                mCurHero.skill.strGesture = n.ToString();

                mCurHero.skill.rangType = nType;
            }

        showRangInfo(true, nType);
    }

    void onRect()
    {
        if (chkRect.value)
            onSelRang(ConfigMgr.RANG_RECT);
        else
            showRangInfo(false);
    }

    void onRound()
    {
        if (chkRound.value)
            onSelRang(ConfigMgr.RANG_ROUND);
        else
            showRangInfo(false);
    }

    void onFan()
    {
        if(chkFan.value)
            onSelRang(ConfigMgr.RANG_FAN);
        else
            showRangInfo(false);
    }

    void onSkill1()
    {
        if (chkSkill1.value)
            onSkill("Skill01");
        else
            showSelRang(false);

    }
    void onSkill2()
    {
        if (chkSkill2.value)
            onSkill("Skill02");
        else
            showSelRang(false);
    }
    void onSkill3()
    {
        if (chkSkill3.value)
            onSkill("Skill03");
        else
            showSelRang(false);
    }
    void onAttack()
    {
        if (chkAttack.value)
            onSkill("Attack");
        else
            showSelRang(false);
    }

    #region ------ tap call back function ------

    void _OnSelectSimple()
    {
        if (mCurHero == null)
            return;

        if (mCurHero.skill == null)
            return;

        if (this.listChkTap[0].value)
        {
            int n = (int)enSimpleGraphicsType.enSGT_SingleTap;
            mCurHero.skill.strGesture = n.ToString();
        }
    }

    void _OnSelectDouble()
    {
        if (mCurHero == null)
            return;

        if (mCurHero.skill == null)
            return;

        if (this.listChkTap[1].value)
        {
            int n = (int)enSimpleGraphicsType.enSGT_DoubleTap;
            mCurHero.skill.strGesture = n.ToString();
        }
    }

    void _OnSelectLong()
    {
        if (mCurHero == null)
            return;

        if (mCurHero.skill == null)
            return;

        if (this.listChkTap[2].value)
        {
            int n = (int)enSimpleGraphicsType.enSGT_LongTap;
            mCurHero.skill.strGesture = n.ToString();
        }
    }

    bool _OnTapSimpleAttack(object objParam)
    {
        onAttack();
        return true;
    }

    bool _OnTapDoubleSkill(object objParam)
    {
        if (mCurHero == null)
            return false;

        string strHero = heroList.value;//.selection;
        execBtn.enabled = true; //execBtn.active = true;
        string str = "2";
        SkillShow.HeroSkill skillInfo = mCfgMgr.getSkillByGesture(strHero, str);
        if (skillInfo != null)
        {
            //skillInfo.rangType = SkillShow.ConfigMgr.RANG_ROUND;
            mCurHero.skill = skillInfo;
            effectList.value = skillInfo.strEffect;
            showSelRang(true, skillInfo.rangType);
        }
        else
        {
            string strError = string.Format("无效手势技能 : {0}::{1}", this.GetType(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            Logger.LogError("{0}", strError);
            //throw new System.Exception(strError);
        }

        return true;
    }

    bool _OnTapLongSkill(object objParam)
    {
        if (mCurHero == null)
            return false;

        string strHero = heroList.value;//.selection;
        execBtn.enabled = true; //execBtn.active = true;
        string str = "3";
        SkillShow.HeroSkill skillInfo = mCfgMgr.getSkillByGesture(strHero, str);
        if (skillInfo != null)
        {
            //skillInfo.rangType = SkillShow.ConfigMgr.RANG_ROUND;
            mCurHero.skill = skillInfo;
            effectList.value = skillInfo.strEffect;
            showSelRang(true, skillInfo.rangType);
        }
        else
        {
            string strError = string.Format("无效手势技能 : {0}::{1}", this.GetType(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            Logger.LogError("{0}", strError);
            //throw new System.Exception(strError);
        }

        return true;
    }

    bool _OnTapLineSkill(object objParam)
    {
        if (mCurHero == null)
            return false;

        string strHero = heroList.value;//.selection;
        execBtn.enabled = true; //execBtn.active = true;
        string str = "4";
        SkillShow.HeroSkill skillInfo = mCfgMgr.getSkillByGesture(strHero, str);
        if (skillInfo != null)
        {

            skillInfo.rangType = SkillShow.ConfigMgr.RANG_RECT;
            mCurHero.skill = skillInfo;
            effectList.value = skillInfo.strEffect;
            showSelRang(true, skillInfo.rangType);
        }
        else
        {
            string strError = string.Format("无效手势技能 : {0}::{1}", this.GetType(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            Logger.LogError("{0}", strError);
            //throw new System.Exception(strError);
        }

        return true;
    }

    bool _OnTapArcSkill(object objParam)
    {
        if (mCurHero == null)
            return false;

        string strHero = heroList.value;//.selection;
        execBtn.enabled = true; //execBtn.active = true;
        string str = "5";
        SkillShow.HeroSkill skillInfo = mCfgMgr.getSkillByGesture(strHero, str);
        if (skillInfo != null)
        {
            skillInfo.rangType = SkillShow.ConfigMgr.RANG_FAN;
            mCurHero.skill = skillInfo;
            effectList.value = skillInfo.strEffect;
            showSelRang(true, skillInfo.rangType);
        }
        else
        {
            string strError = string.Format("无效手势技能 : {0}::{1}", this.GetType(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            Logger.LogError("{0}", strError);
            //throw new System.Exception(strError);
        }

        return true;
    }

    bool _OnTapCircleSkill(object objParam)
    {
        if (mCurHero == null)
            return false;

        string strHero = heroList.value;
        execBtn.enabled = true;
        string str = "6";
        SkillShow.HeroSkill skillInfo = mCfgMgr.getSkillByGesture(strHero, str);
        if (skillInfo != null)
        {
            skillInfo.rangType = SkillShow.ConfigMgr.RANG_ROUND;

            mCurHero.skill = skillInfo;
            effectList.value = skillInfo.strEffect;
            showSelRang(true, skillInfo.rangType);
        }
        else
        {
            string strError = string.Format("无效手势技能 : {0}::{1}", this.GetType(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            Logger.LogError("{0}", strError);
            //throw new System.Exception(strError);
        }

        if (objParam != null)
        {
            CBattleFightGestureData cbfgd = (CBattleFightGestureData)objParam;
            Vector3 v = cbfgd.listVPt[0];
            _TranformCamera(v);
        }

        return true;
    }

    MonoSwitchImp mMonoSwitch = null;
    void _TranformCamera(Vector3 v)
    {
        if (mMonoSwitch == null)
        {
            mMonoSwitch = this.GetComponent<MonoSwitchImp>();
        }

        try
        {
            v.y = 0;
            mMonoSwitch.Switching(v, this.transform.rotation, 2f);
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    #endregion

}
