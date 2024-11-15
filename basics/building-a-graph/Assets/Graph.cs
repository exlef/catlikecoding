using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] Transform pointPrefab;
    [SerializeField] float cubeCount = 10;

    void Start()
    {
        float range = 2;
        float factor = cubeCount / range;
        for (int i = 0; i < cubeCount; i++)
        {
            var go = Instantiate(pointPrefab, transform);

            go.localScale = Vector3.one / factor;
            go.localPosition = new Vector3(i / factor - 1, 0, 0) + new Vector3(go.localScale.x/2, 0, 0);
        }
    }
}
