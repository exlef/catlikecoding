using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] GameObject hourIndicator;
    [SerializeField] Transform hourIndicatorParent;
    [SerializeField] float radius = 4;
    
    void Start()
    {
        for (int i = 0; i < 12; i++)
        {
            var go = Instantiate(hourIndicator, new Vector3(0, 0, -0.25f), Quaternion.identity, hourIndicatorParent);
            float angle = (360/12) * i;
            
            go.transform.Rotate(Vector3.forward, angle);
            // go.transform.up is the local up direction 
            go.transform.localPosition = go.transform.up * radius;
        }
    }
}

//  .NET 8.0.11~arm64 executable path: /Users/amedduman/Library/Application Support/Code/User/globalStorage/ms-dotnettools.vscode-dotnet-runtime/.dotnet/8.0.11~arm64/dotnet
