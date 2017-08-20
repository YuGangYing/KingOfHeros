using UnityEngine;
using UI;

//[AddComponentMenu("NGUI/Examples/Drag and Drop Item (Example)")]
public class PutDragItem : UIDragDropItem
{
	/// <summary>
	/// Prefab object that will be instantiated on the DragDropSurface if it receives the OnDrop event.
	/// </summary>

	public GameObject prefab;
    public uint idHero;

	/// <summary>
	/// Drop a 3D game object onto the surface.
	/// </summary>

	protected override void OnDragDropRelease (GameObject surface)
	{
		if (surface != null)
		{
            GroundDragItem gdi = surface.GetComponent<GroundDragItem>();
            ManeuverPanel mp = UI.PanelManage.me.GetPanel<ManeuverPanel>(PanelID.ManeuverPanel);

            if (gdi != null)
            {
                //数量限制
                if (mp.IsOverCount() && gdi.idHero == 0)
                {
                    NGUITools.Destroy(gameObject);
                    return;
                }

                //gdi.idHero = idHero;
                UILabel idHeroLable = PanelTools.Find<UILabel>(surface, "idHero");
                UILabel idLable = PanelTools.Find<UILabel>(gameObject, "idHero");

                if (idLable != null && idHeroLable != null)
                {
                    uint nID = uint.Parse(idHeroLable.text);
                    if (nID != 0)
                    {
                        mp.HeroLeaveGround(nID);
                    }
                    
                    idHeroLable.text = idLable.text;
                    idHero = uint.Parse(idLable.text);
                    gdi.idHero = idHero;
                }
                
                //英雄头像
                UISprite positionIcon = PanelTools.Find<UISprite>(surface, "icon");
                UISprite itemIcon = PanelTools.Find<UISprite>(gameObject, "icon");

                if (positionIcon != null && itemIcon != null)
                {
                    positionIcon.atlas = itemIcon.atlas;
                    positionIcon.spriteName = itemIcon.spriteName;
                }

                //军队类型
                UISprite armyIcon = PanelTools.Find<UISprite>(surface, "army");
                UISprite goItemIcon = PanelTools.Find<UISprite>(gameObject, "army");
                UILabel armyLabel = PanelTools.Find<UILabel>(surface, "armyLabel");
                UILabel goArmyLabel = PanelTools.Find<UILabel>(gameObject, "armyType");

                if (armyIcon != null && goItemIcon != null)
                {
                    armyIcon.atlas = goItemIcon.atlas;
                    armyIcon.spriteName = goItemIcon.spriteName;
                    armyLabel.text = goArmyLabel.text;
                }

                //英雄等级
                UILabel levelLable = PanelTools.Find<UILabel>(surface, "level");
                UILabel goLevelLable = PanelTools.Find<UILabel>(gameObject, "level");

                if (levelLable != null && goLevelLable != null)
                {
                    levelLable.text = goLevelLable.text;
                }

                //英雄品质
                UISprite quality = PanelTools.Find<UISprite>(surface, "quality");
                UISprite goQuality = PanelTools.Find<UISprite>(gameObject, "quality");

                if (quality != null && goQuality != null)
                {
                    quality.atlas = goQuality.atlas;
                    quality.spriteName = goQuality.spriteName;
                }

                //英雄星级
                UILabel starLable = PanelTools.Find<UILabel>(surface, "starLable");
                UILabel gostarLable = PanelTools.Find<UILabel>(gameObject, "starLable");

                if (starLable != null && gostarLable != null)
                {
                    starLable.text = gostarLable.text;
                }

                UISprite starSprite = PanelTools.Find<UISprite>(surface, "starSprite");
                UISprite goStarSprite = PanelTools.Find<UISprite>(gameObject, "starSprite");

                if (starSprite != null && goStarSprite != null)
                {
                    starSprite.spriteName = goStarSprite.spriteName;
                }

                gdi.UpdateInfo();
                mp.ShowArrow(int.Parse(armyLabel.text), gdi.position);

                mp.HeroOnGround(idHero);

                NGUITools.Destroy(gameObject);
                return;
            }
		}
		base.OnDragDropRelease(surface);
	}

    public void RePutCloneItem(GameObject surface)
    {
        OnDragDropRelease(surface);
    }
}
