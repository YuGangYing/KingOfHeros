using UnityEngine;
using System.Collections;

public class ArmyAnim : MonoBehaviour 
{
    Animation _anim;
	// Use this for initialization
	void Start () {
        _anim = gameObject.GetComponentInChildren<Animation>();
        StartCoroutine(playAnim());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator playAnim()
    {
        if (_anim != null)
        {
            foreach (AnimationState state in _anim)
            {
                _anim.Play(state.name);
                yield return new WaitForSeconds(state.length);
            }
        }
    }
}
