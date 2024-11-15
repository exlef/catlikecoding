using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] GameObject hourIndicator, hourIndicatorParent;
    [SerializeField] float radius = 4;
	[SerializeField] Transform hoursPivot, minutesPivot, secondsPivot;
    
    void Start()
    {
        for (int i = 0; i < 12; i++)
        {
            var go = Instantiate(hourIndicator, new Vector3(0, 0, -0.25f), Quaternion.identity, hourIndicatorParent.transform);
            float angle = (360/12) * i;
            
            go.transform.Rotate(Vector3.forward, angle);
            // go.transform.up is the local up direction 
            go.transform.localPosition = go.transform.up * radius;
        }
    }
}
