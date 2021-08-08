using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using TMPro;

public class SnakeShopPresenter : MonoBehaviour
{
    [SerializeField] private Image _preview;
    [SerializeField] private Image _selectPreview;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private Image _background;
    [SerializeField] private Button _button;
    [Header("Presenter variants")][SerializeField] private Image _buyedBackground;
    [SerializeField] private GameObject _priceGroup;
    [SerializeField] private Color _nonBuyedColor;
    [SerializeField] private Color _buyedColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Sprite _buyedSprite;
    [SerializeField] private Sprite _selectedSprite;

    public event UnityAction<SnakeShopPresenter> ButtonClicked;

    public SnakeData Data { get; private set; }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
    }

    public void Render(SnakeData data)
    {
        Data = data;

        _preview.sprite = data.Preview;
        _price.text = data.Price.ToString();

        _background.color = _nonBuyedColor;

        GemBalance balance = new GemBalance();
        balance.Load(new JsonSaveLoad());

        if (data.Price > balance.Balance)
            _price.color = Color.gray;
    }

    private void OnButtonClicked()
    {
        ButtonClicked?.Invoke(this);
    }

    public void SetBuyed()
    {
        if (!_selectPreview.gameObject.activeSelf)
            _selectPreview.gameObject.SetActive(true);

        _priceGroup.SetActive(false);
        _background.color = _buyedColor;
        _selectPreview.sprite = _buyedSprite;
    }

    public void SetSelected()
    {
        if (!_selectPreview.gameObject.activeSelf)
            _selectPreview.gameObject.SetActive(true);

        _priceGroup.SetActive(false);
        _background.color = _selectedColor;
        _selectPreview.sprite = _selectedSprite;
    }
}
