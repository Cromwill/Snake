using UnityEngine;
using MoreMountains.NiceVibrations;

public class BonusHapticHelper : MonoBehaviour
{
    [Header("Audio clips")]
    [SerializeField] private AudioClip _pickObject;
    [SerializeField] private AudioClip _victory;
    [SerializeField] private BonusFinish _finish;

    private AudioSource _snakeAudioSource;
    private Setting _setting;

    private void OnEnable()
    {
        _finish.Finished += OnFinished;
    }

    private void OnDisable()
    {
        _finish.Finished -= OnFinished;
    }

    private void Awake()
    {
        _snakeAudioSource = GetComponent<AudioSource>();
        _setting = FindObjectOfType<Setting>();
    }

    public void OnDiamondCollected(BonusDiamond diamond)
    {
        HapticPlay(HapticTypes.SoftImpact, _pickObject);
    }

    public void OnFinished()
    {
        HapticPlay(HapticTypes.HeavyImpact, _victory);
    }

    private void HapticPlay(HapticTypes hapticTypes, AudioClip audio, bool isDefoult = true)
    {
        if (_setting.VibrationEnable)
        {
            MMVibrationManager.Haptic(hapticTypes);
        }

        if (_setting.SoundEnable)
        {
            _snakeAudioSource.clip = audio;
            _snakeAudioSource.Play();
        }
    }

}