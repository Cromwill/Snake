using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressView : MonoBehaviour
{
    [SerializeField] private Slider _progress;
    [SerializeField] private TMP_Text _currentLevelView;
    [SerializeField] private TMP_Text _nextLevelView;

    private Snake _snake;

    private void Start()
    {
        _snake = FindObjectOfType<Snake>();

        var currentLevelData = new CurrentLevelData();
        currentLevelData.Load(new JsonSaveLoad());

        _currentLevelView.SetText(currentLevelData.CurrentLevel.ToString());
        _nextLevelView.SetText((currentLevelData.CurrentLevel + 1).ToString());
    }

    private void Update()
    {
        _progress.value = _snake.NormalizeDistanceCovered;
    }

}
