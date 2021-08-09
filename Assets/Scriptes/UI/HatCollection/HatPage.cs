using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HatPage : MonoBehaviour
{
    public void Render(IEnumerable<Sprite> _hats)
    {
        foreach (var hat in _hats)
        {
            var image = new GameObject().AddComponent<Image>();
            image.transform.parent = transform;
            image.sprite = hat;
        }
    }
}
