using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip _music;

    private AudioSource _audioSource;
    private Setting _settings;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _settings = FindObjectOfType<Setting>();
    }

    private void OnEnable()
    {
        _settings.SoundSettingChanged += OnSettingChanged;
    }

    private void OnDisable()
    {
        _settings.SoundSettingChanged -= OnSettingChanged;
    }

    private void Start()
    {
        UpdateMusic();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }

    private void OnSettingChanged()
    {
        UpdateMusic();
    }

    private void UpdateMusic()
    {
        if (_settings.SoundEnable == false)
        {
            _audioSource.Stop();
        }
        else
        {
            _audioSource.clip = _music;
            _audioSource.loop = true;
            _audioSource.Play();
        }
    }

}
