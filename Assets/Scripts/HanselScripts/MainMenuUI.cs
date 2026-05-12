using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}