using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(Animator))]
public class EndOfGameCanvas : MonoBehaviour
{
    [SerializeField] private Pole _pole;
    [SerializeField] private EarnedGemsPresenter _eargedGems;

    private Canvas _selfCanvas;
    private Animator _selfAnimator;

    private void Awake()
    {
        _selfCanvas = GetComponent<Canvas>();
        _selfAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _pole.SnakeCrawled += OnSnakeCrawled;
    }

    private void OnDisable()
    {
        _pole.SnakeCrawled -= OnSnakeCrawled;
    }

    private void OnSnakeCrawled(int gemValue)
    {
        _selfAnimator.SetTrigger("Show");
        _selfCanvas.enabled = true;
        _eargedGems.Render(gemValue);

        GemBalance gemBalance = new GemBalance();
        gemBalance.Load(new JsonSaveLoad());

        gemBalance.Add(gemValue);
        gemBalance.Save(new JsonSaveLoad());
    }
}
