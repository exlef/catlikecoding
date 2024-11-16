using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] Transform pointPrefab;
    [SerializeField, Min(10)] int resolution = 10;

    void Start()
    {
        var range = 2;
        var factor = (float)resolution / range;
        var scale = Vector3.one / factor;
        var position = Vector3.zero;

        for (int i = 0; i < resolution; i++)
        {
            var go = Instantiate(pointPrefab, transform);

            go.localScale = scale;
            position.x = i / factor - 1 + go.localScale.x / 2;
            position.y = position.x * position.x * position.x;
            go.localPosition = position;
        }
    }
}
