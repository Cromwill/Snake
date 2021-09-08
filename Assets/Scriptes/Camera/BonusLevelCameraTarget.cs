using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusLevelCameraTarget : MonoBehaviour
{
    [SerializeField] private SnakeInitializer _snakeInitializer;
    [SerializeField] private BonusFinish _bonusFinish;
    [SerializeField] private float _yPosition = 5f;

    private Snake _followingSnake;

    private void OnEnable()
    {
        _snakeInitializer.Initialized += OnSnakeInitialized;
    }

    private void OnDisable()
    {
        _snakeInitializer.Initialized -= OnSnakeInitialized;
    }

    private void OnSnakeInitialized(Snake snake)
    {
        _followingSnake = snake;
        transform.position = new Vector3(_bonusFinish.transform.position.x, _yPosition, _followingSnake.HeadTransform.position.z);
    }

    private void Update()
    {
        var nextPosition = new Vector3(_bonusFinish.transform.position.x, _yPosition, _followingSnake.HeadTransform.position.z);
        transform.position = Vector3.Lerp(transform.position, nextPosition, 3f * Time.deltaTime);
    }
}
