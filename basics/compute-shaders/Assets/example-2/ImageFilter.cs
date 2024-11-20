using UnityEngine;

public class ImageFilter : MonoBehaviour
{
    [SerializeField] Texture2D texture;
    [Space]
    [SerializeField] ComputeShader computeShader;
    ComputeBuffer dataBuffer;
    int bufferSize;
    const int threadGroupSize = 64;

    static readonly int 
        bufferSizeId = Shader.PropertyToID("_BufferSize"),
        dataBufferId = Shader.PropertyToID("_DataBuffer"),
        widthId = Shader.PropertyToID("_Width"),
        heightId = Shader.PropertyToID("_Height");
    static readonly string kernelName = "CSMain";

    void Start()
    {
        bufferSize = texture.width * texture.height * 3;
        dataBuffer = new ComputeBuffer(bufferSize, sizeof(float) * 3);

        float[] data = new float[bufferSize];        
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                int index = (y * texture.width + x) * 3;
                Color color = texture.GetPixel(x, y);
                data[index]     = color.r;
                data[index + 1] = color.g;
                data[index + 2] = color.b;
            }
        }
        
        dataBuffer.SetData(data);

        int kernelIndex = computeShader.FindKernel(kernelName);
        computeShader.SetInt(bufferSizeId, bufferSize);
        computeShader.SetInt(heightId, texture.height);
        computeShader.SetInt(widthId, texture.width);
        computeShader.SetBuffer(kernelIndex, dataBufferId, dataBuffer);

        int threadGroupsX = Mathf.CeilToInt((float)bufferSize / threadGroupSize);
        int threadGroupsY = 1;
        int threadGroupsZ = 1;

        computeShader.Dispatch(kernelIndex, threadGroupsX, threadGroupsY, threadGroupsZ);

        float[] resultData = new float[bufferSize];
        dataBuffer.GetData(resultData);
        
        for (int i = 0; i < bufferSize / 100; i++)
        {
            Debug.Log($"Result {i}: {resultData[i]}");
        }
    }

    void ApplyBlackAndWhiteFilter()
    {
        Debug.Log("start");
        if (!texture.isReadable)
        {
            Debug.LogError("Texture is not readable. Please check the import settings.");
        }

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                Color pixelColor = texture.GetPixel(x, y);
                float average = (pixelColor.r + pixelColor.g + pixelColor.b) / 3;
                Color newColor = new Color(average, average, average, 1);

                texture.SetPixel(x, y, newColor);
            }
        }

        texture.Apply();
        Debug.Log("end");
    }

    void OnDestroy()
    {
        if (dataBuffer != null)
        {
            dataBuffer.Release();
        }
    }
}
