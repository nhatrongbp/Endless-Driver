using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public Slider musicSlider, soundSlider;
    // Start is called before the first frame update
    public void OnMusicSliderChanged()
    {
        AudioManager.instance.MusicVolume(musicSlider.value);
    }

    // Update is called once per frame
    public void OnSoundSliderChanged()
    {
        AudioManager.instance.SoundVolume(soundSlider.value);
    }
}
