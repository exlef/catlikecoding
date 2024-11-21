using System.Collections;
using UnityEngine;

public class GridExample : MonoBehaviour
{
    [SerializeField] GameObject cubePrefab;
    [SerializeField] int width = 8;
    [SerializeField] int height = 8;
    GameObject[] cubes;

    IEnumerator Start()
    {
        cubes = new GameObject[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var go = Instantiate(cubePrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                cubes[y * width + x] = go;
                yield return null;
            }
        }
        
    }

    public void Clicked(GameObject cube)
    {
        int clickedIndex = -1;
        for (int i = 0; i < height * width; i++)
        {
            if(cube == cubes[i])
            {
                clickedIndex = i;
            }
        }

        float x = clickedIndex % width;
        float y = (clickedIndex - x) / width;
        Debug.Log($"{clickedIndex} {x}, {y}");


        cubes[clickedIndex].GetComponent<Renderer>().material.color = Color.red;

        // do the dispatch and stuff...
    }
}
