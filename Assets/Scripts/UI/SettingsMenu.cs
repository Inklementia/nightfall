using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] AudioMixerGroup _audioMixerBg;
    [SerializeField] Toggle _isFullScreenToggle;
    [SerializeField] Toggle _isStandardResToggle;
    [SerializeField] Toggle _isMediumResToggle;
    [SerializeField] private Slider BgSlider;
    private float _originalBgVolume;
    private bool _isFullScreen;
    private float _currentResolutionWidth;
    private float _currentResolutionHeight;
    private bool _isBGOff = false;

    private void Start()
    {
        _audioMixer.GetFloat("BgVolume", out _originalBgVolume);
        _isFullScreen = Screen.fullScreen;
        _isFullScreenToggle.isOn = _isFullScreen;

        // when game starts get size of the screen (I know it is incorrect, but deadline was pressing onto me)
        if(Screen.currentResolution.width == 1920 && _isFullScreen || Screen.width == 1920 && !_isFullScreen)
        {
            _isStandardResToggle.isOn = true;
        }
        else if(Screen.currentResolution.width == 1600 && _isFullScreen || Screen.width == 1600 && !_isFullScreen)
        {
            _isMediumResToggle.isOn = true;
        }
        print(Screen.currentResolution.width);
        
    }

    // called from canvas

    //for general sound
    public void SetVolume (float volume)
    {
        _audioMixer.SetFloat("Volume", volume);
    }
    // for music
    public void SetBgVolume(float volume)
    {
        if(!_isBGOff) _audioMixerBg.audioMixer.SetFloat("BgVolume", volume);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        _isFullScreen = isFullScreen;
    }

    // to turn off bg music
    public void SetBgMusic(bool isBgOff)
    {
        _isBGOff = isBgOff;
        //float originalVolume = _audioMixer.GetFloat("BgVolume");
        if (isBgOff)
        {
            _audioMixerBg.audioMixer.GetFloat("BgVolume", out _originalBgVolume);
            BgSlider.interactable = false;
            _audioMixerBg.audioMixer.SetFloat("BgVolume", -80f);
        }
        else
        {
            BgSlider.interactable = true;
            _audioMixerBg.audioMixer.SetFloat("BgVolume", _originalBgVolume);
        }
       
    }

    public void SetStandartRes()
    {
        Screen.SetResolution(1920, 1080, _isFullScreen);
    }
    public void SetMediumRes()
    {
        Screen.SetResolution(1600, 900, _isFullScreen);
    }
}
