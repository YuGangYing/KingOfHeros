using UnityEngine;
using System.Collections;

public class Common
{
	public static void SetShaderForEditor(GameObject go){
		#if UNITY_EDITOR
		Renderer[] rss = go.GetComponentsInChildren<Renderer>(true);
		for(int i=0;i<rss.Length;i++){
			for(int j=0;j<rss[i].sharedMaterials.Length;j++){
				if(rss[i].sharedMaterials[j]!=null)
					rss[i].sharedMaterials[j].shader = Shader.Find(rss[i].sharedMaterials[j].shader.name);
			}
		}
		#endif
	}

	public static void SetMaterial (GameObject _obj, Material _material)
	{ 
		#if UNITY_EDITOR
		if (_material != null)
			_material.shader = Shader.Find (_material.shader.name);
		#endif
		if (_obj == null) {
			Debug.LogError ("_obj is null");

			return;
		}

		if (_material == null) {
			Debug.LogError ("_material is null");

			return;
		} 

		Renderer[] tmp_renderers = _obj.GetComponentsInChildren<Renderer> (true);

		if (tmp_renderers == null) {
			Debug.LogError ("tmp_renderers is null");

			return;
		}

		if (tmp_renderers.Length > 0) {
			for (int i = 0; i < tmp_renderers.Length; ++i) {
				tmp_renderers [i].sharedMaterial = _material; 
			}
		} 
	}

	public static T AddObjComponent<T> (GameObject _obj) where T: Component
	{
		if (_obj == null) {
			Debug.LogError ("_obj is null");

			return null;
		}

		T tmp_component = _obj.GetComponent<T> ();

		if (tmp_component == null) {
			tmp_component = _obj.AddComponent<T> ();
		}

		return tmp_component;
	}

	public static GameObject CreateGameObject (string _strPreb)
	{
		if (_strPreb == null || _strPreb.Length == 0) {
			Debug.LogError ("_strPreb is null");

			return null;
		}
		Object probObj = Resources.Load (_strPreb);  //ResourcesMgr.LoadAsset<GameObject>(_strPreb);   
		if (probObj == null) {
			Debug.LogError ("probObj si null");

			return null;
		}

		GameObject newObj = (GameObject)GameObject.Instantiate (probObj, Vector3.zero, Quaternion.identity);

		if (newObj == null) {
			Debug.LogError ("newObj is null");
		}

		return newObj; 
	}

	public static AudioClip CreateAudioClip (string _strPreb)
	{
		if (_strPreb == null || _strPreb.Length == 0) {
			Debug.LogError ("_strPreb is null");
			
			return null;
		}
		
		Object probObj = Resources.Load (_strPreb);  //Object probObj = ResourcesMgr.me.GetPrefab(_strPreb);
		
		//Debug.Log("_strPreb..................miss................." + _strPreb);
		if (probObj == null) {
			
			Debug.LogError ("probObj si null");
			
			return null;
		}
		
		AudioClip newObj = (AudioClip)GameObject.Instantiate (probObj, Vector3.zero, Quaternion.identity);
		
		if (newObj == null) {
			Debug.LogError ("newObj is null");
		}
		
		return newObj; 
	}

	public static void AddChildObj (GameObject _obj, GameObject _parent, Vector3 _locationpos)
	{
		if (_parent == null || _obj == null) {
			Debug.LogError ("AddChildObj function error !!!");

			return;
		}

		_obj.transform.parent = _parent.transform;

		_obj.transform.localPosition = _locationpos;

		_obj.transform.rotation = _parent.transform.rotation; 
	}

	public static void SetLayer (GameObject _obj, string _layername, bool _ischild = false)
	{
		if (_obj == null) {
			return;
		}

		if (!_ischild) {
			_obj.layer = LayerMask.NameToLayer (_layername);
		} else {
			foreach (Transform ts in  _obj.transform.transform) {
				ts.gameObject.layer = LayerMask.NameToLayer (_layername);
			} 
		}
	}

	public static void SetTag (GameObject _obj, string _tagname, bool _ischild = false)
	{
		if (_obj == null) {
			return;
		}

		if (!_ischild) {
			_obj.tag = _tagname;
		} else {
			foreach (Transform ts in _obj.transform.transform) {
				ts.gameObject.tag = _tagname;
			}
		}
	}

	public static bool CheckPosInForwardSector (Vector3 local, Vector3 pos, float angle)
	{
		float angle0 = Vector3.Dot ((pos - local).normalized, Vector3.forward);
		if (angle0 >= Mathf.Cos (angle / 2 * Mathf.PI / 180)) {
			return true;
		}
		return false;
	}
}
 
