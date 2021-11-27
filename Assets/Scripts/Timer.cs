using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float elapsedTime = 0;
    private bool timerGoing = false;

    // Starts and resets the timer and 
    public void BeginTimer() {
        timerGoing = true;
        elapsedTime = 0;
        StartCoroutine(UpdateTimer());
    }

    // Ends the timer, we must call this method
    // Otherwise we will have multiple coroutines
    // Updating the same value, thus boosting elapsedTime too much
    public void EndTimer() {
        timerGoing = false;
    }

    // Updates elapsed time
    private IEnumerator UpdateTimer() {
        while(timerGoing) {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    // Get time passed
    public float GetElapsedTime() {
        return elapsedTime;
    }

    
}
