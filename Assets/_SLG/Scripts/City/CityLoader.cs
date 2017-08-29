using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KOH
{
	public class CityLoader : SingleMonoBehaviour<CityLoader>
	{
		public GameObject cityRoot;

		protected override void Awake ()
		{
			base.Awake ();
			Init ();
		}

		void Init ()
		{
			cityRoot = ResourcesManager.GetInstance.GetCityRoot ();
			Common.SetShaderForEditor (cityRoot);
		}


	}

}
