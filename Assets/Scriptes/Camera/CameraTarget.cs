using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private SnakeSkeleton _followingSnake;
    [SerializeField] private SnakeBoneMovement _snakeBoneMovement;
    [SerializeField] private Transform _pole;
    [SerializeField] private FinishTrigger _finishTrigger;
    [SerializeField] private Animator _cameraAnimation;

    private Coroutine _lookRotationCoroutine;
    private bool _finished;

    private void OnEnable()
    {
        _finishTrigger.PlayerFinished += OnPlayerFinished;
        _snakeBoneMovement.Partially—rawled += OnPlayerPartiallyCrawled;
        _snakeBoneMovement.Full—rawled += OnPlayerFullCrawled;
    }

    private void OnDisable()
    {
        _finishTrigger.PlayerFinished -= OnPlayerFinished;
        _snakeBoneMovement.Partially—rawled -= OnPlayerPartiallyCrawled;
        _snakeBoneMovement.Full—rawled -= OnPlayerFullCrawled;
    }

    private void OnPlayerFullCrawled()
    {

    }

    private void OnPlayerPartiallyCrawled(float distance)
    {
        _cameraAnimation.SetTrigger("PartialyCrawled");

        var headPosition = _followingSnake.Head.transform.position;
        var polePosition = _pole.position;
        headPosition.y = polePosition.y;

        var forwardDirection = headPosition - polePosition;
        float angle = Mathf.Atan(forwardDirection.x / forwardDirection.z);
        angle = angle * 180 / Mathf.PI;

        if (forwardDirection.z < 0)
            angle += 180;

        _lookRotationCoroutine = StartCoroutine(LookRotationCoroutine(_cameraAnimation.transform, angle - _pole.rotation.eulerAngles.y - 90f));
    }

    private void OnPlayerFinished()
    {
        _cameraAnimation.SetTrigger("Finished");
        _finished = true;
    }

    private void Start()
    {
        _finished = false;
    }

    private void LateUpdate()
    {
        if (_finished == false)
        {
            transform.position = _followingSnake.Head.transform.position;
        }
        else
        {
            var nextPosition = new Vector3(_pole.position.x, _followingSnake.Head.transform.position.y, _pole.position.z);
            transform.position = Vector3.Lerp(transform.position, nextPosition, 3f * Time.deltaTime);
        }
    }

    private IEnumerator LookRotationCoroutine(Transform transform, float yRotation)
    {
        var rotation = Quaternion.Euler(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);

        while (Mathf.Abs(transform.rotation.y - yRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        _lookRotationCoroutine = null;
    }
}
