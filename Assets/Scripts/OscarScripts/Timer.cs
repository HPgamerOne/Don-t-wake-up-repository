using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    

    [Header("Images")]
    [SerializeField] Image fillImage;
    [SerializeField] GameObject openEye;
    [SerializeField] GameObject halfOpenEye1;
    [SerializeField] GameObject halfOpenEye2;
    [SerializeField] GameObject closedEye;
    [SerializeField] GameObject bar;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI timeText;
    FadeManager fadeManager;

    [Header("Values")]
    float remainingTime;
    float multiplier = 1;
    [SerializeField] float startTime;

    bool timerRunning = false;
    bool threshold1, threshold2, threshold3 = false;
    void Start()
    {
        remainingTime = startTime;
        // closedEye.gameObject.SetActive(true);
        halfOpenEye1.gameObject.SetActive(false);
        openEye.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);
        fadeManager = GameObject.Find("FadeManagerObject").GetComponent<FadeManager>();
    }
    void Update()
    {
        if (timerRunning)
        {
            DecreaseTime();
            if (remainingTime <= startTime / 2 && !threshold1)
            {
                threshold1 = true;
                closedEye.gameObject.SetActive(false);
                halfOpenEye1.gameObject.SetActive(true);

            }
            else if (remainingTime <= 60 && !threshold2)
            {
                threshold2 = true;
                halfOpenEye1.gameObject.SetActive(false);
                halfOpenEye2.gameObject.SetActive(true);
            }
            else if (remainingTime <= 30 && !threshold3)
            {
                threshold3 = true;
                halfOpenEye2.gameObject.SetActive(false);
                openEye.gameObject.SetActive(true);
            }

            if (remainingTime <= 0)
            {
                //Debug.Log("u lose lmao");
                StopTimer();
                fillImage.fillAmount = 0;
                //lose scene
            }
        }
    }
    /// <summary>
    /// Decrease the remaining time value
    /// </summary>
    private void DecreaseTime()
    {
        remainingTime -= Time.deltaTime * multiplier;
        timeText.text = Mathf.RoundToInt(remainingTime).ToString();
        UpdateProgressValue();
    }
    /// <summary>
    /// Progress bar rörelse shuma shuma
    /// </summary>
    private void UpdateProgressValue()
    {
        fillImage.fillAmount = remainingTime / startTime;
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

        bar.gameObject.SetActive(true);
        closedEye.gameObject.SetActive(true);
        fillImage.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
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
        openEye.gameObject.SetActive(false);
        halfOpenEye1.gameObject.SetActive(false);
        closedEye.gameObject.SetActive(true);
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
