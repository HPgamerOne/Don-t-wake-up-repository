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

    Timer timer;

    CameraController cameraController;

    private bool inMainMenu = true;
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
        currentScene = SceneManager.GetActiveScene().buildIndex;
        BindSceneReferences();

        if (currentScene == 0)
        {
            EnterMainMenu();
        }
    }

    private void BindSceneReferences()
    {
        timer = GameObject.Find("TimerManager").GetComponent<Timer>();
        cameraController = GameObject.Find("Camera Pivot").GetComponent<CameraController>();
    }

    private void EnterMainMenu()
    {
        StopAllCoroutines();

        inMainMenu = true;
        currentlyPaused = false;

        Time.timeScale = 0;
        cameraController.lockCamera = true;

        timer.StopTimer();
        CanvasManager.Instance.ResetForMainMenu();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;

        BindSceneReferences();

        currentlyPaused = false;
        CanvasManager.Instance.ShowPauseMenu(false);

        if (currentScene == 0)
        {
            EnterMainMenu();
        }
        else
        {
            inMainMenu = false;
            Time.timeScale = 1;
            cameraController.lockCamera = false;

            if (noTimerScenes.Contains(currentScene))
            {
                //CanvasManager.Instance.ShowTimerPanels(false);
                timer.StopTimer();
            }
            else
            {
                //CanvasManager.Instance.ShowTimerPanels(true);
            }
        }
    }


    public void PauseGame(bool shouldPause)
    {
        if (inMainMenu)
        {
            return;
        }

        currentlyPaused = shouldPause;
        CanvasManager.Instance.ShowPauseMenu(shouldPause);
        cameraController.lockCamera = shouldPause;
        Time.timeScale = shouldPause ? 0 : 1;
    }

    public void StartMainMenu()
    {
        SoundFXManager.Instance.StopBackgroundMusic();
        StopAllCoroutines();

        Time.timeScale = 1;

        SceneManager.sceneLoaded -= OnSceneLoaded;
        Instance = null;

        Destroy(gameObject);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void StartGame()
    {
        Timer.Instance.ResetTimer();
        StopAllCoroutines();

        inMainMenu = false;
        Time.timeScale = 1;
        cameraController.lockCamera = false;

        StartCoroutine(FadeMainMenu(1.5f));
    }

    public void QuitGame()
    {
        Debug.Log("Quitted game");
        Application.Quit();
    }

    private IEnumerator FadeMainMenu(float fadeTime)
    {
        float elapsedTime = 0f;
        CanvasGroup mainMenuGroup = CanvasManager.Instance.mainMenuGroup;

        mainMenuGroup.interactable = false;
        mainMenuGroup.blocksRaycasts = false;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            mainMenuGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
            yield return null;
        }

        CanvasManager.Instance.ShowMainMenu(false);
    }

    public void StartLose()
    {
        CanvasManager.Instance.ShowLoseMenu(true);
        cameraController.lockCamera = true;
    }

    public void StartWin()
    {
        CanvasManager.Instance.ShowWinMenu(true);
        cameraController.lockCamera = true;
        GameObject totalTimeTextObject = GameObject.Find("TotalTimeText");
        TMP_Text totalTimeText = totalTimeTextObject.GetComponent<TMP_Text>();
        totalTimeText.text = $"Total Time: {Mathf.Floor(Timer.Instance.TotalTime)} seconds";
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

        Time.timeScale = 1;
        currentlyPaused = false;
        inMainMenu = false;
    }

    // Temp code to make it faster to go to next scene, remove for final build
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            NextScene();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !inMainMenu)
        {
            PauseGame(!currentlyPaused);
        }
    }
}