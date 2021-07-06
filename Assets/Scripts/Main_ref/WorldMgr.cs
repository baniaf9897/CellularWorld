using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WorldMgr : MonoBehaviour
{
    public ComputeShader CA;
    CellularAutomatum m_CellularAutomatumManager;
    public bool trigger = false;

    void Update()
    {
        m_CellularAutomatumManager = transform.GetComponent<CellularAutomatum>();
        if (!m_CellularAutomatumManager.initialized || m_CellularAutomatumManager.generateNewGen)
        {
            UpdateTexture();
        };
    }

    private void OnValidate()
    {
        Debug.Log("Updatetexture");
        UpdateTexture();
    }
    void UpdateTexture()
    {
        InitCA();
        SetTexture();
    }
    public void SetTexture()
    {
        m_CellularAutomatumManager = transform.GetComponent<CellularAutomatum>();
        GetComponent<Renderer>().sharedMaterial.SetTexture("_CellularTex", m_CellularAutomatumManager.automatum);
    }
   
    void ComputeCA()
    {
        int threadGroupsX = Mathf.CeilToInt(m_CellularAutomatumManager.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(m_CellularAutomatumManager.height / 8.0f);
        int threadGroupsZ = Mathf.CeilToInt(m_CellularAutomatumManager.depth / 8.0f);

        CA.Dispatch(0, threadGroupsX, threadGroupsY, threadGroupsZ);
    }

    void InitCA()
    {         
        m_CellularAutomatumManager = transform.GetComponent<CellularAutomatum>();
       
        if(m_CellularAutomatumManager.m_generation == 0)
            m_CellularAutomatumManager.InitTexture();

        CA.SetTexture(0, "Automatum", m_CellularAutomatumManager.automatum);
        CA.SetInt("width", m_CellularAutomatumManager.width);
        CA.SetInt("depth", m_CellularAutomatumManager.depth);
        CA.SetInt("height", m_CellularAutomatumManager.height);
        CA.SetInt("currentLayer", m_CellularAutomatumManager.m_generation);

        ComputeCA();

        Debug.Log("Current Layer" + m_CellularAutomatumManager.m_generation);
        
    }
}