using UnityEngine;

public class LiquidWobble : MonoBehaviour
{
    [SerializeField] private Vector3 lastPosition;
    [SerializeField] private Vector3 currentPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        
    }
}
