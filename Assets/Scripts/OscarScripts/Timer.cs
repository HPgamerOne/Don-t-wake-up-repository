using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    public static Timer Instance;

    [Header("Libraries")]
    [SerializeField] AudioLibrary library;
    [Header("Images")]
    [SerializeField] Image fillImage;
    [SerializeField] GameObject openEye;
    [SerializeField] GameObject halfOpenEye1;
    [SerializeField] GameObject halfOpenEye2;
    [SerializeField] GameObject closedEye;
    [SerializeField] GameObject timerPanel;
    [SerializeField] GameObject totalTimePanel;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI totalTimeText;

    [Header("Values")]
    float remainingTime;
    // float multiplier = 1;
    public float startTime;
    private float totalTime = 0;

    bool timerRunning = false;
    bool threshold1, threshold2, threshold3 = false;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject.transform.root);
    }

    void Start()
    {
        remainingTime = startTime;
        ResetEyeUI();

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
            else if (remainingTime <= startTime / 3 && !threshold2)
            {
                threshold2 = true;
                halfOpenEye1.gameObject.SetActive(false);
                halfOpenEye2.gameObject.SetActive(true);
            }
            else if (remainingTime <= startTime / 4 && !threshold3)
            {
                threshold3 = true;
                halfOpenEye2.gameObject.SetActive(false);
                openEye.gameObject.SetActive(true);
                SoundFXManager.Instance.PlaySoundEffect(library.warning, 1f);
            }

            if (remainingTime <= 0)
            {
                //Debug.Log("u lose lmao");
                StopTimer();
                fillImage.fillAmount = 0;
                //FadeManager.Instance.FadeToBlack(2f);
                GameManager.Instance.StartLose();
                //lose scene
            }
        }
    }
    /// <summary>
    /// Decrease the remaining time value
    /// </summary>
    private void DecreaseTime()
    {
        remainingTime -= Time.deltaTime; // * multiplier;
        totalTime += Time.deltaTime;

        timeText.text = Mathf.Round(remainingTime).ToString();
        totalTimeText.text = Mathf.Round(totalTime).ToString();
        UpdateProgressValue();
    }
    /// <summary>
    /// Progress bar rörelse shuma shuma
    /// </summary>
    private void UpdateProgressValue()
    {
        fillImage.fillAmount = remainingTime / startTime;
    }
    private void ResetEyeUI()
    {
        closedEye.gameObject.SetActive(true);
        halfOpenEye1.gameObject.SetActive(false);
        halfOpenEye2.gameObject.SetActive(false);
        openEye.gameObject.SetActive(false);
    }
    public void StopTimer()
    {
        timerRunning = false;
        CanvasManager.Instance.ShowTimerPanels(false);

    }
    /// <summary>
    /// Start the timer
    /// </summary>
    public void StartTimer()
    {
        CanvasManager.Instance.ShowTimerPanels(true);
        timerRunning = true;
    }
    /// <summary>
    /// Reset the timers values to it's starting values
    /// </summary>
    public void ResetTimer()
    {
        // multiplier = 1;
        timerRunning = false;
        threshold1 = false;
        threshold2 = false;
        threshold3 = false;
        remainingTime = startTime;
        ResetEyeUI();
    }


    public void ResetTotalTime()
    {
        totalTime = 0;
    }
    /// <summary>
    /// Get the remaining time on the timer
    /// </summary>
    public float CurrentTime 
    {
        get { return remainingTime; }
    }
    public float TotalTime 
    {
        get { return totalTime; }  
    }
}
