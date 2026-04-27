using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private bool redCar, greenCar, blueCar = false;
    public bool test = false;

    // Update is called once per frame
    void Update()
    {
        if(redCar && greenCar && blueCar)
        {
            FadeManager.Instance.FadeOutObject(gameObject, 2f);
            //Maybe door opening animation
        }
        else if (test)
        {
            Debug.Log("Shit works");
            FadeManager.Instance.FadeOutObject(gameObject, 0.5f);

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
