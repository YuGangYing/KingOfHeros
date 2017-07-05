using UnityEngine;
using System.Collections;

public class SideCtrl : UITweener
{
    public bool isSelected = false;

    protected override void OnUpdate(float factor, bool isFinished)
    {

    }

    void OnClick()
    {
        GameObject side = transform.parent.gameObject;
        TweenPosition tp = side.gameObject.GetComponent<TweenPosition>();
        Transform sprite = transform.FindChild("Sprite");

        if (tp != null)
        {
            if (isSelected)
            {
                isSelected = false;
                tp.PlayReverse();
                sprite.Rotate(new Vector3(0, 0, -180));
            }
            else
            {
                isSelected = true;
                tp.PlayForward();
                sprite.Rotate(new Vector3(0, 0, 180));
            }
        }
    }   
}
