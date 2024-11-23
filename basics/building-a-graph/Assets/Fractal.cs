using UnityEngine;

public class Fractal : MonoBehaviour
{
    [Range(1, 8)] public int depth = 4;

    void Start()
    {
        name = "Fractal " + depth;

        if(depth <= 1) return;

        var child1 = CreateChild(Vector3.right);
        var child2 = CreateChild(Vector3.up);

        child1.transform.SetParent(transform, false);
        child2.transform.SetParent(transform, false);
    }

    Fractal CreateChild(Vector3 direction)
    {
        var child = Instantiate(this);
        child.depth = depth - 1;
        child.transform.localPosition = 0.75f * direction;
        child.transform.localScale = 0.5f * Vector3.one; 
        return child;
    }
}
