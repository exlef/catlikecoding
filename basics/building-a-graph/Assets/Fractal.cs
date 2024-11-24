using UnityEngine;

public class Fractal : MonoBehaviour
{

	struct FractalPart {
		public Vector3 direction;
		public Quaternion rotation;
		public Transform transform;
	}

    [Range(1, 8)] public int depth = 4;
    [SerializeField] Mesh mesh;

	[SerializeField] Material material;

    static Vector3[] directions = {
		Vector3.up, Vector3.right, Vector3.left, Vector3.forward, Vector3.back
	};

	static Quaternion[] rotations = {
		Quaternion.identity,
		Quaternion.Euler(0f, 0f, -90f), Quaternion.Euler(0f, 0f, 90f),
		Quaternion.Euler(90f, 0f, 0f), Quaternion.Euler(-90f, 0f, 0f)
	};

    FractalPart[][] parts;

    void Start()
    {
        parts = new FractalPart[depth][];
        for (int i = 0, length = 1; i < parts.Length; i++, length *= 5)
        {
			parts[i] = new FractalPart[length];
		}

        float scale = 1;

        parts[0][0] = CreatePart(0, 0, scale);
        for (int li = 1; li < parts.Length; li++) // level index
        {
			FractalPart[] levelParts = parts[li];
            scale *= 0.5f;
			for (int fpi = 0; fpi < levelParts.Length; fpi += 5) // fractal part index
            {
                for (int ci = 0; ci < 5; ci++) // child index
                {
					levelParts[fpi + ci] = CreatePart(li, ci, scale);
				}
			}
		}
    }

    void Update()
    {
        Quaternion deltaRotation = Quaternion.Euler(0f, 22.5f * Time.deltaTime, 0f);
		
        FractalPart rootPart = parts[0][0];
        rootPart.rotation *= deltaRotation;
        parts[0][0] = rootPart;

        for (int li = 1; li < parts.Length; li++)
        {
			FractalPart[] parentParts = parts[li - 1];
			FractalPart[] levelParts = parts[li];
			for (int fpi = 0; fpi < levelParts.Length; fpi++)
            {
				FractalPart parentPart = parentParts[fpi / 5];
				FractalPart part = levelParts[fpi];
                
                // parts rotating around their up axis
                part.rotation *= deltaRotation;

                // Calculate the offset direction in parent's local space
                Vector3 offset = part.direction * 1.5f * part.transform.localScale.x;
                
                // Set position relative to parent
                part.transform.localPosition = parentPart.transform.localPosition + 
                    parentPart.transform.rotation * offset;
                
                // Set rotation by combining parent's rotation with child's rotation
                part.transform.rotation = parentPart.transform.rotation * part.rotation;
                
                // copy data back to array since we changed rotation value
                levelParts[fpi] = part;
			}
		}
    }

    FractalPart CreatePart(int levelIndex, int childIndex, float scale)
    {
        var go = new GameObject("levelIndex: " + levelIndex + " childIndex: " + childIndex);
        go.transform.SetParent(transform, false);
        go.AddComponent<MeshFilter>().mesh = mesh;
        go.AddComponent<MeshRenderer>().material = material;

        go.transform.localScale = scale * Vector3.one;

        return new FractalPart {
			direction = directions[childIndex],
			rotation = rotations[childIndex],
			transform = go.transform
		};
    }
}
