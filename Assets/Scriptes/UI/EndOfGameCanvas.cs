using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(Animator))]
public class EndOfGameCanvas : MonoBehaviour
{
    [SerializeField] private HatDataBase _dataBase;
    [SerializeField] private EarnedGemsPresenter _eargedGems;
    [SerializeField] private GameObject _hatBonus;
    [SerializeField] private GameObject _achievementObject;
    [SerializeField] private Image _hatPreview;
    [SerializeField] private Image _hatBonusPreview;
    [SerializeField] private MultiplyGemRewardButton _multiplyRewardButton;

    private Canvas _selfCanvas;
    private Animator _selfAnimator;
    private Pole _pole;
    private BonusFinish _bonusFinish;
    private BonusDiamondCollector _diamondCollector;
    private HatSpawner _hatSpawner;
    private SnakeHat _hat;
    private HatData _hatData;
    private int _earnedGems;

    public event UnityAction Showed;

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
        _multiplyRewardButton.MultiplyUsed += OnUsedMultiply;
        if (_pole)
            _pole.SnakeCrawled += OnSnakeCrawled;
        if (_bonusFinish)
            _bonusFinish.Finished += OnBonusFinishCrawled;
        if (_hatSpawner)
            _hatSpawner.Spawned += OnHatSpawned;
    }

    private void OnDisable()
    {
        _multiplyRewardButton.MultiplyUsed -= OnUsedMultiply;
        if (_pole)
            _pole.SnakeCrawled -= OnSnakeCrawled;
        if (_bonusFinish)
            _bonusFinish.Finished -= OnBonusFinishCrawled;
        if (_hatSpawner)
            _hatSpawner.Spawned -= OnHatSpawned;
    }

    private void OnSnakeCrawled(int gemValue)
    {
        _earnedGems = gemValue;
        _selfAnimator.SetTrigger("Show");
        _selfCanvas.enabled = true;
        _eargedGems.Render(_earnedGems);

        GemBalance gemBalance = new GemBalance();
        gemBalance.Load(new JsonSaveLoad());

        if (_hat != null && _hat.OnSnake)
        {
            _hatBonusPreview.sprite = _hatData.HatPreview;
            _hatBonus.SetActive(true);
            _eargedGems.PlayFromToAnimation(_earnedGems, _earnedGems + 100);
            _earnedGems += 100;

            if (HasInCollection(_hatData) == false)
            {
                _achievementObject.SetActive(true);
                _hatPreview.sprite = _hatData.HatPreview;
                HatCollection collection = new HatCollection(_dataBase);
                collection.Load(new JsonSaveLoad());
                collection.Add(_hatData);
                collection.SelectHat(_hatData);
                collection.Save(new JsonSaveLoad());
            }
        }

        gemBalance.Add(_earnedGems);
        gemBalance.Save(new JsonSaveLoad());
        
        Showed?.Invoke();
    }

    private void OnUsedMultiply(int multiply)
    {
        var bonusGem = _earnedGems * (multiply - 1);
        GemBalance gemBalance = new GemBalance();
        gemBalance.Load(new JsonSaveLoad());
        gemBalance.Add(bonusGem);
        gemBalance.Save(new JsonSaveLoad());
    }

    private void OnBonusFinishCrawled()
    {
        _selfAnimator.SetTrigger("Show");
        _selfCanvas.enabled = true;
        _eargedGems.Render(_diamondCollector.CollectedDiamondCost);

        _earnedGems = _diamondCollector.CollectedDiamondCost;
        GemBalance gemBalance = new GemBalance();
        gemBalance.Load(new JsonSaveLoad());
        gemBalance.Add(_earnedGems);
        gemBalance.Save(new JsonSaveLoad());
        
        Showed?.Invoke();
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
