using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Slider))]
public class ProgressView : MonoBehaviour
{
    [SerializeField] private TMP_Text _currentLevelView;
    [SerializeField] private TMP_Text _nextLevelView;

    private Slider _progress;
    private Snake _snake;

    private void Start()
    {
        _progress = GetComponent<Slider>();
        _snake = FindObjectOfType<Snake>();

        int levelNumber = GameDataStorage.LoadProgress();

        _currentLevelView.SetText(levelNumber.ToString());
        _nextLevelView.SetText((levelNumber + 1).ToString());
    }

    private void Update()
    {
        _progress.value = _snake.NormalizeDistanceCovered;
    }

}
