using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cell 
{
    Vector3 m_pos;
    Color m_color;

    public Cell(Vector3 pos, Color color)
    {
        m_pos = pos;
        m_color = color;
    }
    
}
