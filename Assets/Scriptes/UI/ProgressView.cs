using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressView : MonoBehaviour
{
    [SerializeField] private Slider _progress;
    [SerializeField] private TMP_Text _currentLevelView;
    [SerializeField] private TMP_Text _nextLevelView;

    private SnakeInitializer _snakeInitializer;
    private Snake _snake;

    public float Value => _progress.value;

    private void Awake()
    {
        _snakeInitializer = FindObjectOfType<SnakeInitializer>();
    }

    private void OnEnable()
    {
        _snakeInitializer.Initialized += OnSnakeInitizlized;
    }

    private void OnDisable()
    {
        _snakeInitializer.Initialized -= OnSnakeInitizlized;
    }

    private void OnSnakeInitizlized(Snake snake)
    {
        _snake = snake;
    }

    private void Start()
    {
        var currentLevelData = new CurrentLevelData();
        currentLevelData.Load(new JsonSaveLoad());

        _currentLevelView.SetText((currentLevelData.CurrentLevel + 1).ToString());
        _nextLevelView.SetText((currentLevelData.CurrentLevel + 2).ToString());
    }

    private void Update()
    {
        if (_snake)
            _progress.value = _snake.NormalizeDistanceCovered;
    }

}
