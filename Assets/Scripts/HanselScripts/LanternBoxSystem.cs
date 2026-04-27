using System.Collections.Generic;
using UnityEngine;

public class LanternBoxSystem : MonoBehaviour
{
    [System.Serializable]
    public class LanternBoxMixPair
    {
        public GameObject BigLantern;
        public List<LanternBoxPair> LanternBoxPairs = new List<LanternBoxPair>();

        [HideInInspector] public Renderer BigLanternEmissionLight;
        [HideInInspector] public Light BigLanternPointLight;
        [HideInInspector] public MaterialPropertyBlock BigLanternPropertyBlock;

        [HideInInspector] public Color OriginalBigLanternEmissionColor;
        [HideInInspector] public float OriginalBigLanternPointLightIntensity;

        [HideInInspector] public float Fade;
    }

    [System.Serializable]
    public class LanternBoxPair
    {
        public GameObject LanternBox;
        public GameObject Lantern;

        [HideInInspector] public BoxCollider LanternBoxCollision;
        [HideInInspector] public BoxCollider LanternCollider;
        [HideInInspector] public Light ActivatedPointLight;

        [HideInInspector] public float ActivatedPointLightFade;
        [HideInInspector] public bool IsActive;
    }

    [Header("Lantern Box Mixes")]
    [SerializeField] private List<LanternBoxMixPair> lanternBoxMixPairs = new List<LanternBoxMixPair>();

    [Header("Fade")]
    [SerializeField] private float fadeTime = 2f;

    [Header("Lantern Box Light")]
    [SerializeField] private float activatedPointLightIntensity = 2f;

    private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

    private void Start()
    {
        SetupLanternBoxMixPairs();
    }

    private void Update()
    {
        foreach (LanternBoxMixPair lanternBoxMixPair in lanternBoxMixPairs)
        {
            List<string> activeLanternNames = new List<string>();

            foreach (LanternBoxPair lanternBoxPair in lanternBoxMixPair.LanternBoxPairs)
            {
                lanternBoxPair.IsActive = IsLanternInsideLanternBox(lanternBoxPair);

                float targetActivatedPointLightFade = lanternBoxPair.IsActive ? 1f : 0f;

                lanternBoxPair.ActivatedPointLightFade = Mathf.MoveTowards(
                    lanternBoxPair.ActivatedPointLightFade,
                    targetActivatedPointLightFade,
                    Time.deltaTime / fadeTime
                );

                SetActivatedPointLightBrightness(lanternBoxPair);

                if (lanternBoxPair.IsActive)
                {
                    activeLanternNames.Add(lanternBoxPair.Lantern.name);
                }
            }

            float targetBigLanternFade = activeLanternNames.Count > 0 ? 1f : 0f;

            lanternBoxMixPair.Fade = Mathf.MoveTowards(
                lanternBoxMixPair.Fade,
                targetBigLanternFade,
                Time.deltaTime / fadeTime
            );

            Color targetEmissionColor = GetMixedEmissionColor(lanternBoxMixPair, activeLanternNames);

            SetBigLanternBrightness(lanternBoxMixPair, targetEmissionColor);
        }
    }

    private void SetupLanternBoxMixPairs()
    {
        foreach (LanternBoxMixPair lanternBoxMixPair in lanternBoxMixPairs)
        {
            if (lanternBoxMixPair.BigLantern != null)
            {
                lanternBoxMixPair.BigLanternEmissionLight = lanternBoxMixPair.BigLantern.transform.Find("EmissionLight").GetComponent<Renderer>();
                lanternBoxMixPair.BigLanternPointLight = lanternBoxMixPair.BigLantern.transform.Find("PointLight").GetComponent<Light>();

                lanternBoxMixPair.BigLanternPropertyBlock = new MaterialPropertyBlock();

                Material material = lanternBoxMixPair.BigLanternEmissionLight.sharedMaterial;
                material.EnableKeyword("_EMISSION");

                lanternBoxMixPair.OriginalBigLanternEmissionColor = material.GetColor(EmissionColorId);
                lanternBoxMixPair.OriginalBigLanternPointLightIntensity = lanternBoxMixPair.BigLanternPointLight.intensity;

                SetBigLanternBrightness(lanternBoxMixPair, Color.black);
            }

            foreach (LanternBoxPair lanternBoxPair in lanternBoxMixPair.LanternBoxPairs)
            {
                if (lanternBoxPair.LanternBox != null)
                {
                    lanternBoxPair.LanternBoxCollision = lanternBoxPair.LanternBox.transform.Find("LanternCollision").GetComponent<BoxCollider>();
                    lanternBoxPair.ActivatedPointLight = lanternBoxPair.LanternBox.transform.Find("ActivatedPointLight").GetComponent<Light>();

                    lanternBoxPair.ActivatedPointLight.intensity = 0f;
                }

                if (lanternBoxPair.Lantern != null)
                {
                    lanternBoxPair.LanternCollider = lanternBoxPair.Lantern.GetComponent<BoxCollider>();
                }
            }
        }
    }

    private bool IsLanternInsideLanternBox(LanternBoxPair lanternBoxPair)
    {
        if (lanternBoxPair.LanternBoxCollision == null)
        {
            return false;
        }

        if (lanternBoxPair.LanternCollider == null)
        {
            return false;
        }

        return Physics.ComputePenetration(
            lanternBoxPair.LanternBoxCollision,
            lanternBoxPair.LanternBoxCollision.transform.position,
            lanternBoxPair.LanternBoxCollision.transform.rotation,
            lanternBoxPair.LanternCollider,
            lanternBoxPair.LanternCollider.transform.position,
            lanternBoxPair.LanternCollider.transform.rotation,
            out Vector3 direction,
            out float distance
        );
    }

    private void SetActivatedPointLightBrightness(LanternBoxPair lanternBoxPair)
    {
        if (lanternBoxPair.ActivatedPointLight == null)
        {
            return;
        }

        lanternBoxPair.ActivatedPointLight.intensity = activatedPointLightIntensity * lanternBoxPair.ActivatedPointLightFade;
    }

    private void SetBigLanternBrightness(LanternBoxMixPair lanternBoxMixPair, Color targetEmissionColor)
    {
        if (lanternBoxMixPair.BigLanternEmissionLight == null)
        {
            return;
        }

        if (lanternBoxMixPair.BigLanternPointLight == null)
        {
            return;
        }

        Color emissionColor = Color.Lerp(Color.black, targetEmissionColor, lanternBoxMixPair.Fade);

        lanternBoxMixPair.BigLanternPropertyBlock.Clear();
        lanternBoxMixPair.BigLanternPropertyBlock.SetVector(EmissionColorId, emissionColor);
        lanternBoxMixPair.BigLanternEmissionLight.SetPropertyBlock(lanternBoxMixPair.BigLanternPropertyBlock);

        lanternBoxMixPair.BigLanternPointLight.intensity = lanternBoxMixPair.OriginalBigLanternPointLightIntensity * lanternBoxMixPair.Fade;
        lanternBoxMixPair.BigLanternPointLight.color = GetNormalColorFromEmissionColor(targetEmissionColor);
    }

    private Color GetMixedEmissionColor(LanternBoxMixPair lanternBoxMixPair, List<string> activeLanternNames)
    {
        if (activeLanternNames.Count == 0)
        {
            return lanternBoxMixPair.OriginalBigLanternEmissionColor;
        }

        bool hasRed = HasLantern(activeLanternNames, "RedLantern");
        bool hasYellow = HasLantern(activeLanternNames, "YellowLantern");
        bool hasOrange = HasLantern(activeLanternNames, "OrangeLantern");
        bool hasBlue = HasLantern(activeLanternNames, "BlueLantern");
        bool hasMagenta = HasLantern(activeLanternNames, "MagentaLantern");
        bool hasGreen = HasLantern(activeLanternNames, "GreenLantern");

        if (hasRed && hasGreen && hasBlue)
        {
            return GetWhiteEmissionColor(lanternBoxMixPair.OriginalBigLanternEmissionColor);
        }

        if (hasRed && hasYellow)
        {
            return GetEmissionColorWithHue(lanternBoxMixPair.OriginalBigLanternEmissionColor, 20f);
        }

        if (hasRed && hasBlue)
        {
            return GetEmissionColorWithHue(lanternBoxMixPair.OriginalBigLanternEmissionColor, 300f);
        }

        if (hasYellow && hasBlue)
        {
            return GetEmissionColorWithHue(lanternBoxMixPair.OriginalBigLanternEmissionColor, 120f);
        }

        if (hasRed)
        {
            return GetEmissionColorWithHue(lanternBoxMixPair.OriginalBigLanternEmissionColor, 0f);
        }

        if (hasYellow)
        {
            return GetEmissionColorWithHue(lanternBoxMixPair.OriginalBigLanternEmissionColor, 60f);
        }

        if (hasOrange)
        {
            return GetEmissionColorWithHue(lanternBoxMixPair.OriginalBigLanternEmissionColor, 20f);
        }

        if (hasBlue)
        {
            return GetEmissionColorWithHue(lanternBoxMixPair.OriginalBigLanternEmissionColor, 230f);
        }

        if (hasMagenta)
        {
            return GetEmissionColorWithHue(lanternBoxMixPair.OriginalBigLanternEmissionColor, 300f);
        }

        if (hasGreen)
        {
            return GetEmissionColorWithHue(lanternBoxMixPair.OriginalBigLanternEmissionColor, 120f);
        }

        return lanternBoxMixPair.OriginalBigLanternEmissionColor;
    }

    private bool HasLantern(List<string> activeLanternNames, string lanternName)
    {
        foreach (string activeLanternName in activeLanternNames)
        {
            if (activeLanternName.Contains(lanternName))
            {
                return true;
            }
        }

        return false;
    }

    private Color GetEmissionColorWithHue(Color originalEmissionColor, float hueDegrees)
    {
        Color.RGBToHSV(originalEmissionColor, out float originalHue, out float originalSaturation, out float originalValue);

        float hue = hueDegrees / 360f;

        return Color.HSVToRGB(hue, originalSaturation, originalValue, true);
    }

    private Color GetWhiteEmissionColor(Color originalEmissionColor)
    {
        Color.RGBToHSV(originalEmissionColor, out float originalHue, out float originalSaturation, out float originalValue);

        return new Color(originalValue, originalValue, originalValue, 1f);
    }

    private Color GetNormalColorFromEmissionColor(Color emissionColor)
    {
        Color.RGBToHSV(emissionColor, out float hue, out float saturation, out float value);

        return Color.HSVToRGB(hue, saturation, 1f);
    }
}