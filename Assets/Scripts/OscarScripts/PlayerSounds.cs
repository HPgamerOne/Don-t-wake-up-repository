using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSounds : MonoBehaviour
{

    //[SerializeField] Timer timer;
    [SerializeField] InputActionReference moveAction;
    PlayerController pc;
    /// <summary>
    /// Vi kör one clips istället för en looping clip för ljud filer.
    /// Fixas imorn
    /// </summary>


    public float stepInterval = 0.4f;
    float stepTimer = 0f;
    private void Start()
    {
            pc = GetComponent<PlayerController>();
    }
    void Update()
    {
        if(pc.IsSprinting())
        {
            stepInterval = 0.2f;
        }
        else
        {
            stepInterval = 0.4f;
        }
        if (ActiveMovementCheck())
        {
            stepTimer += Time.deltaTime;
            if (stepTimer > stepInterval)
            {
                stepTimer = 0f;
                TryPlayFootsteps();
            }
        }
    }
    private void TryPlayFootsteps()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f))
        {
            PlaySteps(hit);
        }
    }
    private bool ActiveMovementCheck()
    {
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        bool isMoving = moveInput.magnitude > 0.1;

        return isMoving;
    }

    private void PlaySteps(RaycastHit hit)
    {
        switch (hit.collider.tag)
        {
            case "Wood": SoundFXManager.Instance.PlayWoodFootsteps(1f); break;
            case "Concrete": SoundFXManager.Instance.PlayConcreteFootsteps(1f); break;
            case "Water": SoundFXManager.Instance.PlayWaterFootsteps(1f); break;
            case "Grass": SoundFXManager.Instance.PlayGrassFootsteps(1f); break;
            default: SoundFXManager.Instance.PlayConcreteFootsteps(1f); break;
        }
    }
}
