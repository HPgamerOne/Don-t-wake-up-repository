using TMPro;
using UnityEngine;

public class ShowWinLabel : MonoBehaviour
{
    InteractObject interactObject;
    public TMP_Text winLabel;

    private void Start()
    {
        interactObject = GetComponent<InteractObject>();
    }

    private void Update()
    {
        if (interactObject.hovering)
        {
            winLabel.text = "Win";
        } else
        {
            winLabel.text = "";
        }
    }
}
