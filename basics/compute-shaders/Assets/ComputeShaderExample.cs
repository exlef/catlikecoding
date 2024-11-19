using UnityEngine;

public class ComputeShaderExample : MonoBehaviour
{
    [SerializeField] ComputeShader computeShader;
    ComputeBuffer dataBuffer;
    const int bufferSize = 1024;
    const int threadGroupSize = 64;

    void Start()
    {
        dataBuffer = new ComputeBuffer(bufferSize, sizeof(float));

        float[] data = new float[bufferSize];
        for (int i = 0; i < bufferSize; i++)
        {
            data[i] = 1;
        }
        dataBuffer.SetData(data);

        int kernelIndex = computeShader.FindKernel("CSMain");
        computeShader.SetBuffer(kernelIndex, "_DataBuffer", dataBuffer);

        int threadGroupsX = Mathf.CeilToInt((float)bufferSize / threadGroupSize);
        int threadGroupsY = 1;
        int threadGroupsZ = 1;

        computeShader.Dispatch(kernelIndex, threadGroupsX, threadGroupsY, threadGroupsZ);

        float[] resultData = new float[bufferSize];
        dataBuffer.GetData(resultData);
        for (int i = 0; i < 10; i++)
        {
            Debug.Log($"Result {i}: {resultData[i]}");
        }
    }
}
