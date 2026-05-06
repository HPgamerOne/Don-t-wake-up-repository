using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int currentScene = 0;

    public List<int> doneScenes = new List<int> {0};
    public List<int> uncompletedScenes = new List<int> {1, 2, 3};
    public List<int> noTimerScenes = new List<int> {0, 4};
    public int finalSceneIndex = 4;

    public GameObject mainCanvasObject;
    public GameObject timerManagerObject;
    Timer timer;
    public GameObject timerPanels;

    public GameObject cameraPivotObject;
    CameraController cameraController;
    GameObject cameraObject;
    Camera ssaoCamera;

    GameObject dontWakeUpObject;
    GameObject startButtonObject;
    GameObject quitButtonObject;
    TMP_Text dontWakeUpText;
    Image startButtonImage;
    Image quitButtonImage;

    private bool currentlyPaused = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Start()
    {
        mainCanvasObject = GameObject.Find("MainCanvas");
        timerManagerObject = GameObject.Find("TimerManager");
        timer = timerManagerObject.GetComponent<Timer>();
        timerPanels = GameObject.Find("TimerPanels");
        dontWakeUpObject = GameObject.Find("Don't wake up");
        startButtonObject = GameObject.Find("StartButton");
        quitButtonObject = GameObject.Find("QuitButton");
        dontWakeUpText = dontWakeUpObject.GetComponent<TMP_Text>();
        startButtonImage = startButtonObject.GetComponent<Image>();
        quitButtonImage = quitButtonObject.GetComponent<Image>();

        timerPanels.SetActive(false);
        StartMainMenu();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded");

        cameraPivotObject = GameObject.Find("Camera Pivot");
        cameraController = cameraPivotObject.GetComponent<CameraController>();
        cameraObject = GameObject.Find("SSAO Camera");
        ssaoCamera = cameraObject.GetComponent<Camera>();

        if (currentScene == 0)
        {
            cameraController.lockCamera = true;
        }
    }
    public void PauseGame(bool shouldPause)
    {
        if (shouldPause)
        {
            currentlyPaused = true;
            cameraController.lockCamera = shouldPause;
            Time.timeScale = 0;
        }
        else
        {
            currentlyPaused = false;
            cameraController.lockCamera = shouldPause;
            Time.timeScale = 1;
        }
    }

    public void StartMainMenu()
    {
        Debug.Log("Started Main Menu");
        SceneManager.LoadScene(0);

        doneScenes = new List<int> {0};
        uncompletedScenes = new List<int> {1, 2, 3};
    }

    public void StartGame()
    {
        cameraController.lockCamera = false;

        StartCoroutine(TweenMainMenuElements(70, 60, 1.5f));
    }

    public void QuitGame()
    {
        Debug.Log("Quitted game");
        Application.Quit();
    }

    private IEnumerator TweenMainMenuElements(float startValue, float endValue, float tweenTime)
    {
        float elapsedTime = 0f;

        Color dontWakeUpStartColor = dontWakeUpText.color;
        Color startButtonStartColor = startButtonImage.color;
        Color quitButtonStartColor = quitButtonImage.color;

        while (elapsedTime < tweenTime)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / tweenTime;

            ssaoCamera.fieldOfView = Mathf.Lerp(startValue, endValue, t);

            float alpha = Mathf.Lerp(1f, 0f, t);

            dontWakeUpText.color = new Color(dontWakeUpStartColor.r, dontWakeUpStartColor.g, dontWakeUpStartColor.b, alpha);
            startButtonImage.color = new Color(startButtonStartColor.r, startButtonStartColor.g, startButtonStartColor.b, alpha);
            quitButtonImage.color = new Color(quitButtonStartColor.r, quitButtonStartColor.g, quitButtonStartColor.b, alpha);

            yield return null;
        }

        ssaoCamera.fieldOfView = endValue;

        dontWakeUpText.color = new Color(dontWakeUpStartColor.r, dontWakeUpStartColor.g, dontWakeUpStartColor.b, 0f);
        startButtonImage.color = new Color(startButtonStartColor.r, startButtonStartColor.g, startButtonStartColor.b, 0f);
        quitButtonImage.color = new Color(quitButtonStartColor.r, quitButtonStartColor.g, quitButtonStartColor.b, 0f);
    }

    public void NextScene()
    {
        if (uncompletedScenes.Count > 0)
        {
            int randomIndex = Random.Range(0, uncompletedScenes.Count);
            currentScene = uncompletedScenes[randomIndex];

            doneScenes.Add(currentScene);
            uncompletedScenes.RemoveAt(randomIndex);

            Debug.Log($"Next scene: {currentScene}");
            Debug.Log($"Random index: {randomIndex}");

            SceneManager.LoadScene(currentScene);
        }
        else
        {
            SceneManager.LoadScene(finalSceneIndex);
        }

        if (noTimerScenes.Contains(currentScene))
        {
            timerPanels.SetActive(false);
            timer.StopTimer();
            Debug.Log("Timer panel false");
        }
        else
        {
            Debug.Log("Timer panel true");
            timerPanels.SetActive(true);
        }
    }

    // Temp code to make it faster to go to next scene, remove for final build
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            NextScene();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentlyPaused)
            {
                PauseGame(false);
            }
            else
            {
                PauseGame(true);
            }
        }
    }
}