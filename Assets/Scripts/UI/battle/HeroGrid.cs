using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI;

#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
public class HeroGrid : UIGrid {

    enum Sort_TYPE
    {
        SORTBY_NAME,
        SORTBY_LEVEL,
        SORTBY_STAR,
        SORTBY_QUALITY,
    }

    public int sortFun = 0;
    public delegate void SortDelegate();

    protected override void Sort(List<Transform> list) 
    {
        switch (sortFun)
        {
            case 0:
                list.Sort(SortByName); 
                break;
            case 1:
                list.Sort(SortByLevel);
                break;
            case 2:
                list.Sort(SortByStar);
                break;
            case 3:
                list.Sort(SortByQuality);
                break;
        }
    }

    static protected int SortByName(Transform a, Transform b) 
    {
        int result = string.Compare(a.name, b.name);

        if (result > 0)
        {
            result = -1;
        }
        else if (result < 0)
        {
            result = 1;
        }

        return result; 
    }

    protected int SortByLevel(Transform a, Transform b)
    {
        UILabel alevelLabel = PanelTools.Find<UILabel>(a.gameObject, "level");
        UILabel blevelLabel = PanelTools.Find<UILabel>(b.gameObject, "level");

        int alevel = int.Parse(alevelLabel.text);
        int blevel = int.Parse(blevelLabel.text);
        int result = alevel.CompareTo(blevel);

        if (result > 0)
        {
            result = -1;
        }
        else if (result < 0)
        {
            result = 1;
        }
        else
        {
            result = string.Compare(a.name, b.name);
        }

        return result;
    }

    protected int SortByStar(Transform a, Transform b)
    {
        UILabel astarLabel = PanelTools.Find<UILabel>(a.gameObject, "starLable");
        UILabel bstarLabel = PanelTools.Find<UILabel>(b.gameObject, "starLable");

        int astar = int.Parse(astarLabel.text);
        int bstar = int.Parse(bstarLabel.text);
        int result = astar.CompareTo(bstar);

        if (result > 0)
        {
            result = -1;
        }
        else if (result < 0)
        {
            result = 1;
        }
        else
        {
            result = string.Compare(a.name, b.name);
        }

        return result;
    }

    protected int SortByQuality(Transform a, Transform b)
    {
        UILabel aQualityLabel = PanelTools.Find<UILabel>(a.gameObject, "qualityLable");
        UILabel bQualityLabel = PanelTools.Find<UILabel>(b.gameObject, "qualityLable");

        int result = 0;

        result = string.Compare(aQualityLabel.text, bQualityLabel.text);

        if (result > 0)
        {
            result = -1;
        }
        else if (result < 0)
        {
            result = 1;
        }
        else
        {
            result = string.Compare(a.name, b.name); 
        }

        return result;
    }

    public void Filterfun(int filterType)
    {
        foreach(Transform chlid in transform)
        {

            UILabel armyLabel = PanelTools.Find<UILabel>(chlid.gameObject, "armyType");

            int nFilter = int.Parse(armyLabel.text);

            if (0 == filterType)
            {
                chlid.gameObject.SetActive(true);
            }
            else if (filterType == nFilter)
            {
                chlid.gameObject.SetActive(true);
            }
            else
            {
                chlid.gameObject.SetActive(false);
            }
            
            UILabel idLabel = PanelTools.Find<UILabel>(chlid.gameObject, "idHero");
            uint id = uint.Parse(idLabel.text);

            ManeuverPanel mailPanel = PanelManage.me.GetPanel<ManeuverPanel>(PanelID.ManeuverPanel);

            if (mailPanel.dicMaxHeroSoldier.ContainsKey(id))
            {
                chlid.gameObject.SetActive(false);
            }
            
            
        }
    }
}
