using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EarnedGemsPresenter : MonoBehaviour
{
    [SerializeField] private TMP_Text _gemValue;

    public void Render(int gemValue)
    {
        _gemValue.text = string.Format("+{0}", gemValue.ToString());
    }
}