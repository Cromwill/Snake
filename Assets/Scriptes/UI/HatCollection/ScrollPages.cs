using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(ScrollRect))]
public class ScrollPages : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private float _lerpSpeed = 5f;

    private ScrollRect _scrollRect;
    private float _targetContentPosition;
    private int _currentPage;
    private bool _pointerDown;

    public event UnityAction<int> Scrolled;

    private void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    private void OnEnable()
    {
        _scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
    }

    private void OnDisable()
    {
        _scrollRect.onValueChanged.RemoveListener(OnScrollValueChanged);
    }

    private void Update()
    {
        if (_pointerDown)
            return;

        var nextContentPosition = new Vector2(_targetContentPosition, _content.anchoredPosition.y);
        if (_content.anchoredPosition == nextContentPosition)
            return;

        _content.anchoredPosition = Vector2.Lerp(_content.anchoredPosition, nextContentPosition, _lerpSpeed * Time.deltaTime);
    }

    private void OnScrollValueChanged(Vector2 value)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var pageCount = _content.childCount;

        if (pageCount > 1)
        {
            var controlPoint = 1f / (pageCount - 1);

            var scrollShift = _scrollRect.horizontalNormalizedPosition % (controlPoint);
            _currentPage = (int)(_scrollRect.horizontalNormalizedPosition / controlPoint);

            if (scrollShift >= controlPoint / 2f)
                _currentPage++;

            _targetContentPosition = -1 * controlPoint * _currentPage * _content.sizeDelta.x;

            Scrolled?.Invoke(_currentPage);
        }

        _pointerDown = false;
    }
}
