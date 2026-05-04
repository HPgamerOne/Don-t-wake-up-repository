using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LanternBoxSystem : MonoBehaviour
{
    [Flags]
    private enum LanternColorMask
    {
        None = 0,
        Red = 1 << 0,
        Yellow = 1 << 1,
        Orange = 1 << 2,
        Blue = 1 << 3,
        Magenta = 1 << 4,
        Green = 1 << 5
    }

    private enum BigLanternColor
    {
        Default, Red, Yellow, Orange, Blue, Magenta, Green, Cyan, White
    }

    [System.Serializable]
    public class LanternBoxMixPair
    {
        public GameObject BigLantern;
        public List<LanternBoxPair> LanternBoxPairs = new List<LanternBoxPair>();

        [HideInInspector] public Renderer BigLanternEmissionLight;
        [HideInInspector] public Light BigLanternPointLight;
        [HideInInspector] public UniversalAdditionalLightData BigLanternAdditionalLightData;
        [HideInInspector] public MaterialPropertyBlock BigLanternPropertyBlock;

        [HideInInspector] public Color OriginalBigLanternEmissionColor;
        [HideInInspector] public float OriginalBigLanternPointLightIntensity;

        [HideInInspector] public Color CurrentBigLanternEmissionColor;
        [HideInInspector] public Color StartBigLanternEmissionColor;
        [HideInInspector] public Color TargetBigLanternEmissionColor;

        [HideInInspector] public float BrightnessFade;
        [HideInInspector] public float ColorFade;
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

    private struct LanternNameDefinition
    {
        public string NamePart;
        public LanternColorMask ColorMask;
        public LanternNameDefinition(string namePart, LanternColorMask colorMask)
        {
            NamePart = namePart;
            ColorMask = colorMask;
        }
    }

    private struct BigLanternColorRule
    {
        public LanternColorMask RequiredColors;
        public BigLanternColor ResultColor;

        public BigLanternColorRule(LanternColorMask requiredColors, BigLanternColor resultColor)
        {
            RequiredColors = requiredColors;
            ResultColor = resultColor;
        }
    }

    [Header("Lantern Box Mixes")]
    [SerializeField] private List<LanternBoxMixPair> lanternBoxMixPairs = new List<LanternBoxMixPair>();

    [Header("Fade")]
    [SerializeField] private float fadeTime = 1f;

    [Header("Lantern Box Light")]
    [SerializeField] private float activatedPointLightIntensity = 5f;

    private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

    private const int DefaultRenderingLayerIndex = 0;

    private static readonly LanternNameDefinition[] LanternNameDefinitions =
    {
        new LanternNameDefinition("RedLantern", LanternColorMask.Red),
        new LanternNameDefinition("YellowLantern", LanternColorMask.Yellow),
        new LanternNameDefinition("OrangeLantern", LanternColorMask.Orange),
        new LanternNameDefinition("BlueLantern", LanternColorMask.Blue),
        new LanternNameDefinition("MagentaLantern", LanternColorMask.Magenta),
        new LanternNameDefinition("GreenLantern", LanternColorMask.Green)
    };

    private static readonly BigLanternColorRule[] BigLanternColorRules =
    {
        new BigLanternColorRule(LanternColorMask.Red | LanternColorMask.Green | LanternColorMask.Blue, BigLanternColor.White),

        new BigLanternColorRule(LanternColorMask.Red | LanternColorMask.Yellow, BigLanternColor.Orange),
        new BigLanternColorRule(LanternColorMask.Red | LanternColorMask.Blue, BigLanternColor.Magenta),
        new BigLanternColorRule(LanternColorMask.Yellow | LanternColorMask.Blue, BigLanternColor.Green),

        new BigLanternColorRule(LanternColorMask.Blue | LanternColorMask.Green, BigLanternColor.Cyan),
        new BigLanternColorRule(LanternColorMask.Red | LanternColorMask.Green, BigLanternColor.Yellow),

        new BigLanternColorRule(LanternColorMask.Red, BigLanternColor.Red),
        new BigLanternColorRule(LanternColorMask.Yellow, BigLanternColor.Yellow),
        new BigLanternColorRule(LanternColorMask.Orange, BigLanternColor.Orange),
        new BigLanternColorRule(LanternColorMask.Blue, BigLanternColor.Blue),
        new BigLanternColorRule(LanternColorMask.Magenta, BigLanternColor.Magenta),
        new BigLanternColorRule(LanternColorMask.Green, BigLanternColor.Green)
    };

    private static readonly Dictionary<BigLanternColor, float> HueByBigLanternColor = new Dictionary<BigLanternColor, float>
    {
        { BigLanternColor.Red, 0f },
        { BigLanternColor.Yellow, 60f },
        { BigLanternColor.Orange, 20f },
        { BigLanternColor.Blue, 230f },
        { BigLanternColor.Magenta, 300f },
        { BigLanternColor.Green, 120f },
        { BigLanternColor.Cyan, 180f }
    };

    private static readonly Dictionary<BigLanternColor, int> RenderingLayerByBigLanternColor = new Dictionary<BigLanternColor, int>
    {
        { BigLanternColor.Default, 0 },
        { BigLanternColor.Red, 1 },
        { BigLanternColor.Yellow, 2 },
        { BigLanternColor.Orange, 3 },
        { BigLanternColor.Blue, 4 },
        { BigLanternColor.Magenta, 5 },
        { BigLanternColor.Green, 6 },
        { BigLanternColor.White, 7 }
    };

    private void Start()
    {
        SetupLanternBoxMixPairs();
    }
    private void Update()
    {
        foreach (LanternBoxMixPair lanternBoxMixPair in lanternBoxMixPairs)
        {
            LanternColorMask activeColors = UpdateSmallLanternBoxes(lanternBoxMixPair);

            UpdateBigLanternBrightnessFade(lanternBoxMixPair, activeColors);
            UpdateBigLanternColorTarget(lanternBoxMixPair, activeColors);
            UpdateBigLanternColorFade(lanternBoxMixPair);
            SetBigLanternBrightness(lanternBoxMixPair, lanternBoxMixPair.CurrentBigLanternEmissionColor);
        }
    }
    private void SetChildRenderersRenderingLayer(GameObject root, uint layerMask)
    {
        if (root == null) return;

        Renderer[] renderers = root.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer r in renderers)
        {
            r.renderingLayerMask = layerMask;
        }
    }
    private void SetupLanternBoxMixPairs()
    {
        foreach (LanternBoxMixPair lanternBoxMixPair in lanternBoxMixPairs)
        {
            SetupBigLantern(lanternBoxMixPair);
            SetupSmallLanternBoxes(lanternBoxMixPair);
        }
    }
    private void SetupBigLantern(LanternBoxMixPair lanternBoxMixPair)
    {
        if (lanternBoxMixPair.BigLantern == null) {
            return;
        }

        lanternBoxMixPair.BigLanternEmissionLight = lanternBoxMixPair.BigLantern.transform.Find("EmissionLight").GetComponent<Renderer>();
        lanternBoxMixPair.BigLanternPointLight = lanternBoxMixPair.BigLantern.transform.Find("PointLight").GetComponent<Light>();
        lanternBoxMixPair.BigLanternAdditionalLightData = lanternBoxMixPair.BigLanternPointLight.GetComponent<UniversalAdditionalLightData>();
        lanternBoxMixPair.BigLanternPropertyBlock = new MaterialPropertyBlock();

        Material material = lanternBoxMixPair.BigLanternEmissionLight.sharedMaterial;
        material.EnableKeyword("_EMISSION");

        lanternBoxMixPair.OriginalBigLanternEmissionColor = material.GetColor(EmissionColorId);
        lanternBoxMixPair.OriginalBigLanternPointLightIntensity = lanternBoxMixPair.BigLanternPointLight.intensity;

        lanternBoxMixPair.CurrentBigLanternEmissionColor = lanternBoxMixPair.OriginalBigLanternEmissionColor;
        lanternBoxMixPair.StartBigLanternEmissionColor = lanternBoxMixPair.OriginalBigLanternEmissionColor;
        lanternBoxMixPair.TargetBigLanternEmissionColor = lanternBoxMixPair.OriginalBigLanternEmissionColor;

        lanternBoxMixPair.ColorFade = 1f;
        lanternBoxMixPair.BrightnessFade = 0f;

        SetBigLanternRenderingLayer(lanternBoxMixPair, BigLanternColor.Default);
        SetBigLanternBrightness(lanternBoxMixPair, Color.black);
    }

    private void SetupSmallLanternBoxes(LanternBoxMixPair lanternBoxMixPair)
    {
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

    private LanternColorMask UpdateSmallLanternBoxes(LanternBoxMixPair lanternBoxMixPair)
    {
        LanternColorMask activeColors = LanternColorMask.None;

        foreach (LanternBoxPair lanternBoxPair in lanternBoxMixPair.LanternBoxPairs)
        {
            lanternBoxPair.IsActive = IsLanternInsideLanternBox(lanternBoxPair);
            float targetFade = lanternBoxPair.IsActive ? 1f : 0f;
            lanternBoxPair.ActivatedPointLightFade = Mathf.MoveTowards(lanternBoxPair.ActivatedPointLightFade, targetFade, Time.deltaTime / fadeTime);
            SetActivatedPointLightBrightness(lanternBoxPair);
            if (lanternBoxPair.IsActive) {
                activeColors |= GetLanternColorMask(lanternBoxPair.Lantern);
            }
        }
        return activeColors;
    }

    private void UpdateBigLanternBrightnessFade(LanternBoxMixPair lanternBoxMixPair, LanternColorMask activeColors)
    {
        float targetFade = activeColors != LanternColorMask.None ? 1f : 0f;

        lanternBoxMixPair.BrightnessFade = Mathf.MoveTowards(lanternBoxMixPair.BrightnessFade, targetFade, Time.deltaTime / fadeTime);
    }

    private void UpdateBigLanternColorTarget(LanternBoxMixPair lanternBoxMixPair, LanternColorMask activeColors)
    {
        if (activeColors == LanternColorMask.None)
        {
            lanternBoxMixPair.StartBigLanternEmissionColor = lanternBoxMixPair.CurrentBigLanternEmissionColor;
            lanternBoxMixPair.TargetBigLanternEmissionColor = lanternBoxMixPair.CurrentBigLanternEmissionColor;
            lanternBoxMixPair.ColorFade = 1f;

            if (lanternBoxMixPair.BrightnessFade <= 0f)
            {
                SetBigLanternRenderingLayer(lanternBoxMixPair, BigLanternColor.Default);
            }
            return;
        }
        BigLanternColor targetColorType = GetBigLanternColor(activeColors);
        Color targetEmissionColor = GetEmissionColor(lanternBoxMixPair.OriginalBigLanternEmissionColor, targetColorType);
        SetBigLanternRenderingLayer(lanternBoxMixPair, targetColorType);
        if (!AreColorsDifferent(targetEmissionColor, lanternBoxMixPair.TargetBigLanternEmissionColor))
        {
            return;
        }
        lanternBoxMixPair.StartBigLanternEmissionColor = lanternBoxMixPair.CurrentBigLanternEmissionColor;
        lanternBoxMixPair.TargetBigLanternEmissionColor = targetEmissionColor;
        lanternBoxMixPair.ColorFade = 0f;
    }

    private void UpdateBigLanternColorFade(LanternBoxMixPair lanternBoxMixPair)
    {
        lanternBoxMixPair.ColorFade = Mathf.MoveTowards(lanternBoxMixPair.ColorFade, 1f, Time.deltaTime / fadeTime);
        lanternBoxMixPair.CurrentBigLanternEmissionColor = Color.Lerp(lanternBoxMixPair.StartBigLanternEmissionColor, lanternBoxMixPair.TargetBigLanternEmissionColor, lanternBoxMixPair.ColorFade);
    }

    private bool IsLanternInsideLanternBox(LanternBoxPair lanternBoxPair)
    {
        if (lanternBoxPair.LanternBoxCollision == null || lanternBoxPair.LanternCollider == null) {
            return false;
        }
        return Physics.ComputePenetration(lanternBoxPair.LanternBoxCollision, lanternBoxPair.LanternBoxCollision.transform.position, lanternBoxPair.LanternBoxCollision.transform.rotation, lanternBoxPair.LanternCollider, lanternBoxPair.LanternCollider.transform.position, lanternBoxPair.LanternCollider.transform.rotation, out Vector3 direction, out float distance);
    }

    private void SetActivatedPointLightBrightness(LanternBoxPair lanternBoxPair)
    {
        if (lanternBoxPair.ActivatedPointLight == null) {
            return;
        }
        lanternBoxPair.ActivatedPointLight.intensity = activatedPointLightIntensity * lanternBoxPair.ActivatedPointLightFade;
    }

    private void SetBigLanternBrightness(LanternBoxMixPair lanternBoxMixPair, Color targetEmissionColor)
    {
        if (lanternBoxMixPair.BigLanternEmissionLight == null || lanternBoxMixPair.BigLanternPointLight == null) {
            return;
        }
        Color emissionColor = Color.Lerp(Color.black, targetEmissionColor, lanternBoxMixPair.BrightnessFade);
        lanternBoxMixPair.BigLanternPropertyBlock.Clear();
        lanternBoxMixPair.BigLanternPropertyBlock.SetVector(EmissionColorId, emissionColor);
        lanternBoxMixPair.BigLanternEmissionLight.SetPropertyBlock(lanternBoxMixPair.BigLanternPropertyBlock);
        lanternBoxMixPair.BigLanternPointLight.intensity = lanternBoxMixPair.OriginalBigLanternPointLightIntensity * lanternBoxMixPair.BrightnessFade;
        lanternBoxMixPair.BigLanternPointLight.color = GetNormalColorFromEmissionColor(targetEmissionColor);
    }

    private LanternColorMask GetLanternColorMask(GameObject lantern)
    {
        if (lantern == null) {
            return LanternColorMask.None;
        }

        foreach (LanternNameDefinition lanternNameDefinition in LanternNameDefinitions)
        {
            if (lantern.name.Contains(lanternNameDefinition.NamePart))
            {
                return lanternNameDefinition.ColorMask;
            }
        }
        return LanternColorMask.None;
    }

    private BigLanternColor GetBigLanternColor(LanternColorMask activeColors)
    {
        foreach (BigLanternColorRule rule in BigLanternColorRules)
        {
            if ((activeColors & rule.RequiredColors) == rule.RequiredColors)
            {
                return rule.ResultColor;
            }
        }
        return BigLanternColor.Default;
    }

    private Color GetEmissionColor(Color originalEmissionColor, BigLanternColor bigLanternColor)
    {
        if (bigLanternColor == BigLanternColor.White) {
            return GetWhiteEmissionColor(originalEmissionColor);
        }
        if (HueByBigLanternColor.TryGetValue(bigLanternColor, out float hueDegrees)) {
            return GetEmissionColorWithHue(originalEmissionColor, hueDegrees);
        }
        return originalEmissionColor;
    }

    private void SetBigLanternRenderingLayer(LanternBoxMixPair lanternBoxMixPair, BigLanternColor bigLanternColor)
    {
        if (bigLanternColor == BigLanternColor.Cyan) {
            return;
        }
        int colorLayerIndex = RenderingLayerByBigLanternColor[bigLanternColor];
        uint colorLayerMask = 1u << colorLayerIndex;

        lanternBoxMixPair.BigLanternAdditionalLightData.renderingLayers = colorLayerMask;
        SetChildRenderersRenderingLayer(lanternBoxMixPair.BigLantern, colorLayerMask);
    }

    /*private void SetBigLanternRenderingLayer(LanternBoxMixPair lanternBoxMixPair, BigLanternColor bigLanternColor)
    {
        int colorLayerIndex = RenderingLayerByBigLanternColor[bigLanternColor];
        uint defaultLayerMask = 1u << DefaultRenderingLayerIndex;
        uint colorLayerMask = 1u << colorLayerIndex;
        //lanternBoxMixPair.BigLanternAdditionalLightData.renderingLayers = defaultLayerMask | colorLayerMask;
        lanternBoxMixPair.BigLanternAdditionalLightData.renderingLayers =  colorLayerMask; // Only colormask, not default
    }*/

    private bool AreColorsDifferent(Color colorA, Color colorB)
    {
        return Mathf.Abs(colorA.r - colorB.r) > 0.001f || Mathf.Abs(colorA.g - colorB.g) > 0.001f || Mathf.Abs(colorA.b - colorB.b) > 0.001f || Mathf.Abs(colorA.a - colorB.a) > 0.001f;
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