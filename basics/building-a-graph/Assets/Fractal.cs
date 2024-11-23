using UnityEngine;

public class Fractal : MonoBehaviour
{
    [Range(1, 8)] public int depth = 4;

    void Start()
    {
        name = "Fracta " + depth;
        
        if(depth <= 1) return;

        var go = Instantiate(this);
        go.depth = depth - 1;
        go.transform.SetParent(transform, false);
        go.transform.localPosition = Vector3.right;
        // go.name = 
    }
}
