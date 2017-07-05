using UnityEngine;
using System.Collections;

public class UnitMaterial : MonoBehaviour {

	public Material DefaultMaterial;
	public Material OutlineMaterial;
	public Renderer CurrentRenderer;

	public void ShowOutlineMaterial()
	{
		CurrentRenderer.material = OutlineMaterial;
	}

	public void ShowDefaultMaterial()
	{
		CurrentRenderer.material = DefaultMaterial;
	}

}
