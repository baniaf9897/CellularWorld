using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class WorldMgr : MonoBehaviour
{

    Texture3D texture;
    // Start is called before the first frame update
    void Start()
    {
        UpdateTexture();
        Debug.Log("Texture Updated");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnValidate()
    {
        UpdateTexture();
        Debug.Log("Texture Updated");
    }
    void UpdateTexture()
    {
        CreateTexture3D();
        GetComponent<Renderer>().sharedMaterial.SetTexture("_CellularTex", texture);
    }
    void CreateTexture3D()
    {
        // Configure the texture
        int size = 32;
        TextureFormat format = TextureFormat.RGBA32;
        TextureWrapMode wrapMode = TextureWrapMode.Clamp;

        // Create the texture and apply the configuration
        texture = new Texture3D(size, size, size, format, false);
        texture.wrapMode = wrapMode;

        // Create a 3-dimensional array to store color data
        Color[] colors = new Color[size * size * size];

        // Populate the array so that the x, y, and z values of the texture will map to red, blue, and green colors
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    //float value = Get3DNoise(x, y * size, z * size * size, .001f);
                    Color c = new Color(1.0f, 1.0f, 0.5f, 0.0f);
                    colors[x + (y * size) + (z * size * size)] = c;
                }
            }
        }

        colors[300].a = 1.0f;
        colors[1324].a = 1.0f;


        // Copy the color values to the texture
        texture.SetPixels(colors);

        // Apply the changes to the texture and upload the updated texture to the GPU
        texture.Apply();

        // Save the texture to your Unity Project
        AssetDatabase.CreateAsset(texture, "Assets/3DTexture.asset");
    }

    float Get3DNoise(float x, float y, float z, float frequency)
    {
        float noiseXY = Mathf.PerlinNoise(x * frequency, y * frequency);
        float noiseXZ = Mathf.PerlinNoise(x * frequency, z * frequency);
        float noiseYZ = Mathf.PerlinNoise(y * frequency, z * frequency);

        // Reverse of the permutations of noise for each individual axis
        float noiseYX = Mathf.PerlinNoise(y * frequency, x * frequency);
        float noiseZX = Mathf.PerlinNoise(z * frequency, x * frequency);
        float noiseZY = Mathf.PerlinNoise(z * frequency, y * frequency);

        return (noiseXY + noiseXZ + noiseYZ + noiseYX + noiseZX + noiseZY) / 6.0f;
    }
}
