using System.Collections.Generic;
using UnityEngine;

public class LanternRespawn : MonoBehaviour
{
    [SerializeField] private float respawnY = -50f;

    private List<GameObject> Lanterns = new List<GameObject>();
    private List<Vector3> originalPositions = new List<Vector3>();

    void Start()
    {
        foreach (Transform lanternTransform in transform)
        {
            Lanterns.Add(lanternTransform.gameObject);
            originalPositions.Add(lanternTransform.position);
        }
    }

    void Update()
    {
        for (int i = 0; i < Lanterns.Count; i++)
        {
            if (Lanterns[i].transform.position.y < respawnY)
            {
                Lanterns[i].transform.position = originalPositions[i];
                Lanterns[i].transform.rotation = Quaternion.identity;
            }
        }
    }
}