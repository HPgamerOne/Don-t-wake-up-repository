using System.Collections.Generic;
using UnityEngine;

public class LanternRespawn : MonoBehaviour
{
    [System.Serializable]
    public class RespawnPair
    {
        public GameObject CheckPoint;
        public GameObject RespawnPoint;
    }

    private class LanternInfo
    {
        public GameObject Lantern;
        public Rigidbody Rigidbody;
        public RespawnPair LastTouchedRespawnPair;
    }

    [Header("Respawn")]
    [SerializeField] private float respawnY = -50f;

    [Header("Checkpoints/Respawns")]
    [SerializeField] private List<RespawnPair> respawnPairs = new List<RespawnPair>();

    private List<LanternInfo> lanterns = new List<LanternInfo>();

    void Start()
    {
        foreach (Transform lanternTransform in transform)
        {
            LanternInfo lanternInfo = new LanternInfo();

            lanternInfo.Lantern = lanternTransform.gameObject;
            lanternInfo.Rigidbody = lanternTransform.GetComponent<Rigidbody>();
            lanternInfo.LastTouchedRespawnPair = respawnPairs[0];

            lanterns.Add(lanternInfo);
        }
    }

    void Update()
    {
        foreach (LanternInfo lanternInfo in lanterns)
        {
            CheckCheckpointTouch(lanternInfo);

            if (lanternInfo.Lantern.transform.position.y < respawnY)
            {
                RespawnLantern(lanternInfo);
            }
        }
    }

    private void CheckCheckpointTouch(LanternInfo lanternInfo)
    {
        foreach (RespawnPair respawnPair in respawnPairs)
        {
            BoxCollider checkpointCollider = respawnPair.CheckPoint.GetComponent<BoxCollider>();

            if (checkpointCollider.bounds.Contains(lanternInfo.Lantern.transform.position))
            {
                lanternInfo.LastTouchedRespawnPair = respawnPair;
            }
        }
    }

    private void RespawnLantern(LanternInfo lanternInfo)
    {
        lanternInfo.Rigidbody.linearVelocity = Vector3.zero;
        lanternInfo.Rigidbody.angularVelocity = Vector3.zero;

        lanternInfo.Lantern.transform.position = lanternInfo.LastTouchedRespawnPair.RespawnPoint.transform.position;
        lanternInfo.Lantern.transform.rotation = lanternInfo.LastTouchedRespawnPair.RespawnPoint.transform.rotation;
    }
}