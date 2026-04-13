using UnityEngine;

public class disappearScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    FadeManager fadeManager;
    Timer timer;

    private void Start()
    {
        timer = GameObject.Find("Timer").GetComponent<Timer>();
        fadeManager = GameObject.Find("FadeManagerObject").GetComponent<FadeManager>();
    }


    private void Update()
    {
        if(Mathf.RoundToInt(timer.CurrentTime) == 175)
        {
            fadeManager.FadeOutObject(gameObject, 2f);
        }
    }
}
