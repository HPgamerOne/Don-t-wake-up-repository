using System.Collections;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private bool redCar, greenCar, blueCar, triggered = false;
    [SerializeField] Animator animator;
    [SerializeField] AudioLibrary library;

    void Update()
    {
        //Debug.LogWarning(redCar + " " + blueCar + " " + greenCar);
        if(redCar && greenCar && blueCar && !triggered)
        {
            //Debug.Log("Entered statement");
            triggered = true;
            animator.Play("DoorOpen", 0, 0);
            SoundFXManager.Instance.PlaySoundEffectAtPosition(library.doorCreak,transform, 1f);
            //Debug.Log("Finished animation");
        }
    }

    public bool RedCar 
    {
        get {  return redCar; }
        set {  redCar = value; }
    }
    public bool GreenCar
    {
        get { return greenCar; }
        set { greenCar = value; }
    }
    public bool BlueCar
    {
        get { return blueCar; }
        set { blueCar = value; }
    }
}
