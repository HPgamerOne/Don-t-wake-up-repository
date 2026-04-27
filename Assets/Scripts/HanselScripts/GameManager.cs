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
    public int finalSceneIndex = 4;
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