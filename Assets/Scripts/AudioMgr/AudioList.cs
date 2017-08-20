using System;
using System.Collections.Generic;
using UnityEngine;
using DataMgr;

namespace AudioMgr
{
	public class AudioList
	{
        List<AudioItem> _itemList = null;
        bool _bEnable = true;

        public AudioList()
        {
        }

        public AUDIO_TYPE type
        {
            get;
            set;
        }

        public int maxCount
        {
            get;
            set;
        }

        //声音系数
        public float volumeRatio
        {
            get;
            set;
        }

        public void pause()
        {
            _bEnable = false;
            if (this._itemList == null)
                return;
            foreach (AudioItem item in this._itemList)
            {
                item.isPause = !_bEnable;
                item.pause();
            }
        }

        public void resume()
        {
            _bEnable = true;
            if (this._itemList == null)
                return;
            foreach (AudioItem item in this._itemList)
            {
                item.isPause = !_bEnable;
                item.resume();
            }
        }

        public bool addIem(AudioItem item)
        {
            if (this._itemList == null)
                this._itemList = new List<AudioItem>();
            if(_itemList.Count >= this.maxCount)//超过数量时，直接放弃该请求
                return false;
            item.isPause = !_bEnable;
            this._itemList.Add(item);
            return true;
        }

        public void releaseItem(AudioItem item)
        {
            if (this._itemList == null)
                return;
            if (item != null)
            {
                item.release();
                this._itemList.Remove(item);
            }
        }

        public void release()
        {
            if (this._itemList == null)
                return;
            foreach(AudioItem item in this._itemList)
            {
                if (item != null)
                    item.release();
            }
            _itemList.Clear();
            this._itemList = null;
        }

        public void update()
        {
            if (this._itemList == null)
                return;
            List<AudioItem> itemList  = new List<AudioItem>();
            foreach (AudioItem item in this._itemList)
            {
                if (item != null)
                {
                    if (item.isFinish)
                        item.release();
                    else
                    {
                        item.Update();
                        itemList.Add(item);
                    }
                }
            }
            this._itemList = itemList;
        }

        public float volume
        {
            set
            {
                if (this._itemList == null)
                    return;
                foreach (AudioItem item in this._itemList)
                {
                    if(item!=null)
                        item.volume = value * this.volumeRatio;
                }
            }
        }
	}
}
