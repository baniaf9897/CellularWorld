using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public int m_NumCells = 1;
    public float m_CellSize = 1.0f;

    public Color m_CellColor = new Color(1, 1, 1);

    public GameObject WorldPlane = null;

    List<List<Cell>> m_cells;

    // Start is called before the first frame update
    void Start()
    {
        m_cells = new List<List<Cell>>();

        GenerateRandomCellDistribution();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateRandomCellDistribution()
    {
        List<Cell> firstGen = new List<Cell>();

        for(int i = 0; i < m_NumCells; i++)
        {
            firstGen.Add(new Cell(new Vector3(1.0f,1.0f,1.0f),new Color(0,0,0)));
        };

        m_cells.Add(firstGen);
    }

    Vector3 getDistinctAndRandomPos()
    {
        return new Vector3(0,0,0);
    }

    void AddNextGeneration()
    {

    }

    



}
