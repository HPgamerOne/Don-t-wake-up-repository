using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject whiteroomRoom;
    [SerializeField] private GameObject whiteroomDoorPlane;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject oscarWall;
    [SerializeField] private bool openTrigger = false;
    [SerializeField] private bool midwayTrigger = false;
    [SerializeField] private bool closeTrigger = false;
    [SerializeField] private bool shouldDisappear = true;
    [SerializeField] private Timer timer;

    private float duration = 2f;

    private void Start()
    {
        if (oscarWall != null)
        {
            oscarWall.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (openTrigger)
            {
                animator.Play("DoorOpen", 0, 0);
                gameObject.SetActive(false);
            }

            else if (midwayTrigger)
            {
                if (timer == null)
                {
                    timer = FindAnyObjectByType(typeof(Timer)) as Timer;
                }

                if (whiteroomDoorPlane != null)
                {
                    whiteroomDoorPlane.SetActive(true);
                }

                if (whiteroomRoom != null)
                {
                    whiteroomRoom.SetActive(false);
                }

                if (oscarWall != null)
                {
                    oscarWall.SetActive(true);
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

                if (shouldDisappear)
                {
                    StartCoroutine(Wait());
                }
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
    }
}
