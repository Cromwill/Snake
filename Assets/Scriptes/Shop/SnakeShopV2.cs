using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class SnakeShopV2 : MonoBehaviour
{
    [SerializeField] private SnakeDataBase _dataBase;
    [SerializeField] private SnakePage _pageTemplate;
    [SerializeField] private Transform _pageContainer;
    [SerializeField] private Transform _previewModelContainer;
    [SerializeField] private ScrollPages _scrollPages;
    [SerializeField] private ParticleSystem _unlockEffect;
    [SerializeField] private TMP_Text _snakeName;
    [SerializeField] private Button _unlockButton;
    [SerializeField] private Animator _unlockButtonAnimation;
    [SerializeField] private Animator _balanceAnimation;

    private List<SnakeShopPresenterV2> _presenters;
    private SnakeShopPresenterV2 _selectedPresenter;
    private Canvas _parentCanvas;
    private GameObject _previewModel;
    private int _itemPerPage = 6;

    public int ItemCost => 4000;

    public event UnityAction<int> Initialized;

    private void OnEnable()
    {
        _unlockButton.onClick.AddListener(OnUnlockButtonClicked);
    }

    private void OnDisable()
    {
        _unlockButton.onClick.RemoveListener(OnUnlockButtonClicked);
    }

    private void Start()
    {
        _parentCanvas = GetComponentInParent<Canvas>();
        var pageTransform = _pageTemplate.transform as RectTransform;
        var canvasTransform = _parentCanvas.transform as RectTransform;
        pageTransform.sizeDelta = new Vector2(canvasTransform.sizeDelta.x, pageTransform.sizeDelta.y);

        var snakeInventory = new SnakeInventory(_dataBase);
        snakeInventory.Load(new JsonSaveLoad());

        var snakeList = _dataBase.Data.ToList();
        var allSnakeCount = snakeList.Count;
        var pageCount = allSnakeCount / _itemPerPage;

        if (snakeInventory.Data.Count() == allSnakeCount)
            _unlockButton.gameObject.SetActive(false);

        if (allSnakeCount % _itemPerPage != 0)
            pageCount++;

        var snakeNumber = 0;

        _presenters = new List<SnakeShopPresenterV2>();
        for (int pageIndex = 0; pageIndex < pageCount; pageIndex++)
        {
            var renderData = new List<SnakeData>(_itemPerPage);
            var instPage = Instantiate(_pageTemplate, _pageContainer);

            for (int i = 0; i < _itemPerPage; i++)
            {
                snakeNumber++;
                if (snakeNumber > allSnakeCount)
                    break;

                renderData.Add(snakeList[snakeNumber - 1]);
            }

            var presenters = instPage.Render(renderData);
            _presenters.AddRange(presenters);
            foreach (var presenter in presenters)
            {
                presenter.Clicked += OnShopItemClicked;

                if (string.Equals(presenter.Data.GUID, snakeInventory.SelectedSnake.GUID))
                {
                    presenter.RenderSelected(presenter.Data);
                    _selectedPresenter = presenter;
                    SelectItem(presenter);
                }
            }
        }

        Initialized?.Invoke(pageCount);
    }

    private void OnShopItemClicked(SnakeShopPresenterV2 presenter)
    {
        SnakeInventory snakeInventory = new SnakeInventory(_dataBase);
        snakeInventory.Load(new JsonSaveLoad());

        if (snakeInventory.Contains(presenter.Data))
        {
            snakeInventory.SelectSnake(presenter.Data);
            SelectItem(presenter);
        }
        else
        {
            _unlockButtonAnimation.SetTrigger("Scale");
        }
    }

    private void SelectItem(SnakeShopPresenterV2 presenter)
    {
        SnakeInventory snakeInventory = new SnakeInventory(_dataBase);
        snakeInventory.Load(new JsonSaveLoad());

        _selectedPresenter.RenderUnselected(_selectedPresenter.Data);
        presenter.RenderSelected(presenter.Data);
        _selectedPresenter = presenter;

        _snakeName.text = presenter.Data.Name;

        snakeInventory.SelectSnake(presenter.Data);
        snakeInventory.Save(new JsonSaveLoad());

        if (_previewModel)
            Destroy(_previewModel);

        _previewModel = Instantiate(presenter.Data.PreviewModel, _previewModelContainer);
        _previewModel.transform.localPosition = Vector3.zero;
        _previewModel.transform.localRotation = Quaternion.identity;
    }

    private void OnUnlockButtonClicked()
    {
        GemBalance gemBalance = new GemBalance();
        gemBalance.Load(new JsonSaveLoad());

        if (gemBalance.Balance < ItemCost)
        {
            _balanceAnimation.SetTrigger("Scale");
            return;
        }

        SnakeInventory snakeInventory = new SnakeInventory(_dataBase);
        snakeInventory.Load(new JsonSaveLoad());
        var unlockedSnake = UnlockRandomSnake();
        snakeInventory.Add(unlockedSnake);
        snakeInventory.Save(new JsonSaveLoad());

        gemBalance.SpendGem(ItemCost);
        gemBalance.Save(new JsonSaveLoad());

        var presenter = _presenters.Find(p => p.Data.GUID == unlockedSnake.GUID);
        SelectItem(presenter);

        var page = _presenters.IndexOf(presenter) / _itemPerPage;
        _scrollPages.ForcePageScroll(page);

        Instantiate(_unlockEffect, _previewModelContainer);

        if (snakeInventory.Data.Count() == _dataBase.Data.Count())
            _unlockButton.gameObject.SetActive(false);
    }

    private SnakeData UnlockRandomSnake()
    {
        var snakeInventory = new SnakeInventory(_dataBase);
        snakeInventory.Load(new JsonSaveLoad());

        var lockedItems = new List<SnakeData>();
        foreach (var snake in _dataBase.Data)
            if (snakeInventory.Contains(snake) == false)
                lockedItems.Add(snake);

        return lockedItems[Random.Range(0, lockedItems.Count)];
    }
}
