using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsingIGME601
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager instance;
        public static SoundManager Instance { get { return instance; } }

        [SerializeField] private AudioSource musicSource, effectSource;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            musicSource = GameObject.Find("MusicSource").GetComponent<AudioSource>();
            effectSource = GameObject.Find("EffectSource").GetComponent<AudioSource>();
        }

        public void PlaySound (AudioClip clip)
        {
            effectSource.PlayOneShot(clip);
            Debug.Log("PlaySound");
        }

        public void PlayMusic (AudioClip clip, double startTime)
        {
            musicSource.clip = clip;
            musicSource.PlayScheduled(startTime);
        }

        public void ChangeMasterVolume(float value)
        {
            AudioListener.volume = value;
        }

    }
}


