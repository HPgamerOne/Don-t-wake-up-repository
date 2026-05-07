using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSounds : MonoBehaviour
{

    //[SerializeField] Timer timer;
    [SerializeField] InputActionReference moveAction;
    // Update is called once per frame
    void Update()
    {
        
        if (ActiveMovementCheck())
        {
            //Lowk fixed? Gotta create tags tho and expand audio library
            //Need to check floor etc n shit
            //Shoot ray downward, get tag/layer/any identifcator of what kind of floor it is
            //Play steps sounds corresponding to the type of floor
            RaycastHit hit;
            if(Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                PlaySteps(hit);
            }
        }
        else
        {
            if (SoundFXManager.Instance.FootStepsPlaying)
            {
                SoundFXManager.Instance.StopFootsteps();
            }
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
        if (hit.collider.CompareTag("Wood"))
        {
            if (!SoundFXManager.Instance.FootStepsPlaying)
            {
                SoundFXManager.Instance.PlayWoodFootsteps(1f);
            }
        }
        else if (hit.collider.CompareTag("Concrete"))
        {
            if (!SoundFXManager.Instance.FootStepsPlaying)
            {
                SoundFXManager.Instance.PlayConcreteFootsteps(1f);

            }
        }
        else if (hit.collider.CompareTag("Water"))
        {
            if (!SoundFXManager.Instance.FootStepsPlaying)
            {
                SoundFXManager.Instance.PlayWaterFootsteps(1f);

            }
        }
        else if (hit.collider.CompareTag("Grass"))
        {
            if (!SoundFXManager.Instance.FootStepsPlaying)
            {
                SoundFXManager.Instance.PlayGrassFootsteps(1f);

            }
        }
    }
}
