using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private bool redCar, greenCar, blueCar, triggered = false;

    // Update is called once per frame
    void Update()
    {
        if(!triggered && redCar && greenCar && blueCar)
        {
            Debug.Log("Shit triggered");
            FadeManager.Instance.FadeOutObject(gameObject, 2f);
            triggered = true;
            //Maybe door opening animation
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
