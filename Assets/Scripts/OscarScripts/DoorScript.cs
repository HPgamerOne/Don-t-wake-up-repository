using System.Collections;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private bool redCar, greenCar, blueCar = false;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if(redCar && greenCar && blueCar)
        {
            animator.Play("DoorOpen", 0, 0);
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
