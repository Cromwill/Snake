using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GemBalancePresenter : MonoBehaviour
{
    [SerializeField] private TMP_Text _gemValue;

    private GemBalance _gemBalance;

    private void OnEnable()
    {
        GemBalance.GemChanged += OnGemValueChanged;
    }

    private void OnGemValueChanged(int gemValue)
    {
        if (_gemBalance == null)
        {
            _gemBalance = new GemBalance();
            _gemBalance.Load(new JsonSaveLoad());
        }

        _gemValue.text = _gemBalance.Balance.ToString();
    }

    private void Start()
    {
        _gemBalance = new GemBalance();
        _gemBalance.Load(new JsonSaveLoad());

        _gemValue.text = _gemBalance.Balance.ToString();
    }
}
