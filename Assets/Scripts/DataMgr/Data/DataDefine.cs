using System;
using System.Collections.Generic;

namespace DataMgr
{
    public enum DATA_MODULE:int
    {
        SYSTEM =0 ,//系统信息
        USERINFO,//用户信息
        HEROLIST,   //英雄列表
        MAILLIST,//邮件列表
        FRIENDLIST, //好友列表
        CHATLIST,//聊天信息
        TECHLIST,//科技列表
        OPERLIST,//操作列表
    }
}
