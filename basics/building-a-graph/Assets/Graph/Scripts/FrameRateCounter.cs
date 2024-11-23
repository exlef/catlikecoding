using UnityEngine;
using TMPro;

public class FrameRateCounter : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI display;
    public enum DisplayMode { FPS, MS }
	[SerializeField] DisplayMode displayMode = DisplayMode.FPS;
    [SerializeField,  Range(0.1f, 2f)] float sampleDuration = 1f;

    float duration;
    float bestDuration = float.MaxValue;
    float worstDuration;
    float frames;

    void Update()
    {
        float frameDuration = Time.unscaledDeltaTime;
        frames += 1;
        duration += frameDuration;

        if(frameDuration < bestDuration)
        {
            bestDuration = frameDuration;
        }
        if(frameDuration > worstDuration)
        {
            worstDuration = frameDuration;
        }

        if(duration >= sampleDuration)
        {
            if(displayMode == DisplayMode.FPS)
            {
    		    display.SetText($"FPS\n{1f / bestDuration:0}\n{frames / duration:0}\n{1f / worstDuration:0}");
            }
            else 
            {
				display.SetText($"MS\n{1000f * bestDuration:0.0}\n{1000f * duration / frames:0.0}\n{1000f * worstDuration:0.0}");
			}

            frames = 0;
            duration = 0;
            bestDuration = float.MaxValue;
            worstDuration = 0;
        }
    }
}