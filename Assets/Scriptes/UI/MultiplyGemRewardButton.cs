using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(Button))]
public class MultiplyGemRewardButton : MonoBehaviour
{
    [SerializeField] private EndOfGameCanvas _endOfGameCanvas;
    [SerializeField] private Menu _menu;
    [SerializeField] private TMP_Text _buttonText;

    private Button _button;
    private MultiplyGemReward _multiplyGemReward;
    private AdSettings _adSettings;
    private bool _rewardCollected = false;
    private bool _endCanvasShowed = false;

    public event UnityAction<int> MultiplyUsed;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _multiplyGemReward = new MultiplyGemReward();
    }

    private void OnEnable()
    {
        _adSettings = Singleton<AdSettings>.Instance;

        _button.onClick.AddListener(OnButtonClicked);
        _adSettings.UserEarnedReward += OnUserEarnedReward;
        _endOfGameCanvas.Showed += OnEndOfGameCanvasShowed;
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
        _adSettings.UserEarnedReward -= OnUserEarnedReward;
        _endOfGameCanvas.Showed -= OnEndOfGameCanvasShowed;

        if (_endCanvasShowed == true && _rewardCollected == false)
            _multiplyGemReward.MoveNext();
    }

    private void Start()
    {
        _buttonText.text = $"Collect x{_multiplyGemReward.CurrentGemMultiply}";
    }

    private void OnButtonClicked()
    {
        _adSettings.ShowRewarded();
    }

    private void OnUserEarnedReward()
    {
        MultiplyUsed?.Invoke(_multiplyGemReward.CurrentGemMultiply);

        _rewardCollected = true;
        _multiplyGemReward.Reset();
        _menu.LoadNextLevel();
    }

    private void OnEndOfGameCanvasShowed()
    {
        _endCanvasShowed = true;
    }
}

public class MultiplyGemReward
{
    private readonly int[] MultiplyList = new int[] { 2, 3, 5, 7 };
    private readonly string MultiplyGemIndexKey = nameof(MultiplyGemIndexKey);

    private int _currentIndex;

    public int CurrentGemMultiply => MultiplyList[_currentIndex];

    public MultiplyGemReward()
    {
        if (PlayerPrefs.HasKey(MultiplyGemIndexKey))
            _currentIndex = PlayerPrefs.GetInt(MultiplyGemIndexKey);
        else
            _currentIndex = 0;
    }

    public void MoveNext()
    {
        if (_currentIndex < MultiplyList.Length - 1)
            _currentIndex++;

        PlayerPrefs.SetInt(MultiplyGemIndexKey, _currentIndex);
    }

    public void Reset()
    {
        _currentIndex = 0;
        PlayerPrefs.SetInt(MultiplyGemIndexKey, 0);
    }
}
