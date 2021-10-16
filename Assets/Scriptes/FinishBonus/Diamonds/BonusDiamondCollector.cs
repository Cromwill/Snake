using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDiamondCollector : MonoBehaviour
{
    [SerializeField] private BonusDiamondCollectedPresenter _collectedPresenter;
    [SerializeField] private BonusHapticHelper _hapticHelper;

    public int CollectedDiamondCost { get; private set; }

    private List<BonusDiamond> _allDiamonds;

    public void Init(IEnumerable<BonusDiamond> diamonds)
    {
        _allDiamonds = new List<BonusDiamond>(diamonds);
        CollectedDiamondCost = 0;

        foreach (var diamond in diamonds)
        {
            diamond.Collected += OnDiamondCollected;
            diamond.Collected += _hapticHelper.OnDiamondCollected;
        }
    }

    private void OnDiamondCollected(BonusDiamond diamond)
    {
        diamond.Collected -= OnDiamondCollected;
        diamond.Collected -= _hapticHelper.OnDiamondCollected;

        CollectedDiamondCost += diamond.Cost;
        _allDiamonds.Remove(diamond);

        _collectedPresenter.UpdateCollectedValue(CollectedDiamondCost);
    }

    private void OnDisable()
    {
        if (_allDiamonds == null)
            return;

        foreach (var diamod in _allDiamonds)
            diamod.Collected -= OnDiamondCollected;

        _allDiamonds.Clear();
    }
}
