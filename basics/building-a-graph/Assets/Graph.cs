using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] Transform pointPrefab;
    [SerializeField, Min(10)] int resolution = 10;
    [SerializeField] FunctionLibrary.FunctionName function;

    Transform[] points;

    void Start()
    {
        points = new Transform[resolution * resolution];

        var range = 2f; // the max distance of the area that the cubes will be in
        var factor = resolution / range;
        var scale = Vector3.one / factor;

        for (int i = 0; i < points.Length; i++)
        {
            var go = Instantiate(pointPrefab, transform);
            go.localScale = scale;
            points[i] = go;
        }
    }

    void Update()
    {
        var time = Time.time;
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);
        float step = 2f / resolution;
        
        for (int i = 0; i < points.Length; i++)
        {
            var point = points[i];
            // place cubes in a way that will form a grid which dimensions are 2x2 at XZ plane.
            var x = ((i % resolution) + 0.5f) * step - 1f;
            var z = ((i / resolution) + 0.5f) * step - 1f;
            // calculate y position based on cube's x,z and the time passed since the game starts.
            point.localPosition = f(x, z, time);
            // point.localPosition = new Vector3(x, 0, z);
        }
    }
}
