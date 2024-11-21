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
                cubes[MapTo1D(x, y)] = go;
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

        int x = clickedIndex % width;
        int y = (clickedIndex - x) / width;
        Debug.Log($"{clickedIndex} {x}, {y}");


        cubes[clickedIndex].GetComponent<Renderer>().material.color = Color.red;

        int right = MapTo1D(x + 1, y);
        int left = MapTo1D(x - 1, y);
        int up = MapTo1D(x, y + 1);
        int down = MapTo1D(x, y - 1);


        if(InRange(right)) cubes[right].transform.localPosition += new Vector3(0,1,0);
        if(InRange(left))  cubes[left].transform.localPosition += new Vector3(0,1,0);
        if(InRange(up))    cubes[up].transform.localPosition += new Vector3(0,1,0);
        if(InRange(down))  cubes[down].transform.localPosition += new Vector3(0,1,0);

        // do the dispatch and stuff...
    }

    int MapTo1D(int x, int y)
    {
        if(InRange(x, y) == false) return -1;
        return y * width + x;
    }

    bool InRange(int x)
    {
        Debug.Log(x);
        return x >= 0 && x < width * height;
    }

    bool InRange(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
}
