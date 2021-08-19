using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SnakeHat : MonoBehaviour
{
    private Collider _collider;
    private Animator _animator;
    private Head _parent;
    private FinishTrigger _finishTrigger;
    private Snake _snake;
    private Vector3 _startPosition;
    private HatSound _sound;

    public bool OnSnake { get; private set; }

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _animator = GetComponentInChildren<Animator>();
        _finishTrigger = FindObjectOfType<FinishTrigger>();
        _sound = GetComponent<HatSound>();
    }

    private void OnEnable()
    {
        //_finishTrigger.PlayerFinished += OnPlayerFinished;
    }

    private void OnDisable()
    {
        //_finishTrigger.PlayerFinished -= OnPlayerFinished;
    }

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void OnPlayerFinished()
    {
        ResetState();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Head head))
        {
            _parent = head;
            transform.SetParent(head.HatContainer);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            _parent.ObstacleEntered += OnObstacleEntered;

            _snake = _parent.GetComponentInParent<Snake>();

            _animator.SetBool("Hooked", true);

            OnSnake = true;
            _sound.PlayHatPickedSound();
        }
    }

    private void OnObstacleEntered(Obstacle obstacle)
    {
        //_animator.SetTrigger("Damaged");
        ResetState();
    }

    private void Update()
    {
        if (_parent == null)
            return;

        _animator.SetFloat("Move", _snake.CurrentSpeed / _snake.MaxSpeed);
    }

    private IEnumerator SetStartTransform()
    {
        _collider.enabled = false;

        float distance;
        do
        {
            distance = Vector3.Distance(transform.position, _startPosition);
            transform.position = Vector3.Lerp(transform.position, _startPosition, Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);

        } while (distance > 0.1f);

        _collider.enabled = true;
    }

    private void ResetState()
    {
        _parent.ObstacleEntered -= OnObstacleEntered;
        transform.SetParent(null);
        _parent = null;
        _snake = null;
        StartCoroutine(SetStartTransform());

        _animator.SetBool("Hooked", false);
        OnSnake = false;
        _sound.PlayHatLosedSound();
    }
}
