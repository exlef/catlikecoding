using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionLibrary
{
    public delegate float Function(float x, float t);
    public enum FunctionName {Wave, MultiWave, Ripple}
    static Function[] functions = {Wave, MultiWave, Ripple};
    
    public static Function GetFunction(FunctionName functionName)
    {
        return functions[(int)functionName];
    }

    public static float Wave(float x, float t)
    {
        return Sin(PI * (x + t));
    }

    public static float MultiWave(float x, float t)
    {
        float y = Sin(PI * (x + t));
		y += Sin(2f * PI * (x + t)) / 2f;
		return y / 1.5f;
    }

    public static float Ripple (float x, float t)
    {
		float d = Abs(x);
        float y = Sin(PI * (4 * d - t));
		return y / (1 + 10 * d);
	}
}

