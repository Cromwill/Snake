using UnityEngine;
using MoreMountains.NiceVibrations;

public class HapticsHelper : MonoBehaviour
{
    [SerializeField] private BackgroundMusic _backgroundMusic;
    [Header("Audio clips")]
    [SerializeField] private AudioClip _pickObject;
    [SerializeField] private AudioClip _obstacleFounded;
    [SerializeField] private AudioClip _victory;
    [SerializeField] private AudioClip _startFinish;

    [Header("Audio Settings")]

    [SerializeField] private float _obstacleFoundedVolume;
    [SerializeField] private float _obstacleFoundedPitch;

    private Head _snakeHead;
    private Snake _snake;
    private AudioSource _snakeAudioSource;
    private FinishTrigger _finishTrigger;
    private Setting _setting;
    private SnakeInitializer _initializer;

    private float _defoultSoundVolume;
    private float _defoultSoundPitch;

    private void OnEnable()
    {
        _initializer.Initialized += OnSnakeInitializer;
        _finishTrigger.PlayerFinished += OnPlayerFinishedStart;
    }

    private void OnDisable()
    {
#if UNITY_IOS
        MMNViOS.iOSReleaseHaptics();
#endif

        _initializer.Initialized -= OnSnakeInitializer;
        _finishTrigger.PlayerFinished -= OnPlayerFinishedStart;

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

        _finishTrigger = FindObjectOfType<FinishTrigger>();
        _initializer = FindObjectOfType<SnakeInitializer>();
        _setting = FindObjectOfType<Setting>();
    }

    private void OnSnakeInitializer(Snake snake)
    {
        _snakeAudioSource = snake.GetComponent<AudioSource>();
        _snakeAudioSource.clip = _pickObject;
        var boneMovement = snake.GetComponent<SnakeBoneMovement>();
        boneMovement.FullCrawled += OnPlayerFinished;
        boneMovement.PartiallyCrawled += OnPlayerFinished;
        _defoultSoundPitch = _snakeAudioSource.pitch;
        _defoultSoundVolume = _snakeAudioSource.volume;

        _snakeHead = snake.GetComponentInChildren<Head>();
        _snakeHead.FoodFinded += OnFoodFinded;
        _snakeHead.ObstacleEntered += OnObstacleEntered;
    }

    private void OnFoodFinded(Food food) => HapticPlay(HapticTypes.SoftImpact, _pickObject, false);
    private void OnObstacleEntered(Obstacle obstacle) => HapticPlay(HapticTypes.Warning, _obstacleFounded, false);
    private void OnPlayerFinished() => HapticPlay(HapticTypes.HeavyImpact, _victory, false);
    private void OnPlayerFinished(float value) => HapticPlay(HapticTypes.HeavyImpact, _victory, false);
    private void OnPlayerFinishedStart()
    {
        HapticPlay(HapticTypes.Success, _startFinish, false);
        _backgroundMusic.StopMusic();
    }

    private void HapticPlay(HapticTypes hapticTypes, AudioClip audio, bool isDefoult = true)
    {
        if (_setting.VibrationEnable)
            MMVibrationManager.Haptic(hapticTypes);

        if (_setting.SoundEnable)
        {
            _snakeAudioSource.volume = isDefoult ? _defoultSoundVolume : _obstacleFoundedVolume;
            _snakeAudioSource.pitch = isDefoult ? _defoultSoundPitch : _obstacleFoundedPitch;

            _snakeAudioSource.loop = false;
            _snakeAudioSource.clip = audio;
            _snakeAudioSource.Play();
        }
    }
}
