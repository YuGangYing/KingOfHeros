using UnityEngine;
using UnityEngine.UI;  			//UI命名空间
using UnityEngine.EventSystems;	//事件系统命名空间
using System.Collections;
using System.Collections.Generic;
using System;

public  class GetTimestamp
{
	public static long GetNowTimestamp ()
	{
		/*
		DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime (new System.DateTime (1970, 1, 1, 0, 0, 0, 0));
		DateTime nowTime = DateTime.Now;
		long unixTime = (long)Math.Round ((nowTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);
		*/

		long unixTime = (DateTime.Now.ToUniversalTime ().Ticks - 621355968000000000) / 10000000;
		return unixTime;
	}
}
