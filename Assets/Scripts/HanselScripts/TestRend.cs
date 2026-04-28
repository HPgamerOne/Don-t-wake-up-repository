using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(UniversalAdditionalLightData))]
public class LightRenderingLayerPrinter : MonoBehaviour
{
    private Light targetLight;
    private UniversalAdditionalLightData additionalLightData;

    private void Awake()
    {
        targetLight = GetComponent<Light>();
        additionalLightData = GetComponent<UniversalAdditionalLightData>();
    }

    private void Start()
    {
        PrintRenderingLayers();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SetRenderingLayersToDefaultAndLightLayer2();
            PrintRenderingLayers();
        }
    }

    private void SetRenderingLayersToDefaultAndLightLayer2()
    {
        uint defaultLayer = 1u << 0;
        uint lightLayer2 = 1u << 2;

        additionalLightData.renderingLayers = defaultLayer | lightLayer2;
    }

    [ContextMenu("Print Light Rendering Layers")]
    private void PrintRenderingLayers()
    {
        uint renderingLayerMask = additionalLightData.renderingLayers.value;

        List<string> activeLayers = new List<string>();

        for (int i = 0; i < 32; i++)
        {
            uint layerBit = 1u << i;

            if ((renderingLayerMask & layerBit) != 0)
            {
                string layerName = i == 0 ? "Default Layer" : "Light Layer " + i;
                activeLayers.Add(layerName);
            }
        }

        Debug.Log("Light object: " + targetLight.name);
        Debug.Log("URP Light Rendering Layer Mask Value: " + renderingLayerMask);
        Debug.Log("Light uses Rendering Layers: " + string.Join(", ", activeLayers));
    }
}