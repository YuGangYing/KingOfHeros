using UnityEngine;
using System.Collections.Generic;
using System;

public class TestSort : MonoBehaviour {

    public List<SortObject> sos;

    void Awake()
    {
        sos.Add(new SortObject(0,"a"));
        sos.Add(new SortObject(4,"a4"));
        sos.Add(new SortObject(2,"a2"));
        sos.Add(new SortObject(3,"a3"));
        sos.Add(new SortObject(1,"a1"));
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            sos.Sort();
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            sos.Reverse();
        }
    }
}

[System.Serializable]
public class SortObject : IComparable
{
    public int id;
    public string name;


    public SortObject(int _id,string _name)
    {
        id = _id;
        name = _name;
    }

    public int CompareTo(System.Object obj)
    {
        SortObject sObj = (SortObject)obj;
        int result = 0;
        if (this.id > sObj.id)
        {
            result = 1;
        }
        else
        {
            result = -1;
        }
        return result;
    }
}