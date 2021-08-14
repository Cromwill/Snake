using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class HatPreviewRenderList : MonoBehaviour
{
    [SerializeField] private HatDataBase _dataBase;
    [SerializeField] private HatPage _pageTemplate;

    private int _imagePerPage = 9;

    public event UnityAction<int> Initialized;

    private void Start()
    {
        var allHatCount = _dataBase.Data.Count();
        var pageCount = allHatCount / _imagePerPage;

        if (allHatCount % _imagePerPage != 0)
            pageCount++;

        HatCollection collection = new HatCollection(_dataBase);
        collection.Load(new JsonSaveLoad());
        var collectedHats = collection.Data.ToList();

        var hatNumber = 0;

        for (int pageIndex = 0; pageIndex < pageCount; pageIndex++)
        {
            var instPage = Instantiate(_pageTemplate, transform);

            var renderData = new List<HatData>(_imagePerPage);

            for (int i = 0; i < _imagePerPage; i++)
            {
                hatNumber++;
                if (hatNumber > allHatCount)
                    break;

                if (hatNumber - 1 < collectedHats.Count)
                    renderData.Add(collectedHats[hatNumber - 1]);
                else
                    renderData.Add(null);
            }

            instPage.Render(renderData);
        }

        Initialized?.Invoke(pageCount);
    }
}
