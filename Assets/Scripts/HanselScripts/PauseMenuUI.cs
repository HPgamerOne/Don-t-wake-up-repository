using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    public void ResumeGame()
    {
        GameManager.Instance.PauseGame(false);
    }

    public void ReturnToMainMenu()
    {
        GameManager.Instance.StartMainMenu();
    }
}