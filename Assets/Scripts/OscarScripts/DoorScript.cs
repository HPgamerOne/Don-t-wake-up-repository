using System.Collections;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private bool redCar, greenCar, blueCar = false;

    // Update is called once per frame
    void Update()
    {
        if(redCar && greenCar && blueCar)
        {
            FadeManager.Instance.FadeOutObject(gameObject, 1f);;
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private IEnumerator Wait(float delay)
    {
        yield return new WaitForSeconds(delay);
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
