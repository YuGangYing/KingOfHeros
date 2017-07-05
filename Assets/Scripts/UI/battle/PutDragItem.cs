using UnityEngine;
using UI;
using System;

//[AddComponentMenu("NGUI/Examples/Drag and Drop Item (Example)")]
public class PutDragItem : UIDragDropItem
{
	/// <summary>
	/// Prefab object that will be instantiated on the DragDropSurface if it receives the OnDrop event.
	/// </summary>

	public GameObject prefab;
	public uint idHero;

	//yao
	private Transform nowParent;

	//
	protected  void OnPress (bool isPressed)
	{
		base.OnPress (isPressed);
		nowParent = this.transform.parent;
		//print (this.transform.parent.name + "lllllllllllllllllll");
	}
	
	
	/// <summary>
	/// Drop a 3D game object onto the surface.
	/// </summary>

	protected override void OnDragDropRelease (GameObject surface)
	{
		base.OnDragDropRelease (surface);
		string surfaceParentName = surface.transform.parent.tag; 
		if (this.transform.parent.tag == "Cells") { //cells 内
			if (surface.transform.tag == "Cells") {  //next cells
				this.transform.parent = surface.transform;
				this.transform.localPosition = Vector3.zero;
				this.gameObject.SetActive (false);
				this.gameObject.SetActive (true);
			} else if (surface.transform.tag == "HerosList") { //hero上  分两类
				if (surfaceParentName == "Cells") { //交换
					this.transform.parent = surface.transform.parent;
					this.transform.localPosition = Vector3.zero;
					surface.transform.parent = nowParent;
					surface.transform.localPosition = Vector3.zero;
					this.gameObject.SetActive (false);
					this.gameObject.SetActive (true);	
				} else if (surfaceParentName == "HerosList") {//回到list里面  列表内
					this.transform.parent = surface.transform.parent;
					this.transform.localPosition = Vector3.zero;
					this.transform.parent.gameObject.GetComponent<UIGrid> ().Reposition ();
					this.gameObject.SetActive (false);
					this.gameObject.SetActive (true);
				} else {//返回原来位置
					this.transform.parent = nowParent;
					this.transform.localPosition = Vector3.zero;
					this.gameObject.SetActive (false);
					this.gameObject.SetActive (true);
				}
			} else {//返回原来位置
				this.transform.parent = nowParent;
				this.transform.localPosition = Vector3.zero;
				this.gameObject.SetActive (false);
				this.gameObject.SetActive (true);
			}
		} else {//list 内  
			if (surface.transform.tag == "Cells") {  //next cells
				this.transform.parent = surface.transform;
				this.transform.localPosition = Vector3.zero;
				this.gameObject.SetActive (false);
				this.gameObject.SetActive (true);
				print ("yaoz");

			} else if (surface.transform.tag == "HerosList") {  //其他三种情况   单元式   表外   自身list上
				if (surfaceParentName == "Cells") {//交换  初始化grid
					this.transform.parent = surface.transform.parent;
					this.transform.localPosition = Vector3.zero;
					surface.transform.parent = nowParent;
					surface.transform.localPosition = Vector3.zero;
					this.transform.localPosition = Vector3.zero;

					nowParent.GetComponent<UIGrid> ().Reposition ();
					this.gameObject.SetActive (false);
					this.gameObject.SetActive (true);

				} else {  //返回 初始化
					this.transform.parent = nowParent;
					this.transform.localPosition = Vector3.zero;
					nowParent.GetComponent<UIGrid> ().Reposition ();
					this.gameObject.SetActive (false);
					this.gameObject.SetActive (true);
				}
			} 
		}
	} 


	public void RePutCloneItem (GameObject surface)
	{
		OnDragDropRelease (surface);
	}
}
