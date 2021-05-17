using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeShop : MonoBehaviour
{
    [SerializeField] private SnakeDataBase _dataBase;
    [SerializeField] private SnakeShopListView _listView;

    private IEnumerable<SnakeShopPresenter> _presenters;

    private void OnEnable()
    {
        _presenters = _listView.Render(_dataBase.Data);

        foreach (var presenter in _presenters)
            presenter.SelectedButtonClicked += OnSelectedButtonClicked;
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
    }
}
