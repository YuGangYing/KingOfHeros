using System;
using System.Collections.Generic;
using UnityEngine;
using AudioMgr;
using DataMgr;

public class AudioCenter : SingletonMonoBehaviourNoCreate<AudioCenter>
{
    AudioPlayer _curPlayer = null;
    AudioItem _loadItem = null;
    bool _isBgmEnable = false;
    bool _isSeEnable = false;

    float _volume;

    protected override void Init()
    {
        base.Init();
        this._isBgmEnable = DataManager.getSyscfg().getValue(SYSTEM_CFG.MUSIC);
        this._isSeEnable = DataManager.getSyscfg().getValue(SYSTEM_CFG.MUSIC);
        this.volume = 100;
    }

    //播放
    public AudioItem play(string strAudio,GameObject root=null)
    {
        if (this._curPlayer == null)
        {
            this._curPlayer = AudioPlayer.getPlayer();
            this._curPlayer.isBgmEnable = this._isBgmEnable;
            this._curPlayer.isSeEnable = this._isSeEnable;
        }
        if (this._curPlayer == null)
            return null;
        AudioItem newItem = AudioItem.getItem(strAudio);
        if(newItem==null)
            return null;
        if (!_curPlayer.play(newItem, root))
            return null;

        return newItem;
    }

    //施放。
    public void release()
    {
        if (_curPlayer != null)
            _curPlayer.release();
        this._curPlayer = null;
    }

    //播放加载音乐
    public void playLoadAudio()
    {
        if (this._loadItem == null)
            this._loadItem = AudioItem.getItem(AudioName.LOAD_BGM);
        if (_curPlayer != null)
        {
            this._curPlayer.release();
            //this._curPlayer.play(this._loadItem,null);
        }
    }

    //退出
    public void quit()
    {
        if(this._curPlayer!=null)
            this._curPlayer.release();
        this._curPlayer = null;
    }

    //开关背景音乐
    public bool isBgmEnable
    {
        get { return this._isBgmEnable; }
        set
        {
            if (this._curPlayer != null)
            {
                if (this._curPlayer != null)
                    this._curPlayer.isBgmEnable = value;
                this._isBgmEnable = value;
            }
        }
    }

    //开关背景音效
    public bool isSeEnable
    {
        get { return this._isSeEnable; }
        set 
        {
            if (this._curPlayer != null)
                this._curPlayer.isSeEnable = value;
            this._isSeEnable = value;
        }
    }

    //设置音量
    public float volume
    {
        get
        {
            return this._volume;
        }
        set
        {
            if (this._curPlayer != null)
                this._curPlayer.volume = value;
            this._volume = value;
        }
    }

    void Update()
    {
//        if (this._curPlayer != null)
//            this._curPlayer.update();
    }
}
