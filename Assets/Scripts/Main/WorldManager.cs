using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{

    public ComputeShader RaymarchingShader;
    CellularAutomatum m_CellularAutomatumManager;

    Camera m_Cam;
    Light m_Light;
    RenderTexture m_Target;

    List<Cell> m_RenderingCells;
    ComputeBuffer m_CellBuffer;
    // Start is called before the first frame update

    void InitScene()
    {
        m_Cam = Camera.current;
        m_Light = FindObjectOfType<Light>();
    }

    void InitParameters()
    {
        //set parameter for shader
        bool lightIsDirectional = m_Light.type == LightType.Directional;
        RaymarchingShader.SetMatrix("_CameraToWorld", m_Cam.cameraToWorldMatrix);
        RaymarchingShader.SetMatrix("_CameraInverseProjection", m_Cam.projectionMatrix.inverse);
        RaymarchingShader.SetVector("_Light", (lightIsDirectional) ? m_Light.transform.forward : m_Light.transform.position);
        RaymarchingShader.SetBool("positionLight", !lightIsDirectional);
    }
    void InitRenderTexture()
    {
        if (m_Target == null || m_Target.width != m_Cam.pixelWidth || m_Target.height != m_Cam.pixelHeight)
        {
            if (m_Target != null)
            {
                m_Target.Release();
            }
            m_Target = new RenderTexture(m_Cam.pixelWidth, m_Cam.pixelHeight, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            m_Target.enableRandomWrite = true;
            m_Target.Create();
        }
    }

    void InitCells()
    {

        if(m_CellularAutomatumManager == null)
        {
            m_CellularAutomatumManager = transform.GetComponent<CellularAutomatum>();
        }

        m_CellularAutomatumManager.Generate();
        m_RenderingCells = m_CellularAutomatumManager.GetRenderingCells();

        m_CellBuffer = new ComputeBuffer(m_RenderingCells.Count, Cell.GetSize());
        m_CellBuffer.SetData(m_RenderingCells);
        RaymarchingShader.SetBuffer(0, "cells", m_CellBuffer);
        RaymarchingShader.SetInt("numCells", m_RenderingCells.Count);

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        InitScene();

        InitRenderTexture();
        InitParameters();
        InitCells();

        RaymarchingShader.SetTexture(0, "Source", source);
        RaymarchingShader.SetTexture(0, "Destination", m_Target);

        int threadGroupsX = Mathf.CeilToInt(m_Cam.pixelWidth / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(m_Cam.pixelHeight / 8.0f);
        RaymarchingShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        Graphics.Blit(m_Target, destination);

        m_CellBuffer.Dispose();
    }
}
