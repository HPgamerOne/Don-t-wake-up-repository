using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSounds : MonoBehaviour
{

    [SerializeField] Timer timer;
    [SerializeField] InputActionReference moveAction;
    private Transform playerPos;
    void Start()
    {
       if (playerPos  == null)
        {
            playerPos = gameObject.GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(moveAction.ToString());
        if (moveAction)
        {
            SoundFXManager.Instance.PlayWaterFootsteps(playerPos, 1f);
        }
    }
}
