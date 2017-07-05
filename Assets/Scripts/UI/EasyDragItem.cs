using UnityEngine;
using System.Collections;
namespace UI
{
    [ExecuteInEditMode]
    public class EasyDragItem : MonoBehaviour
    {
        public EasyDrag draggablePanel;

        void Start()
        {
            if (draggablePanel == null)
            {
                draggablePanel = NGUITools.FindInParents<EasyDrag>(gameObject);
            }
        }

        /// <summary>
        /// Create a plane on which we will be performing the dragging.
        /// </summary>

        void OnPress(bool pressed)
        {
            if (enabled && NGUITools.GetActive(gameObject) && draggablePanel != null)
            {
                draggablePanel.Press(pressed);
            }
        }

        /// <summary>
        /// Drag the object along the plane.
        /// </summary>

        void OnDrag(Vector2 delta)
        {
            if (enabled && NGUITools.GetActive(gameObject) && draggablePanel != null)
            {
                draggablePanel.Drag();
            }
        }

        /// <summary>
        /// If the object should support the scroll wheel, do it.
        /// </summary>

        void OnScroll(float delta)
        {
            if (enabled && NGUITools.GetActive(gameObject) && draggablePanel != null)
            {
                draggablePanel.Scroll(delta);
            }
        }
    }

}