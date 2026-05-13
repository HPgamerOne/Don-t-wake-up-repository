using TMPro;
using UnityEngine;

public class ShowWinLabel : MonoBehaviour
{
    InteractObject interactObject;
    public TMP_Text winLabel;

    bool hasWon = false;
    private void Start()
    {
        interactObject = GetComponent<InteractObject>();
    }

    private void Update()
    {
        if (interactObject.hovering && !hasWon)
        {
            winLabel.text = "Don't wake up";
        } else
        {
            winLabel.text = "";
        }

        if (interactObject.interacted && !hasWon)
        {
            hasWon = true;
            GameManager.Instance.StartWin();
        }
    }
}
