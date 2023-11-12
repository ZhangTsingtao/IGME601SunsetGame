using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsingIGME601
{
    public class PlayOneSound : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;
        public void PlayOneSoundEffect()
        {
            if (_clip == null)
            {
                Debug.LogWarning("Audio clip not attached! Check the prefab");
                return;
            }
            SoundManager.Instance.PlaySound(_clip);
        }
    }
}

