using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HatPage : MonoBehaviour
{
    [SerializeField] private HatCollectionItemPresenter _template;

    public void Render(IEnumerable<HatData> _hats)
    {
        foreach (var hat in _hats)
        {
            var inst = Instantiate(_template, transform);
            inst.Render(hat);
        }
    }
}
