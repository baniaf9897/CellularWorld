using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cell 
{
    public Vector3 pos;
    public Vector3 size;
    public Vector3 color;

    public Cell(Vector3 p, Vector3 s, Vector3 c)
    {
        pos = p;
        size = s;
        color = c;
    }

    public static int GetSize()
    {
        return sizeof(float) * 9;
    }

}
