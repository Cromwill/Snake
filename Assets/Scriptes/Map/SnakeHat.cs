using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animator))]
public class SnakeHat : MonoBehaviour
{
    private Collider _collider;
    private Animator _animator;
    private Head _targetHead;
    private FinishTrigger _finishTrigger;
    private Snake _snake;
    private Vector3 _startPosition;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _animator = GetComponent<Animator>();
        _finishTrigger = FindObjectOfType<FinishTrigger>();
    }

    private void OnEnable()
    {
        _finishTrigger.PlayerFinished += OnPlayerFinished;
    }

    private void OnDisable()
    {
        _finishTrigger.PlayerFinished -= OnPlayerFinished;
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
            _targetHead = head;
            _targetHead.ObstacleEntered += OnObstacleEntered;

            _snake = _targetHead.GetComponentInParent<Snake>();

            _animator.SetBool("Hooked", true);
        }
    }

    private void OnObstacleEntered(Obstacle obstacle)
    {
        ResetState();
    }

    private void Update()
    {
        if (_targetHead == null)
            return;

        _animator.SetFloat("Move", _snake.CurrentSpeed / _snake.MaxSpeed);
        transform.position = Vector3.Lerp(transform.position, _targetHead.ColliderCenter + Vector3.up, 25f * Time.deltaTime);

        float yRotation = Mathf.Lerp(transform.rotation.y, _targetHead.transform.rotation.y, 25f * Time.deltaTime);
        transform.rotation = Quaternion.Euler(transform.rotation.x, yRotation, transform.rotation.z);
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
        _targetHead.ObstacleEntered -= OnObstacleEntered;
        _targetHead = null;
        _snake = null;
        StartCoroutine(SetStartTransform());

        _animator.SetBool("Hooked", false);
    }
}
