using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HatCollectionItemPresenter : MonoBehaviour
{
    [SerializeField] private Image _border;
    [SerializeField] private Image _selfImage;
    [SerializeField] private Image _focus;
    [SerializeField] private Sprite _unlockedBorder;
    [SerializeField] private Sprite _lockedBorder;

    public void Render(HatData data)
    {
        if (data == null)
        {
            RenderLocked();
            return;
        }

        _border.sprite = _unlockedBorder;
        _selfImage.enabled = true;
        _selfImage.sprite = data.HatPreview;
        _focus.enabled = true;
    }

    public void RenderLocked()
    {
        _border.sprite = _lockedBorder;
        _selfImage.enabled = false;
        _focus.enabled = false;
    }
}
