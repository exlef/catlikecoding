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

        var range = 2;
        var factor = (float)resolution / range;
        var scale = Vector3.one / factor;
        var position = Vector3.zero;

        for (int i = 0; i < resolution; i++)
        {
            /*
            var go = Instantiate(pointPrefab, transform);
            
            go.localScale = scale;
            position.x = i / factor - 1 + go.localScale.x / 2;
            go.localPosition = position;

            points[i] = go;
            */
        }

        int index = 0;

        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                var go = Instantiate(pointPrefab, transform);

                go.localScale = scale;
                position.x = x / factor - 1 + go.localScale.x / 2;
                position.z = z / factor - 1 + go.localScale.z / 2;
                go.localPosition = position;

                points[index] = go;

                index++;
            }
        }
    }

    void Update () 
    {
        var time = Time.time;
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function); 
		
        for (int i = 0; i < points.Length; i++)
        {
            var point = points[i];
            var position = point.transform.localPosition;

            position.y = f(position.x, position.z, time);

            point.localPosition = position;
        }
    }
}
