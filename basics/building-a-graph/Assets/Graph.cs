using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] Transform pointPrefab;
    [SerializeField, Min(10)] int resolution = 10;

    Transform[] points;

    void Start()
    {
        points = new Transform[resolution];

        var range = 2;
        var factor = (float)resolution / range;
        var scale = Vector3.one / factor;
        scale.y = 1;
        var position = Vector3.zero;

        for (int i = 0; i < resolution; i++)
        {
            var go = Instantiate(pointPrefab, transform);
            
            go.localScale = scale;
            position.x = i / factor - 1 + go.localScale.x / 2;
            go.localPosition = position;

            points[i] = go;
        }
    }

    void Update () 
    {
        var time = Time.time;
		for (int i = 0; i < points.Length; i++)
        {
            var point = points[i];
            var position = point.transform.localPosition;
            
            position.y = Mathf.Sin(Mathf.PI * (position.x + time));
            point.localPosition = position;
        }
	}
}
