using System;
using System.Collections.Generic;

namespace DataMgr
{
    class ConfigMgr
	{
        Dictionary<CONFIG_MODULE,ConfigBase> _cfgList = new Dictionary<CONFIG_MODULE,ConfigBase>();

        public ConfigMgr()
        {
            init();
        }

        public bool init()
        {
            return true;
        }

        public void release()
        {

        }

        //获取指定模块的配置对象
        public ConfigBase getCfg(CONFIG_MODULE key)
        {
            ConfigBase config = null;
            if(this._cfgList.TryGetValue(key,out config))
                return config;
            //加载配置文件
            return this.initCfg(key);
        }

        //自动校验字段个数是否和枚举匹配
        public ConfigBase getCfg<T>(CONFIG_MODULE key) where T : struct, IConvertible
        {
            ConfigBase config = this.getCfg(key);
            if (config == null)
                return null;
            if (config.checkColCount<T>())
            {
                Logger.LogError("read config {0}  failed!", key);
                return null;
            }
            return config;
        }

        public void releaseCfg(CONFIG_MODULE key)
        {
            this._cfgList.Remove(key);
        }

        private ConfigBase initCfg(CONFIG_MODULE key)
        {
            ConfigBase config = new ConfigBase();
            string strFileName = Tools.GetDescription(key);
            if(strFileName==null)
                return null;
            if (config.init(strFileName))
            {
                this._cfgList[key] = config;
                return config;
            }
            else
                Logger.LogError("read config {0}  failed!", strFileName);
            return null;
        }

        static public int getEnumCount<T>() where T : struct, IConvertible
        {
            return Enum.GetValues(typeof(T)).Length;
        }
	}
}
