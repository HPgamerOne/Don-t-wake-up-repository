using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    public void ResumeGame()
    {
        GameManager.Instance.PauseGame(false);
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to main menu");
        GameManager.Instance.StartMainMenu();
    }
}