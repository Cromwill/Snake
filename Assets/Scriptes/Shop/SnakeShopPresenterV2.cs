using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class SnakeShopPresenterV2 : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image _background; 
    [SerializeField] private Image _snakePreview; 
    [SerializeField] private Image _focus;
    [Header("Content")]
    [SerializeField] private Sprite _defaultBackground;
    [SerializeField] private Sprite _buyedBackground;

    private Button _selfButton;

    public SnakeData Data { get; private set; }

    public event UnityAction<SnakeShopPresenterV2> Clicked;

    private void Awake()
    {
        _selfButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _selfButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _selfButton.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        Clicked?.Invoke(this);
    }

    public void RenderSelected(SnakeData data)
    {
        Data = data;
        _background.sprite = _buyedBackground;
        _snakePreview.sprite = data.Preview;
        _focus.enabled = true;
    }

    public void RenderUnselected(SnakeData data)
    {
        Data = data;
        _background.sprite = _buyedBackground;
        _snakePreview.sprite = data.Preview;
        _focus.enabled = false;
    }

    public void RenderLocked(SnakeData data)
    {
        Data = data;
        _background.sprite = _defaultBackground;
        _snakePreview.sprite = data.Preview;
        _focus.enabled = false;
    }
}
