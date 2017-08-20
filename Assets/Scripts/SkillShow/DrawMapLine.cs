using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawMapLine : MonoBehaviour {

    Drawer drawer = null;
    public int nWidth = 10;
    public int nHeight = 10;

    void Start () 
    {
        drawer = gameObject.AddComponent<Drawer>();
        drawer.setLine(Color.gray, 1f);
        init();
    }

    public void init()
    {
        if (drawer == null)
            return;
        drawer.clear();

        float fY = transform.position.y ;
        float fX = transform.position.x;
        float fZ = transform.position.z;

        float xBegin = fX - nWidth;
        float xEnd = fX + nWidth;
        float zBegin = fZ - nWidth;
        float zEnd = fZ + nWidth;

        for (int x = -nWidth; x <= nWidth ; x++)
            drawer.drawLine(new Vector3(fX + x, fY, zBegin), new Vector3(fX + x, fY, zEnd));
        for (int z = -nHeight; z <= nHeight; z++)
            drawer.drawLine(new Vector3(xBegin, fY, fZ + z), new Vector3(xEnd, fY, fZ + z));
    }

    public void OnDestroy()
    {
        if (drawer == null)
            return;
        drawer.clear();        
    }
}
