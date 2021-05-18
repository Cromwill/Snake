using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeShop : MonoBehaviour
{
    [SerializeField] private SnakeDataBase _dataBase;
    [SerializeField] private SnakeShopListView _listView;

    private IEnumerable<SnakeShopPresenter> _presenters;
    private SnakeShopPresenter _selectedPresenter;

    private void OnEnable()
    {
        _presenters = _listView.Render(_dataBase.Data);

        var snakeInventory = new SnakeInventory(_dataBase);
        snakeInventory.Load(new JsonSaveLoad());

        foreach (var presenter in _presenters)
        {
            presenter.SelectedButtonClicked += OnSelectedButtonClicked;

            if (string.Equals(presenter.Data.GUID, snakeInventory.SelectedSnake.GUID))
            {
                presenter.SetSelected();
                _selectedPresenter = presenter;
            }
        }
    }

    private void OnDisable()
    {
        foreach (var presenter in _presenters)
        {
            presenter.SelectedButtonClicked -= OnSelectedButtonClicked;
            Destroy(presenter.gameObject);
        }
    }

    private void OnSelectedButtonClicked(SnakeShopPresenter presenter)
    {
        SnakeInventory snakeInventory = new SnakeInventory(_dataBase);
        snakeInventory.SelectSnake(presenter.Data);

        snakeInventory.Save(new JsonSaveLoad());

        _selectedPresenter.SetUnselected();
        presenter.SetSelected();
        _selectedPresenter = presenter;
    }
}
