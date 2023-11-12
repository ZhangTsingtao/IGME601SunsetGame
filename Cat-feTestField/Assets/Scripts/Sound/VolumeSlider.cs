using System.Collections;
using System.Collections.Generic;
using TsingIGME601;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider slider;

    [Header("True for BGM, false for SFX")]
    public bool isBGM = true;
    private void Start()
    {
        slider = GetComponent<Slider>();

        ChangeVolume(slider.value);

        slider.onValueChanged.AddListener(ChangeVolume);
    }

    void ChangeVolume(float val) 
    {
        if(isBGM)
        {
            SoundManager.Instance.ChangeBGMVolume(val);
        }
        else
        {
            SoundManager.Instance.ChangeSFXVolume(val);
        }
    }
}
