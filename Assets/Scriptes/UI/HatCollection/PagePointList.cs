using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagePointList : MonoBehaviour
{
    [SerializeField] private HatPreviewRenderList _hatRenderList;
    [SerializeField] private ScrollPages _scrollPages;
    [SerializeField] private PagePoint _template;
    [SerializeField] private Transform _container;

    private List<PagePoint> _pagePoints;
    private int _currentPage;

    private void OnEnable()
    {
        _hatRenderList.Initialized += Render;
        _scrollPages.Scrolled += OnPageScrolled;
    }

    private void OnDisable()
    {
        _hatRenderList.Initialized -= Render;
        _scrollPages.Scrolled -= OnPageScrolled;
    }

    private void OnPageScrolled(int pageIndex)
    {
        if (_pagePoints == null || _pagePoints.Count == 0)
            return;

        _pagePoints[_currentPage].Deselect();
        _pagePoints[pageIndex].Select();

        _currentPage = pageIndex;
    }

    public void Render(int pageCount)
    {
        if (pageCount <= 1)
            return;

        _pagePoints = new List<PagePoint>(pageCount);

        for (int i = 0; i < pageCount; i++)
        {
            var inst = Instantiate(_template, _container);
            _pagePoints.Add(inst);
        }

        _currentPage = 0;
        _pagePoints[0].Select();
    }
}
