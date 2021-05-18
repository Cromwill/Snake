using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeShopListView : MonoBehaviour
{
    [SerializeField] private SnakeShopPresenter _presenterTemplate;
    [SerializeField] private Transform _container;

    public IEnumerable<SnakeShopPresenter> Presenters { get; private set; }

    public IEnumerable<SnakeShopPresenter> Render(IEnumerable<SnakeData> datas)
    {
        var presenterList = new List<SnakeShopPresenter>();

        foreach (var data in datas)
        {
            var inst = Instantiate(_presenterTemplate, _container);
            inst.Render(data);

            presenterList.Add(inst);
        }

        Presenters = presenterList;
        return presenterList;
    }
}
