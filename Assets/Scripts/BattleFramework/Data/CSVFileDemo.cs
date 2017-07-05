using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataMgr;

namespace BattleFramework.Data{

	public class CSVFileDemo : MonoBehaviour {

		public string level = "CSVTest";

		void Update()
		{
			if(Input.GetMouseButtonDown(1))
			{
				Application.LoadLevel(level);
			}
			if(Input.GetMouseButtonDown(0))
			{
				DataCenter dataCenter = DataCenter.SingleTon();
			}
		}

	}
}
