using System.Collections;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class HapticsHelper : MonoBehaviour
{
    [SerializeField] private SnakeInitializer _initializer;
    [SerializeField] private InputSelector _inputSelector;
    [SerializeField] private AudioClip _pickObject;
    [SerializeField] private AudioClip _obstacleFounded;
    [SerializeField] private int _moveAudioIndex;

    [Header("Audio Settings")]

    [SerializeField] private float _obstacleFoundedVolume;
    [SerializeField] private float _obstacleFoundedPitch;

    private Head _snakeHead;
    private Snake _snake;
    private AudioSource _snakeAudioSource;

    private float _defoultSoundVolume;
    private float _defoultSoundPitch;

    private void OnEnable()
    {
        _initializer = FindObjectOfType<SnakeInitializer>();
        _initializer.Initialized += OnSnakeInitializer;

        if (_inputSelector == null)
            _inputSelector = FindObjectOfType<InputSelector>();
    }

    private void OnDisable()
    {
#if UNITY_IOS
        MMNViOS.iOSReleaseHaptics();
#endif

        _initializer.Initialized -= OnSnakeInitializer;
        if (_snakeHead != null)
        {
            _snakeHead.FoodFinded -= OnFoodFinded;
            _snakeHead.ObstacleEntered -= OnObstacleEntered;
        }
    }

    private void Awake()
    {
#if UNITY_IOS
        MMNViOS.iOSInitializeHaptics();
#endif
    }

    private void OnSnakeInitializer(Snake snake)
    {
        _snakeAudioSource = snake.GetComponent<AudioSource>();
        _snakeAudioSource.clip = _pickObject;

        _defoultSoundPitch = _snakeAudioSource.pitch;
        _defoultSoundVolume = _snakeAudioSource.volume;

        _snakeHead = snake.GetComponentInChildren<Head>();
        _snakeHead.FoodFinded += OnFoodFinded;
        _snakeHead.ObstacleEntered += OnObstacleEntered;
    }

    private void OnFoodFinded(Food food)
    {
        HapticPlay(HapticTypes.SoftImpact, _pickObject);
    }

    private void OnObstacleEntered(Obstacle obstacle)
    {
        HapticPlay(HapticTypes.Warning, _obstacleFounded);
    }

    private void HapticPlay(HapticTypes hapticTypes, AudioClip audio, bool isDefoult = true)
    {
        _snakeAudioSource.volume = isDefoult ? _defoultSoundVolume : _obstacleFoundedVolume;
        _snakeAudioSource.pitch = isDefoult ? _defoultSoundPitch : _obstacleFoundedPitch;

        MMVibrationManager.Haptic(hapticTypes);
        _snakeAudioSource.clip = audio;
        _snakeAudioSource.Play();
    }
}
