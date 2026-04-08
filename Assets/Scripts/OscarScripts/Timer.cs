using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float startTime;
    float remainingTime;
    float multiplier = 1;

    bool timerRunning = false;
    bool threshold1, threshold2 = false;
    void Start()
    {
        remainingTime = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerRunning)
        {
            DecreaseTime();
            if(remainingTime <= startTime/2 && !threshold1)
            {
                threshold1 = true;
            }
            else if(remainingTime <= 60 && !threshold2)
            {
                threshold2 = true;
            }

            if(remainingTime <= 0)
            {
                Debug.Log("u lose lmao");
            }
        }
    }

    private void DecreaseTime()
    {
        remainingTime -= Time.deltaTime * multiplier;
    }
    /// <summary>
    /// Change how fast the timer runs out
    /// </summary>
    /// <param name="changeRate">The multipler to increase or decrease the rate of change on the timer</param>
    public void ChangeTimerRate(float changeRate)
    {
        multiplier = changeRate;
    }

    /// <summary>
    /// Stop the timer
    /// </summary>
    public void StopTimer()
    {
        timerRunning = false;
    }

    /// <summary>
    /// Start the timer
    /// </summary>
    public void StartTimer()
    {
        timerRunning = true;
    }

    /// <summary>
    /// Reset the timers values to it's starting values
    /// </summary>
    public void ResetTimer()
    {
        multiplier = 1;
        timerRunning = false;
        threshold1 = false;
        threshold2 = false;
        remainingTime = startTime;
    }
    /// <summary>
    /// Get the remaining time on the timer
    /// </summary>
    public float CurrentTime 
    {
        get { return remainingTime; }
    }
}
