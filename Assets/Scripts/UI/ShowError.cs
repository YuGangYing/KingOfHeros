using UnityEngine;
using System.Collections;
//Line End
public class ShowError : MonoBehaviour {

    private float m_fLifeTime;

	// Use this for initialization
    void OnEnable()
    {
        m_fLifeTime = 3.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_fLifeTime > 0)
        {
            m_fLifeTime -= Time.deltaTime;
        }
        else
        {
            m_fLifeTime = 0;
            gameObject.SetActive(false);
        }
	}
}
