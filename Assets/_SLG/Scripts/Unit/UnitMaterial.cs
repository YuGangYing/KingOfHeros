using UnityEngine;
using System.Collections;

public class UnitMaterial : MonoBehaviour {

	public Material DefaultMaterial;
	public Material OutlineMaterial;
	public Renderer CurrentRenderer;

	void Awake(){
		DefaultMaterial.shader = Shader.Find (DefaultMaterial.shader.name);
		OutlineMaterial.shader = Shader.Find (OutlineMaterial.shader.name);
	}

	public void ShowOutlineMaterial()
	{
		CurrentRenderer.material = OutlineMaterial;
	}

	public void ShowDefaultMaterial()
	{
		CurrentRenderer.material = DefaultMaterial;
	}

}
