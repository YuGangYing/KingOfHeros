using System;
using System.Collections.Generic;
using DataMgr;
using UnityEngine;

namespace AudioMgr
{
	public class AudioItem
	{
        AudioSource _player = null;
        //播放状态
        enum PLAY_STATE 
        {
            WAIT = 0,
            PLAYING, //播放中
            COLDING, //冷却中
            FINISH,  //结束
            ERR,     //错误
        }
        ConfigRow _row = null;
        ConfigRow[] _fileList = null;

        //播放开始时间
        float _startTime = 0f;
        //播放序列
        int _curIndex = 0;
        //

        PLAY_STATE _status = PLAY_STATE.WAIT;
        

        static public AudioItem getItem(string strName)
        {
            AudioItem item = new AudioItem();
            if (item.init(strName))
                return item;
            else
                return null;
        }

        protected AudioItem()
        {
            this.release();
            this.status = PLAY_STATE.WAIT;
        }

        public bool isPause
        {
            get;
            set;
        }

        //读取配置
        bool init(string strName)
        {
            if (strName == null || strName == string.Empty)
                return false;
            ConfigBase config = DataManager.getConfig(CONFIG_MODULE.CFG_AUDIO);
            if (config == null)
                return false;

            _row = config.getRow(CFG_AUDIO.NAME,strName);
            if (_row == null)
                return false;

            ConfigBase fileConfig = DataManager.getConfig(CONFIG_MODULE.CFG_AUDIO_FILE);
            if (fileConfig == null)
                return false;

            this._fileList = fileConfig.getRows(CFG_AUDIO.AUDIO_ID,this._row.getIntValue(CFG_AUDIO.AUDIO_ID));
            if (this._fileList == null)
                return false;
            return true;
        }

        AudioClip getAudioClip(int index)
        {
            string strName = DataMgr.ResourceCenter.audioPath + this.getFileName(index);
            return DataMgr.ResourceCenter.LoadAsset<AudioClip>(strName);
        }

        void play(int index)
        {
            if (_player == null)
                return;

            AudioClip clip = this.getAudioClip(index);
            if (clip == null)
            {
                this.status = PLAY_STATE.WAIT;
                return;
            }
            this._player.playOnAwake = true;
            this._player.rolloffMode = AudioRolloffMode.Linear;
            this._player.loop = false;
            this._player.clip = clip;
            if(!this.isPause)//并不实际播放
                this._player.Play();
            if (this.getFadeTime > 0f)
                this._player.volume = 0f;
            this.status = PLAY_STATE.PLAYING;
            this._curIndex = index;
        }

        //获取下一个播放对象(决定循环和播放规则)
        int getNextIndex()
        {
            int index = 0;
            if (this._row == null)
                return index;
            AUDIO_SEQUENCE mode = this._row.getEnumValue<AUDIO_SEQUENCE>(CFG_AUDIO.SEQUENCE, AUDIO_SEQUENCE.UNKNOWN);
            int nLoop = this._row.getIntValue(CFG_AUDIO.AUTOLOOP,0);
            if (nLoop==1) //循环模式
            {
                if (this.fileCount == 1)
                    index = 1;
                else //多个时
                {
                    if (mode == AUDIO_SEQUENCE.RANDOM) //随机方式
                        index = UnityEngine.Random.Range(1, this.fileCount+1);
                    else if (mode == AUDIO_SEQUENCE.SERIAL)//串行
                    {
                        if (this._curIndex < 1 || this._curIndex >= this.fileCount)
                            index = 1;
                        else
                            index = _curIndex + 1;
                    }
                }
            }
            else if (this.status != PLAY_STATE.FINISH)//非循环只能按照顺序执行了,没有随机了
            {
                if (this._curIndex < 1)
                    index = 1;
                else if (this._curIndex >= this.fileCount)//播放完成了
                    this.status = PLAY_STATE.FINISH;
                else
                    index = _curIndex + 1;
            }
            return index;
        }

        public bool isFinish
        {
            get
            {
                if (this.status == PLAY_STATE.FINISH || this.status == PLAY_STATE.ERR)
                    return true;
                else
                    return false;
            }
        }
        //文件数
        public int fileCount
        {
            get
            {
                if(this._fileList==null)
                    return 0;
                return this._fileList.Length;
            }
        }

        public AUDIO_SEQUENCE sequence
        {
            get
            {
                if(_row==null)
                    return AUDIO_SEQUENCE.UNKNOWN;
                return _row.getEnumValue<AUDIO_SEQUENCE>(CFG_AUDIO.SEQUENCE,AUDIO_SEQUENCE.UNKNOWN);
            }
        }

        public bool autoLoop
        {
            get
            {
                if(this._row==null)
                    return false;
                return (_row.getIntValue(CFG_AUDIO.AUTOLOOP,0) ==1);
            }
        }

        public AUDIO_TYPE audiotype
        {
            get
            {
                if (this._row == null)
                    return AUDIO_TYPE.UNKNOWN;
                return this._row.getEnumValue<AUDIO_TYPE>(CFG_AUDIO.TYPE,AUDIO_TYPE.UNKNOWN);
            }
        }

        public AUDIO_SKILL_TYPE skillSubType
        {
            get
            {
                if (this.audiotype != AUDIO_TYPE.SKILL)
                    return AUDIO_SKILL_TYPE.UNKNOWN;
                return this._row.getEnumValue<AUDIO_SKILL_TYPE>(CFG_AUDIO.SUB_TYPE, AUDIO_SKILL_TYPE.UNKNOWN);
            }
        }

        public float interval
        {
            get 
            {
                if (this._row == null)
                    return 0f;
                return this._row.getFloatValue(CFG_AUDIO.INTERVAL,0);
            }
        }

        public AUDIO_FIGHT_TYPE fightSubType
        {
            get
            {
                if (this.audiotype != AUDIO_TYPE.FIGHT)
                    return AUDIO_FIGHT_TYPE.UNKNOWN;
                return this._row.getEnumValue<AUDIO_FIGHT_TYPE>(CFG_AUDIO.SUB_TYPE, AUDIO_FIGHT_TYPE.UNKNOWN);
            }
        }

        //播放时长
        public float getFadeTime
        {
            get
            {
                if (this._row == null)
                    return 0f;
                return this._row.getFloatValue(CFG_AUDIO.FADETIME,0f);
            }
        }

        public AUDIO_DIMENSIONS dimension
        {
            get
            {
                if (this._row == null)
                    return AUDIO_DIMENSIONS.UNKNOWN;
                return this._row.getEnumValue<AUDIO_DIMENSIONS>(CFG_AUDIO.DIMENSIONS, AUDIO_DIMENSIONS.DIMEN_2D);
            }
        }

        public float volume
        {
            get;
            set;
        }

        PLAY_STATE status
        {
            get { return this._status; }
            set
            {
                this._status = value;
                this._startTime = Time.time;
            }
        }

        //文件名
        public string getFileName(int index = 1)
        {
            if(this._fileList==null)
                return string.Empty;

            foreach(ConfigRow row in this._fileList)
            {
                if (row.getIntValue(CFG_AUDIO_FILE.INDEX) == index)
                    return row.getStringValue(CFG_AUDIO_FILE.FILE);
            }
            return string.Empty;
        }

        //播放时长
        public float getDuration(int index = 1)
        {
            if (this._fileList == null)
                return 0f;

            foreach (ConfigRow row in this._fileList)
            {
                if (row.getIntValue(CFG_AUDIO_FILE.INDEX) == index)
                    return row.getFloatValue(CFG_AUDIO_FILE.DURATION);
            }
            return 0f;
        }

        public void release()
        {
            stop();
            GameObject.Destroy(this._player);
            this._player = null;
            this._row = null;
            this._startTime = 0;
            this._fileList = null;
        }
        
        public bool play(GameObject root)
        {
            if (root == null)
                return false;
            this._player = root.AddComponent<AudioSource>();
            this.status = PLAY_STATE.WAIT;
            return true;
        }

        public void pause()
        {
            if (this._player != null)
                this._player.Pause();
        }

        public void resume()
        {
            if (this._player != null)
                this._player.Play();
        }

        public void stop()
        {
            if (this._player != null)
                this._player.Stop();
            this.status = PLAY_STATE.FINISH;
        }

        public void Update()
        {
            if (this._row == null || this.fileCount == 0 || this.isFinish)
            {
                this.status = PLAY_STATE.ERR;
                return;
            }
            if (this.status == PLAY_STATE.PLAYING)//正在播放,暂停的只是暂停播放，并不实际播放
            {
                float fadeTime = this.getFadeTime;
                float curVolume = this._player.volume ;
                if (fadeTime > 0)
                {
                    float fStep = Time.deltaTime / fadeTime;
                    //fade in
                    if (Time.time - this._startTime < fadeTime)
                    {
                        curVolume = curVolume + fStep;
                        if (curVolume >= this.volume)
                            curVolume = this.volume;
                        if (curVolume > 1f)
                            curVolume = 1;
                        this._player.volume = curVolume;
                    }
                    //fade out
                    if (Time.time - this._startTime > this.getDuration(this._curIndex) - this.getFadeTime)
                    {
                        curVolume = curVolume - fStep;
                        if (curVolume <= 0)
                            curVolume = 0;
                        this._player.volume = curVolume;
                    }
                }
                if (Time.time - this._startTime > this.getDuration(this._curIndex))//播放到时
                {
                    this._player.Stop();
                    this._player.clip = null;
                    this.status = PLAY_STATE.COLDING;
                }
                return;
            }
            if (this.status == PLAY_STATE.COLDING) //冷却
            {
                if(Time.time - this._startTime > this.interval)
                this.status = PLAY_STATE.WAIT;
                return;
            }
            int nextIndex = this.getNextIndex();
            if (nextIndex <= 0)
                return;
            //播放
            this.play(nextIndex);
        }
    }
}
