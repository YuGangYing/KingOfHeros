using UnityEngine;

/// <summary>
/// Attach this script to anything in the scene
/// </summary>

public class SpeedControl : MonoBehaviour
{

    [SerializeField]
    private CameraPathBezierAnimator _animator = null;

    [SerializeField]
    private float ease = 0.05f;//1 intstant speed change - 0 never change

    private float targetSpeed = 0;

    void Awake()
    {
        if(_animator == null)
        {
            Debug.LogError("The Camera Path animator has not been linked to this component \nSelf destruct in 3..2..1..");
            Destroy(this);
            return;
        }
        //Link up the event system
        _animator.AnimationPointReachedWithNumber += OnPointReachedByNumber;
    }

    void Start()
    {
        targetSpeed = _animator.pathSpeed;
    }

    void Update()
    {
        _animator.pathSpeed = Mathf.Lerp(_animator.pathSpeed, targetSpeed, ease);
    }

    private void OnPointReachedByNumber(int pointNumber)
    {
        //When a point is reached by number
        //modify the speed
        switch (pointNumber)
        {
            case 0:
                targetSpeed = 2;
                break;

            case 1:
                targetSpeed = 0.5f;
                break;

            case 2:
                targetSpeed = 3;
                break;
        }
    }
}
