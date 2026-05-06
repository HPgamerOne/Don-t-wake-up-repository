using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
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

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        mainCanvasObject = GameObject.Find("MainCanvas");
        timerManagerObject = GameObject.Find("TimerManager");
        timer = timerManagerObject.GetComponent<Timer>();
        timerPanels = GameObject.Find("TimerPanels");

        timerPanels.SetActive(false);
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
    }
}