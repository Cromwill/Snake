using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class SnakeShopPresenter : MonoBehaviour
{
    [SerializeField] private Image _preview;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Button _selectButton;

    public event UnityAction<SnakeShopPresenter> SelectedButtonClicked;

    public SnakeData Data { get; private set; }

    private Text _buttonText;

    private void Awake()
    {
        _buttonText = _selectButton.GetComponentInChildren<Text>();
    }

    private void OnEnable()
    {
        _selectButton.onClick.AddListener(OnSelectButtonClicked);
    }

    private void OnDestroy()
    {
        _selectButton.onClick.RemoveListener(OnSelectButtonClicked);
    }

    public void Render(SnakeData data)
    {
        Data = data;

        _preview.sprite = data.Preview;
        _name.text = data.Name;
    }

    private void OnSelectButtonClicked()
    {
        SelectedButtonClicked?.Invoke(this);
    }

    public void SetSelected()
    {
        _buttonText.text = "Selected";
        _buttonText.color = Color.green;
    }

    public void SetUnselected()
    {
        _buttonText.text = "Select";
        _buttonText.color = Color.black;
    }
}
