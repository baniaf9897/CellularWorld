using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public int m_NumCells = 1;
    public float m_CellSize = 1.0f;

    public Color m_CellColor = new Color(1, 1, 1);

    public GameObject WorldPlane = null;

    //Rendering
    public ComputeShader m_RaymarchingShader;
    Camera cam;
    Light light;
    RenderTexture target;

    List<List<Cell>> m_cells;
    List<Cell> m_RenderingCells;
    ComputeBuffer cellBuffer;
    // Start is called before the first frame update
    void InitScene()
    {
        cam = Camera.current;
        light = FindObjectOfType<Light>();
    }

    void InitParameters()
    {
        //set parameter for shader
        bool lightIsDirectional = light.type == LightType.Directional;
        m_RaymarchingShader.SetMatrix("_CameraToWorld", cam.cameraToWorldMatrix);
        m_RaymarchingShader.SetMatrix("_CameraInverseProjection", cam.projectionMatrix.inverse);
        m_RaymarchingShader.SetVector("_Light", (lightIsDirectional) ? light.transform.forward : light.transform.position);
        m_RaymarchingShader.SetBool("positionLight", !lightIsDirectional);
    }
    void InitRenderTexture()
    {
        if (target == null || target.width != cam.pixelWidth || target.height != cam.pixelHeight)
        {
            if (target != null)
            {
                target.Release();
            }
            target = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            target.enableRandomWrite = true;
            target.Create();
        }
    }

    void InitCells()
    {
        m_RenderingCells = new List<Cell>();

        Cell cell1 = new Cell(new Vector3(0,0,0), new Vector3(1, 1, 1), new Vector3(1.0f,0.0f,0.0f));
        Cell cell2 = new Cell(new Vector3(1.0f, 2.0f, 0.0f),new Vector3(1.0f, .5f, 1.0f), new Vector3(0.0f, 1.0f, 0.0f));
        Cell cell3 = new Cell(new Vector3(3.0f, 1.0f, 2.0f), new Vector3(1, 1, 2), new Vector3(.5f, .5f, 0.0f));
        Cell cell4 = new Cell(new Vector3(1.0f, 2.0f, 3.0f), new Vector3(1.4f, 1.3f, 1.0f), new Vector3(.0f, .5f, .5f));

        m_RenderingCells.Add(cell1);
        m_RenderingCells.Add(cell2);
        m_RenderingCells.Add(cell3);
        m_RenderingCells.Add(cell4);

        cellBuffer = new ComputeBuffer(m_RenderingCells.Count, Cell.GetSize());
        cellBuffer.SetData(m_RenderingCells);
        m_RaymarchingShader.SetBuffer(0, "cells", cellBuffer);
        m_RaymarchingShader.SetInt("numCells", m_RenderingCells.Count);

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        InitScene();
        //buffersToDispose = new List<ComputeBuffer>();

        InitRenderTexture();
        InitParameters();
        InitCells();

        m_RaymarchingShader.SetTexture(0, "Source", source);
        m_RaymarchingShader.SetTexture(0, "Destination", target);

        int threadGroupsX = Mathf.CeilToInt(cam.pixelWidth / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(cam.pixelHeight / 8.0f);
        m_RaymarchingShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        Graphics.Blit(target, destination);

        cellBuffer.Dispose();
        /* foreach (var buffer in buffersToDispose)
         {
             buffer.Dispose();
         }*/
    }


    void Start()
    {
       // m_cells = new List<List<Cell>>();

       // GenerateRandomCellDistribution();

    }




    // Update is called once per frame
    void Update()
    {

    }

    void GenerateRandomCellDistribution()
    {
        /*List<Cell> firstGen = new List<Cell>();

        for (int i = 0; i < m_NumCells; i++)
        {
            firstGen.Add(new Cell(new Vector3(1.0f, 1.0f, 1.0f), new Color(0, 0, 0)));
        };

        m_cells.Add(firstGen);*/
    }

    Vector3 getDistinctAndRandomPos()
    {
        return new Vector3(0, 0, 0);
    }

    void AddNextGeneration()
    {

    }





}
