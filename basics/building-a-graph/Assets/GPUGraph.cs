using UnityEngine;

public class GPUGraph : MonoBehaviour
{
    [SerializeField] ComputeShader computeShader;
    const int maxResolution = 1000;
    [SerializeField, Range(10, maxResolution)] int resolution = 10;
    [SerializeField] FunctionLibrary.FunctionName function;
    [SerializeField] Material material;
	[SerializeField] Mesh mesh;
    ComputeBuffer positionsBuffer;
    bool isTransitioning;
    float transitionProgress;
    FunctionLibrary.FunctionName transitionFunction;
    float duration;
    float transitionDuration = 1;

	void Awake ()
    {
		positionsBuffer = new ComputeBuffer(maxResolution * maxResolution, 3 * sizeof(float));// we claim memory for maxResolution this way we can change the graph resolution in play mode.
	}

    void OnDestroy()
    {
        positionsBuffer.Release();
    }

    string GetKernelName()
    {
        if (!isTransitioning)
        {
            return function.ToString() + "Kernel";
        }
        return function.ToString() + "To" + transitionFunction.ToString() + "Kernel";
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S) && !isTransitioning)
        {
            isTransitioning = true;
            transitionFunction = FunctionLibrary.GetRandomFunctionNameOtherThan(function);
        }
        UpdateFunctionOnGPU();
    }

    void UpdateFunctionOnGPU ()
    {
        // var kernelIndex = (int)function + (int)(isTransitioning ? transitionFunction : function) * FunctionLibrary.FunctionCount;
        string kernelName = GetKernelName();
        int kernelIndex = computeShader.FindKernel(kernelName);

        Debug.Log($"Function: {function}, TransitionFunction: {transitionFunction}, IsTransitioning: {isTransitioning}, KernelIndex: {kernelIndex}");
		float step = 2f / resolution;
		computeShader.SetInt("_Resolution", resolution);
		computeShader.SetFloat("_Step", step);
		computeShader.SetFloat("_Time", Time.time);
        if (isTransitioning)
        {
			computeShader.SetFloat("_TransitionProgress", Mathf.SmoothStep(0f, 1f, duration / transitionDuration));
            duration += Time.deltaTime;
		}
        computeShader.SetBuffer(kernelIndex, "_Positions", positionsBuffer); // Its first argument is the index of the kernel function, because a compute shader can contain multiple kernels and buffers can be linked to specific ones. We could get the kernel index by invoking FindKernel on the compute shader, but our single kernel always has index zero so we can use that value directly.
        int groups = Mathf.CeilToInt(resolution / 8f);
		computeShader.Dispatch(kernelIndex, groups, groups, 1);

        material.SetBuffer("_Positions", positionsBuffer);
		material.SetFloat("_Step", step);

        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / resolution));
		Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, resolution * resolution);

        if(duration >= transitionDuration)
        {
            function = transitionFunction;
            duration = 0;
            isTransitioning = false;
        }
	}
}
