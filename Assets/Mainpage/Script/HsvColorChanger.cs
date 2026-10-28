using UnityEngine;
using UnityEngine.UI; // UI 요소 사용
using UnityEngine.Events; // AddListener 사용
// using System.Collections.Generic; // 사용되지 않음
// using System.Security.Cryptography.X509Certificates; // 사용되지 않음

public class HsvColorChanger : MonoBehaviour
{
    public SpriteRenderer targetSpriteRenderer; // 색상을 변경할 스프라이트 렌더러

    [Header("Sliders")]
    public Slider hueSlider;        // 색상 (Hue) 조절 슬라이더
    public Slider saturationSlider; // 채도 (Saturation) 조절 슬라이더
    public Slider valueSlider;      // 명도 (Value/Brightness) 조절 슬라이더

    [Header("Slider Background Images")]
    public Image hueFillImage;        // Hue 슬라이더의 Fill Image
    public Image saturationFillImage; // Saturation 슬라이더의 Fill Image
    public Image valueFillImage;      // Value 슬라이더의 Fill Image

    private float currentHue = 0.0f;
    private float currentSaturation = 0.0f;
    private float currentValue = 0.0f;

    private const int textureWidth = 256; // 그라데이션 텍스처의 너비

    private SpriteRenderer lastSelectedSpriteRenderer; // 이전에 선택된 SpriteRenderer를 저장하여 중복 업데이트 방지

    void Start()
    {
        InitializeSliders();
        UpdateSliderBackgrounds(); // 초기 배경 업데이트

        // 슬라이더 리스너 등록
        hueSlider.onValueChanged.AddListener(OnHueChanged);
        saturationSlider.onValueChanged.AddListener(OnSaturationChanged);
        valueSlider.onValueChanged.AddListener(OnValueChanged);
    }

    void Update()
    {
        // targetSpriteRenderer가 변경되었는지 확인하고 슬라이더 업데이트 및 색상 불러오기
        if (targetSpriteRenderer != null && targetSpriteRenderer != lastSelectedSpriteRenderer)
        {
            LoadColorFromTarget(); // 새로운 타겟의 색상을 불러와 슬라이더 업데이트
            lastSelectedSpriteRenderer = targetSpriteRenderer; // 현재 타겟 저장
        }
    }
    void InitializeSliders()
    {
        // 슬라이더 값 범위 설정 (0.0f ~ 1.0f)
        hueSlider.minValue = 0f; hueSlider.maxValue = 1f;
        saturationSlider.minValue = 0f; saturationSlider.maxValue = 1f;
        valueSlider.minValue = 0f; valueSlider.maxValue = 1f;

        // Start 시점에선 targetSpriteRenderer가 없을 수 있으므로 초기값만 설정.
        // 실제 색상 불러오기는 LoadColorFromTarget()에서 수행.
        currentHue = 0.0f;
        currentSaturation = 1.0f;
        currentValue = 1.0f;

        hueSlider.value = currentHue;
        saturationSlider.value = currentSaturation;
        valueSlider.value = currentValue;
    }

    /// <summary>
    /// 현재 targetSpriteRenderer의 색상을 PlayerPrefs에서 불러와 슬라이더와 오브젝트에 적용합니다.
    /// 저장된 값이 없으면 targetSpriteRenderer의 현재 색상으로 초기화합니다.
    /// </summary>
    public void LoadColorFromTarget()
    {
        if (targetSpriteRenderer != null)
        {
            string keyPrefix = "CharColor_" + targetSpriteRenderer.gameObject.name + "_";

            // PlayerPrefs에서 저장된 색상 값 불러오기 시도
            if (PlayerPrefs.HasKey(keyPrefix + "H"))
            {
                currentHue = PlayerPrefs.GetFloat(keyPrefix + "H");
                currentSaturation = PlayerPrefs.GetFloat(keyPrefix + "S");
                currentValue = PlayerPrefs.GetFloat(keyPrefix + "V");
            }
            else
            {
                // 저장된 값이 없으면 targetSpriteRenderer의 현재 색상을 HSV로 변환하여 사용
                Color initialColor = targetSpriteRenderer.color;
                Color.RGBToHSV(initialColor, out currentHue, out currentSaturation, out currentValue);
                Debug.Log($"[HsvColorChanger] No saved color for {targetSpriteRenderer.gameObject.name}. Initializing with current sprite color.");
            }

            // 슬라이더 값 업데이트 (onValueChanged 리스너에 의해 자동으로 UpdateTargetColor() 호출됨)
            hueSlider.value = currentHue;
            saturationSlider.value = currentSaturation;
            valueSlider.value = currentValue;
        }
        else
        {
            // 타겟이 없으면 기본값으로 슬라이더를 초기화 (선택사항, 초기 InitializeSliders와 동일하게)
            currentHue = 0.0f;
            currentSaturation = 1.0f;
            currentValue = 1.0f;
            hueSlider.value = currentHue;
            saturationSlider.value = currentSaturation;
            valueSlider.value = currentValue;
        }
        UpdateSliderBackgrounds(); // 슬라이더 배경도 다시 그림
        UpdateTargetColor(); // 불러오거나 초기화된 색상으로 타겟 오브젝트 색상 적용
    }

    // Hue 슬라이더 값이 변경될 때
    void OnHueChanged(float newHue)
    {
        currentHue = newHue;
        UpdateSliderBackgrounds(); // Saturation, Value 슬라이더 배경 업데이트
        UpdateTargetColor();
        SaveColor(); // 색상 변경 시 저장
    }

    // Saturation 슬라이더 값이 변경될 때
    void OnSaturationChanged(float newSaturation)
    {
        currentSaturation = newSaturation;
        UpdateSliderBackgrounds(); // Value 슬라이더 배경 업데이트
        UpdateTargetColor();
        SaveColor(); // 색상 변경 시 저장
    }

    // Value 슬라이더 값이 변경될 때
    void OnValueChanged(float newValue)
    {
        currentValue = newValue;
        UpdateTargetColor();
        SaveColor(); // 색상 변경 시 저장
    }

    // 현재 HSV 값으로 캐릭터 색상 업데이트
    void UpdateTargetColor()
    {
        if (targetSpriteRenderer != null)
        {
            targetSpriteRenderer.color = Color.HSVToRGB(currentHue, currentSaturation, currentValue);
        }
    }

    /// <summary>
    /// 현재 targetSpriteRenderer의 색상 (currentHue, currentSaturation, currentValue)을 PlayerPrefs에 저장합니다.
    /// </summary>
    void SaveColor()
    {
        if (targetSpriteRenderer != null)
        {
            string keyPrefix = "CharColor_" + targetSpriteRenderer.gameObject.name + "_";
            PlayerPrefs.SetFloat(keyPrefix + "H", currentHue);
            PlayerPrefs.SetFloat(keyPrefix + "S", currentSaturation);
            PlayerPrefs.SetFloat(keyPrefix + "V", currentValue);
            PlayerPrefs.Save(); // 변경사항을 즉시 디스크에 저장
            // Debug.Log($"[HsvColorChanger] Saved color for {targetSpriteRenderer.gameObject.name}: H={currentHue}, S={currentSaturation}, V={currentValue}");
        }
    }

    // 애플리케이션이 종료되거나 씬이 언로드될 때 마지막으로 선택된 오브젝트의 색상 저장
    void OnApplicationQuit()
    {
        SaveColor();
    }

    // 슬라이더 배경 이미지 업데이트
    void UpdateSliderBackgrounds()
    {
        // 1. Hue 슬라이더 배경 (무지개색)
        if (hueFillImage != null)
        {
            Texture2D hueTex = new Texture2D(textureWidth, 1);
            for (int i = 0; i < textureWidth; i++)
            {
                float h = (float)i / (textureWidth - 1);
                hueTex.SetPixel(i, 0, Color.HSVToRGB(h, 1.0f, 1.0f)); // Max Saturation, Max Value
            }
            hueTex.Apply();
            hueTex.wrapMode = TextureWrapMode.Clamp;
            Sprite hueSprite = Sprite.Create(hueTex, new Rect(0, 0, textureWidth, 1), Vector2.zero);
            hueFillImage.sprite = hueSprite;
            hueFillImage.type = Image.Type.Sliced; // 또는 Filled, Simple
        }

        // 2. Saturation 슬라이더 배경 (현재 Hue, Value 기반)
        if (saturationFillImage != null)
        {
            Texture2D satTex = new Texture2D(textureWidth, 1);
            for (int i = 0; i < textureWidth; i++)
            {
                float s = (float)i / (textureWidth - 1);
                satTex.SetPixel(i, 0, Color.HSVToRGB(currentHue, s, currentValue));
            }
            satTex.Apply();
            satTex.wrapMode = TextureWrapMode.Clamp;
            Sprite satSprite = Sprite.Create(satTex, new Rect(0, 0, textureWidth, 1), Vector2.zero);
            saturationFillImage.sprite = satSprite;
            saturationFillImage.type = Image.Type.Sliced;
        }

        // 3. Value 슬라이더 배경 (현재 Hue, Saturation 기반)
        if (valueFillImage != null)
        {
            Texture2D valTex = new Texture2D(textureWidth, 1);
            for (int i = 0; i < textureWidth; i++)
            {
                float v = (float)i / (textureWidth - 1);
                valTex.SetPixel(i, 0, Color.HSVToRGB(currentHue, currentSaturation, v));
            }
            valTex.Apply();
            valTex.wrapMode = TextureWrapMode.Clamp;
            Sprite valSprite = Sprite.Create(valTex, new Rect(0, 0, textureWidth, 1), Vector2.zero);
            valueFillImage.sprite = valSprite;
            valueFillImage.type = Image.Type.Sliced;
        }
    }
}