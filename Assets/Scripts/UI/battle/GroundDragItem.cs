using UnityEngine;
using UI;
/// <summary>
/// UIDragDropItem is a base script for your own Drag & Drop operations.
/// </summary>

//[AddComponentMenu("NGUI/Interaction/Drag and Drop Item")]
public class GroundDragItem : MonoBehaviour
{
    public enum Restriction
    {
        None,
        Horizontal,
        Vertical,
        PressAndHold,
    }

    public int position;
    public int heroId;
    public uint idHero;

    /// <summary>
    /// What kind of restriction is applied to the drag & drop logic before dragging is made possible.
    /// </summary>

    public Restriction restriction = Restriction.None;

    /// <summary>
    /// Whether a copy of the item will be dragged instead of the item itself.
    /// </summary>

    public bool cloneOnDrag = false;

    #region Common functionality

    protected Transform mTrans;
    protected Transform mParent;
    protected Collider mCollider;
    protected UIRoot mRoot;
    protected UIGrid mGrid;
    protected UITable mTable;
    protected int mTouchID = int.MinValue;
    protected float mPressTime = 0f;
    protected UIDragScrollView mDragScrollView = null;

    /// <summary>
    /// Cache the transform.
    /// </summary>

    protected virtual void Start()
    {
        mTrans = transform;
        mCollider = GetComponent<Collider>();
        mDragScrollView = GetComponent<UIDragScrollView>();
    }

    /// <summary>
    /// Record the time the item was pressed on.
    /// </summary>

    void OnPress(bool isPressed) 
    { 
        if (isPressed) mPressTime = RealTime.time;
    }

    void OnClick()
    {
        UILabel goArmyLabel = PanelTools.Find<UILabel>(gameObject, "armyLabel");

        if (int.Parse(goArmyLabel.text) != 0)
        {
            ManeuverPanel mp = UI.PanelManage.me.GetPanel<ManeuverPanel>(PanelID.ManeuverPanel);

            mp.ShowArrow(int.Parse(goArmyLabel.text), position);
        }
    }

    /// <summary>
    /// Start the dragging operation.
    /// </summary>

    void OnDragStart()
    {
        if (!enabled || mTouchID != int.MinValue) return;

        // If we have a restriction, check to see if its condition has been met first
        if (restriction != Restriction.None)
        {
            if (restriction == Restriction.Horizontal)
            {
                Vector2 delta = UICamera.currentTouch.totalDelta;
                if (Mathf.Abs(delta.x) < Mathf.Abs(delta.y)) return;
            }
            else if (restriction == Restriction.Vertical)
            {
                Vector2 delta = UICamera.currentTouch.totalDelta;
                if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) return;
            }
            else if (restriction == Restriction.PressAndHold)
            {
                if (mPressTime + 1f > RealTime.time) return;
            }
        }

        if (cloneOnDrag)
        {
            GameObject clone = NGUITools.AddChild(transform.parent.gameObject, gameObject);
            clone.transform.localPosition = transform.localPosition;
            clone.transform.localRotation = transform.localRotation;
            clone.transform.localScale = transform.localScale;

            UIButtonColor bc = clone.GetComponent<UIButtonColor>();
            if (bc != null) bc.defaultColor = GetComponent<UIButtonColor>().defaultColor;

            UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);

            UICamera.currentTouch.pressed = clone;
            UICamera.currentTouch.dragged = clone;

            GroundDragItem item = clone.GetComponent<GroundDragItem>();
            item.Start();
            item.OnDragDropStart();
            ClearInfo();
        }
        else OnDragDropStart();
    }

    /// <summary>
    /// Perform the dragging.
    /// </summary>

    void OnDrag(Vector2 delta)
    {
        if (!enabled || mTouchID != UICamera.currentTouchID) return;
        OnDragDropMove((Vector3)delta * mRoot.pixelSizeAdjustment);
    }

    /// <summary>
    /// Notification sent when the drag event has ended.
    /// </summary>

    void OnDragEnd()
    {
        if (!enabled || mTouchID != UICamera.currentTouchID) return;
        OnDragDropRelease(UICamera.hoveredObject);
    }

    #endregion

    /// <summary>
    /// Perform any logic related to starting the drag & drop operation.
    /// </summary>

    protected virtual void OnDragDropStart()
    {
        // Automatically disable the scroll view
        if (mDragScrollView != null) mDragScrollView.enabled = false;

        // Disable the collider so that it doesn't intercept events
        if (mCollider != null) mCollider.enabled = false;

        mTouchID = UICamera.currentTouchID;
        mParent = mTrans.parent;
        mRoot = NGUITools.FindInParents<UIRoot>(mParent);
        mGrid = NGUITools.FindInParents<UIGrid>(mParent);
        mTable = NGUITools.FindInParents<UITable>(mParent);

        // Re-parent the item
        if (UIDragDropRoot.root != null)
            mTrans.parent = UIDragDropRoot.root;

        Vector3 pos = mTrans.localPosition;
        pos.z = 0f;
        mTrans.localPosition = pos;

        // Notify the widgets that the parent has changed
        NGUITools.MarkParentAsChanged(gameObject);

        if (mTable != null) mTable.repositionNow = true;
        if (mGrid != null) mGrid.repositionNow = true;
    }

    /// <summary>
    /// Adjust the dragged object's position.
    /// </summary>

    protected virtual void OnDragDropMove(Vector3 delta)
    {
        mTrans.localPosition += delta;
    }

    /// <summary>
    /// Drop the item onto the specified object.
    /// </summary>

    
    protected virtual void OnDragDropRelease(GameObject surface)
    {
        if (surface != null)
        {
            GroundDragItem  gdi = surface.GetComponent<GroundDragItem>();
            ManeuverPanel mp = UI.PanelManage.me.GetPanel<ManeuverPanel>(PanelID.ManeuverPanel);

            if (gdi != null)
            {
                //id
                UILabel idHeroLable = PanelTools.Find<UILabel>(surface, "idHero");
                UILabel idLable = PanelTools.Find<UILabel>(gameObject, "idHero");

                if (idLable != null && idHeroLable != null)
                {
                    uint uId = uint.Parse(idLable.text);
                    if (uId == 0)
                    {
                        NGUITools.Destroy(gameObject);
                        return;
                    }
                    
                    uint nID = uint.Parse(idHeroLable.text);
                    if (nID != 0)
                    {
                        GroundDragItem gItem = gameObject.GetComponent<GroundDragItem>();
                        mp.GroundSwap(gItem.position, nID);
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
                UILabel goArmyLabel = PanelTools.Find<UILabel>(gameObject, "armyLabel");

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

                //士兵数
                UILabel armsLable = PanelTools.Find<UILabel>(surface, "arms");
                UILabel goArmsLable = PanelTools.Find<UILabel>(gameObject, "arms");

                if (armsLable != null && goArmsLable != null)
                {
                    armsLable.text = goArmsLable.text;
                }

                //带兵条
                UISlider slider = PanelTools.Find<UISlider>(surface, "Slider");
                UISlider goSlider = PanelTools.Find<UISlider>(gameObject, "Slider");

                if (slider != null && goSlider != null)
                {
                    slider.gameObject.SetActive(true);
                    slider.value = goSlider.value;
                }

                gdi.UpdateInfo();
                mp.ShowArrow(int.Parse(armyLabel.text), gdi.position);

                NGUITools.Destroy(gameObject);
                return;
            }
            else
            {
                UILabel idLable = PanelTools.Find<UILabel>(gameObject, "idHero");
                
                mp.HeroLeaveGround(uint.Parse(idLable.text));
                NGUITools.Destroy(gameObject);
                return;
            }
        }

        NGUITools.Destroy(gameObject);
    }

    public void ClearInfo()
    {
        idHero = 0;
        UILabel idLable = PanelTools.Find<UILabel>(gameObject, "idHero");

        if (idLable != null)
        {
            idLable.text = "0";
            idLable.gameObject.SetActive(false);
        }

        //带兵数
        UILabel armsLable = PanelTools.Find<UILabel>(gameObject, "arms");

        if (idLable != null)
        {
            armsLable.text = "";
            armsLable.gameObject.SetActive(false);
        }

        //士兵类型
        UISprite army = PanelTools.Find<UISprite>(gameObject, "army");
        UILabel armyLable = PanelTools.Find<UILabel>(gameObject, "armyLabel");

        if (army != null)
        {
            army.spriteName = "";
            army.gameObject.SetActive(false);
            armyLable.text = "0";
            armyLable.gameObject.SetActive(false);
        }

        //英雄头像
        UISprite icon = PanelTools.Find<UISprite>(gameObject, "icon");

        if (icon != null)
        {
            icon.spriteName = "";
            icon.gameObject.SetActive(false);
        }

        //英雄等级
        UILabel levelLable = PanelTools.Find<UILabel>(gameObject, "level");
        if (levelLable != null)
        {
            levelLable.text = "";
            levelLable.gameObject.SetActive(false);
        }

        //英雄品质
        UISprite quality = PanelTools.Find<UISprite>(gameObject, "quality");
        if (quality != null)
        {
            quality.spriteName = "";
            quality.gameObject.SetActive(false);
        }

        //带兵条
        UISlider slider = PanelTools.Find<UISlider>(gameObject, "Slider");

        if (slider != null)
        {
            slider.value = 0;
            slider.gameObject.SetActive(false);
        }

        //英雄星级

        UILabel starLable = PanelTools.Find<UILabel>(gameObject, "starLable");
        UISprite star = PanelTools.Find<UISprite>(gameObject, "star");
        UISprite starSprite = PanelTools.Find<UISprite>(gameObject, "starSprite");
        if (starLable != null)
        {
            starLable.text = "";
            starLable.gameObject.SetActive(false);
        }

        if (star != null)
        {
            star.gameObject.SetActive(false);
        }

        if (starSprite != null)
        {
            starSprite.spriteName = "";
            starSprite.gameObject.SetActive(false);
        }
    }

    public void UpdateInfo()
    {
        foreach(Transform child in transform)
        {
            if (child.name == "idHero" || child.name == "armyLabel" || child.name == "starLable")
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
