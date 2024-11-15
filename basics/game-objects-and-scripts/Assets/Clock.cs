using System;
using Unity.VisualScripting;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] GameObject hourIndicator, hourIndicatorParent;
    [SerializeField] float radius = 4;
	[SerializeField] Transform hoursPivot, minutesPivot, secondsPivot;

    const float hoursToDegrees = -30f /* 360 / 12 */, minutesToDegrees = -6f /* 360 / 60 */, secondsToDegrees = -6f /* 360 / 60 */;
    
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

    void Update()
    {
        TimeSpan time = DateTime.Now.TimeOfDay;
        hoursPivot.localRotation = Quaternion.Euler(0, 0, (float)time.TotalHours * hoursToDegrees);
        minutesPivot.localRotation = Quaternion.Euler(0, 0, (float)time.TotalMinutes * minutesToDegrees);
        secondsPivot.localRotation = Quaternion.Euler(0, 0, (float)time.TotalSeconds * secondsToDegrees);
    }
}
