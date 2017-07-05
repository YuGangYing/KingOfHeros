using UnityEngine;
using System.Collections;

public class CameraSwitchEventMgr : MonoBehaviour {

#region material
    public Shader shader;
    private Material m_Material;

    protected Material material
    {
        get
        {
            if (m_Material == null)
            {
                m_Material = new Material(shader);
                m_Material.hideFlags = HideFlags.HideAndDontSave;
            }
            return m_Material;
        }
    }

    protected virtual void OnDisable()
    {
        if (m_Material)
        {
            DestroyImmediate(m_Material);
        }
    }

#endregion material

    [System.Serializable]
    public class CameraInfo
    {
        public Camera cam;
        public float fLifeTime;
        public float fTransTime;
    }

    [System.Serializable]
    public class CameraSwitchEvent
    {
        public int nEventID;
        public CameraInfo[] camInfo;
    }

    public CameraSwitchEvent[] m_Events;

    bool m_bPlay = false;
    CameraSwitchEvent m_currentEvent;
    float m_fTimeDuration = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (m_bPlay && m_currentEvent!= null)
        {
            CameraInfo[] camInfos = m_currentEvent.camInfo;
            float fTimeTotal = 0;
            for (int i = 0; i < camInfos.Length; i ++)
            {
                fTimeTotal += camInfos[i].fLifeTime - camInfos[i].fTransTime;

                if (m_fTimeDuration <= fTimeTotal)
                {
                    if (camInfos[i].cam != null && camInfos[i].cam.gameObject.activeSelf == false)
                    {
                        CameraAlphaSwitch alphaSwitch = camInfos[i].cam.gameObject.GetComponent<CameraAlphaSwitch>();

                        if (alphaSwitch == null)
                        {
                            alphaSwitch = camInfos[i].cam.gameObject.AddComponent<CameraAlphaSwitch>();
                            alphaSwitch.SetCameraInfo(camInfos[i].fLifeTime, camInfos[i].fTransTime, material);
                        }

                        camInfos[i].cam.gameObject.SetActive(true);
                    }
                    break;
                }
            }

            m_fTimeDuration += Time.deltaTime;
        }
	}

    public void SwitchCamera (int nEventID)
    {
        foreach (CameraSwitchEvent ev in m_Events)
        {
            if (ev.nEventID == nEventID)
            {
                m_currentEvent = ev;
                break;
            }
        }

        m_fTimeDuration = 0;
        m_bPlay = true;
    }
}
