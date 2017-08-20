using System;
using System.Collections.Generic;
using UnityEngine; 

namespace Fight
{ 
	public class Soldier : Character
	{ 
        Animator _lodAanimator = null;
        LODGroup _lodGroup = null; 

        public Soldier()
        {
            this._fCurDamage = 100;
        } 

        protected override void initRoot()
        { 
            this._animator = _root.GetComponent<Animator>();
            if (this._animator == null)
            {
                Animator[] aList = _root.GetComponentsInChildren<Animator>();
                foreach(Animator item in aList)
                {
                    if (item.name.Contains("LOD"))
                        this._lodAanimator = item;
                    else
                        this._animator = item;
                }
            }
            this._lodGroup = _root.GetComponent<LODGroup>();  
        }

        public bool enableLod
        {
            set
            {
                if (_lodGroup != null)
                    _lodGroup.enabled = value;
            }
        }

        public override void update()
        {
            base.update(); 
        }

        //克制兵种1
        public ARMY_TYPE restrainiArmy1
        {
            get;
            set;
        }

        //克制兵种2
        public ARMY_TYPE restrainiArmy2
        {
            get;
            set;
        }

        public override void setAnimatorBool(string strName, bool bValue)
        {
            base.setAnimatorBool(strName, bValue);

            if (this._lodAanimator != null)
            {
                this._lodAanimator.SetBool(strName, bValue); 
            }
        }

	}
}
