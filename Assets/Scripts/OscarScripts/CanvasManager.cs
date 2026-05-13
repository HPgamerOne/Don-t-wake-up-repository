using System.Collections;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;

    public CanvasGroup mainMenuGroup;
    public CanvasGroup pauseMenuGroup;
    public CanvasGroup timerPanelsGroup;
    public CanvasGroup WinMenuGroup;
    public CanvasGroup LoseMenuGroup;

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

    private void ShowGroup(CanvasGroup group, bool shouldShow)
    {
        group.alpha = shouldShow ? 1f : 0f;
        group.interactable = shouldShow;
        group.blocksRaycasts = shouldShow;
    }

    public void ShowWinMenu(bool shouldShow)
    {
        ShowGroup(WinMenuGroup, shouldShow);
    }
    public void ShowLoseMenu(bool shouldShow)
    {
        ShowGroup(LoseMenuGroup, shouldShow);
    }

    public void ShowMainMenu(bool shouldShow)
    {
        ShowGroup(mainMenuGroup, shouldShow);
    }

    public void ShowPauseMenu(bool shouldShow)
    {
        ShowGroup(pauseMenuGroup, shouldShow);
    }

    public void ShowTimerPanels(bool shouldShow)
    {
        ShowGroup(timerPanelsGroup, shouldShow);
    }

    public void ResetForMainMenu()
    {
        ShowMainMenu(true);
        ShowPauseMenu(false);
        ShowTimerPanels(false);
        ShowWinMenu(false);
        ShowLoseMenu(false);
    }
}