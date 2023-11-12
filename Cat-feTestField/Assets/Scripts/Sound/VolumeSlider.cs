using System.Collections;
using System.Collections.Generic;
using TsingIGME601;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();

        SoundManager.Instance.ChangeMasterVolume(slider.value);

        slider.onValueChanged.AddListener(ChangeVolume);
    }

    void ChangeVolume(float val) 
    {
        SoundManager.Instance.ChangeMasterVolume(val);
    }
}
