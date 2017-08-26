using System;
using UnityEngine;
using System.Collections.Generic;
using DataMgr;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
namespace AudioMgr
{
    //播放管理器
	public class AudioPlayer
	{
        enum LIST_TYPE
        {
            LIST_BGM = 0,
            LIST_BGS,
            LIST_UI,
            LIST_STATE,
            LIST_FIGHT_STEP,
            LIST_FIGHT_SPECIAL,
            LIST_FIGHT_HIT,
            LIST_SKILL_EXEC,
            LIST_SKILL_HIT,
        }

        GameObject _root = null;
        Dictionary<LIST_TYPE,AudioList> _typeList = new Dictionary<LIST_TYPE,AudioList>();
        bool _isBgmEnable = false;
        bool _isSeEnable = false;

        //监听对象
        protected AudioPlayer()
        {
            _typeList.Clear();
        }

        AudioList getAudioList(AudioItem item)
        {
            switch (item.audiotype)
            {
                case AUDIO_TYPE.BGM:
                    return initList(LIST_TYPE.LIST_BGM, 100, 1f);
                case AUDIO_TYPE.BGS:
                    return initList(LIST_TYPE.LIST_BGS, 100, 0.85f);
                case AUDIO_TYPE.UI:
                    return initList(LIST_TYPE.LIST_UI, 100, 0.70f);
                case AUDIO_TYPE.STATE:
                    return initList(LIST_TYPE.LIST_STATE, 100, 0.55f);
                case AUDIO_TYPE.FIGHT:
                    if (item.fightSubType == AUDIO_FIGHT_TYPE.STEP)
                        return initList(LIST_TYPE.LIST_FIGHT_STEP, 100, 0.40f);
                    else if(item.fightSubType == AUDIO_FIGHT_TYPE.SPECIAL)
                        return initList(LIST_TYPE.LIST_FIGHT_SPECIAL, 100, 0.4f);
                    else if(item.fightSubType == AUDIO_FIGHT_TYPE.HIT)
                        return initList(LIST_TYPE.LIST_SKILL_HIT, 100, 0.4f);
                    break;
                case AUDIO_TYPE.SKILL:
                    if (item.skillSubType == AUDIO_SKILL_TYPE.EXEC)
                        return initList(LIST_TYPE.LIST_SKILL_EXEC, 100, 0.4f);
                    else if (item.skillSubType == AUDIO_SKILL_TYPE.HIT)
                        return initList(LIST_TYPE.LIST_SKILL_HIT, 100, 0.4f);
                    break;
            }            
            return null;
        }

        private AudioList getList(LIST_TYPE type)
        {
            AudioList theList = null;
            if (!this._typeList.TryGetValue(type, out theList))
                return null;
            return theList;
        }

        private AudioList initList(LIST_TYPE index,int max,float ratio)
        {
            AudioList theList = null;
            if (!this._typeList.TryGetValue(index, out theList))
            {
                theList = new AudioList();
                theList.maxCount = max;
                theList.volumeRatio = ratio;
                this._typeList[index] = theList;
            }
            return theList;
        }

        public static AudioPlayer getPlayer()
        {
            AudioPlayer player = new AudioPlayer();
            if (player.init())
                return player;
            return null;
        }

        public bool init()
        {
            if (AudioCenter.me==null || AudioCenter.me.gameObject == null)
                return false;
            //找到监听器
            AudioListener[] theListen = GameObject.FindSceneObjectsOfType(typeof(AudioListener)) as AudioListener[];
            if (theListen != null || theListen.Length > 0)
                this._root = theListen[0].gameObject;
            else
            {
                this._root = Camera.current.gameObject;
                this._root.AddComponent<AudioListener>();
            }
            return true;
        }

        public bool isBgmEnable
        {
            get { return this._isBgmEnable; }
            set
            {
                if (this._isBgmEnable != value)
                {
                    AudioList theList = getList(LIST_TYPE.LIST_BGM);
                    if (theList != null)
                    {
                        if (!value)
                            theList.pause();
                        else
                            theList.resume();
                    }
                    _isBgmEnable = value;
                }
            }
        }

        public bool isSeEnable
        {
            get { return this._isSeEnable; }
            set
            {
                if (this._isSeEnable != value)
                {
                    AudioList theList = getList(LIST_TYPE.LIST_BGS);
                    if (theList != null)
                    {
                        if (!value)
                            theList.pause();
                        else
                            theList.resume();
                    }
                    theList = getList(LIST_TYPE.LIST_UI);
                    if (theList != null)
                    {
                        if (!value)
                            theList.pause();
                        else
                            theList.resume();
                    }
                    _isSeEnable = value;
                }
            }
        }
        
        //播放
        public bool play(AudioItem item,GameObject obj)
        {
            if (obj == null)
                obj = this._root;
            if (item == null || item.fileCount <= 0)
                return false;
            AudioList theList = getAudioList(item);
            if (theList == null)
                return false;
            this.isBgmEnable = this.isBgmEnable;
            this.isSeEnable = this.isSeEnable;
            if (!theList.addIem(item))
                return false;
            bool bRet = false;
            item.volume = AudioCenter.me.volume * theList.volumeRatio;
            if (item.dimension == DataMgr.AUDIO_DIMENSIONS.DIMEN_3D)
                bRet = item.play(obj);
            if (item.dimension == DataMgr.AUDIO_DIMENSIONS.DIMEN_2D)
                bRet = item.play(this._root);
            if (!bRet)
                theList.releaseItem(item);
            return bRet;
        }

        public void update()
        {
            foreach (KeyValuePair<LIST_TYPE, AudioList> item in this._typeList)
            {
                if (item.Value != null)
                    item.Value.update();
            }
        }

        public void pause()
        {
            foreach (KeyValuePair<LIST_TYPE, AudioList> item in this._typeList)
            {
                if (item.Value != null)
                    item.Value.pause();
            }
        }

        public void resume()
        {
            foreach (KeyValuePair<LIST_TYPE, AudioList> item in this._typeList)
            {
                if (item.Value != null)
                    item.Value.resume();
            }
        }

        public void release()
        {
            foreach (KeyValuePair<LIST_TYPE, AudioList> item in this._typeList)
            {
                if (item.Value != null)
                    item.Value.release();
            }
        }

        public float volume
        {
            set
            {
                foreach (KeyValuePair<LIST_TYPE, AudioList> item in this._typeList)
                {
                    if (item.Value != null)
                        item.Value.volume = value;
                }
            }
        }
	}
}
