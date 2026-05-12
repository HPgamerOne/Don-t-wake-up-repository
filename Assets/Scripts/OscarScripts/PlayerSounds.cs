using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSounds : MonoBehaviour
{

    //[SerializeField] Timer timer;
    [SerializeField] InputActionReference moveAction;
    /// <summary>
    /// Vi kör one clips istället för en looping clip för ljud filer.
    /// Fixas imorn
    /// </summary>


    float stepInterval = 0.7f;
    float stepTimer = 0f;
    void Update()
    {
        
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
