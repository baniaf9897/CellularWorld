using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomatum : MonoBehaviour
{
    public List<Cell[,]> Cells = new List<Cell[,]>();

    public RenderTexture automatum;

    List<Cell> m_renderingCells = new List<Cell>();
    public int width = 100;
    public int depth = 100;

    public int height = 20;

    public int initialNumCells = 10;

    public int m_generation = 0;
    public bool initialized = false;

    private void OnValidate()
    {
        initialized = false;
    }
    public void InitTexture()
    {
        automatum = new RenderTexture(width, height, depth, RenderTextureFormat.ARGB32);
        automatum.enableRandomWrite = true;
        automatum.depth = 0;
        automatum.dimension = UnityEngine.Rendering.TextureDimension.Tex3D;
        automatum.Create();

        initialized = true;
    }

    public void Generate()
    {
        if(m_generation > Cells.Count)
        {
            Debug.Log("New Generation");
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
            //Ruleset for additional generations....
        }
    }

    void CreateInitialGeneration()
    {
        Cell[,] initialGen = new Cell[width, depth];

        for(int i = 0; i < initialNumCells; i++)
        {
            int x = Random.Range(0, width);
            int z = Random.Range(0, depth);

            initialGen[x, z] = new Cell(new Vector3(x, 0, z), new Vector3(1, 1, 1), new Vector3(1, 0, 0));
        }

        Cells.Add(initialGen);

        m_renderingCells = FilteringRenderingCells(Cells);
        Debug.Log("Creating intial Gen with NumCells : " +  m_renderingCells.Count);

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
                    if(cells[i][x,y].color.x == 1)
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
