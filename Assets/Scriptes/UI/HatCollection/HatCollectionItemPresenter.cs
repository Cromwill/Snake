using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HatCollectionItemPresenter : MonoBehaviour
{
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

        _selfImage.sprite = data.HatPreview;
        _focus.enabled = true;
    }

    public void RenderLocked()
    {
        _selfImage.sprite = _lockedBorder;
        _focus.enabled = false;
    }
}
