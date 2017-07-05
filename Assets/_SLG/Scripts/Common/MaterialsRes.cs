using UnityEngine;
using System.Collections;
using Fight;
 
public class MaterialsRes : MonoBehaviour 
{
    public Material m_Material;
   
    void Awake()
    {
     
    }

	public void ChangeColor(Color c)
	{
		m_Material.SetColor("_Color", c);
		Common.SetMaterial(gameObject, m_Material);
	}

	public void ChangeColor(Material mat)
    {
		Common.SetMaterial(gameObject, mat);

//        if (m_Material == null)
//        {
//            Logger.LogError("m_Material is null");
//
//            return;
//        } 
//         
//        switch (m_Side)
//        {
//            case SIDE.LEFT:
//                m_Material.SetColor("_Color", Color.red);
//                Common.SetMaterial(gameObject, m_Material);
//                break;
//            case SIDE.RIGHT:
//                Material material = new Material(m_Material);
//                material.SetColor("_Color", Color.blue);
//                Common.SetMaterial(gameObject, material);
//                break;
//        }
    } 
}
