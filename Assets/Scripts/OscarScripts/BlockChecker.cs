using UnityEngine;
using UnityEngine.UIElements;

public class BlockChecker : MonoBehaviour
{
    public string requiredID;
    public bool isFilled;
    [SerializeField] GameObject toyBox;

    private void OnTriggerEnter(Collider other)
    {
        WoodBlock block = other.GetComponent<WoodBlock>();
        if (block != null)
        {
            if(block.id == requiredID)
            {
                isFilled = true;
                toyBox.GetComponent<ToyBoxScript>().UpdateCheckers();

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        WoodBlock block = other.GetComponent<WoodBlock>();
        if (block != null)
        {
            if (block.id == requiredID)
            {
                isFilled = false;
                toyBox.GetComponent<ToyBoxScript>().UpdateCheckers();
            }
        }
    }
 
   
}
