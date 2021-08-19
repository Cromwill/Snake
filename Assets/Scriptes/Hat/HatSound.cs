using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HatSound : MonoBehaviour
{
    [SerializeField] private AudioClip _pickSound;
    [SerializeField] private AudioClip _loseSound;

    private AudioSource _audio;
    private Setting _setting;

    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _setting = FindObjectOfType<Setting>();
    }

    public void PlayHatPickedSound() => AudioPlay(_pickSound);

    public void PlayHatLosedSound() => AudioPlay(_loseSound);

    private void AudioPlay(AudioClip clip)
    {
        if (_setting.SoundEnable)
        {
            _audio.clip = clip;
            _audio.Play();
        }
    }
}