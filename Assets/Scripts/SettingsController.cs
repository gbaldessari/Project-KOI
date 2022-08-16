using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectSlider;
    [SerializeField] private Slider uiSlider;

    private readonly string MUSIC_KEY = "MusicVolumen";
    private readonly string EFFECTS_KEY = "EffectsVolumen";
    private readonly string UI_KEY = "UIVolumen";

    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        effectSlider.value = PlayerPrefs.GetFloat(EFFECTS_KEY, 1f);
        uiSlider.value = PlayerPrefs.GetFloat(UI_KEY, 1f);

        SetMusicVolumen(musicSlider.value);
        SetEffectsVolumen(effectSlider.value);
        SetUIVolumen(uiSlider.value);

        musicSlider.onValueChanged.AddListener(SetMusicVolumen);
        effectSlider.onValueChanged.AddListener(SetEffectsVolumen);
        uiSlider.onValueChanged.AddListener(SetUIVolumen);
    }



    private void SetMusicVolumen(float value)
    {
        float volumen = Mathf.Clamp(Mathf.Log10(value) * 20, -80, 0);
        audioMixer.SetFloat(MUSIC_KEY, volumen);
        PlayerPrefs.SetFloat(MUSIC_KEY, value);
    }

    private void SetEffectsVolumen(float value)
    {
        float volumen = Mathf.Clamp(Mathf.Log10(value) * 20, -80, 0);
        audioMixer.SetFloat(EFFECTS_KEY, volumen);
        PlayerPrefs.SetFloat(EFFECTS_KEY, value);
    }

    private void SetUIVolumen(float value)
    {
        float volumen = Mathf.Clamp(Mathf.Log10(value) * 20, -80, 0);
        audioMixer.SetFloat(UI_KEY, volumen);
        PlayerPrefs.SetFloat(UI_KEY, value);
    }
}