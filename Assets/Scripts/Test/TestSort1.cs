using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TestSort1 : MonoBehaviour {

    public List<SortObject1> sos;

	// Use this for initialization
	void Start () {
        sos.Add(new SortObject1(0,"a"));
        sos.Add(new SortObject1(4,"a4"));
        sos.Add(new SortObject1(2,"a2"));
        sos.Add(new SortObject1(3,"a3"));
        sos.Add(new SortObject1(1,"a1"));
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.A))
        {
            sos.Sort(delegate (SortObject1 small,SortObject1 big){
                return small.id - big.id;
           });
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            sos.Sort(delegate (SortObject1 small,SortObject1 big){
                return big.id - small.id;
            });
        }
	}
}

[System.Serializable]
public class SortObject1
{
    public int id;
    public string name;
    public SortObject1(int _id,string _name)
    {
        id = _id;
        name = _name;
    }

}