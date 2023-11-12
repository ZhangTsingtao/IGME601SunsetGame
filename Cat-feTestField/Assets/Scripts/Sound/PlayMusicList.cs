using System.Collections;
using System.Collections.Generic;
using TsingIGME601;
using UnityEngine;

public class PlayMusicList : MonoBehaviour
{
    public List<AudioClip> AudioClips = new List<AudioClip>();

    [SerializeField] private int musicIndex = 0;

    [SerializeField] private double nextMusicTime = 0;

    private void Update()
    {
        if (nextMusicTime < AudioSettings.dspTime)
        {
            PlayMusics();
        }
    }

    private void PlayMusics()
    {
        SoundManager.Instance.PlayMusic(AudioClips[musicIndex], AudioSettings.dspTime);
 
        nextMusicTime = AudioSettings.dspTime + AudioClips[musicIndex].length + 2.0f;

        musicIndex++;
        if (musicIndex == AudioClips.Count) musicIndex = 0;
    }
}
