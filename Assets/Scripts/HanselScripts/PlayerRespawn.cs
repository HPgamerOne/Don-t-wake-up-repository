using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [System.Serializable]
    public class RespawnPair
    {
        public GameObject CheckPoint;
        public GameObject RespawnPoint;
    }

    [Header("Respawn")]
    [SerializeField] private float respawnY = -50f;
    [SerializeField] private float fadeDuration = 0.5f; // Nino
    [SerializeField] private float respawnDelay = 1f; // Nino

    [Header("Checkpoints/Respawns")]
    [SerializeField] private List<RespawnPair> respawnPairs = new List<RespawnPair>();

    [Header("Extra")]
    [SerializeField] private bool shouldLookDown = false;

    private GameObject player;
    private GameObject cameraPivot;
    private CharacterController characterController;

    private RespawnPair lastTouchedRespawnPair;
    private bool isRespawning = false;
    void Start()
    {
        player = GameObject.Find("Player");
        cameraPivot = GameObject.Find("Camera Pivot");
        characterController = player.GetComponent<CharacterController>();
    }

    void Update()
    {
        CheckCheckpointTouch();

        if (!isRespawning && player.transform.position.y < respawnY)
        {
            StartCoroutine(RespawnPlayer());
        }
    }

    private void CheckCheckpointTouch()
    {
        foreach (RespawnPair respawnPair in respawnPairs)
        {
            BoxCollider checkpointCollider = respawnPair.CheckPoint.GetComponent<BoxCollider>();

            if (checkpointCollider.bounds.Contains(player.transform.position))
            {
                lastTouchedRespawnPair = respawnPair;
            }
        }
    }

    IEnumerator RespawnPlayer()
    {
        isRespawning = true;


        FadeManager.Instance.FadeToBlack(fadeDuration);
        yield return new WaitForSeconds(respawnDelay);

        if (shouldLookDown)
        {
            CameraController cameraController = cameraPivot.GetComponent<CameraController>();
            cameraController.active = false;
            StartCoroutine(TurnOnCameraController(cameraController, 0.25f));
        }

        characterController.enabled = false;
        player.transform.position = lastTouchedRespawnPair.RespawnPoint.transform.position;
        characterController.enabled = true;
        FadeManager.Instance.FadeFromBlack(fadeDuration);
        isRespawning = false;
    }

    IEnumerator TurnOnCameraController(CameraController cameraController, float time)
    {
        yield return new WaitForSeconds(time);
        cameraController.active = true;
    }
}