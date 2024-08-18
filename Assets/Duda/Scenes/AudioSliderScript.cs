using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSliderScript : MonoBehaviour {
    [SerializeField]  private Slider _slider;
    
    [SerializeField] private TextMeshProUGUI _sliderText;

    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private TextMeshProUGUI ValueText;
    [SerializeField] private AudioMixMode MixMode;

    public void onChangeSlider(float Value)
    {
        ValueText.SetText(($"{Value.ToString("N4")}"));
        switch (MixMode)
        {
            case AudioMixMode.LinearAudioSourceVolume:
                AudioSource.volume = Value;
                break;
            case AudioMixMode.LinearMixerVolume:
                Mixer.SetFloat("Volume", (-80 + Value * 100));
                break;
            case AudioMixMode.LogrithmicMixerVolume:
                Mixer.SetFloat("Volume", Mathf.Log10(Value) * 20);
                break;
        }
        PlayerPrefs.SetFloat("Volume", Value);
        PlayerPrefs.Save();
    }
    public enum AudioMixMode
    {
        LinearAudioSourceVolume,
        LinearMixerVolume,
        LogrithmicMixerVolume
    }
    // Start is called before the first frame update
    void Start()
    {
        Mixer.SetFloat("Volume", Mathf.Log10(PlayerPrefs.GetFloat("Volume", 1) * 20));
    
        _slider.value = _slider.maxValue;
        onChangeSlider(_slider.maxValue);
        
        _slider.onValueChanged.AddListener(onChangeSlider);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
