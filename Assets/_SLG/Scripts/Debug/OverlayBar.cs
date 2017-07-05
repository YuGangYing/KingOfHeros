using UnityEngine;
using System.Collections;

public class OverlayBar : MonoBehaviour {

	public GameObject HealthBarPos;
	public UISprite HealthBar;
	public UISprite[] SkillBars;
	public UILabel InfoText;
	public UITweener[] tweens; 
	public UISprite RangerMark;
	public TweenPosition TweenPos;

	void Awake()
	{
		tweens = GetComponentsInChildren<UITweener>();
		TweenPos = GetComponentInChildren<TweenPosition>();
	}

	public void PlayTweener(string text,Color col)
	{
		InfoText.text = text;
		InfoText.color = col;
		TweenPos.to = new Vector3(Random.Range(-10,10),-8,Random.Range(-10,10));
		foreach(UITweener tweener in tweens)
		{
			tweener.enabled=true;
			tweener.ResetToBeginning();
			tweener.PlayForward();
		}
	}

}
