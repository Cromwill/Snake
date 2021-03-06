using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelTestPanel : MonoBehaviour
{
    [SerializeField] private Menu _menu;
    [SerializeField] private Slider _levelSlider;
    [SerializeField] private Button _selectButton;
    [SerializeField] private Text _buttonText;

    private int _currentLevelValue;

    private void OnEnable()
    {
        _levelSlider.onValueChanged.AddListener(OnLevelSliderValueChanged);
        _selectButton.onClick.AddListener(OnSelectButtonClicked);
    }

    private void OnDisable()
    {
        _levelSlider.onValueChanged.RemoveListener(OnLevelSliderValueChanged);
        _selectButton.onClick.RemoveListener(OnSelectButtonClicked);
    }

    private void Start()
    {
        _currentLevelValue = 1;
        OnLevelSliderValueChanged(_currentLevelValue);
    }

    private void OnLevelSliderValueChanged(float value)
    {
        _currentLevelValue = (int)value;
        _buttonText.text = string.Format("Load {0} level", value);
    }

    private void OnSelectButtonClicked()
    {
        _menu.LoadlLevelWithSave(_currentLevelValue - 1);
    }
}
