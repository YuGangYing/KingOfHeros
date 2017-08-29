﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WWWNetwork
{
	public class ChangeBuildingPosAPI : BaseAPI
	{
		public ChangeBuildingPosModel data;
		public override void Send (UnityEngine.Events.UnityAction<UnityEngine.WWW> complete)
		{
			Debug.Log (JsonUtility.ToJson(data));
			this.bytedata = System.Text.ASCIIEncoding.ASCII.GetBytes ("data=" + JsonUtility.ToJson(data));
			api = APIConstant.CHANGE_BUILDING_POS;
			base.Send (complete);
		}
	}

	public class ChangeBuildingPosModel{
		public int id;
		public int pos;
	}
}
