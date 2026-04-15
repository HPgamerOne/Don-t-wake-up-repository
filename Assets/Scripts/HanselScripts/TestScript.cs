using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour
{
    [SerializeField] InputActionReference something;

    private void Update()
    {
        if (something.action.IsPressed())
        {
            print("Pressed");
            SceneManager.LoadScene(1);
        }
    }
}
