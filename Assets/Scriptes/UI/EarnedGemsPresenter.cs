using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EarnedGemsPresenter : MonoBehaviour
{
    [SerializeField] private TMP_Text _gemValue;
    [SerializeField] private FromToNumberAnimation _fromToAnimation;

    public void Render(int gemValue)
    {
        _gemValue.text = string.Format("+{0}", gemValue.ToString());
    }

    public void PlayFromToAnimation(int from, int to)
    {
        _fromToAnimation.StartAnimation(from, to);
    }
}