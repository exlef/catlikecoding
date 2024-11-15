using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] Transform pointPrefab;
    [SerializeField] float cubeCount = 10;

    void Start()
    {
        var range = 2;
        var factor = cubeCount / range;
        var scale = Vector3.one / factor;
        var position = Vector3.zero;
        
        for (int i = 0; i < cubeCount; i++)
        {
            var go = Instantiate(pointPrefab, transform);

            go.localScale = scale;
            position.x = i / factor - 1 + go.localScale.x / 2;
            go.localPosition = position;
        }
    }
}
