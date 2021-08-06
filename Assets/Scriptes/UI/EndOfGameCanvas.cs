using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(Animator))]
public class EndOfGameCanvas : MonoBehaviour
{
    [SerializeField] private EarnedGemsPresenter _eargedGems;
    [SerializeField] private GameObject _hatBonus;

    private Canvas _selfCanvas;
    private Animator _selfAnimator;
    private Pole _pole;
    private BonusFinish _bonusFinish;
    private BonusDiamondCollector _diamondCollector;
    private SnakeHat _hat;

    private void Awake()
    {
        _selfCanvas = GetComponent<Canvas>();
        _selfAnimator = GetComponent<Animator>();
        _pole = FindObjectOfType<Pole>();
        _bonusFinish = FindObjectOfType<BonusFinish>();
        _diamondCollector = FindObjectOfType<BonusDiamondCollector>();
        _hat = FindObjectOfType<SnakeHat>();
    }

    private void OnEnable()
    {
        if (_pole)
            _pole.SnakeCrawled += OnSnakeCrawled;
        if (_bonusFinish)
            _bonusFinish.Crawled += OnBonusFinishCrawled;
    }

    private void OnDisable()
    {
        if (_pole)
            _pole.SnakeCrawled -= OnSnakeCrawled;
        if (_bonusFinish)
            _bonusFinish.Crawled -= OnBonusFinishCrawled;
    }

    private void OnSnakeCrawled(int gemValue)
    {
        _selfAnimator.SetTrigger("Show");
        _selfCanvas.enabled = true;
        _eargedGems.Render(gemValue);

        GemBalance gemBalance = new GemBalance();
        gemBalance.Load(new JsonSaveLoad());

        if (_hat.OnSnake)
        {
            _hatBonus.SetActive(true);
            _eargedGems.PlayFromToAnimation(gemValue, gemValue + 100);
            gemValue += 100;
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
}
