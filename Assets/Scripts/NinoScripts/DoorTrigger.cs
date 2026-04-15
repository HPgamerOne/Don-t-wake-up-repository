using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject whiteroomRoom;
    [SerializeField] private GameObject whiteroomDoorPlane;
    [SerializeField] private GameObject door;
    [SerializeField] private bool openTrigger = false;
    [SerializeField] private bool midwayTrigger = false;
    [SerializeField] private bool closeTrigger = false;
    [SerializeField] private Timer timer;

    float duration = 2f;
    // float time = 0f;

    // private IDissolvable doorDissolve;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (openTrigger)
            {
                animator.Play("DoorOpen", 0, 0);
                gameObject.SetActive(false);
            }
            /*
            else if (closeTrigger)
            {
                animator.Play("DoorClose", 0, 0);
                gameObject.SetActive(false);
            }
            */
            else if (midwayTrigger)
            {
                if (whiteroomDoorPlane != null)
                {
                    whiteroomDoorPlane.SetActive(true);
                }

                if (whiteroomRoom != null)
                {
                    whiteroomRoom.SetActive(false);
                }

                if (timer != null)
                {
                    timer.StartTimer();
                }

                gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (closeTrigger)
            {
                animator.Play("DoorClose", 0, 0);
                gameObject.GetComponent<Collider>().enabled = false;

                StartCoroutine(Wait());
            }
        }
    }
    IEnumerator Wait()
    {
        IDissolvable doorStuff = door.GetComponent<IDissolvable>();

        yield return new WaitForSeconds(2f);

        whiteroomDoorPlane.SetActive(false);
    
        float startValue = 1.5f;
        float endValue = -1.5f;

        float time = 0f;

        while (time < duration)
        {
            
            float t = time / duration;
            float value = Mathf.Lerp(startValue, endValue, t);

            doorStuff.SetDissolve(value);

            time += Time.deltaTime;

            yield return null;
        }
        door.SetActive(false);

        gameObject.SetActive(false);
        /*
        IDissolvable doorStuff = door.GetComponent<IDissolvable>();

        if (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            float value = Mathf.Lerp(3f, 0f, t);

            doorStuff.SetDissolve(t);
        }
        */
    }
}
