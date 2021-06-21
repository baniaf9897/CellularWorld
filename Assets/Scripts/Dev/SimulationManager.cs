using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
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

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        InitScene();
        //buffersToDispose = new List<ComputeBuffer>();

        InitRenderTexture();
        InitParameters();

        m_RaymarchingShader.SetTexture(0, "Source", source);
        m_RaymarchingShader.SetTexture(0, "Destination", target);

        int threadGroupsX = Mathf.CeilToInt(cam.pixelWidth / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(cam.pixelHeight / 8.0f);
        m_RaymarchingShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        Graphics.Blit(target, destination);

       /* foreach (var buffer in buffersToDispose)
        {
            buffer.Dispose();
        }*/
    }


    void Start()
    {

    }

   


    // Update is called once per frame
    void Update()
    {
        
    }

   

    Vector3 getDistinctAndRandomPos()
    {
        return new Vector3(0,0,0);
    }

    void AddNextGeneration()
    {

    }

    



}
