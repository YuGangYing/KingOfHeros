using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraAlphaSwitch : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    float m_fAlpha = 1.0f;
    bool m_bHideCamera = false;
    float m_fLifeTime = 0;
    float m_fTransTime = 0;

    private Material m_Material;
    float m_fTimeDuration = 0;

    void Update()
    {
        if (m_fTimeDuration >= m_fLifeTime)
        {
            m_fTimeDuration = 0;
            m_bHideCamera = false;
            m_fAlpha = 1.0f;
            gameObject.SetActive(false);
        }
        else if (m_fTimeDuration > m_fLifeTime - m_fTransTime)
        {
            m_fAlpha = 1 - (m_fTimeDuration - m_fLifeTime + m_fTransTime) / m_fTransTime;
            m_bHideCamera = true;
        }

        m_fTimeDuration += Time.deltaTime;
    }

    // Called by camera to apply image effect
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (m_bHideCamera && m_Material != null)
        {
            m_Material.SetFloat("_Alpha", m_fAlpha);
            Graphics.Blit(source, destination, m_Material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    public void SetCameraInfo( float fLifeTime, float fTransTime, Material meterial)
    {
        m_Material = meterial;
        m_fLifeTime = fLifeTime;
        m_fTransTime = fTransTime;
        m_fTimeDuration = 0;
    }
}