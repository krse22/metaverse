using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSTracker : MonoBehaviour
{
    public float AverageTrackedFPS { get; private set; }

    private float deltaTime = 0.0f;

    private float currentFPS;
    
    private bool isTracking = false;
    private int trackedCount = 0;
    private float trackedSum = 0;

    public void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        currentFPS = 1f / deltaTime;

        if (isTracking)
        {
            trackedCount++;
            trackedSum += currentFPS;

            AverageTrackedFPS = trackedSum / trackedCount;
        }
    }

    public void StartTracking()
    {
        trackedCount = 0;
        trackedSum = 0;
        isTracking = true;
    }

    public void StopTracking()
    {
        isTracking = false;
        AverageTrackedFPS = trackedSum / trackedCount;
    }

    public IEnumerator WaitForFPSToStabilize()
    {
        float firstFps;
        float secondFps;

        do
        {
            firstFps = currentFPS;

            yield return new WaitForSeconds(0.4f);

            secondFps = currentFPS;

        } while (Mathf.Abs(firstFps - secondFps) > 2);

        yield return new WaitForSeconds(0.2f);
    }
}
