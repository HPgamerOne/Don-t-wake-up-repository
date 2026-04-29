using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class PermProximityLight : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private List<Transform> proximityObjects = new List<Transform>();
    [SerializeField] private float activationDistance = 8f;

    [Header("Fade")]
    [SerializeField] private float fadeTime = 2f;

    private Renderer emissionLight;
    private MaterialPropertyBlock propertyBlock;
    private Light pointLight;

    private Color originalEmissionColor;
    private float originalPointLightIntensity;
    private float fade = 0f;

    private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

    private bool activated = false;

    private void Start()
    {
        if (proximityObjects.Count == 0)
        {
            GameObject playerObject = GameObject.Find("Player");
            proximityObjects.Add(playerObject.transform);
        }

        emissionLight = transform.Find("EmissionLight").GetComponent<Renderer>();
        pointLight = transform.Find("PointLight").GetComponent<Light>();

        propertyBlock = new MaterialPropertyBlock();

        Material material = emissionLight.sharedMaterial;
        material.EnableKeyword("_EMISSION");

        originalEmissionColor = material.GetColor(EmissionColorId);
        originalPointLightIntensity = pointLight.intensity;

        SetLanternBrightness(0f);
    }

    private void Update()
    {
        if (!activated)
        {
            float targetFade = IsAnyProximityObjectClose() ? 1f : 0f;
            fade = Mathf.MoveTowards(fade, targetFade, Time.deltaTime / fadeTime);
            if (fade >= 1)
            {
                activated = true;
            }
        }
        SetLanternBrightness(fade);
    }

    private bool IsAnyProximityObjectClose()
    {
        foreach (Transform proximityObject in proximityObjects)
        {
            float distanceToProximityObject = Vector3.Distance(proximityObject.position, transform.position);
            if (distanceToProximityObject <= activationDistance)
            {
                return true;
            }
        }

        return false;
    }

    private void SetLanternBrightness(float fade)
    {
        Color emissionColor = Color.Lerp(Color.black, originalEmissionColor, fade);

        propertyBlock.Clear();
        propertyBlock.SetVector(EmissionColorId, emissionColor);
        emissionLight.SetPropertyBlock(propertyBlock);
        pointLight.intensity = originalPointLightIntensity * fade;
    }
}