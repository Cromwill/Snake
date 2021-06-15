using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ObstacleTimer : MonoBehaviour
{
    [SerializeField] private float _delayTime;
    [SerializeField] private string _clipName;

    private Animator _selfAnimator;

    void Start()
    {
        _selfAnimator = GetComponent<Animator>();
        if (_delayTime == 0)
            _selfAnimator.Play(_clipName);
        else
            StartCoroutine(PlayAnimatin());
    }

    private IEnumerator PlayAnimatin()
    {
        yield return new WaitForSeconds(_delayTime);
        _selfAnimator.Play(_clipName);
    }
}
