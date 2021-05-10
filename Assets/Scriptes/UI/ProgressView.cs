using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Slider))]
public class ProgressView : MonoBehaviour
{
    [SerializeField] private TMP_Text _currentLevelView;
    [SerializeField] private TMP_Text _nextLevelView;
    [SerializeField] private TMP_Text _gemCountView;

    private Slider _progress;
    private Track _track;

    private void Start()
    {
        _progress = GetComponent<Slider>();
        _track = FindObjectOfType<Track>();

        int levelNumber = GameDataStorage.LoadProgress();

        _currentLevelView.SetText(levelNumber.ToString());
        _nextLevelView.SetText((levelNumber + 1).ToString());
        _gemCountView.SetText(GameDataStorage.LoadGemCount().ToString());
    }

    private void Update()
    {
        _progress.value = _track.PlayerDistanceTraveleds;
    }

}
