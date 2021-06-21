using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomatum : MonoBehaviour
{
    public List<Cell[,]> Cells = new List<Cell[,]>();
    List<Cell> m_renderingCells = new List<Cell>();
    public int width = 100;
    public int depth = 100;

    public int initialNumCells = 10;

    int m_generation = 1;

    public void Generate()
    {
        if(m_generation > Cells.Count)
        {
            AddNewGeneration();
        }
    }

    void AddNewGeneration()
    {
        if(m_generation == 1)
        {
            CreateInitialGeneration();
        }
        else
        {

        }
    }

    void CreateInitialGeneration()
    {
        Cell[,] initialGen = new Cell[width, depth];

        for(int i = 0; i < initialNumCells; i++)
        {
            int x = Random.Range(0, width);
            int z = Random.Range(0, depth);

            initialGen[x, z] = new Cell(new Vector3(x, 0, z), new Vector3(1, 1, 1), new Vector3(Random.Range(0.0f,1.0f), 0, 0));
        }

        Cells.Add(initialGen);

        m_renderingCells = FilteringRenderingCells(Cells);
        Debug.Log("Creating intial Gen");

    }

    public List<Cell> GetRenderingCells()
    {
        return m_renderingCells;
    }

    List<Cell> FilteringRenderingCells(List<Cell[,]> cells)
    {
        List<Cell> c = new List<Cell>();

        for(int i = 0; i < Cells.Count; i++)
        {
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < depth; y++)
                {
                    c.Add(cells[i][x,y]);
                }
            }
        }
        return c;
    }
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            m_generation++;
            Debug.Log("Adding new Generation " + m_generation);
        }
    }

}
