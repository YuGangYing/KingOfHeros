//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
#pragma warning disable 0472
namespace UI
{
    public class DragDropItem : MonoBehaviour
    {
        Transform mTrans;
        Transform mTransClone;
        bool mPressed = false;
        int mTouchID = 0;
        bool mIsDragging = false;
        bool mSticky = false;

        [HideInInspector]
        public DragDropItem swapItem; // 替换的

        // 拖动结束
        public delegate void DragEnd(DragDropItem item, object p);
        public DragEnd DragEndFun; // 拖动结束
        public object dragParam; // 参数

        /// <summary>
        /// Drop the dragged object.
        /// </summary>

        void Drop()
        {
            if (mTransClone != null)
            {
                Destroy(mTransClone.gameObject);
            }

            if (DragEndFun != null)
            {
                DragEndFun(this, dragParam);
            }
        }

        /// <summary>
        /// Cache the transform.
        /// </summary>

        void Awake() { mTrans = transform; }

        UIRoot mRoot;

        /// <summary>
        /// Start the drag event and perform the dragging.
        /// </summary>

        void OnDrag(Vector2 delta)
        {
            if (mPressed && UICamera.currentTouchID == mTouchID && enabled)
            {
                if (!mIsDragging)
                {
                    mIsDragging = true;
                    mRoot = NGUITools.FindInParents<UIRoot>(mTrans.gameObject);

                    mTransClone = ((GameObject)Instantiate(mTrans.gameObject)).transform;
                    mTransClone.gameObject.layer = LayerMask.NameToLayer("UI");
                    mTransClone.parent = mTrans.parent;

                    Vector3 pos = mTrans.localPosition;
                    pos.z = 0f;
                    mTransClone.localPosition = pos;
                    mTransClone.localScale = mTrans.localScale;

                    // Inflate the depth so that the dragged item appears in front of everything else
                    UIWidget[] widgets = mTransClone.GetComponentsInChildren<UIWidget>();
                    for (int i = 0; i < widgets.Length; ++i) widgets[i].depth = widgets[i].depth + 100;

                    mTransClone.gameObject.AddComponent<UIPanel>().depth = 1000;
                    NGUITools.MarkParentAsChanged(mTransClone.gameObject);
                }
                else
                {
                    mTransClone.localPosition += (Vector3)delta * mRoot.pixelSizeAdjustment;
                }
            }
        }

        /// <summary>
        /// Start or stop the drag operation.
        /// </summary>

        void OnPress(bool isPressed)
        {
            if (enabled)
            {
                if (isPressed)
                {
                    if (mPressed) return;

                    mPressed = true;
                    mTouchID = UICamera.currentTouchID;

                    if (!UICamera.current.stickyPress)
                    {
                        mSticky = true;
                        //UICamera.current.stickyPress = true;
                    }
                }
                else
                {
                    mPressed = false;

                    if (mSticky)
                    {
                        mSticky = false;
                        //UICamera.current.stickyPress = false;
                    }
                }

                mIsDragging = false;
                Collider col = GetComponent<Collider>();
                if (col != null) col.enabled = !isPressed;
                if (!isPressed) Drop();
            }
        }
    }

}