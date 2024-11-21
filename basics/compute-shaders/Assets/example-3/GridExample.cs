using UnityEngine;

public class GridExample : MonoBehaviour
{
    [SerializeField] GameObject cubePrefab;
    [SerializeField] int width = 8;
    [SerializeField] int height = 8;
    GameObject[] cubes;

    void Start()
    {
        cubes = new GameObject[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var go = Instantiate(cubePrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                cubes[x * height + y] = go;
            }
        }
        
    }

    public void Clicked(GameObject cube)
    {
        Debug.Log("clicked");
        int clickedIndex = -1;
        for (int i = 0; i < height * width; i++)
        {
            if(cube == cubes[i])
            {
                Debug.Log("find index " + clickedIndex);
                clickedIndex = i;
            }
        }

        cubes[clickedIndex].GetComponent<Renderer>().material.color = Color.red;

        // do the dispatch and stuff...
    }
}
