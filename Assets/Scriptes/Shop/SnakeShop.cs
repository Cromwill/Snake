using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeShop : MonoBehaviour
{
    [SerializeField] private SnakeDataBase _dataBase;
    [SerializeField] private SnakeShopListView _listView;
    [SerializeField] private Transform _previewModelContainer;

    private IEnumerable<SnakeShopPresenter> _presenters;
    private SnakeShopPresenter _selectedPresenter;
    private GameObject _previewModel;

    private void OnEnable()
    {
        _presenters = _listView.Render(_dataBase.Data);

        var snakeInventory = new SnakeInventory(_dataBase);
        snakeInventory.Load(new JsonSaveLoad());

        foreach (var presenter in _presenters)
        {
            presenter.ButtonClicked += OnPresenterButtonClicked;

            if (snakeInventory.Contains(presenter.Data))
                presenter.SetBuyed();

            if (string.Equals(presenter.Data.GUID, snakeInventory.SelectedSnake.GUID))
            {
                presenter.SetSelected();
                _selectedPresenter = presenter;
                SelectItem(presenter);
            }
        }
    }

    private void OnDisable()
    {
        foreach (var presenter in _presenters)
        {
            presenter.ButtonClicked -= OnPresenterButtonClicked;
            Destroy(presenter.gameObject);
        }
    }

    private void OnPresenterButtonClicked(SnakeShopPresenter presenter)
    {
        SnakeInventory snakeInventory = new SnakeInventory(_dataBase);
        snakeInventory.Load(new JsonSaveLoad());

        if (snakeInventory.Contains(presenter.Data))
        {
            snakeInventory.SelectSnake(presenter.Data);
            SelectItem(presenter);
        }
        else if (TryBuyItem(presenter))
        {
            snakeInventory.SelectSnake(presenter.Data);
            SelectItem(presenter);
            snakeInventory.Add(presenter.Data);
        }

        snakeInventory.Save(new JsonSaveLoad());
    }

    private void SelectItem(SnakeShopPresenter presenter)
    {
        _selectedPresenter.SetBuyed();
        presenter.SetSelected();
        _selectedPresenter = presenter;

        if (_previewModel)
            Destroy(_previewModel);

        _previewModel = Instantiate(presenter.Data.PreviewModel, _previewModelContainer);
        _previewModel.transform.localPosition = Vector3.zero;
        _previewModel.transform.localRotation = Quaternion.identity;
    }

    private bool TryBuyItem(SnakeShopPresenter presenter)
    {
        GemBalance balance = new GemBalance();
        balance.Load(new JsonSaveLoad());

        if (presenter.Data.Price > balance.Balance)
            return false;

        _selectedPresenter.SetBuyed();
        presenter.SetSelected();
        _selectedPresenter = presenter;

        balance.SpendGem(presenter.Data.Price);
        balance.Save(new JsonSaveLoad());

        return true;
    }
}
