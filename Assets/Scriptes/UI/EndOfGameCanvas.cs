using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(Animator))]
public class EndOfGameCanvas : MonoBehaviour
{
    [SerializeField] private HatDataBase _dataBase;
    [SerializeField] private EarnedGemsPresenter _eargedGems;
    [SerializeField] private GameObject _hatBonus;
    [SerializeField] private GameObject _achievementObject;

    private Canvas _selfCanvas;
    private Animator _selfAnimator;
    private Pole _pole;
    private BonusFinish _bonusFinish;
    private BonusDiamondCollector _diamondCollector;
    private HatSpawner _hatSpawner;
    private SnakeHat _hat;
    private HatData _hatData;

    private void Awake()
    {
        _selfCanvas = GetComponent<Canvas>();
        _selfAnimator = GetComponent<Animator>();
        _pole = FindObjectOfType<Pole>();
        _bonusFinish = FindObjectOfType<BonusFinish>();
        _diamondCollector = FindObjectOfType<BonusDiamondCollector>();
        _hatSpawner = FindObjectOfType<HatSpawner>();
    }

    private void OnEnable()
    {
        if (_pole)
            _pole.SnakeCrawled += OnSnakeCrawled;
        if (_bonusFinish)
            _bonusFinish.Finished += OnBonusFinishCrawled;
        if (_hatSpawner)
            _hatSpawner.Spawned += OnHatSpawned;
    }

    private void OnDisable()
    {
        if (_pole)
            _pole.SnakeCrawled -= OnSnakeCrawled;
        if (_bonusFinish)
            _bonusFinish.Finished -= OnBonusFinishCrawled;
        if (_hatSpawner)
            _hatSpawner.Spawned -= OnHatSpawned;
    }

    private void OnSnakeCrawled(int gemValue)
    {
        _selfAnimator.SetTrigger("Show");
        _selfCanvas.enabled = true;
        _eargedGems.Render(gemValue);

        GemBalance gemBalance = new GemBalance();
        gemBalance.Load(new JsonSaveLoad());

        if (_hat != null && _hat.OnSnake)
        {
            _hatBonus.SetActive(true);
            _eargedGems.PlayFromToAnimation(gemValue, gemValue + 100);
            gemValue += 100;

            if (HasInCollection(_hatData) == false)
            {
                _achievementObject.SetActive(true);
                HatCollection collection = new HatCollection(_dataBase);
                collection.Load(new JsonSaveLoad());
                collection.Add(_hatData);
                collection.SelectHat(_hatData);
                collection.Save(new JsonSaveLoad());
            }
        }


        gemBalance.Add(gemValue);
        gemBalance.Save(new JsonSaveLoad());
    }

    private void OnBonusFinishCrawled()
    {
        _selfAnimator.SetTrigger("Show");
        _selfCanvas.enabled = true;
        _eargedGems.Render(_diamondCollector.CollectedDiamondCost);

        GemBalance gemBalance = new GemBalance();
        gemBalance.Load(new JsonSaveLoad());
        gemBalance.Add(_diamondCollector.CollectedDiamondCost);
        gemBalance.Save(new JsonSaveLoad());
    }

    private bool HasInCollection(HatData hatData)
    {
        HatCollection collection = new HatCollection(_dataBase);
        collection.Load(new JsonSaveLoad());

        return collection.Contains(hatData);
    }

    private void OnHatSpawned(SnakeHat hat, HatData hatData)
    {
        _hat = hat;
        _hatData = hatData;
    }
}
